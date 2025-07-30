' =====================================================
' SIMULADOR DE GESTIÓN DE INVENTARIO
' Modelos de demanda variable y optimización de políticas
' =====================================================
' Autor: Ingeniería Industrial App
' Fecha: 2025
' Descripción: Simulador completo para análisis de
'              políticas de inventario y optimización
' =====================================================

Option Strict Off
Imports System.Math
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class FormSimuladorInventario

    ' ===== VARIABLES GLOBALES =====
    Private parametrosInventario As Dictionary(Of String, Object)
    Private resultadosSimulacion As Dictionary(Of String, Object)
    Private datosSimulacion As List(Of Dictionary(Of String, Object))
    Private tiempoSimulacion As Integer = 0
    Private inventarioActual As Integer = 0
    Private costosAcumulados As Double = 0

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub FormSimuladorInventario_Load(sender As Object, e As EventArgs) Handles Me.Load
        ConfigurarFormulario()
        InicializarControles()
        InicializarDatos()
    End Sub

    ' ===== CONFIGURACIÓN INICIAL =====
    Private Sub ConfigurarFormulario()
        Me.Text = "Simulador de Gestión de Inventario - Ingeniería Industrial"
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

        ' === PANEL DE CONFIGURACIÓN ===
        CrearPanelConfiguracion(panelPrincipal, 80)

        ' === PANEL DE POLÍTICAS ===
        CrearPanelPoliticas(panelPrincipal, 320)

        ' === PANEL DE SIMULACIÓN ===
        CrearPanelSimulacion(panelPrincipal, 520)

        ' === PANEL DE RESULTADOS ===
        CrearPanelResultados(panelPrincipal, 720)

        ' === PANEL DE GRÁFICOS ===
        CrearPanelGraficos(panelPrincipal, 960)
    End Sub

    Private Sub CrearHeader(panel As Panel)
        Dim lblTitulo As New Label With {
            .Text = "📦 SIMULADOR DE GESTIÓN DE INVENTARIO",
            .Font = New Font("Arial", 20, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panel.Controls.Add(lblTitulo)

        Dim lblSubtitulo As New Label With {
            .Text = "Modelos de demanda variable • Políticas de inventario • Análisis de costos • Optimización de stock",
            .Font = New Font("Arial", 12, FontStyle.Italic),
            .ForeColor = Color.DarkSlateGray,
            .AutoSize = True,
            .Location = New Point(20, 50)
        }
        panel.Controls.Add(lblSubtitulo)
    End Sub

    ' ===== PANEL DE CONFIGURACIÓN =====
    Private Sub CrearPanelConfiguracion(panel As Panel, yPos As Integer)
        Dim gbConfiguracion As New GroupBox With {
            .Text = "⚙️ Parámetros del Sistema de Inventario",
            .Size = New Size(1320, 220),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbConfiguracion)

        ' === DEMANDA ===
        Dim lblDemandaPromedio As New Label With {
            .Text = "Demanda Promedio:",
            .Location = New Point(20, 30),
            .Size = New Size(130, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblDemandaPromedio)

        Dim nudDemandaPromedio As New NumericUpDown With {
            .Name = "nudDemandaPromedio",
            .Location = New Point(160, 30),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 10000,
            .Value = 50
        }
        gbConfiguracion.Controls.Add(nudDemandaPromedio)

        Dim lblUnidadesDemanda As New Label With {
            .Text = "unidades/día",
            .Location = New Point(270, 30),
            .Size = New Size(80, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblUnidadesDemanda)

        Dim lblDesviacionDemanda As New Label With {
            .Text = "Desviación Estándar:",
            .Location = New Point(20, 70),
            .Size = New Size(130, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblDesviacionDemanda)

        Dim nudDesviacionDemanda As New NumericUpDown With {
            .Name = "nudDesviacionDemanda",
            .Location = New Point(160, 70),
            .Size = New Size(100, 25),
            .Minimum = 0,
            .Maximum = 1000,
            .Value = 10
        }
        gbConfiguracion.Controls.Add(nudDesviacionDemanda)

        ' === COSTOS ===
        Dim lblCostoOrden As New Label With {
            .Text = "Costo de Ordenar:",
            .Location = New Point(380, 30),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblCostoOrden)

        Dim nudCostoOrden As New NumericUpDown With {
            .Name = "nudCostoOrden",
            .Location = New Point(510, 30),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 10000,
            .Value = 100
        }
        gbConfiguracion.Controls.Add(nudCostoOrden)

        Dim lblDolaresOrden As New Label With {
            .Text = "$/orden",
            .Location = New Point(620, 30),
            .Size = New Size(50, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblDolaresOrden)

        Dim lblCostoMantener As New Label With {
            .Text = "Costo de Mantener:",
            .Location = New Point(380, 70),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblCostoMantener)

        Dim nudCostoMantener As New NumericUpDown With {
            .Name = "nudCostoMantener",
            .Location = New Point(510, 70),
            .Size = New Size(100, 25),
            .Minimum = 0.1,
            .Maximum = 100,
            .Value = 2,
            .DecimalPlaces = 2,
            .Increment = 0.1
        }
        gbConfiguracion.Controls.Add(nudCostoMantener)

        Dim lblDolaresMantener As New Label With {
            .Text = "$/und/día",
            .Location = New Point(620, 70),
            .Size = New Size(60, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblDolaresMantener)

        Dim lblCostoFaltante As New Label With {
            .Text = "Costo de Faltante:",
            .Location = New Point(380, 110),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblCostoFaltante)

        Dim nudCostoFaltante As New NumericUpDown With {
            .Name = "nudCostoFaltante",
            .Location = New Point(510, 110),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 1000,
            .Value = 20
        }
        gbConfiguracion.Controls.Add(nudCostoFaltante)

        Dim lblDolaresFaltante As New Label With {
            .Text = "$/und/día",
            .Location = New Point(620, 110),
            .Size = New Size(60, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblDolaresFaltante)

        ' === TIEMPO DE ENTREGA ===
        Dim lblTiempoEntrega As New Label With {
            .Text = "Tiempo de Entrega:",
            .Location = New Point(20, 110),
            .Size = New Size(130, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblTiempoEntrega)

        Dim nudTiempoEntrega As New NumericUpDown With {
            .Name = "nudTiempoEntrega",
            .Location = New Point(160, 110),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 365,
            .Value = 7
        }
        gbConfiguracion.Controls.Add(nudTiempoEntrega)

        Dim lblDiasEntrega As New Label With {
            .Text = "días",
            .Location = New Point(270, 110),
            .Size = New Size(40, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblDiasEntrega)

        ' === NIVEL DE SERVICIO ===
        Dim lblNivelServicio As New Label With {
            .Text = "Nivel de Servicio:",
            .Location = New Point(20, 150),
            .Size = New Size(130, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblNivelServicio)

        Dim nudNivelServicio As New NumericUpDown With {
            .Name = "nudNivelServicio",
            .Location = New Point(160, 150),
            .Size = New Size(100, 25),
            .Minimum = 50,
            .Maximum = 99.9,
            .Value = 95,
            .DecimalPlaces = 1,
            .Increment = 0.1
        }
        gbConfiguracion.Controls.Add(nudNivelServicio)

        Dim lblPorcentajeServicio As New Label With {
            .Text = "%",
            .Location = New Point(270, 150),
            .Size = New Size(20, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblPorcentajeServicio)

        ' === BOTONES DE ACCIÓN ===
        Dim btnCalcularEOQ As New Button With {
            .Name = "btnCalcularEOQ",
            .Text = "📊 Calcular EOQ",
            .Location = New Point(750, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightGreen,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnCalcularEOQ)

        Dim btnOptimizar As New Button With {
            .Name = "btnOptimizar",
            .Text = "🎯 Optimizar Política",
            .Location = New Point(920, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightBlue,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnOptimizar)

        Dim btnSimular As New Button With {
            .Name = "btnSimular",
            .Text = "🎮 Simular Sistema",
            .Location = New Point(1090, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightCoral,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnSimular)

        Dim btnExportar As New Button With {
            .Name = "btnExportar",
            .Text = "📋 Exportar",
            .Location = New Point(1090, 80),
            .Size = New Size(150, 40),
            .BackColor = Color.LightYellow,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnExportar)

        ' Configurar eventos
        AddHandler btnCalcularEOQ.Click, AddressOf CalcularEOQ
        AddHandler btnOptimizar.Click, AddressOf OptimizarPolitica
        AddHandler btnSimular.Click, AddressOf SimularSistema
        AddHandler btnExportar.Click, AddressOf ExportarResultados
    End Sub

    ' ===== PANEL DE POLÍTICAS ===
    Private Sub CrearPanelPoliticas(panel As Panel, yPos As Integer)
        Dim gbPoliticas As New GroupBox With {
            .Text = "📋 Políticas de Inventario",
            .Size = New Size(1320, 180),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbPoliticas)

        ' === POLÍTICA (Q, R) ===
        Dim gbPoliticaQR As New GroupBox With {
            .Text = "Política (Q, R) - Cantidad Fija",
            .Size = New Size(400, 140),
            .Location = New Point(20, 30),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbPoliticas.Controls.Add(gbPoliticaQR)

        Dim lblCantidadOrden As New Label With {
            .Text = "Cantidad de Orden (Q):",
            .Location = New Point(15, 30),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 9)
        }
        gbPoliticaQR.Controls.Add(lblCantidadOrden)

        Dim nudCantidadOrden As New NumericUpDown With {
            .Name = "nudCantidadOrden",
            .Location = New Point(165, 30),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 10000,
            .Value = 100
        }
        gbPoliticaQR.Controls.Add(nudCantidadOrden)

        Dim lblPuntoReorden As New Label With {
            .Text = "Punto de Reorden (R):",
            .Location = New Point(15, 70),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 9)
        }
        gbPoliticaQR.Controls.Add(lblPuntoReorden)

        Dim nudPuntoReorden As New NumericUpDown With {
            .Name = "nudPuntoReorden",
            .Location = New Point(165, 70),
            .Size = New Size(100, 25),
            .Minimum = 0,
            .Maximum = 10000,
            .Value = 50
        }
        gbPoliticaQR.Controls.Add(nudPuntoReorden)

        Dim chkUsarQR As New CheckBox With {
            .Name = "chkUsarQR",
            .Text = "Usar esta política",
            .Location = New Point(15, 105),
            .Size = New Size(150, 25),
            .Checked = True
        }
        gbPoliticaQR.Controls.Add(chkUsarQR)

        ' === POLÍTICA (S, s) ===
        Dim gbPoliticaSs As New GroupBox With {
            .Text = "Política (S, s) - Nivel Máximo/Mínimo",
            .Size = New Size(400, 140),
            .Location = New Point(440, 30),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbPoliticas.Controls.Add(gbPoliticaSs)

        Dim lblNivelMaximo As New Label With {
            .Text = "Nivel Máximo (S):",
            .Location = New Point(15, 30),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 9)
        }
        gbPoliticaSs.Controls.Add(lblNivelMaximo)

        Dim nudNivelMaximo As New NumericUpDown With {
            .Name = "nudNivelMaximo",
            .Location = New Point(165, 30),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 10000,
            .Value = 200
        }
        gbPoliticaSs.Controls.Add(nudNivelMaximo)

        Dim lblNivelMinimo As New Label With {
            .Text = "Nivel Mínimo (s):",
            .Location = New Point(15, 70),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 9)
        }
        gbPoliticaSs.Controls.Add(lblNivelMinimo)

        Dim nudNivelMinimo As New NumericUpDown With {
            .Name = "nudNivelMinimo",
            .Location = New Point(165, 70),
            .Size = New Size(100, 25),
            .Minimum = 0,
            .Maximum = 10000,
            .Value = 30
        }
        gbPoliticaSs.Controls.Add(nudNivelMinimo)

        Dim chkUsarSs As New CheckBox With {
            .Name = "chkUsarSs",
            .Text = "Usar esta política",
            .Location = New Point(15, 105),
            .Size = New Size(150, 25)
        }
        gbPoliticaSs.Controls.Add(chkUsarSs)

        ' === POLÍTICA (T, S) ===
        Dim gbPoliticaTS As New GroupBox With {
            .Text = "Política (T, S) - Revisión Periódica",
            .Size = New Size(400, 140),
            .Location = New Point(860, 30),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbPoliticas.Controls.Add(gbPoliticaTS)

        Dim lblPeriodoRevision As New Label With {
            .Text = "Período de Revisión (T):",
            .Location = New Point(15, 30),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 9)
        }
        gbPoliticaTS.Controls.Add(lblPeriodoRevision)

        Dim nudPeriodoRevision As New NumericUpDown With {
            .Name = "nudPeriodoRevision",
            .Location = New Point(165, 30),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 365,
            .Value = 30
        }
        gbPoliticaTS.Controls.Add(nudPeriodoRevision)

        Dim lblNivelObjetivo As New Label With {
            .Text = "Nivel Objetivo (S):",
            .Location = New Point(15, 70),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 9)
        }
        gbPoliticaTS.Controls.Add(lblNivelObjetivo)

        Dim nudNivelObjetivo As New NumericUpDown With {
            .Name = "nudNivelObjetivo",
            .Location = New Point(165, 70),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 10000,
            .Value = 150
        }
        gbPoliticaTS.Controls.Add(nudNivelObjetivo)

        Dim chkUsarTS As New CheckBox With {
            .Name = "chkUsarTS",
            .Text = "Usar esta política",
            .Location = New Point(15, 105),
            .Size = New Size(150, 25)
        }
        gbPoliticaTS.Controls.Add(chkUsarTS)

        ' Configurar eventos para checkboxes mutuamente excluyentes
        AddHandler chkUsarQR.CheckedChanged, AddressOf CambiarPolitica
        AddHandler chkUsarSs.CheckedChanged, AddressOf CambiarPolitica
        AddHandler chkUsarTS.CheckedChanged, AddressOf CambiarPolitica
    End Sub

    ' ===== PANEL DE SIMULACIÓN ===
    Private Sub CrearPanelSimulacion(panel As Panel, yPos As Integer)
        Dim gbSimulacion As New GroupBox With {
            .Text = "🎮 Estado de la Simulación",
            .Size = New Size(1320, 180),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbSimulacion)

        ' === INDICADORES EN TIEMPO REAL ===
        Dim lblDiaActual As New Label With {
            .Name = "lblDiaActual",
            .Text = "Día Actual: 0",
            .Location = New Point(20, 30),
            .Size = New Size(150, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblDiaActual)

        Dim lblInventarioActual As New Label With {
            .Name = "lblInventarioActual",
            .Text = "Inventario Actual: 0",
            .Location = New Point(190, 30),
            .Size = New Size(150, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblInventarioActual)

        Dim lblCostosAcumulados As New Label With {
            .Name = "lblCostosAcumulados",
            .Text = "Costos Acumulados: $0",
            .Location = New Point(360, 30),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblCostosAcumulados)

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

        ' === CONFIGURACIÓN DE SIMULACIÓN ===
        Dim lblDiasSimular As New Label With {
            .Text = "Días a Simular:",
            .Location = New Point(700, 30),
            .Size = New Size(100, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblDiasSimular)

        Dim nudDiasSimular As New NumericUpDown With {
            .Name = "nudDiasSimular",
            .Location = New Point(810, 30),
            .Size = New Size(100, 25),
            .Minimum = 30,
            .Maximum = 3650,
            .Value = 365
        }
        gbSimulacion.Controls.Add(nudDiasSimular)

        ' === VISUALIZACIÓN DEL INVENTARIO ===
        Dim panelVisualizacion As New Panel With {
            .Name = "panelVisualizacion",
            .Location = New Point(20, 110),
            .Size = New Size(1280, 50),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.AliceBlue
        }
        gbSimulacion.Controls.Add(panelVisualizacion)

        AddHandler panelVisualizacion.Paint, AddressOf DibujarInventario
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
        Dim lblEOQCalculado As New Label With {
            .Name = "lblEOQCalculado",
            .Text = "EOQ Calculado: -",
            .Location = New Point(20, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblEOQCalculado)

        Dim lblStockSeguridad As New Label With {
            .Name = "lblStockSeguridad",
            .Text = "Stock de Seguridad: -",
            .Location = New Point(340, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblStockSeguridad)

        Dim lblInventarioPromedio As New Label With {
            .Name = "lblInventarioPromedio",
            .Text = "Inventario Promedio: -",
            .Location = New Point(660, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblInventarioPromedio)

        Dim lblRotacionInventario As New Label With {
            .Name = "lblRotacionInventario",
            .Text = "Rotación de Inventario: -",
            .Location = New Point(20, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblRotacionInventario)

        Dim lblNivelServicioReal As New Label With {
            .Name = "lblNivelServicioReal",
            .Text = "Nivel de Servicio Real: -",
            .Location = New Point(340, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblNivelServicioReal)

        Dim lblFaltantes As New Label With {
            .Name = "lblFaltantes",
            .Text = "Faltantes Totales: -",
            .Location = New Point(660, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblFaltantes)

        ' === ANÁLISIS DE COSTOS ===
        Dim lblCostoOrdenTotal As New Label With {
            .Name = "lblCostoOrdenTotal",
            .Text = "Costo de Ordenar: -",
            .Location = New Point(20, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkBlue
        }
        gbResultados.Controls.Add(lblCostoOrdenTotal)

        Dim lblCostoMantenerTotal As New Label With {
            .Name = "lblCostoMantenerTotal",
            .Text = "Costo de Mantener: -",
            .Location = New Point(240, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen
        }
        gbResultados.Controls.Add(lblCostoMantenerTotal)

        Dim lblCostoFaltanteTotal As New Label With {
            .Name = "lblCostoFaltanteTotal",
            .Text = "Costo de Faltante: -",
            .Location = New Point(460, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkRed
        }
        gbResultados.Controls.Add(lblCostoFaltanteTotal)

        Dim lblCostoTotalAnual As New Label With {
            .Name = "lblCostoTotalAnual",
            .Text = "Costo Total Anual: -",
            .Location = New Point(680, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.Purple
        }
        gbResultados.Controls.Add(lblCostoTotalAnual)

        ' === RECOMENDACIONES ===
        Dim txtRecomendaciones As New TextBox With {
            .Name = "txtRecomendaciones",
            .Location = New Point(20, 150),
            .Size = New Size(1280, 50),
            .Multiline = True,
            .ReadOnly = True,
            .ScrollBars = ScrollBars.Vertical,
            .Text = "Ejecute el análisis para obtener recomendaciones de optimización...",
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

        ' Panel para gráfico de evolución de inventario
        Dim panelGrafico As New Panel With {
            .Name = "panelGrafico",
            .Location = New Point(20, 30),
            .Size = New Size(1280, 250),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        gbGraficos.Controls.Add(panelGrafico)

        AddHandler panelGrafico.Paint, AddressOf DibujarGraficoInventario
    End Sub

    ' ===== MÉTODOS DE INICIALIZACIÓN =====
    Private Sub InicializarDatos()
        parametrosInventario = New Dictionary(Of String, Object)
        resultadosSimulacion = New Dictionary(Of String, Object)
        datosSimulacion = New List(Of Dictionary(Of String, Object))
        inventarioActual = 100 ' Inventario inicial
        costosAcumulados = 0
    End Sub

    ' ===== MÉTODOS DE EVENTOS =====
    Private Sub CambiarPolitica(sender As Object, e As EventArgs)
        Dim chk As CheckBox = DirectCast(sender, CheckBox)
        
        If chk.Checked Then
            ' Desmarcar otros checkboxes
            DirectCast(Me.Controls.Find("chkUsarQR", True)(0), CheckBox).Checked = (chk.Name = "chkUsarQR")
            DirectCast(Me.Controls.Find("chkUsarSs", True)(0), CheckBox).Checked = (chk.Name = "chkUsarSs")
            DirectCast(Me.Controls.Find("chkUsarTS", True)(0), CheckBox).Checked = (chk.Name = "chkUsarTS")
        End If
    End Sub

    Private Sub CalcularEOQ(sender As Object, e As EventArgs)
        Try
            Dim demanda As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudDemandaPromedio", True)(0), NumericUpDown).Value) * 365
            Dim costoOrden As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudCostoOrden", True)(0), NumericUpDown).Value)
            Dim costoMantener As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudCostoMantener", True)(0), NumericUpDown).Value) * 365
            Dim tiempoEntrega As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudTiempoEntrega", True)(0), NumericUpDown).Value)
            Dim desviacionDemanda As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudDesviacionDemanda", True)(0), NumericUpDown).Value)
            Dim nivelServicio As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudNivelServicio", True)(0), NumericUpDown).Value) / 100.0

            ' Calcular EOQ
            Dim eoq As Double = Sqrt((2 * demanda * costoOrden) / costoMantener)

            ' Calcular stock de seguridad
            Dim zValue As Double = ObtenerValorZ(nivelServicio)
            Dim stockSeguridad As Double = zValue * desviacionDemanda * Sqrt(tiempoEntrega)

            ' Calcular punto de reorden
            Dim puntoReorden As Double = (demanda / 365) * tiempoEntrega + stockSeguridad

            ' Actualizar controles
            DirectCast(Me.Controls.Find("nudCantidadOrden", True)(0), NumericUpDown).Value = Math.Round(eoq)
            DirectCast(Me.Controls.Find("nudPuntoReorden", True)(0), NumericUpDown).Value = Math.Round(puntoReorden)

            ' Guardar resultados
            parametrosInventario = New Dictionary(Of String, Object) From {
                {"eoq", eoq},
                {"stockSeguridad", stockSeguridad},
                {"puntoReorden", puntoReorden},
                {"demandaAnual", demanda}
            }

            ActualizarResultados()
            GenerarRecomendaciones()

            MessageBox.Show("EOQ calculado exitosamente." & vbCrLf & 
                          "EOQ: " & Math.Round(eoq, 0) & " unidades" & vbCrLf &
                          "Punto de Reorden: " & Math.Round(puntoReorden, 0) & " unidades",
                          "Cálculo Completo", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error en el cálculo: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OptimizarPolitica(sender As Object, e As EventArgs)
        Try
            ' Obtener política seleccionada
            Dim politicaSeleccionada As String = ""
            If DirectCast(Me.Controls.Find("chkUsarQR", True)(0), CheckBox).Checked Then
                politicaSeleccionada = "QR"
            ElseIf DirectCast(Me.Controls.Find("chkUsarSs", True)(0), CheckBox).Checked Then
                politicaSeleccionada = "Ss"
            ElseIf DirectCast(Me.Controls.Find("chkUsarTS", True)(0), CheckBox).Checked Then
                politicaSeleccionada = "TS"
            End If

            If String.IsNullOrEmpty(politicaSeleccionada) Then
                MessageBox.Show("Seleccione una política de inventario.", "Sin Política", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Primero calcular EOQ como base
            CalcularEOQ(Nothing, Nothing)

            ' Optimizar según la política seleccionada
            Select Case politicaSeleccionada
                Case "QR"
                    OptimizarPoliticaQR()
                Case "Ss"
                    OptimizarPoliticaSs()
                Case "TS"
                    OptimizarPoliticaTS()
            End Select

            MessageBox.Show("Política optimizada exitosamente.", "Optimización Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error en la optimización: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimularSistema(sender As Object, e As EventArgs)
        Try
            Dim diasSimular As Integer = Convert.ToInt32(DirectCast(Me.Controls.Find("nudDiasSimular", True)(0), NumericUpDown).Value)
            Dim demandaPromedio As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudDemandaPromedio", True)(0), NumericUpDown).Value)
            Dim desviacionDemanda As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudDesviacionDemanda", True)(0), NumericUpDown).Value)
            
            Dim progressBar As ProgressBar = DirectCast(Me.Controls.Find("progressBarSimulacion", True)(0), ProgressBar)
            Dim lblPorcentaje As Label = DirectCast(Me.Controls.Find("lblPorcentaje", True)(0), Label)

            progressBar.Maximum = diasSimular
            progressBar.Value = 0
            datosSimulacion.Clear()
            costosAcumulados = 0
            inventarioActual = 100 ' Inventario inicial

            Dim random As New Random()
            Dim totalFaltantes As Integer = 0
            Dim totalOrdenes As Integer = 0
            Dim sumaInventario As Double = 0

            ' Obtener parámetros de la política seleccionada
            Dim cantidadOrden As Integer = Convert.ToInt32(DirectCast(Me.Controls.Find("nudCantidadOrden", True)(0), NumericUpDown).Value)
            Dim puntoReorden As Integer = Convert.ToInt32(DirectCast(Me.Controls.Find("nudPuntoReorden", True)(0), NumericUpDown).Value)
            Dim costoOrden As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudCostoOrden", True)(0), NumericUpDown).Value)
            Dim costoMantener As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudCostoMantener", True)(0), NumericUpDown).Value)
            Dim costoFaltante As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudCostoFaltante", True)(0), NumericUpDown).Value)

            For dia As Integer = 1 To diasSimular
                ' Actualizar progreso
                progressBar.Value = dia
                lblPorcentaje.Text = Math.Round((dia / diasSimular) * 100, 1) & "%"
                Application.DoEvents()

                ' Generar demanda diaria (distribución normal)
                Dim demandaDiaria As Integer = Math.Max(0, CInt(GenerarDemandaNormal(demandaPromedio, desviacionDemanda, random)))

                ' Verificar si hay faltantes
                Dim faltantes As Integer = Math.Max(0, demandaDiaria - inventarioActual)
                totalFaltantes += faltantes

                ' Satisfacer demanda
                inventarioActual = Math.Max(0, inventarioActual - demandaDiaria)

                ' Verificar si necesita ordenar (política Q,R)
                If inventarioActual <= puntoReorden Then
                    inventarioActual += cantidadOrden
                    totalOrdenes += 1
                    costosAcumulados += costoOrden
                End If

                ' Calcular costos diarios
                costosAcumulados += inventarioActual * costoMantener ' Costo de mantener
                costosAcumulados += faltantes * costoFaltante ' Costo de faltante

                sumaInventario += inventarioActual

                ' Guardar datos cada 7 días
                If dia Mod 7 = 0 Then
                    datosSimulacion.Add(New Dictionary(Of String, Object) From {
                        {"dia", dia},
                        {"inventario", inventarioActual},
                        {"costos", costosAcumulados},
                        {"faltantes", totalFaltantes}
                    })
                End If

                ' Actualizar indicadores
                ActualizarIndicadoresSimulacion(dia, inventarioActual, costosAcumulados)
            Next

            ' Calcular métricas finales
            Dim inventarioPromedio As Double = sumaInventario / diasSimular
            Dim rotacionInventario As Double = (demandaPromedio * diasSimular) / inventarioPromedio
            Dim nivelServicioReal As Double = ((demandaPromedio * diasSimular - totalFaltantes) / (demandaPromedio * diasSimular)) * 100

            ' Guardar resultados
            resultadosSimulacion = New Dictionary(Of String, Object) From {
                {"inventarioPromedio", inventarioPromedio},
                {"rotacionInventario", rotacionInventario},
                {"nivelServicioReal", nivelServicioReal},
                {"totalFaltantes", totalFaltantes},
                {"totalOrdenes", totalOrdenes},
                {"costoTotal", costosAcumulados},
                {"costoOrdenTotal", totalOrdenes * costoOrden},
                {"costoMantenerTotal", inventarioPromedio * costoMantener * diasSimular},
                {"costoFaltanteTotal", totalFaltantes * costoFaltante}
            }

            ActualizarResultadosSimulacion()
            GenerarRecomendacionesSimulacion()
            Me.Controls.Find("panelGrafico", True)(0).Invalidate()

            MessageBox.Show("Simulación completada exitosamente." & vbCrLf & 
                          "Días simulados: " & diasSimular & vbCrLf &
                          "Costo total: $" & Math.Round(costosAcumulados, 2),
                          "Simulación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error en la simulación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportarResultados(sender As Object, e As EventArgs)
        If resultadosSimulacion Is Nothing OrElse resultadosSimulacion.Count = 0 Then
            MessageBox.Show("No hay resultados para exportar. Ejecute primero la simulación.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim saveDialog As New SaveFileDialog With {
                .Filter = "Archivo CSV|*.csv|Archivo de Texto|*.txt",
                .Title = "Exportar Resultados de Simulación de Inventario",
                .FileName = "SimulacionInventario_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
            }

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim contenido As New List(Of String) From {
                    "SIMULADOR DE GESTIÓN DE INVENTARIO - RESULTADOS",
                    "Fecha: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    "",
                    "PARÁMETROS:",
                    "Demanda Promedio: " & DirectCast(Me.Controls.Find("nudDemandaPromedio", True)(0), NumericUpDown).Value & " und/día",
                    "Desviación Estándar: " & DirectCast(Me.Controls.Find("nudDesviacionDemanda", True)(0), NumericUpDown).Value,
                    "Costo de Ordenar: $" & DirectCast(Me.Controls.Find("nudCostoOrden", True)(0), NumericUpDown).Value,
                    "Costo de Mantener: $" & DirectCast(Me.Controls.Find("nudCostoMantener", True)(0), NumericUpDown).Value & "/und/día",
                    "Costo de Faltante: $" & DirectCast(Me.Controls.Find("nudCostoFaltante", True)(0), NumericUpDown).Value & "/und/día",
                    "",
                    "RESULTADOS DE LA SIMULACIÓN:",
                    "Inventario Promedio: " & Math.Round(Convert.ToDouble(resultadosSimulacion("inventarioPromedio")), 2),
                    "Rotación de Inventario: " & Math.Round(Convert.ToDouble(resultadosSimulacion("rotacionInventario")), 2),
                    "Nivel de Servicio Real: " & Math.Round(Convert.ToDouble(resultadosSimulacion("nivelServicioReal")), 2) & "%",
                    "Total de Faltantes: " & resultadosSimulacion("totalFaltantes"),
                    "Total de Órdenes: " & resultadosSimulacion("totalOrdenes"),
                    "Costo Total: $" & Math.Round(Convert.ToDouble(resultadosSimulacion("costoTotal")), 2),
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

    ' ===== MÉTODOS AUXILIARES =====
    Private Function ObtenerValorZ(nivelServicio As Double) As Double
        ' Aproximación del valor Z para niveles de servicio comunes
        Select Case Math.Round(nivelServicio * 100, 1)
            Case 90.0 : Return 1.28
            Case 95.0 : Return 1.645
            Case 97.5 : Return 1.96
            Case 99.0 : Return 2.33
            Case 99.5 : Return 2.58
            Case 99.9 : Return 3.09
            Case Else : Return 1.645 ' Default 95%
        End Select
    End Function

    Private Function GenerarDemandaNormal(media As Double, desviacion As Double, random As Random) As Double
        ' Método Box-Muller para generar números aleatorios con distribución normal
        Static tieneSiguiente As Boolean = False
        Static siguienteValor As Double

        If tieneSiguiente Then
            tieneSiguiente = False
            Return siguienteValor * desviacion + media
        End If

        Dim u1 As Double = random.NextDouble()
        Dim u2 As Double = random.NextDouble()
        Dim z0 As Double = Sqrt(-2 * Log(u1)) * Cos(2 * PI * u2)
        Dim z1 As Double = Sqrt(-2 * Log(u1)) * Sin(2 * PI * u2)

        siguienteValor = z1
        tieneSiguiente = True

        Return z0 * desviacion + media
    End Function

    Private Sub OptimizarPoliticaQR()
        ' La política Q,R ya está optimizada con EOQ
        ' Solo ajustar si es necesario
    End Sub

    Private Sub OptimizarPoliticaSs()
        ' Optimizar política (S,s)
        Dim eoq As Double = Convert.ToDouble(parametrosInventario("eoq"))
        Dim stockSeguridad As Double = Convert.ToDouble(parametrosInventario("stockSeguridad"))
        
        DirectCast(Me.Controls.Find("nudNivelMaximo", True)(0), NumericUpDown).Value = Math.Round(eoq + stockSeguridad)
        DirectCast(Me.Controls.Find("nudNivelMinimo", True)(0), NumericUpDown).Value = Math.Round(stockSeguridad)
    End Sub

    Private Sub OptimizarPoliticaTS()
        ' Optimizar política (T,S)
        Dim eoq As Double = Convert.ToDouble(parametrosInventario("eoq"))
        Dim demandaAnual As Double = Convert.ToDouble(parametrosInventario("demandaAnual"))
        
        Dim periodoOptimo As Integer = Math.Round((eoq / demandaAnual) * 365)
        DirectCast(Me.Controls.Find("nudPeriodoRevision", True)(0), NumericUpDown).Value = Math.Max(1, periodoOptimo)
        DirectCast(Me.Controls.Find("nudNivelObjetivo", True)(0), NumericUpDown).Value = Math.Round(eoq * 1.5)
    End Sub

    Private Sub ActualizarResultados()
        If parametrosInventario Is Nothing OrElse parametrosInventario.Count = 0 Then Return

        DirectCast(Me.Controls.Find("lblEOQCalculado", True)(0), Label).Text = 
            "EOQ Calculado: " & Math.Round(Convert.ToDouble(parametrosInventario("eoq")), 0) & " unidades"

        DirectCast(Me.Controls.Find("lblStockSeguridad", True)(0), Label).Text = 
            "Stock de Seguridad: " & Math.Round(Convert.ToDouble(parametrosInventario("stockSeguridad")), 0) & " unidades"
    End Sub

    Private Sub ActualizarResultadosSimulacion()
        If resultadosSimulacion Is Nothing Then Return

        DirectCast(Me.Controls.Find("lblInventarioPromedio", True)(0), Label).Text = 
            "Inventario Promedio: " & Math.Round(Convert.ToDouble(resultadosSimulacion("inventarioPromedio")), 1) & " unidades"

        DirectCast(Me.Controls.Find("lblRotacionInventario", True)(0), Label).Text = 
            "Rotación de Inventario: " & Math.Round(Convert.ToDouble(resultadosSimulacion("rotacionInventario")), 2) & " veces/año"

        DirectCast(Me.Controls.Find("lblNivelServicioReal", True)(0), Label).Text = 
            "Nivel de Servicio Real: " & Math.Round(Convert.ToDouble(resultadosSimulacion("nivelServicioReal")), 1) & "%"

        DirectCast(Me.Controls.Find("lblFaltantes", True)(0), Label).Text = 
            "Faltantes Totales: " & resultadosSimulacion("totalFaltantes") & " unidades"

        DirectCast(Me.Controls.Find("lblCostoOrdenTotal", True)(0), Label).Text = 
            "Costo de Ordenar: $" & Math.Round(Convert.ToDouble(resultadosSimulacion("costoOrdenTotal")), 2)

        DirectCast(Me.Controls.Find("lblCostoMantenerTotal", True)(0), Label).Text = 
            "Costo de Mantener: $" & Math.Round(Convert.ToDouble(resultadosSimulacion("costoMantenerTotal")), 2)

        DirectCast(Me.Controls.Find("lblCostoFaltanteTotal", True)(0), Label).Text = 
            "Costo de Faltante: $" & Math.Round(Convert.ToDouble(resultadosSimulacion("costoFaltanteTotal")), 2)

        DirectCast(Me.Controls.Find("lblCostoTotalAnual", True)(0), Label).Text = 
            "Costo Total Anual: $" & Math.Round(Convert.ToDouble(resultadosSimulacion("costoTotal")), 2)
    End Sub

    Private Sub GenerarRecomendaciones()
        If parametrosInventario Is Nothing Then Return

        Dim recomendaciones As New List(Of String)
        Dim eoq As Double = Convert.ToDouble(parametrosInventario("eoq"))
        
        recomendaciones.Add("• EOQ CALCULADO: Ordene " & Math.Round(eoq, 0) & " unidades cada vez para minimizar costos totales.")
        recomendaciones.Add("• FRECUENCIA DE PEDIDOS: Aproximadamente " & Math.Round(365 / (eoq / 50), 0) & " pedidos por año.")
        recomendaciones.Add("• PUNTO DE REORDEN: Realice pedidos cuando el inventario llegue a " & Math.Round(Convert.ToDouble(parametrosInventario("puntoReorden")), 0) & " unidades.")
        recomendaciones.Add("• STOCK DE SEGURIDAD: Mantenga " & Math.Round(Convert.ToDouble(parametrosInventario("stockSeguridad")), 0) & " unidades como buffer.")

        DirectCast(Me.Controls.Find("txtRecomendaciones", True)(0), TextBox).Text = String.Join(vbCrLf, recomendaciones)
    End Sub

    Private Sub GenerarRecomendacionesSimulacion()
        If resultadosSimulacion Is Nothing Then Return

        Dim recomendaciones As New List(Of String)
        Dim nivelServicio As Double = Convert.ToDouble(resultadosSimulacion("nivelServicioReal"))
        Dim rotacion As Double = Convert.ToDouble(resultadosSimulacion("rotacionInventario"))

        If nivelServicio < 90 Then
            recomendaciones.Add("• NIVEL DE SERVICIO BAJO: " & Math.Round(nivelServicio, 1) & "%. Considere aumentar el stock de seguridad.")
        ElseIf nivelServicio > 98 Then
            recomendaciones.Add("• NIVEL DE SERVICIO ALTO: " & Math.Round(nivelServicio, 1) & "%. Podría reducir stock de seguridad para ahorrar costos.")
        Else
            recomendaciones.Add("• NIVEL DE SERVICIO ADECUADO: " & Math.Round(nivelServicio, 1) & "%. Mantener política actual.")
        End If

        If rotacion < 4 Then
            recomendaciones.Add("• ROTACIÓN BAJA: " & Math.Round(rotacion, 1) & " veces/año. Considere reducir cantidades de pedido.")
        ElseIf rotacion > 12 Then
            recomendaciones.Add("• ROTACIÓN ALTA: " & Math.Round(rotacion, 1) & " veces/año. Podría aumentar cantidades de pedido.")
        End If

        recomendaciones.Add("• MONITOREO: Revise la política cada 3 meses y ajuste según cambios en la demanda.")
        recomendaciones.Add("• MEJORA CONTINUA: Considere implementar sistemas de pronóstico más avanzados.")

        DirectCast(Me.Controls.Find("txtRecomendaciones", True)(0), TextBox).Text = String.Join(vbCrLf, recomendaciones)
    End Sub

    Private Sub ActualizarIndicadoresSimulacion(dia As Integer, inventario As Integer, costos As Double)
        DirectCast(Me.Controls.Find("lblDiaActual", True)(0), Label).Text = 
            "Día Actual: " & dia

        DirectCast(Me.Controls.Find("lblInventarioActual", True)(0), Label).Text = 
            "Inventario Actual: " & inventario

        DirectCast(Me.Controls.Find("lblCostosAcumulados", True)(0), Label).Text = 
            "Costos Acumulados: $" & Math.Round(costos, 2)

        ' Actualizar visualización
        Me.Controls.Find("panelVisualizacion", True)(0).Invalidate()
    End Sub

    ' ===== MÉTODOS DE DIBUJO =====
    Private Sub DibujarInventario(sender As Object, e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Dibujar representación visual del inventario
        Dim anchoTotal As Integer = 1200
        Dim altoTotal As Integer = 40
        Dim inventarioMaximo As Integer = 200

        ' Barra de inventario
        Dim porcentajeInventario As Double = Math.Min(1.0, inventarioActual / inventarioMaximo)
        Dim anchoInventario As Integer = CInt(anchoTotal * porcentajeInventario)

        ' Color según nivel de inventario
        Dim colorInventario As Color
        If porcentajeInventario > 0.7 Then
            colorInventario = Color.Green
        ElseIf porcentajeInventario > 0.3 Then
            colorInventario = Color.Orange
        Else
            colorInventario = Color.Red
        End If

        ' Dibujar barra
        g.FillRectangle(New SolidBrush(colorInventario), 40, 5, anchoInventario, altoTotal)
        g.DrawRectangle(Pens.Black, 40, 5, anchoTotal, altoTotal)

        ' Etiquetas
        g.DrawString("0", New Font("Arial", 8), Brushes.Black, CSng(35), CSng(25))
        g.DrawString(inventarioMaximo.ToString(), New Font("Arial", 8), Brushes.Black, CSng(anchoTotal + 45), CSng(25))
        g.DrawString("Nivel de Inventario: " & inventarioActual & " unidades", 
                    New Font("Arial", 10, FontStyle.Bold), Brushes.Black, 40, 50)
    End Sub

    Private Sub DibujarGraficoInventario(sender As Object, e As PaintEventArgs)
        If datosSimulacion Is Nothing OrElse datosSimulacion.Count = 0 Then
            Dim g As Graphics = e.Graphics
            TextRenderer.DrawText(g, "Ejecute la simulación para ver el gráfico de evolución del inventario", 
                                New Font("Arial", 12), New Point(50, 100), Color.Gray)
            Return
        End If

        DibujarEvolucionInventario(e.Graphics)
    End Sub

    Private Sub DibujarEvolucionInventario(g As Graphics)
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Configuración del gráfico
        Dim margenX As Integer = 60
        Dim margenY As Integer = 40
        Dim anchoGrafico As Integer = 1160
        Dim altoGrafico As Integer = 170

        ' Encontrar valores máximos
        Dim maxDia As Integer = datosSimulacion.Max(Function(d) CInt(d("dia")))
        Dim maxInventario As Integer = datosSimulacion.Max(Function(d) CInt(d("inventario")))

        If maxInventario = 0 Then maxInventario = 1

        ' Dibujar ejes
        Dim penEjes As New Pen(Color.Black, 2)
        g.DrawLine(penEjes, margenX, margenY + altoGrafico, margenX + anchoGrafico, margenY + altoGrafico) ' Eje X
        g.DrawLine(penEjes, margenX, margenY, margenX, margenY + altoGrafico) ' Eje Y

        ' Etiquetas de ejes
        g.DrawString("Días", New Font("Arial", 10), Brushes.Black, CSng(margenX + anchoGrafico / 2), CSng(margenY + altoGrafico + 20))
        g.TranslateTransform(20, margenY + altoGrafico / 2)
        g.RotateTransform(-90)
        g.DrawString("Nivel de Inventario", New Font("Arial", 10), Brushes.Black, CSng(0), CSng(0))
        g.ResetTransform()

        ' Dibujar línea de datos
        If datosSimulacion.Count > 1 Then
            Dim puntos As New List(Of PointF)
            
            For Each dato In datosSimulacion
                Dim x As Single = margenX + (CInt(dato("dia")) / maxDia) * anchoGrafico
                Dim y As Single = margenY + altoGrafico - (CInt(dato("inventario")) / maxInventario) * altoGrafico
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
            Dim valorX As Integer = (i * maxDia / 5)
            g.DrawString(valorX.ToString(), New Font("Arial", 8), Brushes.Black, CSng(x - 10), CSng(margenY + altoGrafico + 5))
            
            Dim y As Integer = margenY + altoGrafico - (i * altoGrafico / 5)
            Dim valorY As Integer = (i * maxInventario / 5)
            g.DrawString(valorY.ToString(), New Font("Arial", 8), Brushes.Black, CSng(margenX - 40), CSng(y - 5))
        Next
    End Sub

End Class
