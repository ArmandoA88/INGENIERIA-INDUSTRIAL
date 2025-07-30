' =====================================================
' GENERADOR DE GRÁFICOS DE CONTROL
' Herramienta para control estadístico de procesos
' =====================================================
' Autor: Repositorio Ingeniería Industrial
' Fecha: 2025
' Descripción: Interfaz gráfica para crear gráficos
'              de control estadístico de calidad
' =====================================================

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Public Class FormGraficoControl

    ' ===== VARIABLES GLOBALES =====
    Private datos As New List(Of Double)
    Private tipoGrafico As TipoGraficoControl = TipoGraficoControl.XR
    Private graficoGenerado As Boolean = False
    Private estadisticas As EstadisticasControl

    ' ===== ENUMERACIONES =====
    Public Enum TipoGraficoControl
        XR      ' X-barra y R (Media y Rango)
        XS      ' X-barra y S (Media y Desviación)
        XmR     ' X individual y Rango móvil
        P       ' Proporción de defectuosos
        NP      ' Número de defectuosos
        C       ' Número de defectos
        U       ' Defectos por unidad
    End Enum

    ' ===== CLASE ESTADÍSTICAS =====
    Public Class EstadisticasControl
        Public Property Media As Double
        Public Property DesviacionEstandar As Double
        Public Property Rango As Double
        Public Property LimiteControlSuperior As Double
        Public Property LimiteControlInferior As Double
        Public Property LineaCentral As Double
        Public Property Cp As Double
        Public Property Cpk As Double
        Public Property EspecificacionSuperior As Double
        Public Property EspecificacionInferior As Double

        Public Sub New()
            Media = 0
            DesviacionEstandar = 0
            Rango = 0
            LimiteControlSuperior = 0
            LimiteControlInferior = 0
            LineaCentral = 0
            Cp = 0
            Cpk = 0
            EspecificacionSuperior = 0
            EspecificacionInferior = 0
        End Sub
    End Class

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub FormGraficoControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Configuración inicial
        Me.Text = "Generador de Gráficos de Control - Ingeniería Industrial"
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
        panelPrincipal.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 320))
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
            .Text = "📊 GRÁFICOS DE CONTROL",
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(10, yPos)
        }
        panel.Controls.Add(lblTitulo)
        yPos += 40

        ' === TIPO DE GRÁFICO ===
        Dim gbTipo As New GroupBox With {
            .Text = "Tipo de Gráfico",
            .Size = New Size(290, 120),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbTipo)

        Dim cmbTipoGrafico As New ComboBox With {
            .Name = "cmbTipoGrafico",
            .Size = New Size(270, 25),
            .Location = New Point(10, 25),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbTipoGrafico.Items.AddRange({"X-R (Media y Rango)", "X-S (Media y Desviación)", "X-mR (Individual y Rango Móvil)", "p (Proporción defectuosos)", "np (Número defectuosos)", "c (Número de defectos)", "u (Defectos por unidad)"})
        cmbTipoGrafico.SelectedIndex = 0
        gbTipo.Controls.Add(cmbTipoGrafico)
        AddHandler cmbTipoGrafico.SelectedIndexChanged, AddressOf CambiarTipoGrafico

        Dim lblDescripcion As New Label With {
            .Name = "lblDescripcion",
            .Text = "Para variables continuas con subgrupos",
            .Size = New Size(270, 40),
            .Location = New Point(10, 55),
            .Font = New Font("Arial", 9),
            .ForeColor = Color.DarkGreen
        }
        gbTipo.Controls.Add(lblDescripcion)

        yPos += 140

        ' === ENTRADA DE DATOS ===
        Dim gbDatos As New GroupBox With {
            .Text = "Entrada de Datos",
            .Size = New Size(290, 150),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbDatos)

        Dim lblValor As New Label With {
            .Text = "Valor:",
            .Location = New Point(10, 25),
            .AutoSize = True
        }
        gbDatos.Controls.Add(lblValor)

        Dim txtValor As New TextBox With {
            .Name = "txtValor",
            .Size = New Size(200, 25),
            .Location = New Point(10, 45)
        }
        gbDatos.Controls.Add(txtValor)

        Dim btnAgregar As New Button With {
            .Text = "➕ Agregar",
            .Size = New Size(90, 30),
            .Location = New Point(10, 80),
            .BackColor = Color.LightGreen,
            .FlatStyle = FlatStyle.Flat
        }
        gbDatos.Controls.Add(btnAgregar)
        AddHandler btnAgregar.Click, AddressOf AgregarDato

        Dim btnEliminar As New Button With {
            .Text = "➖ Eliminar",
            .Size = New Size(90, 30),
            .Location = New Point(110, 80),
            .BackColor = Color.LightCoral,
            .FlatStyle = FlatStyle.Flat
        }
        gbDatos.Controls.Add(btnEliminar)
        AddHandler btnEliminar.Click, AddressOf EliminarDato

        Dim btnLimpiar As New Button With {
            .Text = "🗑️ Limpiar",
            .Size = New Size(90, 30),
            .Location = New Point(210, 80),
            .BackColor = Color.Orange,
            .FlatStyle = FlatStyle.Flat
        }
        gbDatos.Controls.Add(btnLimpiar)
        AddHandler btnLimpiar.Click, AddressOf LimpiarDatos

        Dim lblConteo As New Label With {
            .Name = "lblConteo",
            .Text = "Datos: 0",
            .Location = New Point(10, 120),
            .AutoSize = True,
            .Font = New Font("Arial", 9),
            .ForeColor = Color.DarkBlue
        }
        gbDatos.Controls.Add(lblConteo)

        yPos += 170

        ' === ESPECIFICACIONES ===
        Dim gbEspec As New GroupBox With {
            .Text = "Especificaciones (Opcional)",
            .Size = New Size(290, 100),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbEspec)

        Dim lblEspecSup As New Label With {
            .Text = "Límite Superior:",
            .Location = New Point(10, 25),
            .AutoSize = True
        }
        gbEspec.Controls.Add(lblEspecSup)

        Dim txtEspecSup As New TextBox With {
            .Name = "txtEspecSup",
            .Size = New Size(100, 25),
            .Location = New Point(120, 22)
        }
        gbEspec.Controls.Add(txtEspecSup)

        Dim lblEspecInf As New Label With {
            .Text = "Límite Inferior:",
            .Location = New Point(10, 55),
            .AutoSize = True
        }
        gbEspec.Controls.Add(lblEspecInf)

        Dim txtEspecInf As New TextBox With {
            .Name = "txtEspecInf",
            .Size = New Size(100, 25),
            .Location = New Point(120, 52)
        }
        gbEspec.Controls.Add(txtEspecInf)

        yPos += 120

        ' === CONTROLES DE GRÁFICO ===
        Dim gbGrafico As New GroupBox With {
            .Text = "Controles del Gráfico",
            .Size = New Size(290, 120),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbGrafico)

        Dim btnGenerar As New Button With {
            .Text = "📈 Generar Gráfico",
            .Size = New Size(270, 35),
            .Location = New Point(10, 25),
            .BackColor = Color.LightBlue,
            .FlatStyle = FlatStyle.Flat,
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbGrafico.Controls.Add(btnGenerar)
        AddHandler btnGenerar.Click, AddressOf GenerarGrafico

        Dim btnGuardar As New Button With {
            .Text = "💾 Guardar",
            .Size = New Size(85, 30),
            .Location = New Point(10, 70),
            .BackColor = Color.LightGreen,
            .FlatStyle = FlatStyle.Flat
        }
        gbGrafico.Controls.Add(btnGuardar)
        AddHandler btnGuardar.Click, AddressOf GuardarGrafico

        Dim btnExportar As New Button With {
            .Text = "📄 Exportar",
            .Size = New Size(85, 30),
            .Location = New Point(105, 70),
            .BackColor = Color.LightYellow,
            .FlatStyle = FlatStyle.Flat
        }
        gbGrafico.Controls.Add(btnExportar)
        AddHandler btnExportar.Click, AddressOf ExportarDatos

        Dim btnAyuda As New Button With {
            .Text = "❓ Ayuda",
            .Size = New Size(85, 30),
            .Location = New Point(200, 70),
            .BackColor = Color.LightCyan,
            .FlatStyle = FlatStyle.Flat
        }
        gbGrafico.Controls.Add(btnAyuda)
        AddHandler btnAyuda.Click, AddressOf MostrarAyuda

        yPos += 140

        ' === ESTADÍSTICAS ===
        Dim gbEstadisticas As New GroupBox With {
            .Text = "Estadísticas del Proceso",
            .Size = New Size(290, 200),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panel.Controls.Add(gbEstadisticas)

        Dim lblEstadisticas As New Label With {
            .Name = "lblEstadisticas",
            .Text = "Genere el gráfico para ver las estadísticas",
            .Size = New Size(270, 170),
            .Location = New Point(10, 25),
            .Font = New Font("Consolas", 8),
            .ForeColor = Color.DarkBlue
        }
        gbEstadisticas.Controls.Add(lblEstadisticas)
    End Sub

    ' ===== MÉTODOS DE GESTIÓN DE DATOS =====
    Private Sub AgregarDato(sender As Object, e As EventArgs)
        Dim txtValor As TextBox = DirectCast(Me.Controls.Find("txtValor", True).FirstOrDefault(), TextBox)

        Dim valor As Double
        If Not Double.TryParse(txtValor.Text, valor) Then
            MessageBox.Show("Por favor ingrese un valor numérico válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        datos.Add(valor)
        ActualizarConteo()
        txtValor.Clear()
        txtValor.Focus()
        graficoGenerado = False
    End Sub

    Private Sub EliminarDato(sender As Object, e As EventArgs)
        If datos.Count > 0 Then
            datos.RemoveAt(datos.Count - 1)
            ActualizarConteo()
            graficoGenerado = False
            Me.Controls.Find("panelGrafico", True).FirstOrDefault()?.Invalidate()
        Else
            MessageBox.Show("No hay datos para eliminar", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub LimpiarDatos(sender As Object, e As EventArgs)
        If MessageBox.Show("¿Está seguro de que desea limpiar todos los datos?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            datos.Clear()
            ActualizarConteo()
            graficoGenerado = False
            Me.Controls.Find("panelGrafico", True).FirstOrDefault()?.Invalidate()
        End If
    End Sub

    Private Sub ActualizarConteo()
        Dim lblConteo As Label = DirectCast(Me.Controls.Find("lblConteo", True).FirstOrDefault(), Label)
        lblConteo.Text = $"Datos: {datos.Count}"
    End Sub

    Private Sub CargarDatosEjemplo()
        ' Datos de ejemplo para gráfico X-R
        Dim datosEjemplo() As Double = {10.2, 10.1, 9.9, 10.3, 10.0, 9.8, 10.4, 10.1, 9.7, 10.2, 10.0, 9.9, 10.1, 10.3, 9.8, 10.0, 10.2, 9.9, 10.1, 10.0}
        datos.AddRange(datosEjemplo)
        ActualizarConteo()
    End Sub

    Private Sub CambiarTipoGrafico(sender As Object, e As EventArgs)
        Dim cmb As ComboBox = DirectCast(sender, ComboBox)
        tipoGrafico = DirectCast(cmb.SelectedIndex, TipoGraficoControl)
        
        Dim lblDescripcion As Label = DirectCast(Me.Controls.Find("lblDescripcion", True).FirstOrDefault(), Label)
        
        Select Case tipoGrafico
            Case TipoGraficoControl.XR
                lblDescripcion.Text = "Para variables continuas con subgrupos"
            Case TipoGraficoControl.XS
                lblDescripcion.Text = "Para variables continuas, subgrupos grandes"
            Case TipoGraficoControl.XmR
                lblDescripcion.Text = "Para mediciones individuales"
            Case TipoGraficoControl.P
                lblDescripcion.Text = "Para proporción de unidades defectuosas"
            Case TipoGraficoControl.NP
                lblDescripcion.Text = "Para número de unidades defectuosas"
            Case TipoGraficoControl.C
                lblDescripcion.Text = "Para número de defectos por unidad"
            Case TipoGraficoControl.U
                lblDescripcion.Text = "Para defectos por unidad variable"
        End Select
        
        graficoGenerado = False
    End Sub

    ' ===== GENERACIÓN DEL GRÁFICO =====
    Private Sub GenerarGrafico(sender As Object, e As EventArgs)
        If datos.Count < 5 Then
            MessageBox.Show("Se necesitan al menos 5 datos para generar el gráfico", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        CalcularEstadisticas()
        graficoGenerado = True
        Me.Controls.Find("panelGrafico", True).FirstOrDefault()?.Invalidate()
        ActualizarEstadisticas()
    End Sub

    Private Sub CalcularEstadisticas()
        estadisticas = New EstadisticasControl()

        ' Obtener especificaciones si están definidas
        Dim txtEspecSup As TextBox = DirectCast(Me.Controls.Find("txtEspecSup", True).FirstOrDefault(), TextBox)
        Dim txtEspecInf As TextBox = DirectCast(Me.Controls.Find("txtEspecInf", True).FirstOrDefault(), TextBox)
        
        Double.TryParse(txtEspecSup.Text, estadisticas.EspecificacionSuperior)
        Double.TryParse(txtEspecInf.Text, estadisticas.EspecificacionInferior)

        Select Case tipoGrafico
            Case TipoGraficoControl.XR, TipoGraficoControl.XS, TipoGraficoControl.XmR
                CalcularEstadisticasVariables()
            Case TipoGraficoControl.P, TipoGraficoControl.NP
                CalcularEstadisticasAtributos()
            Case TipoGraficoControl.C, TipoGraficoControl.U
                CalcularEstadisticasDefectos()
        End Select
    End Sub

    Private Sub CalcularEstadisticasVariables()
        ' Calcular estadísticas básicas
        estadisticas.Media = datos.Average()
        estadisticas.DesviacionEstandar = Math.Sqrt(datos.Sum(Function(x) Math.Pow(x - estadisticas.Media, 2)) / (datos.Count - 1))
        estadisticas.Rango = datos.Max() - datos.Min()
        estadisticas.LineaCentral = estadisticas.Media

        ' Constantes para gráficos de control (para n=1 en X-mR)
        Dim A2 As Double = 2.66  ' Para n=1 (X-mR)
        Dim D3 As Double = 0     ' Para n=1
        Dim D4 As Double = 3.27  ' Para n=1

        Select Case tipoGrafico
            Case TipoGraficoControl.XR
                ' Límites para gráfico X-barra
                estadisticas.LimiteControlSuperior = estadisticas.Media + A2 * estadisticas.DesviacionEstandar
                estadisticas.LimiteControlInferior = estadisticas.Media - A2 * estadisticas.DesviacionEstandar
                
            Case TipoGraficoControl.XS
                ' Similar a X-R pero usando desviación estándar
                estadisticas.LimiteControlSuperior = estadisticas.Media + 3 * estadisticas.DesviacionEstandar
                estadisticas.LimiteControlInferior = estadisticas.Media - 3 * estadisticas.DesviacionEstandar
                
            Case TipoGraficoControl.XmR
                ' Límites para gráfico X individual
                Dim rangoMovilPromedio As Double = CalcularRangoMovilPromedio()
                estadisticas.LimiteControlSuperior = estadisticas.Media + 2.66 * rangoMovilPromedio
                estadisticas.LimiteControlInferior = estadisticas.Media - 2.66 * rangoMovilPromedio
        End Select

        ' Calcular índices de capacidad si hay especificaciones
        If estadisticas.EspecificacionSuperior > 0 AndAlso estadisticas.EspecificacionInferior > 0 Then
            Dim tolerancia As Double = estadisticas.EspecificacionSuperior - estadisticas.EspecificacionInferior
            estadisticas.Cp = tolerancia / (6 * estadisticas.DesviacionEstandar)
            
            Dim cpkSup As Double = (estadisticas.EspecificacionSuperior - estadisticas.Media) / (3 * estadisticas.DesviacionEstandar)
            Dim cpkInf As Double = (estadisticas.Media - estadisticas.EspecificacionInferior) / (3 * estadisticas.DesviacionEstandar)
            estadisticas.Cpk = Math.Min(cpkSup, cpkInf)
        End If
    End Sub

    Private Function CalcularRangoMovilPromedio() As Double
        If datos.Count < 2 Then Return 0
        
        Dim rangosMoviles As New List(Of Double)
        For i As Integer = 1 To datos.Count - 1
            rangosMoviles.Add(Math.Abs(datos(i) - datos(i - 1)))
        Next
        
        Return rangosMoviles.Average()
    End Function

    Private Sub CalcularEstadisticasAtributos()
        ' Para gráficos p y np (simplificado)
        estadisticas.Media = datos.Average()
        estadisticas.LineaCentral = estadisticas.Media
        estadisticas.DesviacionEstandar = Math.Sqrt(estadisticas.Media * (1 - estadisticas.Media) / datos.Count)
        estadisticas.LimiteControlSuperior = estadisticas.Media + 3 * estadisticas.DesviacionEstandar
        estadisticas.LimiteControlInferior = Math.Max(0, estadisticas.Media - 3 * estadisticas.DesviacionEstandar)
    End Sub

    Private Sub CalcularEstadisticasDefectos()
        ' Para gráficos c y u (simplificado)
        estadisticas.Media = datos.Average()
        estadisticas.LineaCentral = estadisticas.Media
        estadisticas.DesviacionEstandar = Math.Sqrt(estadisticas.Media)
        estadisticas.LimiteControlSuperior = estadisticas.Media + 3 * estadisticas.DesviacionEstandar
        estadisticas.LimiteControlInferior = Math.Max(0, estadisticas.Media - 3 * estadisticas.DesviacionEstandar)
    End Sub

    ' ===== DIBUJO DEL GRÁFICO =====
    Private Sub PanelGrafico_Paint(sender As Object, e As PaintEventArgs)
        If Not graficoGenerado OrElse datos.Count = 0 Then
            DibujarMensajeInicial(e.Graphics, DirectCast(sender, Panel))
            Return
        End If

        DibujarGraficoControl(e.Graphics, DirectCast(sender, Panel))
    End Sub

    Private Sub DibujarMensajeInicial(g As Graphics, panel As Panel)
        Dim mensaje As String = "Agregue datos y haga clic en 'Generar Gráfico' para visualizar el control estadístico"
        Dim font As New Font("Arial", 14, FontStyle.Italic)
        Dim brush As New SolidBrush(Color.Gray)
        Dim rect As New Rectangle(50, panel.Height \ 2 - 20, panel.Width - 100, 40)
        Dim format As New StringFormat With {
            .Alignment = StringAlignment.Center,
            .LineAlignment = StringAlignment.Center
        }
        
        g.DrawString(mensaje, font, brush, rect, format)
    End Sub

    Private Sub DibujarGraficoControl(g As Graphics, panel As Panel)
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Márgenes
        Dim margenIzq As Integer = 80
        Dim margenDer As Integer = 50
        Dim margenSup As Integer = 60
        Dim margenInf As Integer = 80

        ' Área del gráfico
        Dim areaGrafico As New Rectangle(margenIzq, margenSup, panel.Width - margenIzq - margenDer, panel.Height - margenSup - margenInf)

        ' Dibujar título
        Dim fontTitulo As New Font("Arial", 16, FontStyle.Bold)
        Dim titulo As String = $"Gráfico de Control {tipoGrafico}"
        g.DrawString(titulo, fontTitulo, Brushes.Black, New Point(panel.Width \ 2 - 100, 20))

        ' Dibujar ejes
        g.DrawLine(Pens.Black, areaGrafico.Left, areaGrafico.Bottom, areaGrafico.Right, areaGrafico.Bottom) ' Eje X
        g.DrawLine(Pens.Black, areaGrafico.Left, areaGrafico.Bottom, areaGrafico.Left, areaGrafico.Top) ' Eje Y

        ' Calcular escalas
        Dim valorMin As Double = Math.Min(datos.Min(), estadisticas.LimiteControlInferior) - 0.5
        Dim valorMax As Double = Math.Max(datos.Max(), estadisticas.LimiteControlSuperior) + 0.5
        Dim rangoValores As Double = valorMax - valorMin

        ' Dibujar líneas de control
        DibujarLineasControl(g, areaGrafico, valorMin, rangoValores)

        ' Dibujar puntos de datos
        DibujarPuntosDatos(g, areaGrafico, valorMin, rangoValores)

        ' Dibujar escalas
        DibujarEscalas(g, areaGrafico, valorMin, valorMax)

        ' Dibujar leyenda
        DibujarLeyenda(g, panel)
    End Sub

    Private Sub DibujarLineasControl(g As Graphics, areaGrafico As Rectangle, valorMin As Double, rangoValores As Double)
        ' Línea central
        Dim yLC As Integer = areaGrafico.Bottom - CInt(((estadisticas.LineaCentral - valorMin) / rangoValores) * areaGrafico.Height)
        g.DrawLine(New Pen(Color.Blue, 2), areaGrafico.Left, yLC, areaGrafico.Right, yLC)
        g.DrawString("LC", New Font("Arial", 8), Brushes.Blue, areaGrafico.Right + 5, yLC - 8)

        ' Límite superior de control
        Dim yLSC As Integer = areaGrafico.Bottom - CInt(((estadisticas.LimiteControlSuperior - valorMin) / rangoValores) * areaGrafico.Height)
        Dim penRojoLSC As New Pen(Color.Red, 2)
        penRojoLSC.DashStyle = DashStyle.Dash
        g.DrawLine(penRojoLSC, areaGrafico.Left, yLSC, areaGrafico.Right, yLSC)
        g.DrawString("LSC", New Font("Arial", 8), Brushes.Red, areaGrafico.Right + 5, yLSC - 8)

        ' Límite inferior de control
        Dim yLIC As Integer = areaGrafico.Bottom - CInt(((estadisticas.LimiteControlInferior - valorMin) / rangoValores) * areaGrafico.Height)
        Dim penRojoLIC As New Pen(Color.Red, 2)
        penRojoLIC.DashStyle = DashStyle.Dash
        g.DrawLine(penRojoLIC, areaGrafico.Left, yLIC, areaGrafico.Right, yLIC)
        g.DrawString("LIC", New Font("Arial", 8), Brushes.Red, areaGrafico.Right + 5, yLIC - 8)

        ' Líneas de especificación si están definidas
        If estadisticas.EspecificacionSuperior > 0 Then
            Dim yEspecSup As Integer = areaGrafico.Bottom - CInt(((estadisticas.EspecificacionSuperior - valorMin) / rangoValores) * areaGrafico.Height)
            Dim penVerdeES As New Pen(Color.Green, 2)
            penVerdeES.DashStyle = DashStyle.Dot
            g.DrawLine(penVerdeES, areaGrafico.Left, yEspecSup, areaGrafico.Right, yEspecSup)
            g.DrawString("ES", New Font("Arial", 8), Brushes.Green, areaGrafico.Right + 5, yEspecSup - 8)
        End If

        If estadisticas.EspecificacionInferior > 0 Then
            Dim yEspecInf As Integer = areaGrafico.Bottom - CInt(((estadisticas.EspecificacionInferior - valorMin) / rangoValores) * areaGrafico.Height)
            Dim penVerdeEI As New Pen(Color.Green, 2)
            penVerdeEI.DashStyle = DashStyle.Dot
            g.DrawLine(penVerdeEI, areaGrafico.Left, yEspecInf, areaGrafico.Right, yEspecInf)
            g.DrawString("EI", New Font("Arial", 8), Brushes.Green, areaGrafico.Right + 5, yEspecInf - 8)
        End If
    End Sub

    Private Sub DibujarPuntosDatos(g As Graphics, areaGrafico As Rectangle, valorMin As Double, rangoValores As Double)
        If datos.Count = 0 Then Return

        Dim anchoSegmento As Double = areaGrafico.Width / (datos.Count - 1)
        Dim puntos As New List(Of Point)

        For i As Integer = 0 To datos.Count - 1
            Dim x As Integer = areaGrafico.Left + CInt(i * anchoSegmento)
            Dim y As Integer = areaGrafico.Bottom - CInt(((datos(i) - valorMin) / rangoValores) * areaGrafico.Height)
            puntos.Add(New Point(x, y))

            ' Determinar color del punto
            Dim colorPunto As Color = Color.Blue
            If datos(i) > estadisticas.LimiteControlSuperior OrElse datos(i) < estadisticas.LimiteControlInferior Then
                colorPunto = Color.Red ' Fuera de control
            ElseIf estadisticas.EspecificacionSuperior > 0 AndAlso estadisticas.EspecificacionInferior > 0 Then
                If datos(i) > estadisticas.EspecificacionSuperior OrElse datos(i) < estadisticas.EspecificacionInferior Then
                    colorPunto = Color.Orange ' Fuera de especificación
                End If
            End If

            ' Dibujar punto
            g.FillEllipse(New SolidBrush(colorPunto), x - 3, y - 3, 6, 6)
            g.DrawEllipse(Pens.Black, x - 3, y - 3, 6, 6)

            ' Etiqueta del punto
            g.DrawString((i + 1).ToString(), New Font("Arial", 7), Brushes.Black, x - 5, areaGrafico.Bottom + 5)
        Next

        ' Conectar puntos con líneas
        If puntos.Count > 1 Then
            g.DrawLines(New Pen(Color.Blue, 1), puntos.ToArray())
        End If
    End Sub

    Private Sub DibujarEscalas(g As Graphics, areaGrafico As Rectangle, valorMin As Double, valorMax As Double)
        Dim fontEscala As New Font("Arial", 8)
        
        ' Escala Y
        For i As Integer = 0 To 5
            Dim valor As Double = valorMin + ((valorMax - valorMin) / 5) * i
            Dim y As Integer = areaGrafico.Bottom - CInt((i / 5) * areaGrafico.Height)
            g.DrawString(valor.ToString("N2"), fontEscala, Brushes.Black, 5, y - 8)
            g.DrawLine(Pens.Gray, areaGrafico.Left - 5, y, areaGrafico.Left, y)
        Next

        ' Etiqueta del eje Y
        g.DrawString("Valores", New Font("Arial", 10, FontStyle.Bold), Brushes.Black, 10, areaGrafico.Top - 20)
        g.DrawString("Muestra", New Font("Arial", 10, FontStyle.Bold), Brushes.Black, areaGrafico.Left + areaGrafico.Width \ 2 - 30, areaGrafico.Bottom + 40)
    End Sub

    Private Sub DibujarLeyenda(g As Graphics, panel As Panel)
        Dim x As Integer = 20
        Dim y As Integer = panel.Height - 60
        Dim fontLeyenda As New Font("Arial", 9)

        ' Puntos
        g.FillEllipse(Brushes.Blue, x, y, 10, 10)
        g.DrawString("En control", fontLeyenda, Brushes.Black, x + 15, y - 2)

        g.FillEllipse(Brushes.Red, x + 100, y, 10, 10)
        g.DrawString("Fuera de control", fontLeyenda, Brushes.Black, x + 115, y - 2)

        g.FillEllipse(Brushes.Orange, x + 230, y, 10, 10)
        g.DrawString("Fuera de especificación", fontLeyenda, Brushes.Black, x + 245, y - 2)

        ' Líneas
        g.DrawLine(New Pen(Color.Blue, 2), x, y + 20, x + 15, y + 20)
        g.DrawString("LC (Línea Central)", fontLeyenda, Brushes.Black, x + 20, y + 18)

        Dim penRojoLeyenda As New Pen(Color.Red, 2)
        penRojoLeyenda.DashStyle = DashStyle.Dash
        g.DrawLine(penRojoLeyenda, x + 150, y + 20, x + 165, y + 20)
        g.DrawString("LSC/LIC (Límites Control)", fontLeyenda, Brushes.Black, x + 170, y + 18)

        Dim penVerdeLeyenda As New Pen(Color.Green, 2)
        penVerdeLeyenda.DashStyle = DashStyle.Dot
        g.DrawLine(penVerdeLeyenda, x + 350, y + 20, x + 365, y + 20)
        g.DrawString("ES/EI (Especificaciones)", fontLeyenda, Brushes.Black, x + 370, y + 18)
    End Sub

    ' ===== ACTUALIZAR ESTADÍSTICAS =====
    Private Sub ActualizarEstadisticas()
        Dim lblEstadisticas As Label = DirectCast(Me.Controls.Find("lblEstadisticas", True).FirstOrDefault(), Label)
        
        If estadisticas Is Nothing Then Return

        Dim texto As String = "ESTADÍSTICAS DEL PROCESO:" & vbCrLf & vbCrLf &
                             $"Media (X̄): {estadisticas.Media:N3}" & vbCrLf &
                             $"Desv. Estándar (σ): {estadisticas.DesviacionEstandar:N3}" & vbCrLf &
                             $"Rango (R): {estadisticas.Rango:N3}" & vbCrLf & vbCrLf &
                             "LÍMITES DE CONTROL:" & vbCrLf &
                             $"LSC: {estadisticas.LimiteControlSuperior:N3}" & vbCrLf &
                             $"LC:  {estadisticas.LineaCentral:N3}" & vbCrLf &
                             $"LIC: {estadisticas.LimiteControlInferior:N3}" & vbCrLf

        If estadisticas.Cp > 0 Then
            texto &= vbCrLf & "ÍNDICES DE CAPACIDAD:" & vbCrLf &
                    $"Cp:  {estadisticas.Cp:N3}" & vbCrLf &
                    $"Cpk: {estadisticas.Cpk:N3}" & vbCrLf

            ' Interpretación de Cp
            If estadisticas.Cp >= 1.33 Then
                texto &= "Proceso CAPAZ" & vbCrLf
            ElseIf estadisticas.Cp >= 1.0 Then
                texto &= "Proceso MARGINAL" & vbCrLf
            Else
                texto &= "Proceso NO CAPAZ" & vbCrLf
            End If
        End If

        ' Análisis de puntos fuera de control
        Dim puntosFC As Integer = 0
        For Each valor As Double In datos
            If valor > estadisticas.LimiteControlSuperior OrElse valor < estadisticas.LimiteControlInferior Then
                puntosFC += 1
            End If
        Next

        texto &= vbCrLf & $"Puntos fuera de control: {puntosFC}/{datos.Count}"

        lblEstadisticas.Text = texto
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
                    .Title = "Guardar Gráfico de Control",
                    .FileName = "GraficoControl_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".png"
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
        If datos.Count = 0 Then
            MessageBox.Show("No hay datos para exportar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim saveDialog As New SaveFileDialog With {
                .Filter = "CSV File|*.csv",
                .Title = "Exportar Datos de Control",
                .FileName = "DatosControl_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".csv"
            }
            
            If saveDialog.ShowDialog() = DialogResult.OK Then
                Using writer As New StreamWriter(saveDialog.FileName)
                    writer.WriteLine("Muestra,Valor,Estado")
                    For i As Integer = 0 To datos.Count - 1
                        Dim estado As String = "En Control"
                        If datos(i) > estadisticas.LimiteControlSuperior OrElse datos(i) < estadisticas.LimiteControlInferior Then
                            estado = "Fuera de Control"
                        End If
                        writer.WriteLine($"{i + 1},{datos(i)},{estado}")
                    Next
                    
                    ' Agregar estadísticas
                    writer.WriteLine()
                    writer.WriteLine("ESTADÍSTICAS")
                    writer.WriteLine($"Media,{estadisticas.Media}")
                    writer.WriteLine($"Desviación Estándar,{estadisticas.DesviacionEstandar}")
                    writer.WriteLine($"LSC,{estadisticas.LimiteControlSuperior}")
                    writer.WriteLine($"LC,{estadisticas.LineaCentral}")
                    writer.WriteLine($"LIC,{estadisticas.LimiteControlInferior}")
                    If estadisticas.Cp > 0 Then
                        writer.WriteLine($"Cp,{estadisticas.Cp}")
                        writer.WriteLine($"Cpk,{estadisticas.Cpk}")
                    End If
                End Using
                MessageBox.Show("Datos exportados exitosamente a:" & vbCrLf & saveDialog.FileName, "Exportado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Error al exportar los datos:" & vbCrLf & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MostrarAyuda(sender As Object, e As EventArgs)
        MessageBox.Show("GENERADOR DE GRÁFICOS DE CONTROL" & vbCrLf & vbCrLf &
                      "CONTROL ESTADÍSTICO DE PROCESOS (SPC):" & vbCrLf &
                      "Herramienta para monitorear la estabilidad de procesos" & vbCrLf & vbCrLf &
                      "TIPOS DE GRÁFICOS:" & vbCrLf &
                      "• X-R: Media y rango para variables continuas" & vbCrLf &
                      "• X-S: Media y desviación para subgrupos grandes" & vbCrLf &
                      "• X-mR: Valores individuales y rango móvil" & vbCrLf &
                      "• p: Proporción de defectuosos" & vbCrLf &
                      "• np: Número de defectuosos" & vbCrLf &
                      "• c: Número de defectos" & vbCrLf &
                      "• u: Defectos por unidad" & vbCrLf & vbCrLf &
                      "INTERPRETACIÓN:" & vbCrLf &
                      "• Puntos azules: Proceso en control" & vbCrLf &
                      "• Puntos rojos: Fuera de límites de control" & vbCrLf &
                      "• Puntos naranjas: Fuera de especificación" & vbCrLf &
                      "• LSC/LIC: Límites de control (±3σ)" & vbCrLf &
                      "• ES/EI: Especificaciones del cliente" & vbCrLf & vbCrLf &
                      "ÍNDICES DE CAPACIDAD:" & vbCrLf &
                      "• Cp ≥ 1.33: Proceso capaz" & vbCrLf &
                      "• Cp 1.0-1.33: Proceso marginal" & vbCrLf &
                      "• Cp < 1.0: Proceso no capaz" & vbCrLf &
                      "• Cpk: Considera centrado del proceso",
                      "Ayuda - Gráficos de Control", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
