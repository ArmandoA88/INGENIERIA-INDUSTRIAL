' =====================================================
' GENERADOR DE GRÁFICOS DE PARETO
' Herramienta para análisis 80/20
' =====================================================
' Autor: Repositorio Ingeniería Industrial
' Fecha: 2025
' Descripción: Interfaz gráfica para crear gráficos
'              de Pareto con análisis automático
' =====================================================

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Public Class FormGraficoPareto

    ' ===== VARIABLES GLOBALES =====
    Private datos As New List(Of DatoPareto)
    Private datosOrdenados As New List(Of DatoPareto)
    Private graficoGenerado As Boolean = False

    ' ===== CLASE DATO PARETO =====
    Public Class DatoPareto
        Public Property Categoria As String
        Public Property Valor As Double
        Public Property Porcentaje As Double
        Public Property PorcentajeAcumulado As Double
        Public Property Color As Color

        Public Sub New(categoria As String, valor As Double)
            Me.Categoria = categoria
            Me.Valor = valor
            Me.Porcentaje = 0
            Me.PorcentajeAcumulado = 0
            Me.Color = Color.SteelBlue
        End Sub
    End Class

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub FormGraficoPareto_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Configuración inicial
        Me.Text = "Generador de Gráficos de Pareto - Ingeniería Industrial"
        Me.Size = New Size(1200, 800)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 248, 255)

        ' Inicializar interfaz
        InicializarInterfaz()
        CargarDatosEjemplo()
    End Sub

    ' ===== INICIALIZACIÓN DE LA INTERFAZ =====
    Private Sub InicializarInterfaz()
        ' Panel principal
        Dim panelPrincipal As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1
        }
        panelPrincipal.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 300))
        panelPrincipal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        Me.Controls.Add(panelPrincipal)

        ' === PANEL DE CONTROLES ===
        Dim panelControles As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(230, 230, 250),
            .Padding = New Padding(10),
            .AutoScroll = True
        }
        panelPrincipal.Controls.Add(panelControles, 0, 0)

        CrearControlesEntrada(panelControles)

        ' === ÁREA DE GRÁFICO ===
        Dim panelGrafico As New Panel With {
            .Name = "panelGrafico",
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .BorderStyle = BorderStyle.FixedSingle
        }
        panelPrincipal.Controls.Add(panelGrafico, 1, 0)

        ' Configurar eventos del área de gráfico
        AddHandler panelGrafico.Paint, AddressOf PanelGrafico_Paint
    End Sub

    ' ===== CREAR CONTROLES DE ENTRADA =====
    Private Sub CrearControlesEntrada(panel As Panel)
        Dim yPos As Integer = 10

        ' Título
        Dim lblTitulo As New Label With {
            .Text = "📊 GRÁFICO DE PARETO",
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(10, yPos)
        }
        panel.Controls.Add(lblTitulo)
        yPos += 40

        ' === ENTRADA DE DATOS ===
        Dim gbEntrada As New GroupBox With {
            .Text = "Entrada de Datos",
            .Size = New Size(270, 200),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbEntrada)

        ' Categoría
        Dim lblCategoria As New Label With {
            .Text = "Categoría:",
            .Location = New Point(10, 25),
            .AutoSize = True
        }
        gbEntrada.Controls.Add(lblCategoria)

        Dim txtCategoria As New TextBox With {
            .Name = "txtCategoria",
            .Size = New Size(200, 25),
            .Location = New Point(10, 45)
        }
        gbEntrada.Controls.Add(txtCategoria)

        ' Valor
        Dim lblValor As New Label With {
            .Text = "Valor:",
            .Location = New Point(10, 75),
            .AutoSize = True
        }
        gbEntrada.Controls.Add(lblValor)

        Dim txtValor As New TextBox With {
            .Name = "txtValor",
            .Size = New Size(200, 25),
            .Location = New Point(10, 95)
        }
        gbEntrada.Controls.Add(txtValor)

        ' Botones de gestión de datos
        Dim btnAgregar As New Button With {
            .Text = "➕ Agregar",
            .Size = New Size(95, 30),
            .Location = New Point(10, 130),
            .BackColor = Color.LightGreen,
            .FlatStyle = FlatStyle.Flat
        }
        gbEntrada.Controls.Add(btnAgregar)
        AddHandler btnAgregar.Click, AddressOf AgregarDato

        Dim btnEliminar As New Button With {
            .Text = "➖ Eliminar",
            .Size = New Size(95, 30),
            .Location = New Point(115, 130),
            .BackColor = Color.LightCoral,
            .FlatStyle = FlatStyle.Flat
        }
        gbEntrada.Controls.Add(btnEliminar)
        AddHandler btnEliminar.Click, AddressOf EliminarDato

        Dim btnLimpiar As New Button With {
            .Text = "🗑️ Limpiar Todo",
            .Size = New Size(200, 30),
            .Location = New Point(10, 165),
            .BackColor = Color.Orange,
            .FlatStyle = FlatStyle.Flat
        }
        gbEntrada.Controls.Add(btnLimpiar)
        AddHandler btnLimpiar.Click, AddressOf LimpiarDatos

        yPos += 220

        ' === LISTA DE DATOS ===
        Dim gbDatos As New GroupBox With {
            .Text = "Datos Ingresados",
            .Size = New Size(270, 200),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbDatos)

        Dim lstDatos As New ListBox With {
            .Name = "lstDatos",
            .Size = New Size(250, 170),
            .Location = New Point(10, 25),
            .Font = New Font("Consolas", 9)
        }
        gbDatos.Controls.Add(lstDatos)

        yPos += 220

        ' === CONTROLES DE GRÁFICO ===
        Dim gbGrafico As New GroupBox With {
            .Text = "Controles del Gráfico",
            .Size = New Size(270, 150),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbGrafico)

        Dim btnGenerar As New Button With {
            .Text = "📈 Generar Gráfico",
            .Size = New Size(250, 35),
            .Location = New Point(10, 25),
            .BackColor = Color.LightBlue,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbGrafico.Controls.Add(btnGenerar)
        AddHandler btnGenerar.Click, AddressOf GenerarGrafico

        Dim btnGuardar As New Button With {
            .Text = "💾 Guardar PNG",
            .Size = New Size(120, 30),
            .Location = New Point(10, 70),
            .BackColor = Color.LightGreen,
            .FlatStyle = FlatStyle.Flat
        }
        gbGrafico.Controls.Add(btnGuardar)
        AddHandler btnGuardar.Click, AddressOf GuardarGrafico

        Dim btnExportar As New Button With {
            .Text = "📄 Exportar CSV",
            .Size = New Size(120, 30),
            .Location = New Point(140, 70),
            .BackColor = Color.LightYellow,
            .FlatStyle = FlatStyle.Flat
        }
        gbGrafico.Controls.Add(btnExportar)
        AddHandler btnExportar.Click, AddressOf ExportarDatos

        Dim btnAyuda As New Button With {
            .Text = "❓ Ayuda",
            .Size = New Size(250, 30),
            .Location = New Point(10, 110),
            .BackColor = Color.LightCyan,
            .FlatStyle = FlatStyle.Flat
        }
        gbGrafico.Controls.Add(btnAyuda)
        AddHandler btnAyuda.Click, AddressOf MostrarAyuda

        yPos += 170

        ' === ANÁLISIS ===
        Dim gbAnalisis As New GroupBox With {
            .Text = "Análisis Pareto",
            .Size = New Size(270, 120),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbAnalisis)

        Dim lblAnalisis As New Label With {
            .Name = "lblAnalisis",
            .Text = "Genere el gráfico para ver el análisis",
            .Size = New Size(250, 90),
            .Location = New Point(10, 25),
            .Font = New Font("Arial", 9),
            .ForeColor = Color.DarkBlue
        }
        gbAnalisis.Controls.Add(lblAnalisis)
    End Sub

    ' ===== MÉTODOS DE GESTIÓN DE DATOS =====
    Private Sub AgregarDato(sender As Object, e As EventArgs)
        Dim txtCategoria As TextBox = DirectCast(Me.Controls.Find("txtCategoria", True).FirstOrDefault(), TextBox)
        Dim txtValor As TextBox = DirectCast(Me.Controls.Find("txtValor", True).FirstOrDefault(), TextBox)

        If String.IsNullOrWhiteSpace(txtCategoria.Text) Then
            MessageBox.Show("Por favor ingrese una categoría", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim valor As Double
        If Not Double.TryParse(txtValor.Text, valor) OrElse valor <= 0 Then
            MessageBox.Show("Por favor ingrese un valor numérico válido mayor a 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Verificar si la categoría ya existe
        If datos.Any(Function(d) d.Categoria.ToLower() = txtCategoria.Text.ToLower()) Then
            MessageBox.Show("Esta categoría ya existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Agregar el dato
        datos.Add(New DatoPareto(txtCategoria.Text, valor))
        ActualizarListaDatos()

        ' Limpiar campos
        txtCategoria.Clear()
        txtValor.Clear()
        txtCategoria.Focus()
    End Sub

    Private Sub EliminarDato(sender As Object, e As EventArgs)
        Dim lstDatos As ListBox = DirectCast(Me.Controls.Find("lstDatos", True).FirstOrDefault(), ListBox)
        
        If lstDatos.SelectedIndex >= 0 AndAlso lstDatos.SelectedIndex < datos.Count Then
            datos.RemoveAt(lstDatos.SelectedIndex)
            ActualizarListaDatos()
            graficoGenerado = False
            Me.Controls.Find("panelGrafico", True).FirstOrDefault()?.Invalidate()
        Else
            MessageBox.Show("Seleccione un elemento de la lista para eliminar", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub LimpiarDatos(sender As Object, e As EventArgs)
        If MessageBox.Show("¿Está seguro de que desea limpiar todos los datos?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            datos.Clear()
            ActualizarListaDatos()
            graficoGenerado = False
            Me.Controls.Find("panelGrafico", True).FirstOrDefault()?.Invalidate()
        End If
    End Sub

    Private Sub ActualizarListaDatos()
        Dim lstDatos As ListBox = DirectCast(Me.Controls.Find("lstDatos", True).FirstOrDefault(), ListBox)
        lstDatos.Items.Clear()

        For Each dato As DatoPareto In datos
            lstDatos.Items.Add($"{dato.Categoria}: {dato.Valor:N2}")
        Next
    End Sub

    Private Sub CargarDatosEjemplo()
        ' Datos de ejemplo para demostración
        datos.Add(New DatoPareto("Defecto A", 45))
        datos.Add(New DatoPareto("Defecto B", 30))
        datos.Add(New DatoPareto("Defecto C", 15))
        datos.Add(New DatoPareto("Defecto D", 8))
        datos.Add(New DatoPareto("Defecto E", 2))
        ActualizarListaDatos()
    End Sub

    ' ===== GENERACIÓN DEL GRÁFICO =====
    Private Sub GenerarGrafico(sender As Object, e As EventArgs)
        If datos.Count = 0 Then
            MessageBox.Show("No hay datos para generar el gráfico", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Ordenar datos por valor descendente
        datosOrdenados = datos.OrderByDescending(Function(d) d.Valor).ToList()

        ' Calcular porcentajes
        Dim total As Double = datosOrdenados.Sum(Function(d) d.Valor)
        Dim acumulado As Double = 0

        For i As Integer = 0 To datosOrdenados.Count - 1
            datosOrdenados(i).Porcentaje = (datosOrdenados(i).Valor / total) * 100
            acumulado += datosOrdenados(i).Porcentaje
            datosOrdenados(i).PorcentajeAcumulado = acumulado
            
            ' Asignar colores
            If i < datosOrdenados.Count * 0.2 Then ' Primeros 20% - Pocos vitales
                datosOrdenados(i).Color = Color.Red
            ElseIf acumulado <= 80 Then ' Hasta el 80% acumulado
                datosOrdenados(i).Color = Color.Orange
            Else ' Muchos triviales
                datosOrdenados(i).Color = Color.LightBlue
            End If
        Next

        graficoGenerado = True
        Me.Controls.Find("panelGrafico", True).FirstOrDefault()?.Invalidate()
        ActualizarAnalisis()
    End Sub

    ' ===== DIBUJO DEL GRÁFICO =====
    Private Sub PanelGrafico_Paint(sender As Object, e As PaintEventArgs)
        If Not graficoGenerado OrElse datosOrdenados.Count = 0 Then
            DibujarMensajeInicial(e.Graphics, DirectCast(sender, Panel))
            Return
        End If

        DibujarGraficoPareto(e.Graphics, DirectCast(sender, Panel))
    End Sub

    Private Sub DibujarMensajeInicial(g As Graphics, panel As Panel)
        Dim mensaje As String = "Agregue datos y haga clic en 'Generar Gráfico' para visualizar el análisis de Pareto"
        Dim font As New Font("Arial", 14, FontStyle.Italic)
        Dim brush As New SolidBrush(Color.Gray)
        Dim rect As New Rectangle(50, panel.Height \ 2 - 20, panel.Width - 100, 40)
        Dim format As New StringFormat With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center
        }
        
        g.DrawString(mensaje, font, brush, rect, format)
    End Sub

    Private Sub DibujarGraficoPareto(g As Graphics, panel As Panel)
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Márgenes
        Dim margenIzq As Integer = 80
        Dim margenDer As Integer = 80
        Dim margenSup As Integer = 60
        Dim margenInf As Integer = 100

        ' Área del gráfico
        Dim areaGrafico As New Rectangle(margenIzq, margenSup, panel.Width - margenIzq - margenDer, panel.Height - margenSup - margenInf)

        ' Dibujar título
        Dim fontTitulo As New Font("Arial", 16, FontStyle.Bold)
        g.DrawString("Gráfico de Pareto", fontTitulo, Brushes.Black, New Point(panel.Width \ 2 - 80, 20))

        ' Dibujar ejes
        g.DrawLine(Pens.Black, areaGrafico.Left, areaGrafico.Bottom, areaGrafico.Right, areaGrafico.Bottom) ' Eje X
        g.DrawLine(Pens.Black, areaGrafico.Left, areaGrafico.Bottom, areaGrafico.Left, areaGrafico.Top) ' Eje Y izquierdo
        g.DrawLine(Pens.Black, areaGrafico.Right, areaGrafico.Bottom, areaGrafico.Right, areaGrafico.Top) ' Eje Y derecho

        ' Calcular dimensiones de barras
        Dim anchoBarra As Integer = areaGrafico.Width \ datosOrdenados.Count
        Dim valorMaximo As Double = datosOrdenados.Max(Function(d) d.Valor)

        ' Dibujar barras
        For i As Integer = 0 To datosOrdenados.Count - 1
            Dim dato As DatoPareto = datosOrdenados(i)
            Dim x As Integer = areaGrafico.Left + (i * anchoBarra)
            Dim alturaBarra As Integer = CInt((dato.Valor / valorMaximo) * areaGrafico.Height)
            Dim y As Integer = areaGrafico.Bottom - alturaBarra

            ' Dibujar barra
            Dim rectBarra As New Rectangle(x + 5, y, anchoBarra - 10, alturaBarra)
            g.FillRectangle(New SolidBrush(dato.Color), rectBarra)
            g.DrawRectangle(Pens.Black, rectBarra)

            ' Etiqueta de categoría
            Dim fontCategoria As New Font("Arial", 8)
            Dim rectTexto As New Rectangle(x, areaGrafico.Bottom + 5, anchoBarra, 20)
            Dim format As New StringFormat With {.Alignment = StringAlignment.Center}
            g.DrawString(dato.Categoria, fontCategoria, Brushes.Black, rectTexto, format)

            ' Valor sobre la barra
            Dim valorTexto As String = dato.Valor.ToString("N1")
            Dim rectValor As New Rectangle(x, y - 20, anchoBarra, 15)
            g.DrawString(valorTexto, fontCategoria, Brushes.Black, rectValor, format)
        Next

        ' Dibujar línea de porcentaje acumulado
        Dim puntos As New List(Of Point)
        For i As Integer = 0 To datosOrdenados.Count - 1
            Dim x As Integer = areaGrafico.Left + (i * anchoBarra) + (anchoBarra \ 2)
            Dim y As Integer = areaGrafico.Bottom - CInt((datosOrdenados(i).PorcentajeAcumulado / 100) * areaGrafico.Height)
            puntos.Add(New Point(x, y))
        Next

        If puntos.Count > 1 Then
            g.DrawLines(New Pen(Color.Red, 3), puntos.ToArray())
            
            ' Dibujar puntos
            For Each punto As Point In puntos
                g.FillEllipse(Brushes.Red, punto.X - 3, punto.Y - 3, 6, 6)
            Next
        End If

        ' Dibujar línea del 80%
        Dim y80 As Integer = areaGrafico.Bottom - CInt(0.8 * areaGrafico.Height)
        Dim penVerde As New Pen(Color.Green, 2)
        penVerde.DashStyle = Drawing2D.DashStyle.Dash
        g.DrawLine(penVerde, areaGrafico.Left, y80, areaGrafico.Right, y80)
        g.DrawString("80%", New Font("Arial", 10, FontStyle.Bold), Brushes.Green, areaGrafico.Right + 5, y80 - 10)

        ' Etiquetas de ejes
        Dim fontEje As New Font("Arial", 10, FontStyle.Bold)
        
        ' Eje Y izquierdo (Valores)
        g.DrawString("Valores", fontEje, Brushes.Black, 10, areaGrafico.Top + areaGrafico.Height \ 2, New StringFormat With {.FormatFlags = StringFormatFlags.DirectionVertical})
        
        ' Eje Y derecho (Porcentaje)
        g.DrawString("% Acumulado", fontEje, Brushes.Red, areaGrafico.Right + 10, areaGrafico.Top + areaGrafico.Height \ 2, New StringFormat With {.FormatFlags = StringFormatFlags.DirectionVertical})
        
        ' Escalas
        DibujarEscalas(g, areaGrafico, valorMaximo)

        ' Leyenda
        DibujarLeyenda(g, panel)
    End Sub

    Private Sub DibujarEscalas(g As Graphics, areaGrafico As Rectangle, valorMaximo As Double)
        Dim fontEscala As New Font("Arial", 8)
        
        ' Escala izquierda (valores)
        For i As Integer = 0 To 5
            Dim valor As Double = (valorMaximo / 5) * i
            Dim y As Integer = areaGrafico.Bottom - CInt((i / 5) * areaGrafico.Height)
            g.DrawString(valor.ToString("N0"), fontEscala, Brushes.Black, 5, y - 8)
            g.DrawLine(Pens.Gray, areaGrafico.Left - 5, y, areaGrafico.Left, y)
        Next

        ' Escala derecha (porcentajes)
        For i As Integer = 0 To 5
            Dim porcentaje As Integer = (100 / 5) * i
            Dim y As Integer = areaGrafico.Bottom - CInt((i / 5) * areaGrafico.Height)
            g.DrawString($"{porcentaje}%", fontEscala, Brushes.Red, areaGrafico.Right + 5, y - 8)
            g.DrawLine(Pens.Gray, areaGrafico.Right, y, areaGrafico.Right + 5, y)
        Next
    End Sub

    Private Sub DibujarLeyenda(g As Graphics, panel As Panel)
        Dim x As Integer = 20
        Dim y As Integer = panel.Height - 80
        Dim fontLeyenda As New Font("Arial", 9)

        ' Barras
        g.FillRectangle(Brushes.Red, x, y, 15, 10)
        g.DrawString("Pocos Vitales (20%)", fontLeyenda, Brushes.Black, x + 20, y - 2)

        g.FillRectangle(Brushes.Orange, x + 150, y, 15, 10)
        g.DrawString("Importantes", fontLeyenda, Brushes.Black, x + 170, y - 2)

        g.FillRectangle(Brushes.LightBlue, x + 270, y, 15, 10)
        g.DrawString("Muchos Triviales", fontLeyenda, Brushes.Black, x + 290, y - 2)

        ' Línea acumulada
        g.DrawLine(New Pen(Color.Red, 3), x, y + 20, x + 15, y + 20)
        g.DrawString("% Acumulado", fontLeyenda, Brushes.Black, x + 20, y + 18)
    End Sub

    ' ===== ANÁLISIS =====
    Private Sub ActualizarAnalisis()
        Dim lblAnalisis As Label = DirectCast(Me.Controls.Find("lblAnalisis", True).FirstOrDefault(), Label)
        
        If datosOrdenados.Count = 0 Then Return

        ' Encontrar el punto del 80%
        Dim indice80 As Integer = 0
        For i As Integer = 0 To datosOrdenados.Count - 1
            If datosOrdenados(i).PorcentajeAcumulado >= 80 Then
                indice80 = i + 1
                Exit For
            End If
        Next

        Dim porcentajeCategorias As Double = (indice80 / datosOrdenados.Count) * 100
        Dim valorVitales As Double = datosOrdenados.Take(indice80).Sum(Function(d) d.Valor)
        Dim totalValor As Double = datosOrdenados.Sum(Function(d) d.Valor)
        Dim porcentajeValor As Double = (valorVitales / totalValor) * 100

        lblAnalisis.Text = $"ANÁLISIS PARETO:" & vbCrLf &
                          $"• {indice80} categorías ({porcentajeCategorias:N1}%)" & vbCrLf &
                          $"  representan el 80% del problema" & vbCrLf &
                          $"• Valor acumulado: {valorVitales:N2}" & vbCrLf &
                          $"  ({porcentajeValor:N1}% del total)"
    End Sub

    ' ===== MÉTODOS DE EXPORTACIÓN =====
    Private Sub GuardarGrafico(sender As Object, e As EventArgs)
        If Not graficoGenerado Then
            MessageBox.Show("Primero debe generar el gráfico", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim panelGrafico As Panel = DirectCast(Me.Controls.Find("panelGrafico", True).FirstOrDefault(), Panel)
            If panelGrafico IsNot Nothing Then
                Dim bitmap As New Bitmap(panelGrafico.Width, panelGrafico.Height)
                panelGrafico.DrawToBitmap(bitmap, New Rectangle(0, 0, panelGrafico.Width, panelGrafico.Height))
                
                Dim saveDialog As New SaveFileDialog With {
                    .Filter = "PNG Image|*.png",
                    .Title = "Guardar Gráfico de Pareto",
                    .FileName = "GraficoPareto_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".png"
                }
                
                If saveDialog.ShowDialog() = DialogResult.OK Then
                    bitmap.Save(saveDialog.FileName, Imaging.ImageFormat.Png)
                    MessageBox.Show("Gráfico guardado exitosamente en:" & vbCrLf & saveDialog.FileName, "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error al guardar el gráfico:" & vbCrLf & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportarDatos(sender As Object, e As EventArgs)
        If datosOrdenados.Count = 0 Then
            MessageBox.Show("No hay datos para exportar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim saveDialog As New SaveFileDialog With {
                .Filter = "CSV File|*.csv",
                .Title = "Exportar Datos de Pareto",
                .FileName = "DatosPareto_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".csv"
            }
            
            If saveDialog.ShowDialog() = DialogResult.OK Then
                Using writer As New StreamWriter(saveDialog.FileName)
                    writer.WriteLine("Categoria,Valor,Porcentaje,Porcentaje_Acumulado")
                    For Each dato As DatoPareto In datosOrdenados
                        writer.WriteLine($"{dato.Categoria},{dato.Valor},{dato.Porcentaje:N2},{dato.PorcentajeAcumulado:N2}")
                    Next
                End Using
                MessageBox.Show("Datos exportados exitosamente a:" & vbCrLf & saveDialog.FileName, "Exportado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error al exportar los datos:" & vbCrLf & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MostrarAyuda(sender As Object, e As EventArgs)
        MessageBox.Show("GENERADOR DE GRÁFICOS DE PARETO" & vbCrLf & vbCrLf &
                      "PRINCIPIO DE PARETO (80/20):" & vbCrLf &
                      "El 80% de los efectos proviene del 20% de las causas" & vbCrLf & vbCrLf &
                      "INSTRUCCIONES:" & vbCrLf &
                      "1. Ingrese categorías y sus valores" & vbCrLf &
                      "2. Use 'Agregar' para añadir datos" & vbCrLf &
                      "3. Haga clic en 'Generar Gráfico'" & vbCrLf &
                      "4. Analice los resultados en el panel derecho" & vbCrLf & vbCrLf &
                      "INTERPRETACIÓN:" & vbCrLf &
                      "• Barras rojas: Pocos vitales (20% de causas)" & vbCrLf &
                      "• Barras naranjas: Causas importantes" & vbCrLf &
                      "• Barras azules: Muchos triviales" & vbCrLf &
                      "• Línea roja: Porcentaje acumulado" & vbCrLf &
                      "• Línea verde: Punto del 80%" & vbCrLf & vbCrLf &
                      "APLICACIONES:" & vbCrLf &
                      "• Análisis de defectos" & vbCrLf &
                      "• Priorización de problemas" & vbCrLf &
                      "• Gestión de inventarios" & vbCrLf &
                      "• Análisis de costos",
                      "Ayuda - Gráfico de Pareto", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
