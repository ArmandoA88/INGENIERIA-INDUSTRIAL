' =====================================================
' CALCULADORA DE PRODUCTIVIDAD - INTEGRADA
' Ingeniería Industrial - Visual Basic .NET
' =====================================================
' Autor: Repositorio Ingeniería Industrial
' Fecha: 2025
' Descripción: Calculadora de productividad integrada en la suite principal
' =====================================================

Public Class FormCalculadoraProductividad
    Inherits Form

    ' ===== VARIABLES GLOBALES =====
    Private productividadLaboral As Double = 0
    Private productividadMateriales As Double = 0
    Private productividadMaquinaria As Double = 0
    Private oeeGlobal As Double = 0

    ' ===== CONSTRUCTOR =====
    Public Sub New()
        InitializeComponent()
    End Sub

    ' ===== INICIALIZACIÓN DEL FORMULARIO =====
    Private Sub InitializeComponent()
        ' Configuración inicial del formulario
        Me.Text = "Calculadora de Productividad - Ingeniería Industrial"
        Me.Size = New Size(900, 700)
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
            .Text = "CALCULADORA DE PRODUCTIVIDAD",
            .Font = New Font("Arial", 16, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panelPrincipal.Controls.Add(lblTitulo)

        ' === SECCIÓN 1: PRODUCTIVIDAD LABORAL ===
        Dim gbLaboral As New GroupBox With {
            .Text = "1. Productividad Laboral",
            .Size = New Size(400, 160),
            .Location = New Point(20, 60)
        }
        panelPrincipal.Controls.Add(gbLaboral)

        Dim lblUnidadesProducidas As New Label With {
            .Text = "Unidades producidas:",
            .Location = New Point(10, 30),
            .Size = New Size(120, 20)
        }
        gbLaboral.Controls.Add(lblUnidadesProducidas)

        Dim txtUnidadesProducidas As New TextBox With {
            .Name = "txtUnidadesProducidas",
            .Size = New Size(100, 25),
            .Location = New Point(140, 30)
        }
        gbLaboral.Controls.Add(txtUnidadesProducidas)

        Dim lblHorasHombre As New Label With {
            .Text = "Horas-hombre:",
            .Location = New Point(10, 65),
            .Size = New Size(120, 20)
        }
        gbLaboral.Controls.Add(lblHorasHombre)

        Dim txtHorasHombre As New TextBox With {
            .Name = "txtHorasHombre",
            .Size = New Size(100, 25),
            .Location = New Point(140, 65)
        }
        gbLaboral.Controls.Add(txtHorasHombre)

        Dim btnCalcularLaboral As New Button With {
            .Name = "btnCalcularLaboral",
            .Text = "Calcular",
            .Size = New Size(80, 30),
            .Location = New Point(260, 45),
            .BackColor = Color.LightBlue
        }
        gbLaboral.Controls.Add(btnCalcularLaboral)

        Dim lblResultadoLaboral As New Label With {
            .Name = "lblResultadoLaboral",
            .Text = "Productividad Laboral: -- unid/h-h",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen,
            .Location = New Point(10, 110),
            .Size = New Size(350, 20)
        }
        gbLaboral.Controls.Add(lblResultadoLaboral)

        ' === SECCIÓN 2: PRODUCTIVIDAD DE MATERIALES ===
        Dim gbMateriales As New GroupBox With {
            .Text = "2. Productividad de Materiales",
            .Size = New Size(400, 160),
            .Location = New Point(440, 60)
        }
        panelPrincipal.Controls.Add(gbMateriales)

        Dim lblUnidadesProducidasMat As New Label With {
            .Text = "Unidades producidas:",
            .Location = New Point(10, 30),
            .Size = New Size(120, 20)
        }
        gbMateriales.Controls.Add(lblUnidadesProducidasMat)

        Dim txtUnidadesProducidasMat As New TextBox With {
            .Name = "txtUnidadesProducidasMat",
            .Size = New Size(100, 25),
            .Location = New Point(140, 30)
        }
        gbMateriales.Controls.Add(txtUnidadesProducidasMat)

        Dim lblMaterialUtilizado As New Label With {
            .Text = "Material utilizado (kg):",
            .Location = New Point(10, 65),
            .Size = New Size(120, 20)
        }
        gbMateriales.Controls.Add(lblMaterialUtilizado)

        Dim txtMaterialUtilizado As New TextBox With {
            .Name = "txtMaterialUtilizado",
            .Size = New Size(100, 25),
            .Location = New Point(140, 65)
        }
        gbMateriales.Controls.Add(txtMaterialUtilizado)

        Dim btnCalcularMateriales As New Button With {
            .Name = "btnCalcularMateriales",
            .Text = "Calcular",
            .Size = New Size(80, 30),
            .Location = New Point(260, 45),
            .BackColor = Color.LightGreen
        }
        gbMateriales.Controls.Add(btnCalcularMateriales)

        Dim lblResultadoMateriales As New Label With {
            .Name = "lblResultadoMateriales",
            .Text = "Productividad Materiales: -- unid/kg",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen,
            .Location = New Point(10, 110),
            .Size = New Size(350, 20)
        }
        gbMateriales.Controls.Add(lblResultadoMateriales)

        ' === SECCIÓN 3: PRODUCTIVIDAD DE MAQUINARIA ===
        Dim gbMaquinaria As New GroupBox With {
            .Text = "3. Productividad de Maquinaria",
            .Size = New Size(400, 160),
            .Location = New Point(20, 240)
        }
        panelPrincipal.Controls.Add(gbMaquinaria)

        Dim lblUnidadesProducidasMaq As New Label With {
            .Text = "Unidades producidas:",
            .Location = New Point(10, 30),
            .Size = New Size(120, 20)
        }
        gbMaquinaria.Controls.Add(lblUnidadesProducidasMaq)

        Dim txtUnidadesProducidasMaq As New TextBox With {
            .Name = "txtUnidadesProducidasMaq",
            .Size = New Size(100, 25),
            .Location = New Point(140, 30)
        }
        gbMaquinaria.Controls.Add(txtUnidadesProducidasMaq)

        Dim lblHorasMaquina As New Label With {
            .Text = "Horas-máquina:",
            .Location = New Point(10, 65),
            .Size = New Size(120, 20)
        }
        gbMaquinaria.Controls.Add(lblHorasMaquina)

        Dim txtHorasMaquina As New TextBox With {
            .Name = "txtHorasMaquina",
            .Size = New Size(100, 25),
            .Location = New Point(140, 65)
        }
        gbMaquinaria.Controls.Add(txtHorasMaquina)

        Dim btnCalcularMaquinaria As New Button With {
            .Name = "btnCalcularMaquinaria",
            .Text = "Calcular",
            .Size = New Size(80, 30),
            .Location = New Point(260, 45),
            .BackColor = Color.LightYellow
        }
        gbMaquinaria.Controls.Add(btnCalcularMaquinaria)

        Dim lblResultadoMaquinaria As New Label With {
            .Name = "lblResultadoMaquinaria",
            .Text = "Productividad Maquinaria: -- unid/h-m",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen,
            .Location = New Point(10, 110),
            .Size = New Size(350, 20)
        }
        gbMaquinaria.Controls.Add(lblResultadoMaquinaria)

        ' === SECCIÓN 4: EFICIENCIA GLOBAL (OEE) ===
        Dim gbOEE As New GroupBox With {
            .Text = "4. Eficiencia Global (OEE)",
            .Size = New Size(400, 200),
            .Location = New Point(440, 240)
        }
        panelPrincipal.Controls.Add(gbOEE)

        Dim lblDisponibilidad As New Label With {
            .Text = "Disponibilidad (%):",
            .Location = New Point(10, 30),
            .Size = New Size(120, 20)
        }
        gbOEE.Controls.Add(lblDisponibilidad)

        Dim txtDisponibilidad As New TextBox With {
            .Name = "txtDisponibilidad",
            .Size = New Size(100, 25),
            .Location = New Point(140, 30)
        }
        gbOEE.Controls.Add(txtDisponibilidad)

        Dim lblRendimiento As New Label With {
            .Text = "Rendimiento (%):",
            .Location = New Point(10, 65),
            .Size = New Size(120, 20)
        }
        gbOEE.Controls.Add(lblRendimiento)

        Dim txtRendimiento As New TextBox With {
            .Name = "txtRendimiento",
            .Size = New Size(100, 25),
            .Location = New Point(140, 65)
        }
        gbOEE.Controls.Add(txtRendimiento)

        Dim lblCalidad As New Label With {
            .Text = "Calidad (%):",
            .Location = New Point(10, 100),
            .Size = New Size(120, 20)
        }
        gbOEE.Controls.Add(lblCalidad)

        Dim txtCalidad As New TextBox With {
            .Name = "txtCalidad",
            .Size = New Size(100, 25),
            .Location = New Point(140, 100)
        }
        gbOEE.Controls.Add(txtCalidad)

        Dim btnCalcularOEE As New Button With {
            .Name = "btnCalcularOEE",
            .Text = "Calcular OEE",
            .Size = New Size(100, 30),
            .Location = New Point(260, 60),
            .BackColor = Color.LightCoral
        }
        gbOEE.Controls.Add(btnCalcularOEE)

        Dim lblResultadoOEE As New Label With {
            .Name = "lblResultadoOEE",
            .Text = "OEE: -- %",
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.DarkRed,
            .Location = New Point(10, 145),
            .Size = New Size(350, 25)
        }
        gbOEE.Controls.Add(lblResultadoOEE)

        ' === SECCIÓN 5: RESUMEN GENERAL ===
        Dim gbResumen As New GroupBox With {
            .Text = "5. Resumen General de Productividad",
            .Size = New Size(820, 120),
            .Location = New Point(20, 460)
        }
        panelPrincipal.Controls.Add(gbResumen)

        Dim lblResumenLaboral As New Label With {
            .Name = "lblResumenLaboral",
            .Text = "Productividad Laboral: -- unid/h-h",
            .Location = New Point(20, 30),
            .Size = New Size(200, 20)
        }
        gbResumen.Controls.Add(lblResumenLaboral)

        Dim lblResumenMateriales As New Label With {
            .Name = "lblResumenMateriales",
            .Text = "Productividad Materiales: -- unid/kg",
            .Location = New Point(240, 30),
            .Size = New Size(200, 20)
        }
        gbResumen.Controls.Add(lblResumenMateriales)

        Dim lblResumenMaquinaria As New Label With {
            .Name = "lblResumenMaquinaria",
            .Text = "Productividad Maquinaria: -- unid/h-m",
            .Location = New Point(460, 30),
            .Size = New Size(200, 20)
        }
        gbResumen.Controls.Add(lblResumenMaquinaria)

        Dim lblResumenOEE As New Label With {
            .Name = "lblResumenOEE",
            .Text = "Eficiencia Global (OEE): -- %",
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .Location = New Point(680, 30),
            .Size = New Size(130, 20)
        }
        gbResumen.Controls.Add(lblResumenOEE)

        Dim btnGenerarReporte As New Button With {
            .Name = "btnGenerarReporte",
            .Text = "Generar Reporte",
            .Size = New Size(120, 35),
            .Location = New Point(350, 65),
            .BackColor = Color.DarkBlue,
            .ForeColor = Color.White,
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResumen.Controls.Add(btnGenerarReporte)

        ' === BOTONES DE ACCIÓN ===
        Dim btnNuevo As New Button With {
            .Name = "btnNuevo",
            .Text = "Nuevo Análisis",
            .Size = New Size(120, 35),
            .Location = New Point(400, 600),
            .BackColor = Color.DarkGreen,
            .ForeColor = Color.White
        }
        panelPrincipal.Controls.Add(btnNuevo)

        Dim btnAyuda As New Button With {
            .Name = "btnAyuda",
            .Text = "Ayuda",
            .Size = New Size(120, 35),
            .Location = New Point(530, 600)
        }
        panelPrincipal.Controls.Add(btnAyuda)

        Dim btnCerrar As New Button With {
            .Name = "btnCerrar",
            .Text = "Cerrar",
            .Size = New Size(120, 35),
            .Location = New Point(660, 600),
            .BackColor = Color.Red,
            .ForeColor = Color.White
        }
        panelPrincipal.Controls.Add(btnCerrar)
    End Sub

    ' ===== CONFIGURACIÓN DE EVENTOS =====
    Private Sub ConfigurarEventos()
        ' Buscar controles y asignar eventos
        AddHandler Me.Controls.Find("btnCalcularLaboral", True)(0).Click, AddressOf CalcularProductividadLaboral
        AddHandler Me.Controls.Find("btnCalcularMateriales", True)(0).Click, AddressOf CalcularProductividadMateriales
        AddHandler Me.Controls.Find("btnCalcularMaquinaria", True)(0).Click, AddressOf CalcularProductividadMaquinaria
        AddHandler Me.Controls.Find("btnCalcularOEE", True)(0).Click, AddressOf CalcularOEE
        AddHandler Me.Controls.Find("btnGenerarReporte", True)(0).Click, AddressOf GenerarReporte
        AddHandler Me.Controls.Find("btnNuevo", True)(0).Click, AddressOf NuevoAnalisis
        AddHandler Me.Controls.Find("btnAyuda", True)(0).Click, AddressOf MostrarAyuda
        AddHandler Me.Controls.Find("btnCerrar", True)(0).Click, AddressOf CerrarFormulario
    End Sub

    ' ===== MÉTODOS DE CÁLCULO =====
    Private Sub CalcularProductividadLaboral(sender As Object, e As EventArgs)
        Dim txtUnidades As TextBox = Me.Controls.Find("txtUnidadesProducidas", True)(0)
        Dim txtHoras As TextBox = Me.Controls.Find("txtHorasHombre", True)(0)
        Dim lblResultado As Label = Me.Controls.Find("lblResultadoLaboral", True)(0)
        Dim lblResumen As Label = Me.Controls.Find("lblResumenLaboral", True)(0)

        Dim unidades, horas As Double
        If Double.TryParse(txtUnidades.Text, unidades) AndAlso Double.TryParse(txtHoras.Text, horas) AndAlso horas > 0 Then
            productividadLaboral = unidades / horas
            lblResultado.Text = $"Productividad Laboral: {productividadLaboral:F2} unid/h-h"
            lblResumen.Text = $"Productividad Laboral: {productividadLaboral:F2} unid/h-h"
        Else
            MessageBox.Show("Por favor ingrese valores válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CalcularProductividadMateriales(sender As Object, e As EventArgs)
        Dim txtUnidades As TextBox = Me.Controls.Find("txtUnidadesProducidasMat", True)(0)
        Dim txtMaterial As TextBox = Me.Controls.Find("txtMaterialUtilizado", True)(0)
        Dim lblResultado As Label = Me.Controls.Find("lblResultadoMateriales", True)(0)
        Dim lblResumen As Label = Me.Controls.Find("lblResumenMateriales", True)(0)

        Dim unidades, material As Double
        If Double.TryParse(txtUnidades.Text, unidades) AndAlso Double.TryParse(txtMaterial.Text, material) AndAlso material > 0 Then
            productividadMateriales = unidades / material
            lblResultado.Text = $"Productividad Materiales: {productividadMateriales:F2} unid/kg"
            lblResumen.Text = $"Productividad Materiales: {productividadMateriales:F2} unid/kg"
        Else
            MessageBox.Show("Por favor ingrese valores válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CalcularProductividadMaquinaria(sender As Object, e As EventArgs)
        Dim txtUnidades As TextBox = Me.Controls.Find("txtUnidadesProducidasMaq", True)(0)
        Dim txtHoras As TextBox = Me.Controls.Find("txtHorasMaquina", True)(0)
        Dim lblResultado As Label = Me.Controls.Find("lblResultadoMaquinaria", True)(0)
        Dim lblResumen As Label = Me.Controls.Find("lblResumenMaquinaria", True)(0)

        Dim unidades, horas As Double
        If Double.TryParse(txtUnidades.Text, unidades) AndAlso Double.TryParse(txtHoras.Text, horas) AndAlso horas > 0 Then
            productividadMaquinaria = unidades / horas
            lblResultado.Text = $"Productividad Maquinaria: {productividadMaquinaria:F2} unid/h-m"
            lblResumen.Text = $"Productividad Maquinaria: {productividadMaquinaria:F2} unid/h-m"
        Else
            MessageBox.Show("Por favor ingrese valores válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub CalcularOEE(sender As Object, e As EventArgs)
        Dim txtDisponibilidad As TextBox = Me.Controls.Find("txtDisponibilidad", True)(0)
        Dim txtRendimiento As TextBox = Me.Controls.Find("txtRendimiento", True)(0)
        Dim txtCalidad As TextBox = Me.Controls.Find("txtCalidad", True)(0)
        Dim lblResultado As Label = Me.Controls.Find("lblResultadoOEE", True)(0)
        Dim lblResumen As Label = Me.Controls.Find("lblResumenOEE", True)(0)

        Dim disponibilidad, rendimiento, calidad As Double
        If Double.TryParse(txtDisponibilidad.Text, disponibilidad) AndAlso 
           Double.TryParse(txtRendimiento.Text, rendimiento) AndAlso 
           Double.TryParse(txtCalidad.Text, calidad) Then
            
            oeeGlobal = (disponibilidad / 100) * (rendimiento / 100) * (calidad / 100) * 100
            lblResultado.Text = $"OEE: {oeeGlobal:F1} %"
            lblResumen.Text = $"Eficiencia Global (OEE): {oeeGlobal:F1} %"
            
            ' Cambiar color según el nivel de OEE
            If oeeGlobal >= 85 Then
                lblResultado.ForeColor = Color.DarkGreen
            ElseIf oeeGlobal >= 65 Then
                lblResultado.ForeColor = Color.Orange
            Else
                lblResultado.ForeColor = Color.Red
            End If
        Else
            MessageBox.Show("Por favor ingrese valores válidos para todos los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub GenerarReporte(sender As Object, e As EventArgs)
        Dim reporte As String = "REPORTE DE PRODUCTIVIDAD" & vbCrLf & vbCrLf &
                               "Fecha: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm") & vbCrLf & vbCrLf &
                               "RESULTADOS:" & vbCrLf &
                               "• Productividad Laboral: " & If(productividadLaboral > 0, $"{productividadLaboral:F2} unid/h-h", "No calculada") & vbCrLf &
                               "• Productividad de Materiales: " & If(productividadMateriales > 0, $"{productividadMateriales:F2} unid/kg", "No calculada") & vbCrLf &
                               "• Productividad de Maquinaria: " & If(productividadMaquinaria > 0, $"{productividadMaquinaria:F2} unid/h-m", "No calculada") & vbCrLf &
                               "• Eficiencia Global (OEE): " & If(oeeGlobal > 0, $"{oeeGlobal:F1} %", "No calculada") & vbCrLf & vbCrLf &
                               "INTERPRETACIÓN OEE:" & vbCrLf &
                               "• Excelente: ≥ 85%" & vbCrLf &
                               "• Bueno: 65% - 84%" & vbCrLf &
                               "• Regular: < 65%"

        MessageBox.Show(reporte, "Reporte de Productividad", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LimpiarFormulario()
        ' Limpiar todos los campos de texto
        For Each control As Control In Me.Controls
            If TypeOf control Is Panel Then
                LimpiarControlesEnPanel(control)
            End If
        Next

        ' Resetear variables
        productividadLaboral = 0
        productividadMateriales = 0
        productividadMaquinaria = 0
        oeeGlobal = 0

        ' Resetear labels de resultados
        Me.Controls.Find("lblResultadoLaboral", True)(0).Text = "Productividad Laboral: -- unid/h-h"
        Me.Controls.Find("lblResultadoMateriales", True)(0).Text = "Productividad Materiales: -- unid/kg"
        Me.Controls.Find("lblResultadoMaquinaria", True)(0).Text = "Productividad Maquinaria: -- unid/h-m"
        Me.Controls.Find("lblResultadoOEE", True)(0).Text = "OEE: -- %"
        Me.Controls.Find("lblResumenLaboral", True)(0).Text = "Productividad Laboral: -- unid/h-h"
        Me.Controls.Find("lblResumenMateriales", True)(0).Text = "Productividad Materiales: -- unid/kg"
        Me.Controls.Find("lblResumenMaquinaria", True)(0).Text = "Productividad Maquinaria: -- unid/h-m"
        Me.Controls.Find("lblResumenOEE", True)(0).Text = "Eficiencia Global (OEE): -- %"
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
        Dim ayuda As String = "CALCULADORA DE PRODUCTIVIDAD" & vbCrLf & vbCrLf &
                             "Esta herramienta permite calcular diferentes métricas de productividad:" & vbCrLf & vbCrLf &
                             "1. PRODUCTIVIDAD LABORAL:" & vbCrLf &
                             "   Fórmula: Unidades Producidas / Horas-Hombre" & vbCrLf &
                             "   Mide la eficiencia del trabajo humano" & vbCrLf & vbCrLf &
                             "2. PRODUCTIVIDAD DE MATERIALES:" & vbCrLf &
                             "   Fórmula: Unidades Producidas / Material Utilizado" & vbCrLf &
                             "   Mide la eficiencia en el uso de materiales" & vbCrLf & vbCrLf &
                             "3. PRODUCTIVIDAD DE MAQUINARIA:" & vbCrLf &
                             "   Fórmula: Unidades Producidas / Horas-Máquina" & vbCrLf &
                             "   Mide la eficiencia de los equipos" & vbCrLf & vbCrLf &
                             "4. EFICIENCIA GLOBAL (OEE):" & vbCrLf &
                             "   Fórmula: Disponibilidad × Rendimiento × Calidad" & vbCrLf &
                             "   Mide la eficiencia global del proceso productivo" & vbCrLf & vbCrLf &
                             "NIVELES OEE:" & vbCrLf &
                             "• Excelente: ≥ 85% (Clase Mundial)" & vbCrLf &
                             "• Bueno: 65% - 84% (Aceptable)" & vbCrLf &
                             "• Regular: < 65% (Requiere mejora)"

        MessageBox.Show(ayuda, "Ayuda - Calculadora de Productividad", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub CerrarFormulario(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

End Class
