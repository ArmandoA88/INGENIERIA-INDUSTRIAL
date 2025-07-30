' =====================================================
' CALCULADORA DE INVENTARIOS (EOQ) - INTEGRADA
' Ingeniería Industrial - Visual Basic .NET
' =====================================================
' Autor: Repositorio Ingeniería Industrial
' Fecha: 2025
' Descripción: Calculadora de inventarios EOQ integrada en la suite principal
' =====================================================

Public Class FormCalculadoraInventarios
    Inherits Form

    ' ===== VARIABLES GLOBALES =====
    Private eoq As Double = 0
    Private puntoReorden As Double = 0
    Private stockSeguridad As Double = 0
    Private costoTotalAnual As Double = 0
    Private numeroOrdenesAnual As Double = 0

    ' ===== CONSTRUCTOR =====
    Public Sub New()
        InitializeComponent()
    End Sub

    ' ===== INICIALIZACIÓN DEL FORMULARIO =====
    Private Sub InitializeComponent()
        ' Configuración inicial del formulario
        Me.Text = "Calculadora de Inventarios (EOQ) - Ingeniería Industrial"
        Me.Size = New Size(950, 750)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        
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
            .AutoScroll = True,
            .Padding = New Padding(20)
        }
        Me.Controls.Add(panelPrincipal)

        ' Título
        Dim lblTitulo As New Label With {
            .Text = "CALCULADORA DE INVENTARIOS (EOQ)",
            .Font = New Font("Arial", 16, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panelPrincipal.Controls.Add(lblTitulo)

        ' === SECCIÓN 1: DATOS BÁSICOS ===
        Dim gbDatosBasicos As New GroupBox With {
            .Text = "1. Datos Básicos del Producto",
            .Size = New Size(450, 200),
            .Location = New Point(20, 60)
        }
        panelPrincipal.Controls.Add(gbDatosBasicos)

        Dim lblDemandaAnual As New Label With {
            .Text = "Demanda anual (D):",
            .Location = New Point(10, 30),
            .Size = New Size(120, 20)
        }
        gbDatosBasicos.Controls.Add(lblDemandaAnual)

        Dim txtDemandaAnual As New TextBox With {
            .Name = "txtDemandaAnual",
            .Size = New Size(100, 25),
            .Location = New Point(140, 30)
        }
        gbDatosBasicos.Controls.Add(txtDemandaAnual)

        Dim lblUnidadesDemanda As New Label With {
            .Text = "unidades/año",
            .Location = New Point(250, 32),
            .Size = New Size(80, 20)
        }
        gbDatosBasicos.Controls.Add(lblUnidadesDemanda)

        Dim lblCostoOrden As New Label With {
            .Text = "Costo por orden (S):",
            .Location = New Point(10, 65),
            .Size = New Size(120, 20)
        }
        gbDatosBasicos.Controls.Add(lblCostoOrden)

        Dim txtCostoOrden As New TextBox With {
            .Name = "txtCostoOrden",
            .Size = New Size(100, 25),
            .Location = New Point(140, 65)
        }
        gbDatosBasicos.Controls.Add(txtCostoOrden)

        Dim lblMonedaOrden As New Label With {
            .Text = "$/orden",
            .Location = New Point(250, 67),
            .Size = New Size(50, 20)
        }
        gbDatosBasicos.Controls.Add(lblMonedaOrden)

        Dim lblCostoMantenimiento As New Label With {
            .Text = "Costo mantenimiento (H):",
            .Location = New Point(10, 100),
            .Size = New Size(120, 20)
        }
        gbDatosBasicos.Controls.Add(lblCostoMantenimiento)

        Dim txtCostoMantenimiento As New TextBox With {
            .Name = "txtCostoMantenimiento",
            .Size = New Size(100, 25),
            .Location = New Point(140, 100)
        }
        gbDatosBasicos.Controls.Add(txtCostoMantenimiento)

        Dim lblMonedaMantenimiento As New Label With {
            .Text = "$/unidad/año",
            .Location = New Point(250, 102),
            .Size = New Size(80, 20)
        }
        gbDatosBasicos.Controls.Add(lblMonedaMantenimiento)

        Dim lblCostoUnitario As New Label With {
            .Text = "Costo unitario (C):",
            .Location = New Point(10, 135),
            .Size = New Size(120, 20)
        }
        gbDatosBasicos.Controls.Add(lblCostoUnitario)

        Dim txtCostoUnitario As New TextBox With {
            .Name = "txtCostoUnitario",
            .Size = New Size(100, 25),
            .Location = New Point(140, 135)
        }
        gbDatosBasicos.Controls.Add(txtCostoUnitario)

        Dim lblMonedaUnitario As New Label With {
            .Text = "$/unidad",
            .Location = New Point(250, 137),
            .Size = New Size(60, 20)
        }
        gbDatosBasicos.Controls.Add(lblMonedaUnitario)

        Dim btnCalcularEOQ As New Button With {
            .Name = "btnCalcularEOQ",
            .Text = "Calcular EOQ",
            .Size = New Size(100, 35),
            .Location = New Point(340, 80),
            .BackColor = Color.DarkBlue,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbDatosBasicos.Controls.Add(btnCalcularEOQ)

        ' === SECCIÓN 2: PARÁMETROS ADICIONALES ===
        Dim gbParametros As New GroupBox With {
            .Text = "2. Parámetros para Punto de Reorden",
            .Size = New Size(450, 160),
            .Location = New Point(480, 60)
        }
        panelPrincipal.Controls.Add(gbParametros)

        Dim lblTiempoEntrega As New Label With {
            .Text = "Tiempo de entrega (L):",
            .Location = New Point(10, 30),
            .Size = New Size(130, 20)
        }
        gbParametros.Controls.Add(lblTiempoEntrega)

        Dim txtTiempoEntrega As New TextBox With {
            .Name = "txtTiempoEntrega",
            .Size = New Size(100, 25),
            .Location = New Point(150, 30)
        }
        gbParametros.Controls.Add(txtTiempoEntrega)

        Dim lblDiasTiempo As New Label With {
            .Text = "días",
            .Location = New Point(260, 32),
            .Size = New Size(40, 20)
        }
        gbParametros.Controls.Add(lblDiasTiempo)

        Dim lblDesviacionDemanda As New Label With {
            .Text = "Desviación estándar (σ):",
            .Location = New Point(10, 65),
            .Size = New Size(130, 20)
        }
        gbParametros.Controls.Add(lblDesviacionDemanda)

        Dim txtDesviacionDemanda As New TextBox With {
            .Name = "txtDesviacionDemanda",
            .Size = New Size(100, 25),
            .Location = New Point(150, 65)
        }
        gbParametros.Controls.Add(txtDesviacionDemanda)

        Dim lblUnidadesDesviacion As New Label With {
            .Text = "unid/día",
            .Location = New Point(260, 67),
            .Size = New Size(50, 20)
        }
        gbParametros.Controls.Add(lblUnidadesDesviacion)

        Dim lblNivelServicio As New Label With {
            .Text = "Nivel de servicio (Z):",
            .Location = New Point(10, 100),
            .Size = New Size(130, 20)
        }
        gbParametros.Controls.Add(lblNivelServicio)

        Dim cmbNivelServicio As New ComboBox With {
            .Name = "cmbNivelServicio",
            .Size = New Size(150, 25),
            .Location = New Point(150, 100),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbNivelServicio.Items.AddRange({"90% (Z=1.28)", "95% (Z=1.65)", "97.5% (Z=1.96)", "99% (Z=2.33)", "99.5% (Z=2.58)", "99.9% (Z=3.09)"})
        cmbNivelServicio.SelectedIndex = 2 ' 97.5% por defecto
        gbParametros.Controls.Add(cmbNivelServicio)

        Dim btnCalcularPuntoReorden As New Button With {
            .Name = "btnCalcularPuntoReorden",
            .Text = "Calcular Punto de Reorden",
            .Size = New Size(200, 35),
            .Location = New Point(125, 130),
            .BackColor = Color.DarkGreen,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 9, FontStyle.Bold)
        }
        gbParametros.Controls.Add(btnCalcularPuntoReorden)

        ' === SECCIÓN 3: RESULTADOS EOQ ===
        Dim gbResultadosEOQ As New GroupBox With {
            .Text = "3. Resultados EOQ",
            .Size = New Size(450, 180),
            .Location = New Point(20, 280)
        }
        panelPrincipal.Controls.Add(gbResultadosEOQ)

        Dim lblResultadoEOQ As New Label With {
            .Name = "lblResultadoEOQ",
            .Text = "Cantidad Económica de Pedido (EOQ): -- unidades",
            .Font = New Font("Arial", 11, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .Location = New Point(10, 30),
            .Size = New Size(420, 25)
        }
        gbResultadosEOQ.Controls.Add(lblResultadoEOQ)

        Dim lblNumeroOrdenes As New Label With {
            .Name = "lblNumeroOrdenes",
            .Text = "Número de órdenes por año: -- órdenes",
            .Location = New Point(10, 60),
            .Size = New Size(420, 20)
        }
        gbResultadosEOQ.Controls.Add(lblNumeroOrdenes)

        Dim lblCostoTotalAnual As New Label With {
            .Name = "lblCostoTotalAnual",
            .Text = "Costo total anual: $-- ",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen,
            .Location = New Point(10, 85),
            .Size = New Size(420, 20)
        }
        gbResultadosEOQ.Controls.Add(lblCostoTotalAnual)

        Dim lblCostoOrdenAnual As New Label With {
            .Name = "lblCostoOrdenAnual",
            .Text = "Costo de ordenar anual: $--",
            .Location = New Point(10, 110),
            .Size = New Size(420, 20)
        }
        gbResultadosEOQ.Controls.Add(lblCostoOrdenAnual)

        Dim lblCostoMantenimientoAnual As New Label With {
            .Name = "lblCostoMantenimientoAnual",
            .Text = "Costo de mantenimiento anual: $--",
            .Location = New Point(10, 135),
            .Size = New Size(420, 20)
        }
        gbResultadosEOQ.Controls.Add(lblCostoMantenimientoAnual)

        ' === SECCIÓN 4: RESULTADOS PUNTO DE REORDEN ===
        Dim gbResultadosPunto As New GroupBox With {
            .Text = "4. Resultados Punto de Reorden",
            .Size = New Size(450, 180),
            .Location = New Point(480, 280)
        }
        panelPrincipal.Controls.Add(gbResultadosPunto)

        Dim lblResultadoPuntoReorden As New Label With {
            .Name = "lblResultadoPuntoReorden",
            .Text = "Punto de Reorden (ROP): -- unidades",
            .Font = New Font("Arial", 11, FontStyle.Bold),
            .ForeColor = Color.DarkRed,
            .Location = New Point(10, 30),
            .Size = New Size(420, 25)
        }
        gbResultadosPunto.Controls.Add(lblResultadoPuntoReorden)

        Dim lblStockSeguridad As New Label With {
            .Name = "lblStockSeguridad",
            .Text = "Stock de Seguridad: -- unidades",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkOrange,
            .Location = New Point(10, 60),
            .Size = New Size(420, 20)
        }
        gbResultadosPunto.Controls.Add(lblStockSeguridad)

        Dim lblDemandaPromedio As New Label With {
            .Name = "lblDemandaPromedio",
            .Text = "Demanda promedio durante entrega: -- unidades",
            .Location = New Point(10, 85),
            .Size = New Size(420, 20)
        }
        gbResultadosPunto.Controls.Add(lblDemandaPromedio)

        Dim lblInventarioMaximo As New Label With {
            .Name = "lblInventarioMaximo",
            .Text = "Inventario máximo: -- unidades",
            .Location = New Point(10, 110),
            .Size = New Size(420, 20)
        }
        gbResultadosPunto.Controls.Add(lblInventarioMaximo)

        Dim lblInventarioPromedio As New Label With {
            .Name = "lblInventarioPromedio",
            .Text = "Inventario promedio: -- unidades",
            .Location = New Point(10, 135),
            .Size = New Size(420, 20)
        }
        gbResultadosPunto.Controls.Add(lblInventarioPromedio)

        ' === SECCIÓN 5: ANÁLISIS DE SENSIBILIDAD ===
        Dim gbAnalisis As New GroupBox With {
            .Text = "5. Análisis de Sensibilidad",
            .Size = New Size(910, 120),
            .Location = New Point(20, 480)
        }
        panelPrincipal.Controls.Add(gbAnalisis)

        Dim lblAnalisisInfo As New Label With {
            .Text = "Variación del EOQ ante cambios en los parámetros:",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(10, 25),
            .Size = New Size(300, 20)
        }
        gbAnalisis.Controls.Add(lblAnalisisInfo)

        Dim btnAnalisisSensibilidad As New Button With {
            .Name = "btnAnalisisSensibilidad",
            .Text = "Realizar Análisis",
            .Size = New Size(120, 30),
            .Location = New Point(320, 20),
            .BackColor = Color.Purple,
            .ForeColor = Color.White
        }
        gbAnalisis.Controls.Add(btnAnalisisSensibilidad)

        Dim lblResultadoAnalisis As New Label With {
            .Name = "lblResultadoAnalisis",
            .Text = "Haga clic en 'Realizar Análisis' para ver el impacto de cambios en D, S y H sobre el EOQ",
            .Location = New Point(10, 60),
            .Size = New Size(880, 40)
        }
        gbAnalisis.Controls.Add(lblResultadoAnalisis)

        ' === BOTONES DE ACCIÓN ===
        Dim btnGenerarReporte As New Button With {
            .Name = "btnGenerarReporte",
            .Text = "Generar Reporte",
            .Size = New Size(120, 35),
            .Location = New Point(400, 620),
            .BackColor = Color.DarkBlue,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelPrincipal.Controls.Add(btnGenerarReporte)

        Dim btnNuevo As New Button With {
            .Name = "btnNuevo",
            .Text = "Nuevo Análisis",
            .Size = New Size(120, 35),
            .Location = New Point(530, 620),
            .BackColor = Color.DarkGreen,
            .ForeColor = Color.White
        }
        panelPrincipal.Controls.Add(btnNuevo)

        Dim btnAyuda As New Button With {
            .Name = "btnAyuda",
            .Text = "Ayuda",
            .Size = New Size(120, 35),
            .Location = New Point(660, 620)
        }
        panelPrincipal.Controls.Add(btnAyuda)

        Dim btnCerrar As New Button With {
            .Name = "btnCerrar",
            .Text = "Cerrar",
            .Size = New Size(120, 35),
            .Location = New Point(790, 620),
            .BackColor = Color.Red,
            .ForeColor = Color.White
        }
        panelPrincipal.Controls.Add(btnCerrar)
    End Sub

    ' ===== CONFIGURACIÓN DE EVENTOS =====
    Private Sub ConfigurarEventos()
        ' Buscar controles y asignar eventos
        AddHandler Me.Controls.Find("btnCalcularEOQ", True)(0).Click, AddressOf CalcularEOQ
        AddHandler Me.Controls.Find("btnCalcularPuntoReorden", True)(0).Click, AddressOf CalcularPuntoReorden
        AddHandler Me.Controls.Find("btnAnalisisSensibilidad", True)(0).Click, AddressOf RealizarAnalisisSensibilidad
        AddHandler Me.Controls.Find("btnGenerarReporte", True)(0).Click, AddressOf GenerarReporte
        AddHandler Me.Controls.Find("btnNuevo", True)(0).Click, AddressOf NuevoAnalisis
        AddHandler Me.Controls.Find("btnAyuda", True)(0).Click, AddressOf MostrarAyuda
        AddHandler Me.Controls.Find("btnCerrar", True)(0).Click, AddressOf CerrarFormulario
    End Sub

    ' ===== MÉTODOS DE CÁLCULO =====
    Private Sub CalcularEOQ(sender As Object, e As EventArgs)
        Dim txtDemanda As TextBox = Me.Controls.Find("txtDemandaAnual", True)(0)
        Dim txtCostoOrden As TextBox = Me.Controls.Find("txtCostoOrden", True)(0)
        Dim txtCostoMantenimiento As TextBox = Me.Controls.Find("txtCostoMantenimiento", True)(0)
        Dim txtCostoUnitario As TextBox = Me.Controls.Find("txtCostoUnitario", True)(0)

        Dim demanda, costoOrden, costoMantenimiento, costoUnitario As Double

        If Double.TryParse(txtDemanda.Text, demanda) AndAlso 
           Double.TryParse(txtCostoOrden.Text, costoOrden) AndAlso 
           Double.TryParse(txtCostoMantenimiento.Text, costoMantenimiento) AndAlso
           Double.TryParse(txtCostoUnitario.Text, costoUnitario) AndAlso
           demanda > 0 AndAlso costoOrden > 0 AndAlso costoMantenimiento > 0 Then

            ' Calcular EOQ
            eoq = Math.Sqrt((2 * demanda * costoOrden) / costoMantenimiento)
            numeroOrdenesAnual = demanda / eoq

            ' Calcular costos
            Dim costoOrdenAnual As Double = (demanda / eoq) * costoOrden
            Dim costoMantenimientoAnual As Double = (eoq / 2) * costoMantenimiento
            costoTotalAnual = costoOrdenAnual + costoMantenimientoAnual

            ' Actualizar labels
            Me.Controls.Find("lblResultadoEOQ", True)(0).Text = $"Cantidad Económica de Pedido (EOQ): {eoq:F0} unidades"
            Me.Controls.Find("lblNumeroOrdenes", True)(0).Text = $"Número de órdenes por año: {numeroOrdenesAnual:F1} órdenes"
            Me.Controls.Find("lblCostoTotalAnual", True)(0).Text = $"Costo total anual: ${costoTotalAnual:F2}"
            Me.Controls.Find("lblCostoOrdenAnual", True)(0).Text = $"Costo de ordenar anual: ${costoOrdenAnual:F2}"
            Me.Controls.Find("lblCostoMantenimientoAnual", True)(0).Text = $"Costo de mantenimiento anual: ${costoMantenimientoAnual:F2}"

        Else
            MessageBox.Show("Por favor ingrese valores válidos y mayores a cero para todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CalcularPuntoReorden(sender As Object, e As EventArgs)
        Dim txtDemanda As TextBox = Me.Controls.Find("txtDemandaAnual", True)(0)
        Dim txtTiempoEntrega As TextBox = Me.Controls.Find("txtTiempoEntrega", True)(0)
        Dim txtDesviacion As TextBox = Me.Controls.Find("txtDesviacionDemanda", True)(0)
        Dim cmbNivel As ComboBox = Me.Controls.Find("cmbNivelServicio", True)(0)

        Dim demanda, tiempoEntrega, desviacion As Double

        If Double.TryParse(txtDemanda.Text, demanda) AndAlso 
           Double.TryParse(txtTiempoEntrega.Text, tiempoEntrega) AndAlso 
           Double.TryParse(txtDesviacion.Text, desviacion) AndAlso
           cmbNivel.SelectedIndex >= 0 AndAlso
           demanda > 0 AndAlso tiempoEntrega > 0 Then

            ' Obtener valor Z según el nivel de servicio
            Dim valorZ As Double = ObtenerValorZ(cmbNivel.SelectedIndex)

            ' Calcular demanda diaria promedio
            Dim demandaDiaria As Double = demanda / 365

            ' Calcular demanda promedio durante el tiempo de entrega
            Dim demandaPromedio As Double = demandaDiaria * tiempoEntrega

            ' Calcular stock de seguridad
            stockSeguridad = valorZ * desviacion * Math.Sqrt(tiempoEntrega)

            ' Calcular punto de reorden
            puntoReorden = demandaPromedio + stockSeguridad

            ' Calcular inventarios
            Dim inventarioMaximo As Double = eoq + stockSeguridad
            Dim inventarioPromedio As Double = (eoq / 2) + stockSeguridad

            ' Actualizar labels
            Me.Controls.Find("lblResultadoPuntoReorden", True)(0).Text = $"Punto de Reorden (ROP): {puntoReorden:F0} unidades"
            Me.Controls.Find("lblStockSeguridad", True)(0).Text = $"Stock de Seguridad: {stockSeguridad:F0} unidades"
            Me.Controls.Find("lblDemandaPromedio", True)(0).Text = $"Demanda promedio durante entrega: {demandaPromedio:F0} unidades"
            Me.Controls.Find("lblInventarioMaximo", True)(0).Text = $"Inventario máximo: {inventarioMaximo:F0} unidades"
            Me.Controls.Find("lblInventarioPromedio", True)(0).Text = $"Inventario promedio: {inventarioPromedio:F0} unidades"

        Else
            MessageBox.Show("Por favor ingrese valores válidos para todos los campos y seleccione un nivel de servicio.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Function ObtenerValorZ(indice As Integer) As Double
        Select Case indice
            Case 0 : Return 1.28  ' 90%
            Case 1 : Return 1.65  ' 95%
            Case 2 : Return 1.96  ' 97.5%
            Case 3 : Return 2.33  ' 99%
            Case 4 : Return 2.58  ' 99.5%
            Case 5 : Return 3.09  ' 99.9%
            Case Else : Return 1.96
        End Select
    End Function

    Private Sub RealizarAnalisisSensibilidad(sender As Object, e As EventArgs)
        If eoq = 0 Then
            MessageBox.Show("Primero debe calcular el EOQ.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim txtDemanda As TextBox = Me.Controls.Find("txtDemandaAnual", True)(0)
        Dim txtCostoOrden As TextBox = Me.Controls.Find("txtCostoOrden", True)(0)
        Dim txtCostoMantenimiento As TextBox = Me.Controls.Find("txtCostoMantenimiento", True)(0)

        Dim demanda, costoOrden, costoMantenimiento As Double
        Double.TryParse(txtDemanda.Text, demanda)
        Double.TryParse(txtCostoOrden.Text, costoOrden)
        Double.TryParse(txtCostoMantenimiento.Text, costoMantenimiento)

        ' Calcular variaciones del 20%
        Dim eoqDemandaMas20 As Double = Math.Sqrt((2 * demanda * 1.2 * costoOrden) / costoMantenimiento)
        Dim eoqCostoOrdenMas20 As Double = Math.Sqrt((2 * demanda * costoOrden * 1.2) / costoMantenimiento)
        Dim eoqCostoMantMas20 As Double = Math.Sqrt((2 * demanda * costoOrden) / (costoMantenimiento * 1.2))

        Dim variacionDemanda As Double = ((eoqDemandaMas20 - eoq) / eoq) * 100
        Dim variacionCostoOrden As Double = ((eoqCostoOrdenMas20 - eoq) / eoq) * 100
        Dim variacionCostoMant As Double = ((eoqCostoMantMas20 - eoq) / eoq) * 100

        Dim resultado As String = $"ANÁLISIS DE SENSIBILIDAD (variación +20%):" & vbCrLf &
                                $"• Demanda +20% → EOQ cambia {variacionDemanda:F1}%" & vbCrLf &
                                $"• Costo orden +20% → EOQ cambia {variacionCostoOrden:F1}%" & vbCrLf &
                                $"• Costo mant. +20% → EOQ cambia {variacionCostoMant:F1}%"

        Me.Controls.Find("lblResultadoAnalisis", True)(0).Text = resultado
    End Sub

    Private Sub GenerarReporte(sender As Object, e As EventArgs)
        If eoq = 0 Then
            MessageBox.Show("Primero debe realizar los cálculos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim reporte As String = "REPORTE DE ANÁLISIS DE INVENTARIOS (EOQ)" & vbCrLf & vbCrLf &
                               "Fecha: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm") & vbCrLf & vbCrLf &
                               "PARÁMETROS DE ENTRADA:" & vbCrLf &
                               "• Demanda anual: " & Me.Controls.Find("txtDemandaAnual", True)(0).Text & " unidades" & vbCrLf &
                               "• Costo por orden: $" & Me.Controls.Find("txtCostoOrden", True)(0).Text & vbCrLf &
                               "• Costo de mantenimiento: $" & Me.Controls.Find("txtCostoMantenimiento", True)(0).Text & " por unidad/año" & vbCrLf &
                               "• Tiempo de entrega: " & Me.Controls.Find("txtTiempoEntrega", True)(0).Text & " días" & vbCrLf & vbCrLf &
                               "RESULTADOS PRINCIPALES:" & vbCrLf &
                               "• EOQ: " & eoq.ToString("F0") & " unidades" & vbCrLf &
                               "• Punto de Reorden: " & puntoReorden.ToString("F0") & " unidades" & vbCrLf &
                               "• Stock de Seguridad: " & stockSeguridad.ToString("F0") & " unidades" & vbCrLf &
                               "• Número de órdenes anuales: " & numeroOrdenesAnual.ToString("F1") & vbCrLf &
                               "• Costo total anual: $" & costoTotalAnual.ToString("F2") & vbCrLf & vbCrLf &
                               "RECOMENDACIONES:" & vbCrLf &
                               "• Realizar pedidos de " & eoq.ToString("F0") & " unidades cada vez" & vbCrLf &
                               "• Reordenar cuando el inventario llegue a " & puntoReorden.ToString("F0") & " unidades" & vbCrLf &
                               "• Mantener un stock de seguridad de " & stockSeguridad.ToString("F0") & " unidades"

        MessageBox.Show(reporte, "Reporte de Inventarios EOQ", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LimpiarFormulario()
        ' Limpiar todos los campos de texto
        For Each control As Control In Me.Controls
            If TypeOf control Is Panel Then
                LimpiarControlesEnPanel(control)
            End If
        Next

        ' Resetear variables
        eoq = 0
        puntoReorden = 0
        stockSeguridad = 0
        costoTotalAnual = 0
        numeroOrdenesAnual = 0

        ' Resetear labels de resultados
        Me.Controls.Find("lblResultadoEOQ", True)(0).Text = "Cantidad Económica de Pedido (EOQ): -- unidades"
        Me.Controls.Find("lblNumeroOrdenes", True)(0).Text = "Número de órdenes por año: -- órdenes"
        Me.Controls.Find("lblCostoTotalAnual", True)(0).Text = "Costo total anual: $--"
        Me.Controls.Find("lblCostoOrdenAnual", True)(0).Text = "Costo de ordenar anual: $--"
        Me.Controls.Find("lblCostoMantenimientoAnual", True)(0).Text = "Costo de mantenimiento anual: $--"
        Me.Controls.Find("lblResultadoPuntoReorden", True)(0).Text = "Punto de Reorden (ROP): -- unidades"
        Me.Controls.Find("lblStockSeguridad", True)(0).Text = "Stock de Seguridad: -- unidades"
        Me.Controls.Find("lblDemandaPromedio", True)(0).Text = "Demanda promedio durante entrega: -- unidades"
        Me.Controls.Find("lblInventarioMaximo", True)(0).Text = "Inventario máximo: -- unidades"
        Me.Controls.Find("lblInventarioPromedio", True)(0).Text = "Inventario promedio: -- unidades"
        Me.Controls.Find("lblResultadoAnalisis", True)(0).Text = "Haga clic en 'Realizar Análisis' para ver el impacto de cambios en D, S y H sobre el EOQ"
    End Sub

    Private Sub LimpiarControlesEnPanel(panel As Control)
        For Each control As Control In panel.Controls
            If TypeOf control Is TextBox Then
                DirectCast(control, TextBox).Clear()
            ElseIf TypeOf control Is GroupBox Then
                LimpiarControlesEnPanel(control)
            End If
        Next
    End Sub

    Private Sub NuevoAnalisis(sender As Object, e As EventArgs)
        If MessageBox.Show("¿Desea crear un nuevo análisis? Se perderán los datos actuales.", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            LimpiarFormulario()
        End If
    End Sub

    Private Sub MostrarAyuda(sender As Object, e As EventArgs)
        Dim ayuda As String = "CALCULADORA DE INVENTARIOS (EOQ)" & vbCrLf & vbCrLf &
                             "Esta herramienta calcula la cantidad económica de pedido y el punto de reorden:" & vbCrLf & vbCrLf &
                             "1. CANTIDAD ECONÓMICA DE PEDIDO (EOQ):" & vbCrLf &
                             "   Fórmula: EOQ = √(2DS/H)" & vbCrLf &
                             "   Donde:" & vbCrLf &
                             "   • D = Demanda anual (unidades)" & vbCrLf &
                             "   • S = Costo por orden ($)" & vbCrLf &
                             "   • H = Costo de mantenimiento ($/unidad/año)" & vbCrLf & vbCrLf &
                             "2. PUNTO DE REORDEN (ROP):" & vbCrLf &
                             "   Fórmula: ROP = Demanda promedio × Tiempo entrega + Stock seguridad" & vbCrLf &
                             "   Stock seguridad = Z × σ × √L" & vbCrLf &
                             "   Donde:" & vbCrLf &
                             "   • Z = Valor Z para el nivel de servicio deseado" & vbCrLf &
                             "   • σ = Desviación estándar de la demanda diaria" & vbCrLf &
                             "   • L = Tiempo de entrega (días)" & vbCrLf & vbCrLf &
                             "3. ANÁLISIS DE SENSIBILIDAD:" & vbCrLf &
                             "   Muestra cómo cambia el EOQ ante variaciones en los parámetros" & vbCrLf & vbCrLf &
                             "NIVELES DE SERVICIO TÍPICOS:" & vbCrLf &
                             "• 90% - Productos de baja criticidad" & vbCrLf &
                             "• 95% - Productos estándar" & vbCrLf &
                             "• 97.5% - Productos importantes" & vbCrLf &
                             "• 99% - Productos críticos"

        MessageBox.Show(ayuda, "Ayuda - Calculadora de Inventarios EOQ", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub CerrarFormulario(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

End Class
