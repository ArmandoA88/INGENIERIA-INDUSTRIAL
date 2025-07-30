' =====================================================
' SIMULADOR DE TEORÍA DE COLAS
' Modelos M/M/1, M/M/c, M/M/1/K
' =====================================================
' Autor: Ingeniería Industrial App
' Fecha: 2025
' Descripción: Simulador completo para análisis de
'              sistemas de colas con múltiples modelos
' =====================================================

Option Strict Off
Imports System.Math
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class FormSimuladorColas

    ' ===== VARIABLES GLOBALES =====
    Private resultados As Dictionary(Of String, Double)
    Private datosSimulacion As List(Of Dictionary(Of String, Object))
    Private tiempoSimulacion As Integer = 0
    Private clientesEnSistema As Integer = 0
    Private servidoresOcupados As Integer = 0

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub FormSimuladorColas_Load(sender As Object, e As EventArgs) Handles Me.Load
        ConfigurarFormulario()
        InicializarControles()
        InicializarResultados()
    End Sub

    ' ===== CONFIGURACIÓN INICIAL =====
    Private Sub ConfigurarFormulario()
        Me.Text = "Simulador de Teoría de Colas - Ingeniería Industrial"
        Me.Size = New Size(1400, 900)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 248, 255)
        Me.MinimumSize = New Size(1200, 800)
    End Sub

    Private Sub InicializarControles()
        ' Panel principal con scroll
        Dim panelPrincipal As New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .Padding = New Padding(10)
        }
        Me.Controls.Add(panelPrincipal)

        ' === HEADER ===
        CrearHeader(panelPrincipal)

        ' === PANEL DE PARÁMETROS ===
        CrearPanelParametros(panelPrincipal, 80)

        ' === PANEL DE SIMULACIÓN ===
        CrearPanelSimulacion(panelPrincipal, 320)

        ' === PANEL DE RESULTADOS ===
        CrearPanelResultados(panelPrincipal, 560)

        ' === PANEL DE GRÁFICOS ===
        CrearPanelGraficos(panelPrincipal, 800)
    End Sub

    Private Sub CrearHeader(panel As Panel)
        Dim lblTitulo As New Label With {
            .Text = "🎮 SIMULADOR DE TEORÍA DE COLAS",
            .Font = New Font("Arial", 20, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panel.Controls.Add(lblTitulo)

        Dim lblSubtitulo As New Label With {
            .Text = "Análisis de sistemas de espera • Modelos M/M/1, M/M/c, M/M/1/K • Optimización de servidores",
            .Font = New Font("Arial", 12, FontStyle.Italic),
            .ForeColor = Color.DarkSlateGray,
            .AutoSize = True,
            .Location = New Point(20, 50)
        }
        panel.Controls.Add(lblSubtitulo)
    End Sub

    ' ===== PANEL DE PARÁMETROS =====
    Private Sub CrearPanelParametros(panel As Panel, yPos As Integer)
        Dim gbParametros As New GroupBox With {
            .Text = "📊 Parámetros del Sistema",
            .Size = New Size(1320, 220),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbParametros)

        ' === MODELO DE COLA ===
        Dim lblModelo As New Label With {
            .Text = "Modelo de Cola:",
            .Location = New Point(20, 30),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbParametros.Controls.Add(lblModelo)

        Dim cmbModelo As New ComboBox With {
            .Name = "cmbModelo",
            .Location = New Point(150, 30),
            .Size = New Size(200, 25),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbModelo.Items.AddRange({"M/M/1 (Un servidor)", "M/M/c (Múltiples servidores)", "M/M/1/K (Capacidad limitada)"})
        cmbModelo.SelectedIndex = 0
        gbParametros.Controls.Add(cmbModelo)

        ' === TASA DE LLEGADA (λ) ===
        Dim lblLambda As New Label With {
            .Text = "Tasa de Llegada (λ):",
            .Location = New Point(20, 70),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbParametros.Controls.Add(lblLambda)

        Dim txtLambda As New TextBox With {
            .Name = "txtLambda",
            .Location = New Point(150, 70),
            .Size = New Size(100, 25),
            .Text = "8"
        }
        gbParametros.Controls.Add(txtLambda)

        Dim lblLambdaUnidad As New Label With {
            .Text = "clientes/hora",
            .Location = New Point(260, 70),
            .Size = New Size(80, 25),
            .ForeColor = Color.Gray
        }
        gbParametros.Controls.Add(lblLambdaUnidad)

        ' === TASA DE SERVICIO (μ) ===
        Dim lblMu As New Label With {
            .Text = "Tasa de Servicio (μ):",
            .Location = New Point(20, 110),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbParametros.Controls.Add(lblMu)

        Dim txtMu As New TextBox With {
            .Name = "txtMu",
            .Location = New Point(150, 110),
            .Size = New Size(100, 25),
            .Text = "10"
        }
        gbParametros.Controls.Add(txtMu)

        Dim lblMuUnidad As New Label With {
            .Text = "clientes/hora",
            .Location = New Point(260, 110),
            .Size = New Size(80, 25),
            .ForeColor = Color.Gray
        }
        gbParametros.Controls.Add(lblMuUnidad)

        ' === NÚMERO DE SERVIDORES ===
        Dim lblServidores As New Label With {
            .Text = "Número de Servidores (c):",
            .Location = New Point(20, 150),
            .Size = New Size(150, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbParametros.Controls.Add(lblServidores)

        Dim nudServidores As New NumericUpDown With {
            .Name = "nudServidores",
            .Location = New Point(180, 150),
            .Size = New Size(70, 25),
            .Minimum = 1,
            .Maximum = 20,
            .Value = 1
        }
        gbParametros.Controls.Add(nudServidores)

        ' === CAPACIDAD DEL SISTEMA ===
        Dim lblCapacidad As New Label With {
            .Text = "Capacidad del Sistema (K):",
            .Location = New Point(380, 30),
            .Size = New Size(160, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbParametros.Controls.Add(lblCapacidad)

        Dim nudCapacidad As New NumericUpDown With {
            .Name = "nudCapacidad",
            .Location = New Point(550, 30),
            .Size = New Size(70, 25),
            .Minimum = 1,
            .Maximum = 100,
            .Value = 20
        }
        gbParametros.Controls.Add(nudCapacidad)

        ' === TIEMPO DE SIMULACIÓN ===
        Dim lblTiempo As New Label With {
            .Text = "Tiempo de Simulación:",
            .Location = New Point(380, 70),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbParametros.Controls.Add(lblTiempo)

        Dim nudTiempo As New NumericUpDown With {
            .Name = "nudTiempo",
            .Location = New Point(530, 70),
            .Size = New Size(90, 25),
            .Minimum = 100,
            .Maximum = 10000,
            .Value = 1000,
            .Increment = 100
        }
        gbParametros.Controls.Add(nudTiempo)

        Dim lblTiempoUnidad As New Label With {
            .Text = "horas",
            .Location = New Point(630, 70),
            .Size = New Size(50, 25),
            .ForeColor = Color.Gray
        }
        gbParametros.Controls.Add(lblTiempoUnidad)

        ' === BOTONES DE ACCIÓN ===
        Dim btnCalcular As New Button With {
            .Name = "btnCalcular",
            .Text = "🔄 Calcular Métricas",
            .Location = New Point(750, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightGreen,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbParametros.Controls.Add(btnCalcular)

        Dim btnSimular As New Button With {
            .Name = "btnSimular",
            .Text = "🎮 Ejecutar Simulación",
            .Location = New Point(920, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightBlue,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbParametros.Controls.Add(btnSimular)

        Dim btnLimpiar As New Button With {
            .Name = "btnLimpiar",
            .Text = "🗑️ Limpiar",
            .Location = New Point(1090, 30),
            .Size = New Size(100, 40),
            .BackColor = Color.LightCoral,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbParametros.Controls.Add(btnLimpiar)

        Dim btnExportar As New Button With {
            .Name = "btnExportar",
            .Text = "📊 Exportar",
            .Location = New Point(1210, 30),
            .Size = New Size(100, 40),
            .BackColor = Color.LightYellow,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbParametros.Controls.Add(btnExportar)

        ' === INFORMACIÓN DEL MODELO ===
        Dim lblInfo As New Label With {
            .Name = "lblInfoModelo",
            .Text = "M/M/1: Un servidor, llegadas Poisson, servicio exponencial",
            .Location = New Point(380, 110),
            .Size = New Size(500, 50),
            .ForeColor = Color.DarkBlue,
            .Font = New Font("Arial", 9, FontStyle.Italic)
        }
        gbParametros.Controls.Add(lblInfo)

        ' Configurar eventos
        AddHandler cmbModelo.SelectedIndexChanged, AddressOf CambiarModelo
        AddHandler btnCalcular.Click, AddressOf CalcularMetricas
        AddHandler btnSimular.Click, AddressOf EjecutarSimulacion
        AddHandler btnLimpiar.Click, AddressOf LimpiarResultados
        AddHandler btnExportar.Click, AddressOf ExportarResultados
    End Sub

    ' ===== PANEL DE SIMULACIÓN ===
    Private Sub CrearPanelSimulacion(panel As Panel, yPos As Integer)
        Dim gbSimulacion As New GroupBox With {
            .Text = "🎮 Estado de la Simulación",
            .Size = New Size(1320, 220),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbSimulacion)

        ' === INDICADORES EN TIEMPO REAL ===
        Dim lblTiempoActual As New Label With {
            .Name = "lblTiempoActual",
            .Text = "Tiempo Actual: 0 horas",
            .Location = New Point(20, 30),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblTiempoActual)

        Dim lblClientesSistema As New Label With {
            .Name = "lblClientesSistema",
            .Text = "Clientes en Sistema: 0",
            .Location = New Point(240, 30),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblClientesSistema)

        Dim lblServidoresOcupados As New Label With {
            .Name = "lblServidoresOcupados",
            .Text = "Servidores Ocupados: 0",
            .Location = New Point(460, 30),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblServidoresOcupados)

        ' === BARRA DE PROGRESO ===
        Dim lblProgreso As New Label With {
            .Text = "Progreso de Simulación:",
            .Location = New Point(20, 70),
            .Size = New Size(150, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblProgreso)

        Dim progressBar As New ProgressBar With {
            .Name = "progressBarSimulacion",
            .Location = New Point(180, 70),
            .Size = New Size(400, 25),
            .Style = ProgressBarStyle.Continuous
        }
        gbSimulacion.Controls.Add(progressBar)

        Dim lblPorcentaje As New Label With {
            .Name = "lblPorcentaje",
            .Text = "0%",
            .Location = New Point(590, 70),
            .Size = New Size(50, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblPorcentaje)

        ' === VISUALIZACIÓN DEL SISTEMA ===
        Dim panelVisualizacion As New Panel With {
            .Name = "panelVisualizacion",
            .Location = New Point(20, 110),
            .Size = New Size(1280, 90),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.AliceBlue
        }
        gbSimulacion.Controls.Add(panelVisualizacion)

        AddHandler panelVisualizacion.Paint, AddressOf DibujarSistema
    End Sub

    ' ===== PANEL DE RESULTADOS =====
    Private Sub CrearPanelResultados(panel As Panel, yPos As Integer)
        Dim gbResultados As New GroupBox With {
            .Text = "📊 Resultados del Análisis",
            .Size = New Size(1320, 220),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbResultados)

        ' === MÉTRICAS PRINCIPALES ===
        Dim lblUtilizacion As New Label With {
            .Name = "lblUtilizacion",
            .Text = "Utilización del Sistema (ρ): -",
            .Location = New Point(20, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblUtilizacion)

        Dim lblClientesPromedio As New Label With {
            .Name = "lblClientesPromedio",
            .Text = "Clientes Promedio en Sistema (L): -",
            .Location = New Point(340, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblClientesPromedio)

        Dim lblClientesCola As New Label With {
            .Name = "lblClientesCola",
            .Text = "Clientes Promedio en Cola (Lq): -",
            .Location = New Point(660, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblClientesCola)

        Dim lblTiempoSistema As New Label With {
            .Name = "lblTiempoSistema",
            .Text = "Tiempo Promedio en Sistema (W): -",
            .Location = New Point(20, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblTiempoSistema)

        Dim lblTiempoCola As New Label With {
            .Name = "lblTiempoCola",
            .Text = "Tiempo Promedio en Cola (Wq): -",
            .Location = New Point(340, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblTiempoCola)

        Dim lblProbabilidadVacio As New Label With {
            .Name = "lblProbabilidadVacio",
            .Text = "Probabilidad Sistema Vacío (P0): -",
            .Location = New Point(660, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblProbabilidadVacio)

        ' === ANÁLISIS DE COSTOS ===
        Dim lblCostoEspera As New Label With {
            .Name = "lblCostoEspera",
            .Text = "Costo de Espera: -",
            .Location = New Point(20, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkRed
        }
        gbResultados.Controls.Add(lblCostoEspera)

        Dim lblCostoServicio As New Label With {
            .Name = "lblCostoServicio",
            .Text = "Costo de Servicio: -",
            .Location = New Point(240, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkBlue
        }
        gbResultados.Controls.Add(lblCostoServicio)

        Dim lblCostoTotal As New Label With {
            .Name = "lblCostoTotal",
            .Text = "Costo Total: -",
            .Location = New Point(460, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen
        }
        gbResultados.Controls.Add(lblCostoTotal)

        ' === RECOMENDACIONES ===
        Dim txtRecomendaciones As New TextBox With {
            .Name = "txtRecomendaciones",
            .Location = New Point(20, 150),
            .Size = New Size(1280, 50),
            .Multiline = True,
            .ReadOnly = True,
            .ScrollBars = ScrollBars.Vertical,
            .Text = "Ejecute el análisis para obtener recomendaciones del sistema...",
            .BackColor = Color.LightYellow
        }
        gbResultados.Controls.Add(txtRecomendaciones)
    End Sub

    ' ===== PANEL DE GRÁFICOS =====
    Private Sub CrearPanelGraficos(panel As Panel, yPos As Integer)
        Dim gbGraficos As New GroupBox With {
            .Text = "📈 Análisis Gráfico",
            .Size = New Size(1320, 300),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbGraficos)

        ' Panel para gráfico de evolución temporal
        Dim panelGrafico As New Panel With {
            .Name = "panelGrafico",
            .Location = New Point(20, 30),
            .Size = New Size(1280, 250),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        gbGraficos.Controls.Add(panelGrafico)

        AddHandler panelGrafico.Paint, AddressOf DibujarGrafico
    End Sub

    ' ===== MÉTODOS DE CÁLCULO =====
    Private Sub CalcularMetricas(sender As Object, e As EventArgs)
        Try
            Dim lambda As Double = Convert.ToDouble(Me.Controls.Find("txtLambda", True)(0).Text)
            Dim mu As Double = Convert.ToDouble(Me.Controls.Find("txtMu", True)(0).Text)
            Dim c As Integer = Convert.ToInt32(DirectCast(Me.Controls.Find("nudServidores", True)(0), NumericUpDown).Value)
            Dim k As Integer = Convert.ToInt32(DirectCast(Me.Controls.Find("nudCapacidad", True)(0), NumericUpDown).Value)
            Dim modelo As String = DirectCast(Me.Controls.Find("cmbModelo", True)(0), ComboBox).SelectedItem.ToString()

            ' Validaciones
            If lambda <= 0 Or mu <= 0 Then
                MessageBox.Show("Las tasas de llegada y servicio deben ser positivas.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Calcular según el modelo seleccionado
            Select Case modelo
                Case "M/M/1 (Un servidor)"
                    CalcularMM1(lambda, mu)
                Case "M/M/c (Múltiples servidores)"
                    CalcularMMc(lambda, mu, c)
                Case "M/M/1/K (Capacidad limitada)"
                    CalcularMM1K(lambda, mu, k)
            End Select

            ActualizarResultados()
            GenerarRecomendaciones()

        Catch ex As Exception
            MessageBox.Show("Error en el cálculo: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CalcularMM1(lambda As Double, mu As Double)
        If lambda >= mu Then
            MessageBox.Show("Para el modelo M/M/1, la tasa de llegada debe ser menor que la tasa de servicio (λ < μ).", "Sistema Inestable", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim rho As Double = lambda / mu
        Dim L As Double = rho / (1 - rho)
        Dim Lq As Double = (rho * rho) / (1 - rho)
        Dim W As Double = 1 / (mu - lambda)
        Dim Wq As Double = rho / (mu - lambda)
        Dim P0 As Double = 1 - rho

        resultados = New Dictionary(Of String, Double) From {
            {"rho", rho},
            {"L", L},
            {"Lq", Lq},
            {"W", W},
            {"Wq", Wq},
            {"P0", P0}
        }
    End Sub

    Private Sub CalcularMMc(lambda As Double, mu As Double, c As Integer)
        Dim rho As Double = lambda / mu
        Dim rho_c As Double = rho / c

        If rho_c >= 1 Then
            MessageBox.Show("Para el modelo M/M/c, ρ/c debe ser menor que 1.", "Sistema Inestable", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Calcular P0 para M/M/c
        Dim suma1 As Double = 0
        For n As Integer = 0 To c - 1
            suma1 += (rho ^ n) / Factorial(n)
        Next

        Dim termino2 As Double = (rho ^ c) / (Factorial(c) * (1 - rho_c))
        Dim P0 As Double = 1 / (suma1 + termino2)

        ' Calcular otras métricas
        Dim Lq As Double = (P0 * (rho ^ (c + 1))) / (Factorial(c) * ((1 - rho_c) ^ 2))
        Dim L As Double = Lq + rho
        Dim Wq As Double = Lq / lambda
        Dim W As Double = Wq + (1 / mu)

        resultados = New Dictionary(Of String, Double) From {
            {"rho", rho_c},
            {"L", L},
            {"Lq", Lq},
            {"W", W},
            {"Wq", Wq},
            {"P0", P0}
        }
    End Sub

    Private Sub CalcularMM1K(lambda As Double, mu As Double, k As Integer)
        Dim rho As Double = lambda / mu
        Dim P0 As Double

        If rho = 1 Then
            P0 = 1 / (k + 1)
        Else
            P0 = (1 - rho) / (1 - (rho ^ (k + 1)))
        End If

        ' Calcular lambda efectiva
        Dim Pk As Double = P0 * (rho ^ k)
        Dim lambda_eff As Double = lambda * (1 - Pk)

        ' Calcular métricas
        Dim L As Double
        If rho = 1 Then
            L = k / 2
        Else
            L = (rho * (1 - (k + 1) * (rho ^ k) + k * (rho ^ (k + 1)))) / ((1 - rho) * (1 - (rho ^ (k + 1))))
        End If

        Dim Lq As Double = L - (lambda_eff / mu)
        Dim W As Double = L / lambda_eff
        Dim Wq As Double = Lq / lambda_eff

        resultados = New Dictionary(Of String, Double) From {
            {"rho", lambda_eff / mu},
            {"L", L},
            {"Lq", Lq},
            {"W", W},
            {"Wq", Wq},
            {"P0", P0},
            {"lambda_eff", lambda_eff}
        }
    End Sub

    Private Function Factorial(n As Integer) As Double
        If n <= 1 Then Return 1
        Dim result As Double = 1
        For i As Integer = 2 To n
            result *= i
        Next
        Return result
    End Function

    ' ===== SIMULACIÓN DISCRETA =====
    Private Sub EjecutarSimulacion(sender As Object, e As EventArgs)
        Try
            Dim lambda As Double = Convert.ToDouble(Me.Controls.Find("txtLambda", True)(0).Text)
            Dim mu As Double = Convert.ToDouble(Me.Controls.Find("txtMu", True)(0).Text)
            Dim tiempoMax As Integer = Convert.ToInt32(DirectCast(Me.Controls.Find("nudTiempo", True)(0), NumericUpDown).Value)

            ' Inicializar simulación
            datosSimulacion = New List(Of Dictionary(Of String, Object))
            tiempoSimulacion = 0
            clientesEnSistema = 0
            servidoresOcupados = 0

            Dim progressBar As ProgressBar = DirectCast(Me.Controls.Find("progressBarSimulacion", True)(0), ProgressBar)
            Dim lblPorcentaje As Label = DirectCast(Me.Controls.Find("lblPorcentaje", True)(0), Label)

            progressBar.Maximum = tiempoMax
            progressBar.Value = 0

            ' Ejecutar simulación paso a paso
            Dim random As New Random()
            Dim tiempoProximaLlegada As Double = -Log(random.NextDouble()) / lambda
            Dim tiempoProximaSalida As Double = Double.MaxValue

            For tiempo As Integer = 0 To tiempoMax Step 1
                ' Actualizar progreso
                progressBar.Value = tiempo
                lblPorcentaje.Text = Math.Round((tiempo / tiempoMax) * 100, 1) & "%"
                Application.DoEvents()

                ' Procesar llegadas
                If tiempo >= tiempoProximaLlegada Then
                    clientesEnSistema += 1
                    If servidoresOcupados = 0 Then
                        servidoresOcupados = 1
                        tiempoProximaSalida = tiempo + (-Log(random.NextDouble()) / mu)
                    End If
                    tiempoProximaLlegada = tiempo + (-Log(random.NextDouble()) / lambda)
                End If

                ' Procesar salidas
                If tiempo >= tiempoProximaSalida And servidoresOcupados > 0 Then
                    clientesEnSistema -= 1
                    If clientesEnSistema > 0 Then
                        tiempoProximaSalida = tiempo + (-Log(random.NextDouble()) / mu)
                    Else
                        servidoresOcupados = 0
                        tiempoProximaSalida = Double.MaxValue
                    End If
                End If

                ' Guardar datos para gráfico
                If tiempo Mod 10 = 0 Then ' Cada 10 unidades de tiempo
                    datosSimulacion.Add(New Dictionary(Of String, Object) From {
                        {"tiempo", tiempo},
                        {"clientes", clientesEnSistema},
                        {"servidores", servidoresOcupados}
                    })
                End If

                ' Actualizar indicadores en tiempo real
                ActualizarIndicadoresSimulacion(tiempo, clientesEnSistema, servidoresOcupados)
            Next

            MessageBox.Show("Simulación completada exitosamente.", "Simulación", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Controls.Find("panelGrafico", True)(0).Invalidate() ' Redibujar gráfico

        Catch ex As Exception
            MessageBox.Show("Error en la simulación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===== MÉTODOS AUXILIARES =====
    Private Sub InicializarResultados()
        resultados = New Dictionary(Of String, Double)
        datosSimulacion = New List(Of Dictionary(Of String, Object))
    End Sub

    Private Sub ActualizarResultados()
        If resultados Is Nothing Then Return

        ' Actualizar labels con resultados
        DirectCast(Me.Controls.Find("lblUtilizacion", True)(0), Label).Text = 
            "Utilización del Sistema (ρ): " & Math.Round(resultados("rho"), 4).ToString("F4")

        DirectCast(Me.Controls.Find("lblClientesPromedio", True)(0), Label).Text = 
            "Clientes Promedio en Sistema (L): " & Math.Round(resultados("L"), 4).ToString("F4")

        DirectCast(Me.Controls.Find("lblClientesCola", True)(0), Label).Text = 
            "Clientes Promedio en Cola (Lq): " & Math.Round(resultados("Lq"), 4).ToString("F4")

        DirectCast(Me.Controls.Find("lblTiempoSistema", True)(0), Label).Text = 
            "Tiempo Promedio en Sistema (W): " & Math.Round(resultados("W"), 4).ToString("F4") & " horas"

        DirectCast(Me.Controls.Find("lblTiempoCola", True)(0), Label).Text = 
            "Tiempo Promedio en Cola (Wq): " & Math.Round(resultados("Wq"), 4).ToString("F4") & " horas"

        DirectCast(Me.Controls.Find("lblProbabilidadVacio", True)(0), Label).Text = 
            "Probabilidad Sistema Vacío (P0): " & Math.Round(resultados("P0"), 4).ToString("F4")

        ' Calcular costos (valores ejemplo)
        Dim costoEspera As Double = resultados("L") * 25 ' $25 por cliente esperando
        Dim costoServicio As Double = 100 ' $100 por servidor
        Dim costoTotal As Double = costoEspera + costoServicio

        DirectCast(Me.Controls.Find("lblCostoEspera", True)(0), Label).Text = 
            "Costo de Espera: $" & Math.Round(costoEspera, 2).ToString("F2")

        DirectCast(Me.Controls.Find("lblCostoServicio", True)(0), Label).Text = 
            "Costo de Servicio: $" & Math.Round(costoServicio, 2).ToString("F2")

        DirectCast(Me.Controls.Find("lblCostoTotal", True)(0), Label).Text = 
            "Costo Total: $" & Math.Round(costoTotal, 2).ToString("F2")
    End Sub

    Private Sub GenerarRecomendaciones()
        If resultados Is Nothing Then Return

        Dim recomendaciones As New List(Of String)
        Dim rho As Double = resultados("rho")
        Dim L As Double = resultados("L")
        Dim Wq As Double = resultados("Wq")

        ' Análisis de utilización
        If rho < 0.5 Then
            recomendaciones.Add("• BAJA UTILIZACIÓN: El sistema está subutilizado (" & Math.Round(rho * 100, 1) & "%). Considere reducir servidores.")
        ElseIf rho > 0.8 Then
            recomendaciones.Add("• ALTA UTILIZACIÓN: El sistema está sobrecargado (" & Math.Round(rho * 100, 1) & "%). Considere agregar servidores.")
        Else
            recomendaciones.Add("• UTILIZACIÓN ÓPTIMA: El sistema tiene una utilización balanceada (" & Math.Round(rho * 100, 1) & "%).")
        End If

        ' Análisis de tiempo de espera
        If Wq > 0.5 Then
            recomendaciones.Add("• TIEMPO DE ESPERA ALTO: Los clientes esperan " & Math.Round(Wq, 2) & " horas en promedio. Mejore el servicio.")
        ElseIf Wq < 0.1 Then
            recomendaciones.Add("• TIEMPO DE ESPERA BAJO: Excelente tiempo de respuesta (" & Math.Round(Wq * 60, 1) & " minutos).")
        End If

        ' Análisis de clientes en sistema
        If L > 10 Then
            recomendaciones.Add("• MUCHOS CLIENTES: Hay " & Math.Round(L, 1) & " clientes promedio en el sistema. Considere optimizar.")
        End If

        ' Recomendaciones específicas
        recomendaciones.Add("• Para mejorar el servicio: Aumentar μ (capacitación, tecnología, procesos)")
        recomendaciones.Add("• Para reducir llegadas: Implementar citas, distribución de demanda")
        recomendaciones.Add("• Punto óptimo: Balancear costo de servicio vs. costo de espera")

        DirectCast(Me.Controls.Find("txtRecomendaciones", True)(0), TextBox).Text = String.Join(vbCrLf, recomendaciones)
    End Sub

    Private Sub ActualizarIndicadoresSimulacion(tiempo As Integer, clientes As Integer, servidores As Integer)
        DirectCast(Me.Controls.Find("lblTiempoActual", True)(0), Label).Text = 
            "Tiempo Actual: " & tiempo & " horas"

        DirectCast(Me.Controls.Find("lblClientesSistema", True)(0), Label).Text = 
            "Clientes en Sistema: " & clientes

        DirectCast(Me.Controls.Find("lblServidoresOcupados", True)(0), Label).Text = 
            "Servidores Ocupados: " & servidores

        ' Actualizar visualización
        Me.Controls.Find("panelVisualizacion", True)(0).Invalidate()
    End Sub

    Private Sub CambiarModelo(sender As Object, e As EventArgs)
        Dim combo As ComboBox = DirectCast(sender, ComboBox)
        Dim lblInfo As Label = DirectCast(Me.Controls.Find("lblInfoModelo", True)(0), Label)

        Select Case combo.SelectedIndex
            Case 0 ' M/M/1
                lblInfo.Text = "M/M/1: Un servidor, llegadas Poisson, servicio exponencial" & vbCrLf &
                              "Aplicable: Ventanillas únicas, cajeros automáticos"
            Case 1 ' M/M/c
                lblInfo.Text = "M/M/c: Múltiples servidores, llegadas Poisson, servicio exponencial" & vbCrLf &
                              "Aplicable: Bancos, call centers, supermercados"
            Case 2 ' M/M/1/K
                lblInfo.Text = "M/M/1/K: Un servidor con capacidad limitada" & vbCrLf &
                              "Aplicable: Sistemas con espacio limitado, buffers"
        End Select
    End Sub

    Private Sub LimpiarResultados(sender As Object, e As EventArgs)
        ' Limpiar todos los resultados
        InicializarResultados()
        
        ' Resetear labels
        DirectCast(Me.Controls.Find("lblUtilizacion", True)(0), Label).Text = "Utilización del Sistema (ρ): -"
        DirectCast(Me.Controls.Find("lblClientesPromedio", True)(0), Label).Text = "Clientes Promedio en Sistema (L): -"
        DirectCast(Me.Controls.Find("lblClientesCola", True)(0), Label).Text = "Clientes Promedio en Cola (Lq): -"
        DirectCast(Me.Controls.Find("lblTiempoSistema", True)(0), Label).Text = "Tiempo Promedio en Sistema (W): -"
        DirectCast(Me.Controls.Find("lblTiempoCola", True)(0), Label).Text = "Tiempo Promedio en Cola (Wq): -"
        DirectCast(Me.Controls.Find("lblProbabilidadVacio", True)(0), Label).Text = "Probabilidad Sistema Vacío (P0): -"
        DirectCast(Me.Controls.Find("lblCostoEspera", True)(0), Label).Text = "Costo de Espera: -"
        DirectCast(Me.Controls.Find("lblCostoServicio", True)(0), Label).Text = "Costo de Servicio: -"
        DirectCast(Me.Controls.Find("lblCostoTotal", True)(0), Label).Text = "Costo Total: -"
        
        ' Limpiar recomendaciones
        DirectCast(Me.Controls.Find("txtRecomendaciones", True)(0), TextBox).Text = 
            "Ejecute el análisis para obtener recomendaciones del sistema..."
        
        ' Resetear progreso
        DirectCast(Me.Controls.Find("progressBarSimulacion", True)(0), ProgressBar).Value = 0
        DirectCast(Me.Controls.Find("lblPorcentaje", True)(0), Label).Text = "0%"
        
        ' Limpiar gráficos
        Me.Controls.Find("panelGrafico", True)(0).Invalidate()
        Me.Controls.Find("panelVisualizacion", True)(0).Invalidate()
    End Sub

    Private Sub ExportarResultados(sender As Object, e As EventArgs)
        If resultados Is Nothing OrElse resultados.Count = 0 Then
            MessageBox.Show("No hay resultados para exportar. Ejecute primero el análisis.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim saveDialog As New SaveFileDialog With {
                .Filter = "Archivo CSV|*.csv|Archivo de Texto|*.txt",
                .Title = "Exportar Resultados de Simulación de Colas",
                .FileName = "SimulacionColas_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
            }

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim contenido As New List(Of String) From {
                    "SIMULADOR DE TEORÍA DE COLAS - RESULTADOS",
                    "Fecha: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    "",
                    "PARÁMETROS:",
                    "Tasa de Llegada (λ): " & DirectCast(Me.Controls.Find("txtLambda", True)(0), TextBox).Text,
                    "Tasa de Servicio (μ): " & DirectCast(Me.Controls.Find("txtMu", True)(0), TextBox).Text,
                    "Modelo: " & DirectCast(Me.Controls.Find("cmbModelo", True)(0), ComboBox).SelectedItem.ToString(),
                    "",
                    "RESULTADOS:",
                    "Utilización (ρ): " & Math.Round(resultados("rho"), 4).ToString("F4"),
                    "Clientes en Sistema (L): " & Math.Round(resultados("L"), 4).ToString("F4"),
                    "Clientes en Cola (Lq): " & Math.Round(resultados("Lq"), 4).ToString("F4"),
                    "Tiempo en Sistema (W): " & Math.Round(resultados("W"), 4).ToString("F4") & " horas",
                    "Tiempo en Cola (Wq): " & Math.Round(resultados("Wq"), 4).ToString("F4") & " horas",
                    "Probabilidad Sistema Vacío (P0): " & Math.Round(resultados("P0"), 4).ToString("F4"),
                    "",
                    "RECOMENDACIONES:",
                    DirectCast(Me.Controls.Find("txtRecomendaciones", True)(0), TextBox).Text
                }

                System.IO.File.WriteAllLines(saveDialog.FileName, contenido)
                MessageBox.Show("Resultados exportados exitosamente a:" & vbCrLf & saveDialog.FileName, 
                              "Exportación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===== MÉTODOS DE DIBUJO =====
    Private Sub DibujarSistema(sender As Object, e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Dibujar llegadas
        Dim brushLlegada As New SolidBrush(Color.Blue)
        g.FillEllipse(brushLlegada, 50, 30, 30, 30)
        g.DrawString("λ", New Font("Arial", 12, FontStyle.Bold), Brushes.White, CSng(60), CSng(40))

        ' Dibujar flecha de llegada
        Dim pen As New Pen(Color.Black, 2)
        pen.EndCap = LineCap.ArrowAnchor
        g.DrawLine(pen, 90, 45, 150, 45)

        ' Dibujar cola
        Dim brushCola As New SolidBrush(Color.Orange)
        For i As Integer = 0 To Math.Min(clientesEnSistema - 1, 5)
            g.FillRectangle(brushCola, 160 + (i * 25), 35, 20, 20)
        Next

        ' Dibujar servidor
        Dim brushServidor As New SolidBrush(If(servidoresOcupados > 0, Color.Red, Color.Green))
        g.FillRectangle(brushServidor, 400, 25, 60, 40)
        g.DrawString("μ", New Font("Arial", 12, FontStyle.Bold), Brushes.White, CSng(425), CSng(40))

        ' Dibujar flecha de salida
        g.DrawLine(pen, 470, 45, 530, 45)

        ' Etiquetas
        g.DrawString("Llegadas", New Font("Arial", 9), Brushes.Black, CSng(40), CSng(10))
        g.DrawString("Cola de Espera", New Font("Arial", 9), Brushes.Black, CSng(160), CSng(10))
        g.DrawString("Servidor", New Font("Arial", 9), Brushes.Black, CSng(410), CSng(10))
        g.DrawString("Salidas", New Font("Arial", 9), Brushes.Black, CSng(530), CSng(40))

        ' Información del estado
        g.DrawString("Clientes en sistema: " & clientesEnSistema, New Font("Arial", 10, FontStyle.Bold), Brushes.DarkBlue, CSng(50), CSng(70))
    End Sub

    Private Sub DibujarGrafico(sender As Object, e As PaintEventArgs)
        If datosSimulacion Is Nothing OrElse datosSimulacion.Count = 0 Then
            Dim g As Graphics = e.Graphics
            TextRenderer.DrawText(g, "Ejecute la simulación para ver el gráfico de evolución temporal", 
                                New Font("Arial", 12), New Point(50, 100), Color.Gray)
            Return
        End If

        DibujarGraficoEvolucion(e.Graphics)
    End Sub

    Private Sub DibujarGraficoEvolucion(g As Graphics)
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Configuración del gráfico
        Dim margenX As Integer = 60
        Dim margenY As Integer = 40
        Dim anchoGrafico As Integer = 1160
        Dim altoGrafico As Integer = 170

        ' Encontrar valores máximos
        Dim maxTiempo As Integer = datosSimulacion.Max(Function(d) CInt(d("tiempo")))
        Dim maxClientes As Integer = datosSimulacion.Max(Function(d) CInt(d("clientes")))

        If maxClientes = 0 Then maxClientes = 1

        ' Dibujar ejes
        Dim penEjes As New Pen(Color.Black, 2)
        g.DrawLine(penEjes, margenX, margenY + altoGrafico, margenX + anchoGrafico, margenY + altoGrafico) ' Eje X
        g.DrawLine(penEjes, margenX, margenY, margenX, margenY + altoGrafico) ' Eje Y

        ' Etiquetas de ejes
        g.DrawString("Tiempo (horas)", New Font("Arial", 10), Brushes.Black, CSng(margenX + anchoGrafico / 2), CSng(margenY + altoGrafico + 20))
        g.TranslateTransform(20, margenY + altoGrafico / 2)
        g.RotateTransform(-90)
        g.DrawString("Clientes en Sistema", New Font("Arial", 10), Brushes.Black, 0, 0)
        g.ResetTransform()

        ' Dibujar línea de datos
        If datosSimulacion.Count > 1 Then
            Dim puntos As New List(Of PointF)
            
            For Each dato In datosSimulacion
                Dim x As Single = margenX + (CInt(dato("tiempo")) / maxTiempo) * anchoGrafico
                Dim y As Single = margenY + altoGrafico - (CInt(dato("clientes")) / maxClientes) * altoGrafico
                puntos.Add(New PointF(x, y))
            Next

            If puntos.Count > 1 Then
                Dim penLinea As New Pen(Color.Blue, 2)
                g.DrawLines(penLinea, puntos.ToArray())
            End If
        End If

        ' Marcas en los ejes
        For i As Integer = 0 To 5
            Dim x As Integer = margenX + (i * anchoGrafico / 5)
            Dim valorX As Integer = (i * maxTiempo / 5)
            g.DrawString(valorX.ToString(), New Font("Arial", 8), Brushes.Black, CSng(x - 10), CSng(margenY + altoGrafico + 5))
            
            Dim y As Integer = margenY + altoGrafico - (i * altoGrafico / 5)
            Dim valorY As Integer = (i * maxClientes / 5)
            g.DrawString(valorY.ToString(), New Font("Arial", 8), Brushes.Black, CSng(margenX - 30), CSng(y - 5))
        Next
    End Sub

End Class
