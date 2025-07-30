' =====================================================
' CALCULADORA DE ESTUDIO DE TIEMPOS
' Ingeniería Industrial - Visual Basic .NET
' =====================================================
' Autor: Repositorio Ingeniería Industrial
' Fecha: 2025
' Descripción: Calculadora para determinar tiempos estándar
'              en estudios de tiempos y movimientos
' =====================================================

Public Class CalculadoraEstudioTiempos

    ' ===== VARIABLES GLOBALES =====
    Private tiemposObservados As New List(Of Double)
    Private factorCalificacion As Double = 1.0
    Private suplementos As Double = 0.15 ' 15% por defecto

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub CalculadoraEstudioTiempos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configuración inicial del formulario
        Me.Text = "Calculadora de Estudio de Tiempos - Ingeniería Industrial"
        Me.Size = New Size(800, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        
        ' Inicializar controles
        InicializarControles()
        ConfigurarEventos()
        LimpiarFormulario()
    End Sub

    ' ===== INICIALIZACIÓN DE CONTROLES =====
    Private Sub InicializarControles()
        ' Panel principal
        Dim panelPrincipal As New Panel With {
            .Dock = DockStyle.Fill,
            .Padding = New Padding(20)
        }
        Me.Controls.Add(panelPrincipal)

        ' Título
        Dim lblTitulo As New Label With {
            .Text = "CALCULADORA DE ESTUDIO DE TIEMPOS",
            .Font = New Font("Arial", 16, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panelPrincipal.Controls.Add(lblTitulo)

        ' === SECCIÓN 1: TIEMPOS OBSERVADOS ===
        Dim gbTiempos As New GroupBox With {
            .Text = "1. Tiempos Observados (minutos)",
            .Size = New Size(350, 200),
            .Location = New Point(20, 60)
        }
        panelPrincipal.Controls.Add(gbTiempos)

        ' Campo para ingresar tiempo
        Dim lblTiempo As New Label With {
            .Text = "Tiempo observado:",
            .Location = New Point(10, 30)
        }
        gbTiempos.Controls.Add(lblTiempo)

        Dim txtTiempo As New TextBox With {
            .Name = "txtTiempo",
            .Size = New Size(100, 25),
            .Location = New Point(120, 27)
        }
        gbTiempos.Controls.Add(txtTiempo)

        Dim btnAgregar As New Button With {
            .Name = "btnAgregar",
            .Text = "Agregar",
            .Size = New Size(80, 25),
            .Location = New Point(230, 27)
        }
        gbTiempos.Controls.Add(btnAgregar)

        ' Lista de tiempos
        Dim lstTiempos As New ListBox With {
            .Name = "lstTiempos",
            .Size = New Size(200, 120),
            .Location = New Point(10, 60)
        }
        gbTiempos.Controls.Add(lstTiempos)

        Dim btnEliminar As New Button With {
            .Name = "btnEliminar",
            .Text = "Eliminar Seleccionado",
            .Size = New Size(120, 25),
            .Location = New Point(220, 60)
        }
        gbTiempos.Controls.Add(btnEliminar)

        Dim btnLimpiar As New Button With {
            .Name = "btnLimpiar",
            .Text = "Limpiar Todo",
            .Size = New Size(120, 25),
            .Location = New Point(220, 90)
        }
        gbTiempos.Controls.Add(btnLimpiar)

        ' === SECCIÓN 2: FACTOR DE CALIFICACIÓN ===
        Dim gbCalificacion As New GroupBox With {
            .Text = "2. Factor de Calificación",
            .Size = New Size(350, 120),
            .Location = New Point(20, 270)
        }
        panelPrincipal.Controls.Add(gbCalificacion)

        ' Método Westinghouse
        Dim lblHabilidad As New Label With {
            .Text = "Habilidad:",
            .Location = New Point(10, 25)
        }
        gbCalificacion.Controls.Add(lblHabilidad)

        Dim cmbHabilidad As New ComboBox With {
            .Name = "cmbHabilidad",
            .Size = New Size(120, 25),
            .Location = New Point(80, 22),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbHabilidad.Items.AddRange({"Superior (+15%)", "Excelente (+11%)", "Buena (+6%)", "Promedio (0%)", "Regular (-10%)", "Deficiente (-16%)"})
        cmbHabilidad.SelectedIndex = 3 ' Promedio por defecto
        gbCalificacion.Controls.Add(cmbHabilidad)

        Dim lblEsfuerzo As New Label With {
            .Text = "Esfuerzo:",
            .Location = New Point(10, 55)
        }
        gbCalificacion.Controls.Add(lblEsfuerzo)

        Dim cmbEsfuerzo As New ComboBox With {
            .Name = "cmbEsfuerzo",
            .Size = New Size(120, 25),
            .Location = New Point(80, 52),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbEsfuerzo.Items.AddRange({"Excesivo (+13%)", "Excelente (+10%)", "Bueno (+5%)", "Promedio (0%)", "Regular (-4%)", "Deficiente (-8%)"})
        cmbEsfuerzo.SelectedIndex = 3 ' Promedio por defecto
        gbCalificacion.Controls.Add(cmbEsfuerzo)

        Dim lblCondiciones As New Label With {
            .Text = "Condiciones:",
            .Location = New Point(210, 25)
        }
        gbCalificacion.Controls.Add(lblCondiciones)

        Dim cmbCondiciones As New ComboBox With {
            .Name = "cmbCondiciones",
            .Size = New Size(120, 25),
            .Location = New Point(210, 22),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbCondiciones.Items.AddRange({"Ideales (+6%)", "Excelentes (+4%)", "Buenas (+2%)", "Promedio (0%)", "Regulares (-3%)", "Deficientes (-7%)"})
        cmbCondiciones.SelectedIndex = 3 ' Promedio por defecto
        gbCalificacion.Controls.Add(cmbCondiciones)

        Dim lblConsistencia As New Label With {
            .Text = "Consistencia:",
            .Location = New Point(210, 55)
        }
        gbCalificacion.Controls.Add(lblConsistencia)

        Dim cmbConsistencia As New ComboBox With {
            .Name = "cmbConsistencia",
            .Size = New Size(120, 25),
            .Location = New Point(210, 52),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbConsistencia.Items.AddRange({"Perfecta (+4%)", "Excelente (+3%)", "Buena (+1%)", "Promedio (0%)", "Regular (-2%)", "Deficiente (-4%)"})
        cmbConsistencia.SelectedIndex = 3 ' Promedio por defecto
        gbCalificacion.Controls.Add(cmbConsistencia)

        Dim lblFactorTotal As New Label With {
            .Name = "lblFactorTotal",
            .Text = "Factor Total: 1.00",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(10, 85)
        }
        gbCalificacion.Controls.Add(lblFactorTotal)

        ' === SECCIÓN 3: SUPLEMENTOS ===
        Dim gbSuplementos As New GroupBox With {
            .Text = "3. Suplementos (%)",
            .Size = New Size(350, 100),
            .Location = New Point(20, 400)
        }
        panelPrincipal.Controls.Add(gbSuplementos)

        Dim lblNecesidades As New Label With {
            .Text = "Necesidades personales:",
            .Location = New Point(10, 25)
        }
        gbSuplementos.Controls.Add(lblNecesidades)

        Dim txtNecesidades As New TextBox With {
            .Name = "txtNecesidades",
            .Text = "5",
            .Size = New Size(50, 25),
            .Location = New Point(150, 22)
        }
        gbSuplementos.Controls.Add(txtNecesidades)

        Dim lblFatiga As New Label With {
            .Text = "Fatiga básica:",
            .Location = New Point(10, 55)
        }
        gbSuplementos.Controls.Add(lblFatiga)

        Dim txtFatiga As New TextBox With {
            .Name = "txtFatiga",
            .Text = "4",
            .Size = New Size(50, 25),
            .Location = New Point(150, 52)
        }
        gbSuplementos.Controls.Add(txtFatiga)

        Dim lblVariables As New Label With {
            .Text = "Suplementos variables:",
            .Location = New Point(220, 25)
        }
        gbSuplementos.Controls.Add(lblVariables)

        Dim txtVariables As New TextBox With {
            .Name = "txtVariables",
            .Text = "6",
            .Size = New Size(50, 25),
            .Location = New Point(220, 52)
        }
        gbSuplementos.Controls.Add(txtVariables)

        ' === SECCIÓN 4: RESULTADOS ===
        Dim gbResultados As New GroupBox With {
            .Text = "4. Resultados",
            .Size = New Size(350, 180),
            .Location = New Point(400, 60)
        }
        panelPrincipal.Controls.Add(gbResultados)

        Dim lblTiempoPromedio As New Label With {
            .Name = "lblTiempoPromedio",
            .Text = "Tiempo Promedio Observado: -- min",
            .Location = New Point(10, 25),
            .Size = New Size(300, 20)
        }
        gbResultados.Controls.Add(lblTiempoPromedio)

        Dim lblTiempoNormal As New Label With {
            .Name = "lblTiempoNormal",
            .Text = "Tiempo Normal: -- min",
            .Font = New Font("Arial", 9, FontStyle.Bold),
            .Location = New Point(10, 50),
            .Size = New Size(300, 20)
        }
        gbResultados.Controls.Add(lblTiempoNormal)

        Dim lblTiempoEstandar As New Label With {
            .Name = "lblTiempoEstandar",
            .Text = "Tiempo Estándar: -- min",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen,
            .Location = New Point(10, 75),
            .Size = New Size(300, 20)
        }
        gbResultados.Controls.Add(lblTiempoEstandar)

        Dim lblProductividad As New Label With {
            .Name = "lblProductividad",
            .Text = "Productividad Estándar: -- unid/hora",
            .Location = New Point(10, 100),
            .Size = New Size(300, 20)
        }
        gbResultados.Controls.Add(lblProductividad)

        ' Botón calcular
        Dim btnCalcular As New Button With {
            .Name = "btnCalcular",
            .Text = "CALCULAR TIEMPO ESTÁNDAR",
            .Size = New Size(200, 40),
            .Location = New Point(75, 130),
            .BackColor = Color.DarkBlue,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(btnCalcular)

        ' === SECCIÓN 5: INFORMACIÓN ADICIONAL ===
        Dim gbInfo As New GroupBox With {
            .Text = "5. Información del Estudio",
            .Size = New Size(350, 120),
            .Location = New Point(400, 250)
        }
        panelPrincipal.Controls.Add(gbInfo)

        Dim lblOperacion As New Label With {
            .Text = "Operación:",
            .Location = New Point(10, 25)
        }
        gbInfo.Controls.Add(lblOperacion)

        Dim txtOperacion As New TextBox With {
            .Name = "txtOperacion",
            .Size = New Size(200, 25),
            .Location = New Point(80, 22)
        }
        gbInfo.Controls.Add(txtOperacion)

        Dim lblOperador As New Label With {
            .Text = "Operador:",
            .Location = New Point(10, 55)
        }
        gbInfo.Controls.Add(lblOperador)

        Dim txtOperador As New TextBox With {
            .Name = "txtOperador",
            .Size = New Size(200, 25),
            .Location = New Point(80, 52)
        }
        gbInfo.Controls.Add(txtOperador)

        Dim lblFecha As New Label With {
            .Text = "Fecha:",
            .Location = New Point(10, 85)
        }
        gbInfo.Controls.Add(lblFecha)

        Dim dtpFecha As New DateTimePicker With {
            .Name = "dtpFecha",
            .Size = New Size(200, 25),
            .Location = New Point(80, 82),
            .Value = DateTime.Now
        }
        gbInfo.Controls.Add(dtpFecha)

        ' === BOTONES DE ACCIÓN ===
        Dim btnGuardar As New Button With {
            .Name = "btnGuardar",
            .Text = "Guardar Estudio",
            .Size = New Size(120, 35),
            .Location = New Point(400, 380),
            .BackColor = Color.DarkGreen,
            .ForeColor = Color.White
        }
        panelPrincipal.Controls.Add(btnGuardar)

        Dim btnNuevo As New Button With {
            .Name = "btnNuevo",
            .Text = "Nuevo Estudio",
            .Size = New Size(120, 35),
            .Location = New Point(530, 380)
        }
        panelPrincipal.Controls.Add(btnNuevo)

        Dim btnExportar As New Button With {
            .Name = "btnExportar",
            .Text = "Exportar Reporte",
            .Size = New Size(120, 35),
            .Location = New Point(400, 420)
        }
        panelPrincipal.Controls.Add(btnExportar)

        Dim btnAyuda As New Button With {
            .Name = "btnAyuda",
            .Text = "Ayuda",
            .Size = New Size(120, 35),
            .Location = New Point(530, 420)
        }
        panelPrincipal.Controls.Add(btnAyuda)
    End Sub

    ' ===== CONFIGURACIÓN DE EVENTOS =====
    Private Sub ConfigurarEventos()
        ' Buscar controles y asignar eventos
        AddHandler Me.Controls.Find("btnAgregar", True)(0).Click, AddressOf AgregarTiempo
        AddHandler Me.Controls.Find("btnEliminar", True)(0).Click, AddressOf EliminarTiempo
        AddHandler Me.Controls.Find("btnLimpiar", True)(0).Click, AddressOf LimpiarTiempos
        AddHandler Me.Controls.Find("btnCalcular", True)(0).Click, AddressOf CalcularTiempoEstandar
        AddHandler Me.Controls.Find("btnNuevo", True)(0).Click, AddressOf NuevoEstudio
        AddHandler Me.Controls.Find("btnGuardar", True)(0).Click, AddressOf GuardarEstudio
        AddHandler Me.Controls.Find("btnExportar", True)(0).Click, AddressOf ExportarReporte
        AddHandler Me.Controls.Find("btnAyuda", True)(0).Click, AddressOf MostrarAyuda

        ' Eventos para actualizar factor de calificación
        For Each cmb As ComboBox In {Me.Controls.Find("cmbHabilidad", True)(0), 
                                     Me.Controls.Find("cmbEsfuerzo", True)(0),
                                     Me.Controls.Find("cmbCondiciones", True)(0),
                                     Me.Controls.Find("cmbConsistencia", True)(0)}
            AddHandler cmb.SelectedIndexChanged, AddressOf ActualizarFactorCalificacion
        Next

        ' Eventos para actualizar suplementos
        For Each txt As TextBox In {Me.Controls.Find("txtNecesidades", True)(0),
                                    Me.Controls.Find("txtFatiga", True)(0),
                                    Me.Controls.Find("txtVariables", True)(0)}
            AddHandler txt.TextChanged, AddressOf ActualizarSuplementos
        Next
    End Sub

    ' ===== MÉTODOS DE FUNCIONALIDAD =====
    Private Sub AgregarTiempo(sender As Object, e As EventArgs)
        Dim txtTiempo As TextBox = Me.Controls.Find("txtTiempo", True)(0)
        Dim lstTiempos As ListBox = Me.Controls.Find("lstTiempos", True)(0)
        
        Dim tiempo As Double
        If Double.TryParse(txtTiempo.Text, tiempo) AndAlso tiempo > 0 Then
            tiemposObservados.Add(tiempo)
            lstTiempos.Items.Add($"Obs {tiemposObservados.Count}: {tiempo:F3} min")
            txtTiempo.Clear()
            txtTiempo.Focus()
            ActualizarEstadisticas()
        Else
            MessageBox.Show("Por favor ingrese un tiempo válido mayor a 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub EliminarTiempo(sender As Object, e As EventArgs)
        Dim lstTiempos As ListBox = Me.Controls.Find("lstTiempos", True)(0)
        
        If lstTiempos.SelectedIndex >= 0 Then
            tiemposObservados.RemoveAt(lstTiempos.SelectedIndex)
            lstTiempos.Items.RemoveAt(lstTiempos.SelectedIndex)
            ActualizarNumeracion()
            ActualizarEstadisticas()
        End If
    End Sub

    Private Sub LimpiarTiempos(sender As Object, e As EventArgs)
        tiemposObservados.Clear()
        Dim lstTiempos As ListBox = Me.Controls.Find("lstTiempos", True)(0)
        lstTiempos.Items.Clear()
        ActualizarEstadisticas()
    End Sub

    Private Sub ActualizarNumeracion()
        Dim lstTiempos As ListBox = Me.Controls.Find("lstTiempos", True)(0)
        lstTiempos.Items.Clear()
        For i As Integer = 0 To tiemposObservados.Count - 1
            lstTiempos.Items.Add($"Obs {i + 1}: {tiemposObservados(i):F3} min")
        Next
    End Sub

    Private Sub ActualizarFactorCalificacion(sender As Object, e As EventArgs)
        Dim habilidad As Double = ObtenerValorHabilidad()
        Dim esfuerzo As Double = ObtenerValorEsfuerzo()
        Dim condiciones As Double = ObtenerValorCondiciones()
        Dim consistencia As Double = ObtenerValorConsistencia()
        
        factorCalificacion = 1.0 + (habilidad + esfuerzo + condiciones + consistencia) / 100.0
        
        Dim lblFactorTotal As Label = Me.Controls.Find("lblFactorTotal", True)(0)
        lblFactorTotal.Text = $"Factor Total: {factorCalificacion:F3}"
    End Sub

    Private Sub ActualizarSuplementos(sender As Object, e As EventArgs)
        Dim txtNecesidades As TextBox = Me.Controls.Find("txtNecesidades", True)(0)
        Dim txtFatiga As TextBox = Me.Controls.Find("txtFatiga", True)(0)
        Dim txtVariables As TextBox = Me.Controls.Find("txtVariables", True)(0)
        
        Dim necesidades, fatiga, variables As Double
        Double.TryParse(txtNecesidades.Text, necesidades)
        Double.TryParse(txtFatiga.Text, fatiga)
        Double.TryParse(txtVariables.Text, variables)
        
        suplementos = (necesidades + fatiga + variables) / 100.0
    End Sub

    Private Sub CalcularTiempoEstandar(sender As Object, e As EventArgs)
        If tiemposObservados.Count = 0 Then
            MessageBox.Show("Debe ingresar al menos un tiempo observado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Calcular tiempo promedio observado
        Dim tiempoPromedio As Double = tiemposObservados.Average()
        
        ' Calcular tiempo normal
        Dim tiempoNormal As Double = tiempoPromedio * factorCalificacion
        
        ' Calcular tiempo estándar
        Dim tiempoEstandar As Double = tiempoNormal * (1 + suplementos)
        
        ' Calcular productividad estándar (unidades por hora)
        Dim productividadEstandar As Double = 60 / tiempoEstandar
        
        ' Actualizar labels de resultados
        Dim lblTiempoPromedio As Label = Me.Controls.Find("lblTiempoPromedio", True)(0)
        Dim lblTiempoNormal As Label = Me.Controls.Find("lblTiempoNormal", True)(0)
        Dim lblTiempoEstandar As Label = Me.Controls.Find("lblTiempoEstandar", True)(0)
        Dim lblProductividad As Label = Me.Controls.Find("lblProductividad", True)(0)
        
        lblTiempoPromedio.Text = $"Tiempo Promedio Observado: {tiempoPromedio:F3} min"
        lblTiempoNormal.Text = $"Tiempo Normal: {tiempoNormal:F3} min"
        lblTiempoEstandar.Text = $"Tiempo Estándar: {tiempoEstandar:F3} min"
        lblProductividad.Text = $"Productividad Estándar: {productividadEstandar:F1} unid/hora"
    End Sub

    ' ===== MÉTODOS AUXILIARES =====
    Private Function ObtenerValorHabilidad() As Double
        Dim cmb As ComboBox = Me.Controls.Find("cmbHabilidad", True)(0)
        Select Case cmb.SelectedIndex
            Case 0 : Return 15 ' Superior
            Case 1 : Return 11 ' Excelente
            Case 2 : Return 6  ' Buena
            Case 3 : Return 0  ' Promedio
            Case 4 : Return -10 ' Regular
            Case 5 : Return -16 ' Deficiente
            Case Else : Return 0
        End Select
    End Function

    Private Function ObtenerValorEsfuerzo() As Double
        Dim cmb As ComboBox = Me.Controls.Find("cmbEsfuerzo", True)(0)
        Select Case cmb.SelectedIndex
            Case 0 : Return 13 ' Excesivo
            Case 1 : Return 10 ' Excelente
            Case 2 : Return 5  ' Bueno
            Case 3 : Return 0  ' Promedio
            Case 4 : Return -4 ' Regular
            Case 5 : Return -8 ' Deficiente
            Case Else : Return 0
        End Select
    End Function

    Private Function ObtenerValorCondiciones() As Double
        Dim cmb As ComboBox = Me.Controls.Find("cmbCondiciones", True)(0)
        Select Case cmb.SelectedIndex
            Case 0 : Return 6  ' Ideales
            Case 1 : Return 4  ' Excelentes
            Case 2 : Return 2  ' Buenas
            Case 3 : Return 0  ' Promedio
            Case 4 : Return -3 ' Regulares
            Case 5 : Return -7 ' Deficientes
            Case Else : Return 0
        End Select
    End Function

    Private Function ObtenerValorConsistencia() As Double
        Dim cmb As ComboBox = Me.Controls.Find("cmbConsistencia", True)(0)
        Select Case cmb.SelectedIndex
            Case 0 : Return 4  ' Perfecta
            Case 1 : Return 3  ' Excelente
            Case 2 : Return 1  ' Buena
            Case 3 : Return 0  ' Promedio
            Case 4 : Return -2 ' Regular
            Case 5 : Return -4 ' Deficiente
            Case Else : Return 0
        End Select
    End Function

    Private Sub ActualizarEstadisticas()
        If tiemposObservados.Count > 0 Then
            Dim promedio As Double = tiemposObservados.Average()
            Dim lblTiempoPromedio As Label = Me.Controls.Find("lblTiempoPromedio", True)(0)
            lblTiempoPromedio.Text = $"Tiempo Promedio Observado: {promedio:F3} min"
        End If
    End Sub

    Private Sub LimpiarFormulario()
        ' Limpiar tiempos
        tiemposObservados.Clear()
        Dim lstTiempos As ListBox = Me.Controls.Find("lstTiempos", True)(0)
        lstTiempos.Items.Clear()
        
        ' Resetear factor de calificación
        factorCalificacion = 1.0
        Dim lblFactorTotal As Label = Me.Controls.Find("lblFactorTotal", True)(0)
        lblFactorTotal.Text = "Factor Total: 1.00"
        
        ' Resetear suplementos
        suplementos = 0.15
        
        ' Limpiar resultados
        Dim lblTiempoPromedio As Label = Me.Controls.Find("lblTiempoPromedio", True)(0)
        Dim lblTiempoNormal As Label = Me.Controls.Find("lblTiempoNormal", True)(0)
        Dim lblTiempoEstandar As Label = Me.Controls.Find("lblTiempoEstandar", True)(0)
        Dim lblProductividad As Label = Me.Controls.Find("lblProductividad", True)(0)
        
        lblTiempoPromedio.Text = "Tiempo Promedio Observado: -- min"
        lblTiempoNormal.Text = "Tiempo Normal: -- min"
        lblTiempoEstandar.Text = "Tiempo Estándar: -- min"
        lblProductividad.Text = "Productividad Estándar: -- unid/hora"
    End Sub

    ' ===== MÉTODOS DE ARCHIVO =====
    Private Sub NuevoEstudio(sender As Object, e As EventArgs)
        If MessageBox.Show("¿Desea crear un nuevo estudio? Se perderán los datos actuales.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            LimpiarFormulario()
            ' Limpiar información del estudio
            Me.Controls.Find("txtOperacion", True)(0).Text = ""
            Me.Controls.Find("txtOperador", True)(0).Text = ""
            Me.Controls.Find("dtpFecha", True)(0).Value = DateTime.Now
        End If
    End Sub

    Private Sub GuardarEstudio(sender As Object, e As EventArgs)
        MessageBox.Show("Funcionalidad de guardado en desarrollo.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ExportarReporte(sender As Object, e As EventArgs)
        MessageBox.Show("Funcionalidad de exportación en desarrollo.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MostrarAyuda(sender As Object, e As EventArgs)
        Dim ayuda As String = "CALCULADORA DE ESTUDIO DE TIEMPOS" & vbCrLf & vbCrLf &
                             "1. Ingrese los tiempos observados en minutos" & vbCrLf &
                             "2. Seleccione los factores de calificación según el método Westinghouse" & vbCrLf &
                             "3. Ajuste los suplementos según las condiciones de trabajo" & vbCrLf &
                             "4. Haga clic en 'Calcular' para obtener el tiempo estándar" & vbCrLf & vbCrLf &
                             "FÓRMULAS:" & vbCrLf &
                             "• Tiempo Normal = Tiempo Observado × Factor de Calificación" & vbCrLf &
                             "• Tiempo Estándar = Tiempo Normal × (1 + % Suplementos)" & vbCrLf &
                             "• Productividad = 60 / Tiempo Estándar (unidades/hora)"
        
        MessageBox.Show(ayuda, "Ayuda - Calculadora de Estudio de Tiempos", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

End Class
