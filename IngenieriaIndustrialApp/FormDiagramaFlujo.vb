' =====================================================
' GENERADOR DE DIAGRAMAS DE FLUJO
' Herramienta para crear diagramas de proceso
' =====================================================
' Autor: Repositorio Ingeniería Industrial
' Fecha: 2025
' Descripción: Interfaz gráfica para crear diagramas
'              de flujo con símbolos estándar ANSI
' =====================================================

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Public Class FormDiagramaFlujo

    ' ===== VARIABLES GLOBALES =====
    Private elementos As New List(Of ElementoDiagrama)
    Private elementoSeleccionado As ElementoDiagrama = Nothing
    Private tipoElementoActual As TipoElemento = TipoElemento.Proceso
    Private arrastrando As Boolean = False
    Private puntoInicial As Point
    Private contadorElementos As Integer = 0

    ' ===== ENUMERACIONES =====
    Public Enum TipoElemento
        Inicio
        Proceso
        Decision
        Documento
        Almacenamiento
        Conector
        Fin
    End Enum

    ' ===== CLASE ELEMENTO DIAGRAMA =====
    Public Class ElementoDiagrama
        Public Property ID As Integer
        Public Property Tipo As TipoElemento
        Public Property Posicion As Point
        Public Property Tamaño As Size
        Public Property Texto As String
        Public Property Color As Color
        Public Property Seleccionado As Boolean

        Public Sub New(id As Integer, tipo As TipoElemento, posicion As Point, texto As String)
            Me.ID = id
            Me.Tipo = tipo
            Me.Posicion = posicion
            Me.Texto = texto
            Me.Tamaño = New Size(120, 60)
            Me.Color = ObtenerColorPorTipo(tipo)
            Me.Seleccionado = False
        End Sub

        Private Function ObtenerColorPorTipo(tipo As TipoElemento) As Color
            Select Case tipo
                Case TipoElemento.Inicio
                    Return Color.LightGreen
                Case TipoElemento.Proceso
                    Return Color.LightBlue
                Case TipoElemento.Decision
                    Return Color.LightYellow
                Case TipoElemento.Documento
                    Return Color.LightCyan
                Case TipoElemento.Almacenamiento
                    Return Color.LightPink
                Case TipoElemento.Conector
                    Return Color.LightGray
                Case TipoElemento.Fin
                    Return Color.LightCoral
                Case Else
                    Return Color.White
            End Select
        End Function
    End Class

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub FormDiagramaFlujo_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Configuración inicial
        Me.Text = "Generador de Diagramas de Flujo - Ingeniería Industrial"
        Me.Size = New Size(1200, 800)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 248, 255)

        ' Inicializar interfaz
        InicializarInterfaz()
    End Sub

    ' ===== INICIALIZACIÓN DE LA INTERFAZ =====
    Private Sub InicializarInterfaz()
        ' Panel principal
        Dim panelPrincipal As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1
        }
        panelPrincipal.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 200))
        panelPrincipal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        Me.Controls.Add(panelPrincipal)

        ' === PANEL DE HERRAMIENTAS ===
        Dim panelHerramientas As New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(230, 230, 250),
            .Padding = New Padding(10)
        }
        panelPrincipal.Controls.Add(panelHerramientas, 0, 0)

        ' Título del panel
        Dim lblTitulo As New Label With {
            .Text = "🛠️ HERRAMIENTAS",
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(10, 10)
        }
        panelHerramientas.Controls.Add(lblTitulo)

        ' Botones de elementos
        CrearBotonesElementos(panelHerramientas)

        ' Separador
        Dim separador As New Panel With {
            .Size = New Size(180, 2),
            .Location = New Point(10, 320),
            .BackColor = Color.DarkBlue
        }
        panelHerramientas.Controls.Add(separador)

        ' Controles de acción
        CrearControlesAccion(panelHerramientas)

        ' === ÁREA DE DIBUJO ===
        Dim panelDibujo As New Panel With {
            .Name = "panelDibujo",
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .BorderStyle = BorderStyle.FixedSingle
        }
        panelPrincipal.Controls.Add(panelDibujo, 1, 0)

        ' Configurar eventos del área de dibujo
        AddHandler panelDibujo.Paint, AddressOf PanelDibujo_Paint
        AddHandler panelDibujo.MouseDown, AddressOf PanelDibujo_MouseDown
        AddHandler panelDibujo.MouseMove, AddressOf PanelDibujo_MouseMove
        AddHandler panelDibujo.MouseUp, AddressOf PanelDibujo_MouseUp
    End Sub

    ' ===== CREAR BOTONES DE ELEMENTOS =====
    Private Sub CrearBotonesElementos(panel As Panel)
        Dim yPos As Integer = 50

        ' Botón Inicio/Fin
        Dim btnInicio As New Button With {
            .Name = "btnInicio",
            .Text = "⭕ Inicio/Fin",
            .Size = New Size(180, 30),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightGreen,
            .FlatStyle = FlatStyle.Flat,
            .Tag = TipoElemento.Inicio
        }
        panel.Controls.Add(btnInicio)
        AddHandler btnInicio.Click, AddressOf SeleccionarTipoElemento

        yPos += 35

        ' Botón Proceso
        Dim btnProceso As New Button With {
            .Name = "btnProceso",
            .Text = "⬜ Proceso",
            .Size = New Size(180, 30),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightBlue,
            .FlatStyle = FlatStyle.Flat,
            .Tag = TipoElemento.Proceso
        }
        panel.Controls.Add(btnProceso)
        AddHandler btnProceso.Click, AddressOf SeleccionarTipoElemento

        yPos += 35

        ' Botón Decisión
        Dim btnDecision As New Button With {
            .Name = "btnDecision",
            .Text = "🔶 Decisión",
            .Size = New Size(180, 30),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightYellow,
            .FlatStyle = FlatStyle.Flat,
            .Tag = TipoElemento.Decision
        }
        panel.Controls.Add(btnDecision)
        AddHandler btnDecision.Click, AddressOf SeleccionarTipoElemento

        yPos += 35

        ' Botón Documento
        Dim btnDocumento As New Button With {
            .Name = "btnDocumento",
            .Text = "📄 Documento",
            .Size = New Size(180, 30),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightCyan,
            .FlatStyle = FlatStyle.Flat,
            .Tag = TipoElemento.Documento
        }
        panel.Controls.Add(btnDocumento)
        AddHandler btnDocumento.Click, AddressOf SeleccionarTipoElemento

        yPos += 35

        ' Botón Almacenamiento
        Dim btnAlmacenamiento As New Button With {
            .Name = "btnAlmacenamiento",
            .Text = "🗃️ Almacén",
            .Size = New Size(180, 30),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightPink,
            .FlatStyle = FlatStyle.Flat,
            .Tag = TipoElemento.Almacenamiento
        }
        panel.Controls.Add(btnAlmacenamiento)
        AddHandler btnAlmacenamiento.Click, AddressOf SeleccionarTipoElemento

        yPos += 35

        ' Botón Conector
        Dim btnConector As New Button With {
            .Name = "btnConector",
            .Text = "⚪ Conector",
            .Size = New Size(180, 30),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightGray,
            .FlatStyle = FlatStyle.Flat,
            .Tag = TipoElemento.Conector
        }
        panel.Controls.Add(btnConector)
        AddHandler btnConector.Click, AddressOf SeleccionarTipoElemento
    End Sub

    ' ===== CREAR CONTROLES DE ACCIÓN =====
    Private Sub CrearControlesAccion(panel As Panel)
        Dim yPos As Integer = 340

        ' Botón Limpiar
        Dim btnLimpiar As New Button With {
            .Text = "🗑️ Limpiar Todo",
            .Size = New Size(180, 35),
            .Location = New Point(10, yPos),
            .BackColor = Color.Orange,
            .FlatStyle = FlatStyle.Flat
        }
        panel.Controls.Add(btnLimpiar)
        AddHandler btnLimpiar.Click, AddressOf LimpiarDiagrama

        yPos += 40

        ' Botón Guardar
        Dim btnGuardar As New Button With {
            .Text = "💾 Guardar PNG",
            .Size = New Size(180, 35),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightGreen,
            .FlatStyle = FlatStyle.Flat
        }
        panel.Controls.Add(btnGuardar)
        AddHandler btnGuardar.Click, AddressOf GuardarDiagrama

        yPos += 40

        ' Botón Ayuda
        Dim btnAyuda As New Button With {
            .Text = "❓ Ayuda",
            .Size = New Size(180, 35),
            .Location = New Point(10, yPos),
            .BackColor = Color.LightBlue,
            .FlatStyle = FlatStyle.Flat
        }
        panel.Controls.Add(btnAyuda)
        AddHandler btnAyuda.Click, AddressOf MostrarAyuda

        yPos += 50

        ' Información del elemento seleccionado
        Dim lblInfo As New Label With {
            .Name = "lblInfo",
            .Text = "Seleccione un tipo de elemento y haga clic en el área de dibujo",
            .Size = New Size(180, 60),
            .Location = New Point(10, yPos),
            .Font = New Font("Arial", 8),
            .ForeColor = Color.DarkBlue
        }
        panel.Controls.Add(lblInfo)
    End Sub

    ' ===== EVENTOS DE SELECCIÓN =====
    Private Sub SeleccionarTipoElemento(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        tipoElementoActual = DirectCast(btn.Tag, TipoElemento)

        ' Actualizar información
        Dim lblInfo As Label = DirectCast(Me.Controls.Find("lblInfo", True).FirstOrDefault(), Label)
        If lblInfo IsNot Nothing Then
            lblInfo.Text = $"Elemento seleccionado: {tipoElementoActual}" & vbCrLf & "Haga clic en el área para agregar"
        End If

        ' Resaltar botón seleccionado
        For Each control As Control In btn.Parent.Controls
            If TypeOf control Is Button AndAlso control.Tag IsNot Nothing Then
                Dim tipoElemento As TipoElemento = DirectCast(control.Tag, TipoElemento)
                control.BackColor = ObtenerColorPorTipo(tipoElemento)
            End If
        Next
        btn.BackColor = Color.Yellow
    End Sub

    ' ===== EVENTOS DEL ÁREA DE DIBUJO =====
    Private Sub PanelDibujo_MouseDown(sender As Object, e As MouseEventArgs)
        Dim panel As Panel = DirectCast(sender, Panel)

        If e.Button = MouseButtons.Left Then
            ' Verificar si se hizo clic en un elemento existente
            elementoSeleccionado = Nothing
            For Each elemento As ElementoDiagrama In elementos
                Dim rect As New Rectangle(elemento.Posicion, elemento.Tamaño)
                If rect.Contains(e.Location) Then
                    elementoSeleccionado = elemento
                    arrastrando = True
                    puntoInicial = e.Location
                    Exit For
                End If
            Next

            ' Si no se seleccionó ningún elemento, crear uno nuevo
            If elementoSeleccionado Is Nothing Then
                AgregarElemento(e.Location)
            End If

            panel.Invalidate()
        ElseIf e.Button = MouseButtons.Right Then
            ' Menú contextual para editar texto
            Dim elementoEnPosicion As ElementoDiagrama = Nothing
            For Each elemento As ElementoDiagrama In elementos
                Dim rect As New Rectangle(elemento.Posicion, elemento.Tamaño)
                If rect.Contains(e.Location) Then
                    elementoEnPosicion = elemento
                    Exit For
                End If
            Next

            If elementoEnPosicion IsNot Nothing Then
                EditarTextoElemento(elementoEnPosicion)
                panel.Invalidate()
            End If
        End If
    End Sub

    Private Sub PanelDibujo_MouseMove(sender As Object, e As MouseEventArgs)
        If arrastrando AndAlso elementoSeleccionado IsNot Nothing Then
            Dim deltaX As Integer = e.X - puntoInicial.X
            Dim deltaY As Integer = e.Y - puntoInicial.Y
            elementoSeleccionado.Posicion = New Point(elementoSeleccionado.Posicion.X + deltaX, elementoSeleccionado.Posicion.Y + deltaY)
            puntoInicial = e.Location
            DirectCast(sender, Panel).Invalidate()
        End If
    End Sub

    Private Sub PanelDibujo_MouseUp(sender As Object, e As MouseEventArgs)
        arrastrando = False
    End Sub

    ' ===== MÉTODOS DE DIBUJO =====
    Private Sub PanelDibujo_Paint(sender As Object, e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Dibujar cada elemento
        For Each elemento As ElementoDiagrama In elementos
            DibujarElemento(g, elemento)
        Next
    End Sub

    Private Sub DibujarElemento(g As Graphics, elemento As ElementoDiagrama)
        Dim rect As New Rectangle(elemento.Posicion, elemento.Tamaño)
        Dim brush As New SolidBrush(elemento.Color)
        Dim pen As New Pen(If(elemento.Seleccionado, Color.Red, Color.Black), 2)

        Select Case elemento.Tipo
            Case TipoElemento.Inicio, TipoElemento.Fin
                ' Óvalo
                g.FillEllipse(brush, rect)
                g.DrawEllipse(pen, rect)

            Case TipoElemento.Proceso
                ' Rectángulo
                g.FillRectangle(brush, rect)
                g.DrawRectangle(pen, rect)

            Case TipoElemento.Decision
                ' Rombo
                Dim puntos() As Point = {
                    New Point(rect.X + rect.Width \ 2, rect.Y),
                    New Point(rect.X + rect.Width, rect.Y + rect.Height \ 2),
                    New Point(rect.X + rect.Width \ 2, rect.Y + rect.Height),
                    New Point(rect.X, rect.Y + rect.Height \ 2)
                }
                g.FillPolygon(brush, puntos)
                g.DrawPolygon(pen, puntos)

            Case TipoElemento.Documento
                ' Rectángulo con ondas en la parte inferior
                Dim path As New GraphicsPath()
                path.AddLine(rect.X, rect.Y, rect.X + rect.Width, rect.Y)
                path.AddLine(rect.X + rect.Width, rect.Y, rect.X + rect.Width, rect.Y + rect.Height - 10)
                
                ' Crear ondas
                For i As Integer = 0 To rect.Width Step 20
                    path.AddCurve({New Point(rect.X + i, rect.Y + rect.Height - 10),
                                  New Point(rect.X + i + 10, rect.Y + rect.Height),
                                  New Point(rect.X + i + 20, rect.Y + rect.Height - 10)})
                Next
                
                path.AddLine(rect.X, rect.Y + rect.Height - 10, rect.X, rect.Y)
                g.FillPath(brush, path)
                g.DrawPath(pen, path)

            Case TipoElemento.Almacenamiento
                ' Rectángulo con líneas verticales
                g.FillRectangle(brush, rect)
                g.DrawRectangle(pen, rect)
                g.DrawLine(pen, rect.X + 10, rect.Y, rect.X + 10, rect.Y + rect.Height)
                g.DrawLine(pen, rect.X + rect.Width - 10, rect.Y, rect.X + rect.Width - 10, rect.Y + rect.Height)

            Case TipoElemento.Conector
                ' Círculo pequeño
                Dim circleRect As New Rectangle(rect.X + 30, rect.Y + 15, 30, 30)
                g.FillEllipse(brush, circleRect)
                g.DrawEllipse(pen, circleRect)
        End Select

        ' Dibujar texto
        If Not String.IsNullOrEmpty(elemento.Texto) Then
            Dim font As New Font("Arial", 8, FontStyle.Bold)
            Dim textBrush As New SolidBrush(Color.Black)
            Dim format As New StringFormat With {
                .Alignment = StringAlignment.Center,
                .LineAlignment = StringAlignment.Center
            }
            g.DrawString(elemento.Texto, font, textBrush, rect, format)
        End If

        brush.Dispose()
        pen.Dispose()
    End Sub

    ' ===== MÉTODOS DE GESTIÓN =====
    Private Sub AgregarElemento(posicion As Point)
        contadorElementos += 1
        Dim texto As String = InputBox($"Ingrese el texto para el elemento {tipoElementoActual}:", "Texto del Elemento", $"{tipoElementoActual} {contadorElementos}")
        
        If Not String.IsNullOrEmpty(texto) Then
            Dim nuevoElemento As New ElementoDiagrama(contadorElementos, tipoElementoActual, posicion, texto)
            elementos.Add(nuevoElemento)
        End If
    End Sub

    Private Sub EditarTextoElemento(elemento As ElementoDiagrama)
        Dim nuevoTexto As String = InputBox("Editar texto del elemento:", "Editar Elemento", elemento.Texto)
        If Not String.IsNullOrEmpty(nuevoTexto) Then
            elemento.Texto = nuevoTexto
        End If
    End Sub

    Private Sub LimpiarDiagrama(sender As Object, e As EventArgs)
        If MessageBox.Show("¿Está seguro de que desea limpiar todo el diagrama?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            elementos.Clear()
            contadorElementos = 0
            Me.Controls.Find("panelDibujo", True).FirstOrDefault()?.Invalidate()
        End If
    End Sub

    Private Sub GuardarDiagrama(sender As Object, e As EventArgs)
        Try
            Dim panelDibujo As Panel = DirectCast(Me.Controls.Find("panelDibujo", True).FirstOrDefault(), Panel)
            If panelDibujo IsNot Nothing Then
                Dim bitmap As New Bitmap(panelDibujo.Width, panelDibujo.Height)
                panelDibujo.DrawToBitmap(bitmap, New Rectangle(0, 0, panelDibujo.Width, panelDibujo.Height))
                
                Dim saveDialog As New SaveFileDialog With {
                    .Filter = "PNG Image|*.png",
                    .Title = "Guardar Diagrama de Flujo",
                    .FileName = "DiagramaFlujo_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".png"
                }
                
                If saveDialog.ShowDialog() = DialogResult.OK Then
                    bitmap.Save(saveDialog.FileName, Imaging.ImageFormat.Png)
                    MessageBox.Show("Diagrama guardado exitosamente en:" & vbCrLf & saveDialog.FileName, "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error al guardar el diagrama:" & vbCrLf & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MostrarAyuda(sender As Object, e As EventArgs)
        MessageBox.Show("GENERADOR DE DIAGRAMAS DE FLUJO" & vbCrLf & vbCrLf &
                      "INSTRUCCIONES:" & vbCrLf &
                      "1. Seleccione un tipo de elemento del panel izquierdo" & vbCrLf &
                      "2. Haga clic en el área blanca para agregar el elemento" & vbCrLf &
                      "3. Ingrese el texto cuando se le solicite" & vbCrLf &
                      "4. Arrastre los elementos para moverlos" & vbCrLf &
                      "5. Clic derecho en un elemento para editar su texto" & vbCrLf & vbCrLf &
                      "SÍMBOLOS:" & vbCrLf &
                      "⭕ Inicio/Fin: Puntos de inicio y final del proceso" & vbCrLf &
                      "⬜ Proceso: Actividades o tareas del proceso" & vbCrLf &
                      "🔶 Decisión: Puntos de decisión (Sí/No)" & vbCrLf &
                      "📄 Documento: Documentos generados o utilizados" & vbCrLf &
                      "🗃️ Almacén: Almacenamiento de datos o materiales" & vbCrLf &
                      "⚪ Conector: Conexión entre partes del diagrama",
                      "Ayuda - Diagrama de Flujo", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' ===== FUNCIÓN AUXILIAR =====
    Private Function ObtenerColorPorTipo(tipo As TipoElemento) As Color
        Select Case tipo
            Case TipoElemento.Inicio
                Return Color.LightGreen
            Case TipoElemento.Proceso
                Return Color.LightBlue
            Case TipoElemento.Decision
                Return Color.LightYellow
            Case TipoElemento.Documento
                Return Color.LightCyan
            Case TipoElemento.Almacenamiento
                Return Color.LightPink
            Case TipoElemento.Conector
                Return Color.LightGray
            Case TipoElemento.Fin
                Return Color.LightCoral
            Case Else
                Return Color.White
        End Select
    End Function

End Class
