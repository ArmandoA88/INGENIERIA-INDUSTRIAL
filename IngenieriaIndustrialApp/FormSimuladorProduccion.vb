' =====================================================
' SIMULADOR DE LÍNEA DE PRODUCCIÓN
' Balance de línea y análisis de cuellos de botella
' =====================================================
' Autor: Ingeniería Industrial App
' Fecha: 2025
' Descripción: Simulador completo para análisis de
'              líneas de producción y optimización
' =====================================================

Option Strict Off
Imports System.Math
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms

Public Class FormSimuladorProduccion

    ' ===== VARIABLES GLOBALES =====
    Private estaciones As List(Of Dictionary(Of String, Object))
    Private resultadosBalance As Dictionary(Of String, Object)
    Private datosSimulacion As List(Of Dictionary(Of String, Object))
    Private tiempoSimulacion As Integer = 0
    Private unidadesProducidas As Integer = 0
    Private inventarioEnProceso As List(Of Integer)

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub FormSimuladorProduccion_Load(sender As Object, e As EventArgs) Handles Me.Load
        ConfigurarFormulario()
        InicializarControles()
        InicializarDatos()
    End Sub

    ' ===== CONFIGURACIÓN INICIAL =====
    Private Sub ConfigurarFormulario()
        Me.Text = "Simulador de Línea de Producción - Ingeniería Industrial"
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

        ' === PANEL DE ESTACIONES ===
        CrearPanelEstaciones(panelPrincipal, 280)

        ' === PANEL DE SIMULACIÓN ===
        CrearPanelSimulacion(panelPrincipal, 480)

        ' === PANEL DE RESULTADOS ===
        CrearPanelResultados(panelPrincipal, 680)

        ' === PANEL DE GRÁFICOS ===
        CrearPanelGraficos(panelPrincipal, 920)
    End Sub

    Private Sub CrearHeader(panel As Panel)
        Dim lblTitulo As New Label With {
            .Text = "🏭 SIMULADOR DE LÍNEA DE PRODUCCIÓN",
            .Font = New Font("Arial", 20, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panel.Controls.Add(lblTitulo)

        Dim lblSubtitulo As New Label With {
            .Text = "Balance de línea • Análisis de cuellos de botella • Optimización de capacidad • Simulación de flujo",
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
            .Text = "⚙️ Configuración de la Línea",
            .Size = New Size(1320, 180),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbConfiguracion)

        ' === PARÁMETROS GENERALES ===
        Dim lblDemanda As New Label With {
            .Text = "Demanda Objetivo:",
            .Location = New Point(20, 30),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblDemanda)

        Dim nudDemanda As New NumericUpDown With {
            .Name = "nudDemanda",
            .Location = New Point(150, 30),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 10000,
            .Value = 100
        }
        gbConfiguracion.Controls.Add(nudDemanda)

        Dim lblUnidadesDemanda As New Label With {
            .Text = "unidades/hora",
            .Location = New Point(260, 30),
            .Size = New Size(80, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblUnidadesDemanda)

        Dim lblTiempoTurno As New Label With {
            .Text = "Tiempo de Turno:",
            .Location = New Point(20, 70),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblTiempoTurno)

        Dim nudTiempoTurno As New NumericUpDown With {
            .Name = "nudTiempoTurno",
            .Location = New Point(150, 70),
            .Size = New Size(100, 25),
            .Minimum = 1,
            .Maximum = 24,
            .Value = 8
        }
        gbConfiguracion.Controls.Add(nudTiempoTurno)

        Dim lblHorasTurno As New Label With {
            .Text = "horas",
            .Location = New Point(260, 70),
            .Size = New Size(50, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblHorasTurno)

        Dim lblNumEstaciones As New Label With {
            .Text = "Número de Estaciones:",
            .Location = New Point(20, 110),
            .Size = New Size(140, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblNumEstaciones)

        Dim nudNumEstaciones As New NumericUpDown With {
            .Name = "nudNumEstaciones",
            .Location = New Point(170, 110),
            .Size = New Size(80, 25),
            .Minimum = 2,
            .Maximum = 20,
            .Value = 5
        }
        gbConfiguracion.Controls.Add(nudNumEstaciones)

        ' === TIPO DE LÍNEA ===
        Dim lblTipoLinea As New Label With {
            .Text = "Tipo de Línea:",
            .Location = New Point(380, 30),
            .Size = New Size(100, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblTipoLinea)

        Dim cmbTipoLinea As New ComboBox With {
            .Name = "cmbTipoLinea",
            .Location = New Point(490, 30),
            .Size = New Size(200, 25),
            .DropDownStyle = ComboBoxStyle.DropDownList
        }
        cmbTipoLinea.Items.AddRange({"Línea de Ensamble", "Línea de Manufactura", "Línea Mixta", "Línea en U"})
        cmbTipoLinea.SelectedIndex = 0
        gbConfiguracion.Controls.Add(cmbTipoLinea)

        ' === CONFIGURACIÓN DE BUFFER ===
        Dim lblBuffer As New Label With {
            .Text = "Tamaño de Buffer:",
            .Location = New Point(380, 70),
            .Size = New Size(120, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbConfiguracion.Controls.Add(lblBuffer)

        Dim nudBuffer As New NumericUpDown With {
            .Name = "nudBuffer",
            .Location = New Point(510, 70),
            .Size = New Size(80, 25),
            .Minimum = 0,
            .Maximum = 100,
            .Value = 5
        }
        gbConfiguracion.Controls.Add(nudBuffer)

        Dim lblUnidadesBuffer As New Label With {
            .Text = "unidades",
            .Location = New Point(600, 70),
            .Size = New Size(60, 25),
            .ForeColor = Color.Gray
        }
        gbConfiguracion.Controls.Add(lblUnidadesBuffer)

        ' === BOTONES DE ACCIÓN ===
        Dim btnGenerarEstaciones As New Button With {
            .Name = "btnGenerarEstaciones",
            .Text = "🔄 Generar Estaciones",
            .Location = New Point(750, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightGreen,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnGenerarEstaciones)

        Dim btnBalancear As New Button With {
            .Name = "btnBalancear",
            .Text = "⚖️ Balancear Línea",
            .Location = New Point(920, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightBlue,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnBalancear)

        Dim btnSimular As New Button With {
            .Name = "btnSimular",
            .Text = "🎮 Simular Producción",
            .Location = New Point(1090, 30),
            .Size = New Size(150, 40),
            .BackColor = Color.LightCoral,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnSimular)

        Dim btnExportar As New Button With {
            .Name = "btnExportar",
            .Text = "📊 Exportar",
            .Location = New Point(1090, 80),
            .Size = New Size(150, 40),
            .BackColor = Color.LightYellow,
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        gbConfiguracion.Controls.Add(btnExportar)

        ' Configurar eventos
        AddHandler nudNumEstaciones.ValueChanged, AddressOf CambiarNumeroEstaciones
        AddHandler btnGenerarEstaciones.Click, AddressOf GenerarEstaciones
        AddHandler btnBalancear.Click, AddressOf BalancearLinea
        AddHandler btnSimular.Click, AddressOf SimularProduccion
        AddHandler btnExportar.Click, AddressOf ExportarResultados
    End Sub

    ' ===== PANEL DE ESTACIONES ===
    Private Sub CrearPanelEstaciones(panel As Panel, yPos As Integer)
        Dim gbEstaciones As New GroupBox With {
            .Text = "🔧 Configuración de Estaciones de Trabajo",
            .Size = New Size(1320, 180),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbEstaciones)

        ' DataGridView para estaciones
        Dim dgvEstaciones As New DataGridView With {
            .Name = "dgvEstaciones",
            .Location = New Point(20, 30),
            .Size = New Size(1280, 130),
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        ' Configurar columnas
        dgvEstaciones.Columns.Add("Estacion", "Estación")
        dgvEstaciones.Columns.Add("Descripcion", "Descripción")
        dgvEstaciones.Columns.Add("TiempoCiclo", "Tiempo Ciclo (min)")
        dgvEstaciones.Columns.Add("Operarios", "Operarios")
        dgvEstaciones.Columns.Add("Eficiencia", "Eficiencia (%)")
        dgvEstaciones.Columns.Add("Capacidad", "Capacidad (und/h)")
        dgvEstaciones.Columns.Add("Utilizacion", "Utilización (%)")

        ' Configurar tipos de columnas
        DirectCast(dgvEstaciones.Columns("TiempoCiclo"), DataGridViewTextBoxColumn).DefaultCellStyle.Format = "F2"
        DirectCast(dgvEstaciones.Columns("Eficiencia"), DataGridViewTextBoxColumn).DefaultCellStyle.Format = "F1"
        DirectCast(dgvEstaciones.Columns("Capacidad"), DataGridViewTextBoxColumn).DefaultCellStyle.Format = "F0"
        DirectCast(dgvEstaciones.Columns("Utilizacion"), DataGridViewTextBoxColumn).DefaultCellStyle.Format = "F1"

        gbEstaciones.Controls.Add(dgvEstaciones)
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
        Dim lblTiempoActual As New Label With {
            .Name = "lblTiempoActual",
            .Text = "Tiempo Transcurrido: 0 min",
            .Location = New Point(20, 30),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblTiempoActual)

        Dim lblUnidadesProducidas As New Label With {
            .Name = "lblUnidadesProducidas",
            .Text = "Unidades Producidas: 0",
            .Location = New Point(240, 30),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblUnidadesProducidas)

        Dim lblTasaProduccion As New Label With {
            .Name = "lblTasaProduccion",
            .Text = "Tasa de Producción: 0 und/h",
            .Location = New Point(460, 30),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbSimulacion.Controls.Add(lblTasaProduccion)

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

        ' === VISUALIZACIÓN DE LA LÍNEA ===
        Dim panelVisualizacion As New Panel With {
            .Name = "panelVisualizacion",
            .Location = New Point(20, 110),
            .Size = New Size(1280, 50),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.AliceBlue
        }
        gbSimulacion.Controls.Add(panelVisualizacion)

        AddHandler panelVisualizacion.Paint, AddressOf DibujarLineaProduccion
    End Sub

    ' ===== PANEL DE RESULTADOS =====
    Private Sub CrearPanelResultados(panel As Panel, yPos As Integer)
        Dim gbResultados As New GroupBox With {
            .Text = "📊 Resultados del Balance y Análisis",
            .Size = New Size(1320, 220),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbResultados)

        ' === MÉTRICAS DE BALANCE ===
        Dim lblEficienciaLinea As New Label With {
            .Name = "lblEficienciaLinea",
            .Text = "Eficiencia de Línea: -",
            .Location = New Point(20, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblEficienciaLinea)

        Dim lblTiempoCicloTeorico As New Label With {
            .Name = "lblTiempoCicloTeorico",
            .Text = "Tiempo Ciclo Teórico: -",
            .Location = New Point(340, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblTiempoCicloTeorico)

        Dim lblNumEstacionesMinimas As New Label With {
            .Name = "lblNumEstacionesMinimas",
            .Text = "Estaciones Mínimas Teóricas: -",
            .Location = New Point(660, 30),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblNumEstacionesMinimas)

        Dim lblCuelloBotella As New Label With {
            .Name = "lblCuelloBotella",
            .Text = "Cuello de Botella: -",
            .Location = New Point(20, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkRed
        }
        gbResultados.Controls.Add(lblCuelloBotella)

        Dim lblCapacidadLinea As New Label With {
            .Name = "lblCapacidadLinea",
            .Text = "Capacidad de Línea: -",
            .Location = New Point(340, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblCapacidadLinea)

        Dim lblTiempoOcioso As New Label With {
            .Name = "lblTiempoOcioso",
            .Text = "Tiempo Ocioso Total: -",
            .Location = New Point(660, 70),
            .Size = New Size(300, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        gbResultados.Controls.Add(lblTiempoOcioso)

        ' === ANÁLISIS DE COSTOS ===
        Dim lblCostoManoObra As New Label With {
            .Name = "lblCostoManoObra",
            .Text = "Costo Mano de Obra: -",
            .Location = New Point(20, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkBlue
        }
        gbResultados.Controls.Add(lblCostoManoObra)

        Dim lblCostoInventario As New Label With {
            .Name = "lblCostoInventario",
            .Text = "Costo Inventario WIP: -",
            .Location = New Point(240, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkGreen
        }
        gbResultados.Controls.Add(lblCostoInventario)

        Dim lblCostoTotal As New Label With {
            .Name = "lblCostoTotal",
            .Text = "Costo Total por Hora: -",
            .Location = New Point(460, 110),
            .Size = New Size(200, 25),
            .Font = New Font("Arial", 10, FontStyle.Bold),
            .ForeColor = Color.DarkRed
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
            .Text = "Ejecute el balance de línea para obtener recomendaciones de optimización...",
            .BackColor = Color.LightYellow
        }
        gbResultados.Controls.Add(txtRecomendaciones)
    End Sub

    ' ===== PANEL DE GRÁFICOS ===
    Private Sub CrearPanelGraficos(panel As Panel, yPos As Integer)
        Dim gbGraficos As New GroupBox With {
            .Text = "📈 Análisis Gráfico",
            .Size = New Size(1320, 300),
            .Location = New Point(20, yPos),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.White
        }
        panel.Controls.Add(gbGraficos)

        ' Panel para gráfico de balance
        Dim panelGrafico As New Panel With {
            .Name = "panelGrafico",
            .Location = New Point(20, 30),
            .Size = New Size(1280, 250),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        gbGraficos.Controls.Add(panelGrafico)

        AddHandler panelGrafico.Paint, AddressOf DibujarGraficoBalance
    End Sub

    ' ===== MÉTODOS DE INICIALIZACIÓN =====
    Private Sub InicializarDatos()
        estaciones = New List(Of Dictionary(Of String, Object))
        resultadosBalance = New Dictionary(Of String, Object)
        datosSimulacion = New List(Of Dictionary(Of String, Object))
        inventarioEnProceso = New List(Of Integer)
        
        ' Generar estaciones por defecto
        GenerarEstacionesDefecto()
    End Sub

    Private Sub GenerarEstacionesDefecto()
        estaciones.Clear()
        Dim numEstaciones As Integer = 5

        ' Datos ejemplo para diferentes tipos de estaciones
        Dim tiemposCiclo() As Double = {2.5, 3.2, 2.8, 3.5, 2.1}
        Dim descripciones() As String = {
            "Preparación de materiales",
            "Ensamble principal",
            "Soldadura y fijación",
            "Control de calidad",
            "Empaque y etiquetado"
        }

        For i As Integer = 0 To numEstaciones - 1
            Dim estacion As New Dictionary(Of String, Object) From {
                {"numero", i + 1},
                {"descripcion", descripciones(i)},
                {"tiempoCiclo", tiemposCiclo(i)},
                {"operarios", 1},
                {"eficiencia", 95.0},
                {"capacidad", 0.0},
                {"utilizacion", 0.0}
            }
            estaciones.Add(estacion)
        Next

        ActualizarGridEstaciones()
    End Sub

    ' ===== MÉTODOS DE EVENTOS =====
    Private Sub CambiarNumeroEstaciones(sender As Object, e As EventArgs)
        Dim numEstaciones As Integer = DirectCast(sender, NumericUpDown).Value
        ' Actualizar número de estaciones cuando cambie el valor
    End Sub

    Private Sub GenerarEstaciones(sender As Object, e As EventArgs)
        Try
            Dim numEstaciones As Integer = Convert.ToInt32(DirectCast(Me.Controls.Find("nudNumEstaciones", True)(0), NumericUpDown).Value)
            
            estaciones.Clear()
            Dim random As New Random()

            For i As Integer = 0 To numEstaciones - 1
                Dim tiempoCiclo As Double = 2.0 + (random.NextDouble() * 2.0) ' Entre 2 y 4 minutos
                Dim estacion As New Dictionary(Of String, Object) From {
                    {"numero", i + 1},
                    {"descripcion", "Estación " & (i + 1)},
                    {"tiempoCiclo", Math.Round(tiempoCiclo, 2)},
                    {"operarios", 1},
                    {"eficiencia", 90.0 + (random.NextDouble() * 10.0)}, ' Entre 90% y 100%
                    {"capacidad", 0.0},
                    {"utilizacion", 0.0}
                }
                estaciones.Add(estacion)
            Next

            ActualizarGridEstaciones()
            MessageBox.Show("Estaciones generadas exitosamente.", "Generación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error al generar estaciones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BalancearLinea(sender As Object, e As EventArgs)
        Try
            If estaciones.Count = 0 Then
                MessageBox.Show("Primero debe generar las estaciones de trabajo.", "Sin Estaciones", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim demanda As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudDemanda", True)(0), NumericUpDown).Value)
            Dim tiempoTurno As Double = Convert.ToDouble(DirectCast(Me.Controls.Find("nudTiempoTurno", True)(0), NumericUpDown).Value)

            ' Calcular tiempo ciclo teórico
            Dim tiempoCicloTeorico As Double = (tiempoTurno * 60) / demanda ' minutos por unidad

            ' Calcular capacidades y utilizaciones
            For Each estacion In estaciones
                Dim tiempoCiclo As Double = Convert.ToDouble(estacion("tiempoCiclo"))
                Dim eficiencia As Double = Convert.ToDouble(estacion("eficiencia")) / 100.0
                Dim operarios As Integer = Convert.ToInt32(estacion("operarios"))
                
                ' Capacidad = (60 min/hora) / (tiempo ciclo * operarios) * eficiencia
                Dim capacidad As Double = (60.0 / tiempoCiclo) * operarios * eficiencia
                estacion("capacidad") = Math.Round(capacidad, 2)
                
                ' Utilización = demanda / capacidad
                Dim utilizacion As Double = (demanda / capacidad) * 100.0
                estacion("utilizacion") = Math.Round(utilizacion, 2)
            Next

            ' Encontrar cuello de botella
            Dim cuelloBotella = estaciones.OrderBy(Function(est) Convert.ToDouble(est("capacidad"))).First()
            Dim capacidadLinea As Double = Convert.ToDouble(cuelloBotella("capacidad"))

            ' Calcular métricas de balance
            Dim tiempoTotalTrabajo As Double = estaciones.Sum(Function(est) Convert.ToDouble(est("tiempoCiclo")))
            Dim numEstacionesMinimas As Integer = Math.Ceiling(tiempoTotalTrabajo / tiempoCicloTeorico)
            Dim eficienciaLinea As Double = (tiempoTotalTrabajo / (estaciones.Count * tiempoCicloTeorico)) * 100.0
            Dim tiempoOciosoTotal As Double = (estaciones.Count * tiempoCicloTeorico) - tiempoTotalTrabajo

            ' Guardar resultados
            resultadosBalance = New Dictionary(Of String, Object) From {
                {"tiempoCicloTeorico", tiempoCicloTeorico},
                {"numEstacionesMinimas", numEstacionesMinimas},
                {"eficienciaLinea", eficienciaLinea},
                {"cuelloBotella", cuelloBotella("numero")},
                {"capacidadLinea", capacidadLinea},
                {"tiempoOciosoTotal", tiempoOciosoTotal}
            }

            ActualizarGridEstaciones()
            ActualizarResultados()
            GenerarRecomendaciones()
            Me.Controls.Find("panelGrafico", True)(0).Invalidate()

        Catch ex As Exception
            MessageBox.Show("Error en el balance de línea: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SimularProduccion(sender As Object, e As EventArgs)
        Try
            If estaciones.Count = 0 Then
                MessageBox.Show("Primero debe configurar las estaciones de trabajo.", "Sin Configuración", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim tiempoSimulacion As Integer = 480 ' 8 horas en minutos
            Dim progressBar As ProgressBar = DirectCast(Me.Controls.Find("progressBarSimulacion", True)(0), ProgressBar)
            Dim lblPorcentaje As Label = DirectCast(Me.Controls.Find("lblPorcentaje", True)(0), Label)

            progressBar.Maximum = tiempoSimulacion
            progressBar.Value = 0
            datosSimulacion.Clear()
            unidadesProducidas = 0

            ' Inicializar inventario en proceso
            inventarioEnProceso.Clear()
            For i As Integer = 0 To estaciones.Count - 1
                inventarioEnProceso.Add(0)
            Next

            ' Ejecutar simulación
            For tiempo As Integer = 0 To tiempoSimulacion Step 1
                ' Actualizar progreso
                progressBar.Value = tiempo
                lblPorcentaje.Text = Math.Round((tiempo / tiempoSimulacion) * 100, 1) & "%"
                Application.DoEvents()

                ' Simular flujo en cada estación
                For i As Integer = estaciones.Count - 1 To 0 Step -1
                    Dim estacion = estaciones(i)
                    Dim tiempoCiclo As Double = Convert.ToDouble(estacion("tiempoCiclo"))
                    
                    ' Procesar unidades cada tiempo de ciclo
                    If tiempo Mod Math.Max(1, CInt(tiempoCiclo)) = 0 Then
                        If i = estaciones.Count - 1 Then
                            ' Última estación - producir
                            If inventarioEnProceso(i) > 0 Then
                                inventarioEnProceso(i) -= 1
                                unidadesProducidas += 1
                            End If
                        Else
                            ' Estaciones intermedias - transferir
                            If inventarioEnProceso(i) > 0 And inventarioEnProceso(i + 1) < 10 Then
                                inventarioEnProceso(i) -= 1
                                inventarioEnProceso(i + 1) += 1
                            End If
                        End If
                        
                        ' Primera estación - recibir material
                        If i = 0 And inventarioEnProceso(0) < 10 Then
                            inventarioEnProceso(0) += 1
                        End If
                    End If
                Next

                ' Guardar datos cada 30 minutos
                If tiempo Mod 30 = 0 Then
                    Dim tasaProduccion As Double = If(tiempo > 0, (unidadesProducidas / tiempo) * 60, 0)
                    datosSimulacion.Add(New Dictionary(Of String, Object) From {
                        {"tiempo", tiempo},
                        {"unidades", unidadesProducidas},
                        {"tasa", tasaProduccion}
                    })
                End If

                ' Actualizar indicadores
                ActualizarIndicadoresSimulacion(tiempo, unidadesProducidas)
            Next

            MessageBox.Show("Simulación completada exitosamente." & vbCrLf & 
                          "Unidades producidas: " & unidadesProducidas, 
                          "Simulación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error en la simulación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ExportarResultados(sender As Object, e As EventArgs)
        If resultadosBalance Is Nothing OrElse resultadosBalance.Count = 0 Then
            MessageBox.Show("No hay resultados para exportar. Ejecute primero el balance de línea.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim saveDialog As New SaveFileDialog With {
                .Filter = "Archivo CSV|*.csv|Archivo de Texto|*.txt",
                .Title = "Exportar Resultados de Simulación de Producción",
                .FileName = "SimulacionProduccion_" & DateTime.Now.ToString("yyyyMMdd_HHmmss")
            }

            If saveDialog.ShowDialog() = DialogResult.OK Then
                Dim contenido As New List(Of String) From {
                    "SIMULADOR DE LÍNEA DE PRODUCCIÓN - RESULTADOS",
                    "Fecha: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                    "",
                    "CONFIGURACIÓN:",
                    "Demanda Objetivo: " & DirectCast(Me.Controls.Find("nudDemanda", True)(0), NumericUpDown).Value & " und/h",
                    "Tiempo de Turno: " & DirectCast(Me.Controls.Find("nudTiempoTurno", True)(0), NumericUpDown).Value & " horas",
                    "Número de Estaciones: " & estaciones.Count,
                    "",
                    "RESULTADOS DEL BALANCE:",
                    "Eficiencia de Línea: " & Math.Round(Convert.ToDouble(resultadosBalance("eficienciaLinea")), 2) & "%",
                    "Tiempo Ciclo Teórico: " & Math.Round(Convert.ToDouble(resultadosBalance("tiempoCicloTeorico")), 2) & " min",
                    "Estaciones Mínimas: " & resultadosBalance("numEstacionesMinimas"),
                    "Cuello de Botella: Estación " & resultadosBalance("cuelloBotella"),
                    "Capacidad de Línea: " & Math.Round(Convert.ToDouble(resultadosBalance("capacidadLinea")), 2) & " und/h",
                    "",
                    "ESTACIONES:",
                    "Estación,Descripción,Tiempo Ciclo,Capacidad,Utilización"
                }

                For Each estacion In estaciones
                    contenido.Add(estacion("numero") & "," & 
                                estacion("descripcion") & "," & 
                                estacion("tiempoCiclo") & "," & 
                                estacion("capacidad") & "," & 
                                estacion("utilizacion"))
                Next

                contenido.Add("")
                contenido.Add("RECOMENDACIONES:")
                contenido.Add(DirectCast(Me.Controls.Find("txtRecomendaciones", True)(0), TextBox).Text)

                System.IO.File.WriteAllLines(saveDialog.FileName, contenido)
                MessageBox.Show("Resultados exportados exitosamente a:" & vbCrLf & saveDialog.FileName, 
                              "Exportación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error al exportar: " & ex.Message, "Error de Exportación", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ===== MÉTODOS AUXILIARES =====
    Private Sub ActualizarGridEstaciones()
        Dim dgv As DataGridView = DirectCast(Me.Controls.Find("dgvEstaciones", True)(0), DataGridView)
        dgv.Rows.Clear()

        For Each estacion In estaciones
            dgv.Rows.Add(
                estacion("numero"),
                estacion("descripcion"),
                estacion("tiempoCiclo"),
                estacion("operarios"),
                estacion("eficiencia"),
                estacion("capacidad"),
                estacion("utilizacion")
            )

            ' Colorear fila del cuello de botella
            If resultadosBalance.ContainsKey("cuelloBotella") AndAlso 
               Convert.ToInt32(estacion("numero")) = Convert.ToInt32(resultadosBalance("cuelloBotella")) Then
                dgv.Rows(dgv.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightCoral
            End If
        Next
    End Sub

    Private Sub ActualizarResultados()
        If resultadosBalance Is Nothing Then Return

        DirectCast(Me.Controls.Find("lblEficienciaLinea", True)(0), Label).Text = 
            "Eficiencia de Línea: " & Math.Round(Convert.ToDouble(resultadosBalance("eficienciaLinea")), 2) & "%"

        DirectCast(Me.Controls.Find("lblTiempoCicloTeorico", True)(0), Label).Text = 
            "Tiempo Ciclo Teórico: " & Math.Round(Convert.ToDouble(resultadosBalance("tiempoCicloTeorico")), 2) & " min"

        DirectCast(Me.Controls.Find("lblNumEstacionesMinimas", True)(0), Label).Text = 
            "Estaciones Mínimas Teóricas: " & resultadosBalance("numEstacionesMinimas")

        DirectCast(Me.Controls.Find("lblCuelloBotella", True)(0), Label).Text = 
            "Cuello de Botella: Estación " & resultadosBalance("cuelloBotella")

        DirectCast(Me.Controls.Find("lblCapacidadLinea", True)(0), Label).Text = 
            "Capacidad de Línea: " & Math.Round(Convert.ToDouble(resultadosBalance("capacidadLinea")), 2) & " und/h"

        DirectCast(Me.Controls.Find("lblTiempoOcioso", True)(0), Label).Text = 
            "Tiempo Ocioso Total: " & Math.Round(Convert.ToDouble(resultadosBalance("tiempoOciosoTotal")), 2) & " min"

        ' Calcular costos (valores ejemplo)
        Dim costoManoObra As Double = estaciones.Count * 15 ' $15/hora por operario
        Dim costoInventario As Double = inventarioEnProceso.Sum() * 5 ' $5 por unidad WIP
        Dim costoTotal As Double = costoManoObra + costoInventario

        DirectCast(Me.Controls.Find("lblCostoManoObra", True)(0), Label).Text = 
            "Costo Mano de Obra: $" & costoManoObra & "/h"

        DirectCast(Me.Controls.Find("lblCostoInventario", True)(0), Label).Text = 
            "Costo Inventario WIP: $" & costoInventario

        DirectCast(Me.Controls.Find("lblCostoTotal", True)(0), Label).Text = 
            "Costo Total por Hora: $" & costoTotal
    End Sub

    Private Sub GenerarRecomendaciones()
        If resultadosBalance Is Nothing Then Return

        Dim recomendaciones As New List(Of String)
        Dim eficiencia As Double = Convert.ToDouble(resultadosBalance("eficienciaLinea"))
        Dim cuelloBotella As Integer = Convert.ToInt32(resultadosBalance("cuelloBotella"))

        ' Análisis de eficiencia
        If eficiencia < 70 Then
            recomendaciones.Add("• EFICIENCIA BAJA: La línea tiene " & Math.Round(eficiencia, 1) & "% de eficiencia. Considere redistribuir tareas.")
        ElseIf eficiencia > 90 Then
            recomendaciones.Add("• EXCELENTE EFICIENCIA: La línea está bien balanceada (" & Math.Round(eficiencia, 1) & "%).")
        Else
            recomendaciones.Add("• EFICIENCIA ACEPTABLE: La línea tiene " & Math.Round(eficiencia, 1) & "% de eficiencia. Hay oportunidades de mejora.")
        End If

        ' Análisis del cuello de botella
        recomendaciones.Add("• CUELLO DE BOTELLA: La Estación " & cuelloBotella & " limita la capacidad de la línea.")
        recomendaciones.Add("• Para mejorar: Agregar operario, mejorar método, o dividir operaciones en la Estación " & cuelloBotella)

        ' Recomendaciones específicas
        Dim utilizacionMaxima As Double = estaciones.Max(Function(est) Convert.ToDouble(est("utilizacion")))
        If utilizacionMaxima > 100 Then
            recomendaciones.Add("• SOBRECARGA: Algunas estaciones están sobrecargadas. Redistribuir carga de trabajo.")
        End If

        ' Recomendaciones generales
        recomendaciones.Add("• Implementar sistema pull (Kanban) para reducir inventario WIP")
        recomendaciones.Add("• Capacitar operarios en múltiples estaciones para flexibilidad")
        recomendaciones.Add("• Considerar automatización en estaciones con alta utilización")

        DirectCast(Me.Controls.Find("txtRecomendaciones", True)(0), TextBox).Text = String.Join(vbCrLf, recomendaciones)
    End Sub

    Private Sub ActualizarIndicadoresSimulacion(tiempo As Integer, unidades As Integer)
        DirectCast(Me.Controls.Find("lblTiempoActual", True)(0), Label).Text = 
            "Tiempo Transcurrido: " & tiempo & " min"

        DirectCast(Me.Controls.Find("lblUnidadesProducidas", True)(0), Label).Text = 
            "Unidades Producidas: " & unidades

        Dim tasaProduccion As Double = If(tiempo > 0, (unidades / tiempo) * 60, 0)
        DirectCast(Me.Controls.Find("lblTasaProduccion", True)(0), Label).Text = 
            "Tasa de Producción: " & Math.Round(tasaProduccion, 1) & " und/h"

        ' Actualizar visualización
        Me.Controls.Find("panelVisualizacion", True)(0).Invalidate()
    End Sub

    ' ===== MÉTODOS DE DIBUJO =====
    Private Sub DibujarLineaProduccion(sender As Object, e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        If estaciones.Count = 0 Then Return

        Dim anchoEstacion As Integer = Math.Min(200, 1200 \ estaciones.Count)
        Dim altoEstacion As Integer = 30

        For i As Integer = 0 To estaciones.Count - 1
            Dim x As Integer = 50 + (i * (anchoEstacion + 20))
            Dim y As Integer = 10

            ' Color según utilización
            Dim utilizacion As Double = If(estaciones(i).ContainsKey("utilizacion"), 
                                         Convert.ToDouble(estaciones(i)("utilizacion")), 0)
            Dim colorEstacion As Color
            If utilizacion > 100 Then
                colorEstacion = Color.Red
            ElseIf utilizacion > 80 Then
                colorEstacion = Color.Orange
            ElseIf utilizacion > 50 Then
                colorEstacion = Color.Yellow
            Else
                colorEstacion = Color.LightGreen
            End If

            ' Dibujar estación
            g.FillRectangle(New SolidBrush(colorEstacion), x, y, anchoEstacion, altoEstacion)
            g.DrawRectangle(Pens.Black, x, y, anchoEstacion, altoEstacion)

            ' Texto de la estación
            Dim texto As String = "E" & (i + 1)
            Dim tamaño As SizeF = g.MeasureString(texto, New Font("Arial", 10, FontStyle.Bold))
            g.DrawString(texto, New Font("Arial", 10, FontStyle.Bold), Brushes.Black, 
                        CSng(x + (anchoEstacion - tamaño.Width) / 2), CSng(y + (altoEstacion - tamaño.Height) / 2))

            ' Flecha entre estaciones
            If i < estaciones.Count - 1 Then
                Dim pen As New Pen(Color.Black, 2)
                pen.EndCap = LineCap.ArrowAnchor
                g.DrawLine(pen, x + anchoEstacion, y + altoEstacion \ 2, 
                          x + anchoEstacion + 15, y + altoEstacion \ 2)
            End If

            ' Mostrar inventario WIP si existe
            If inventarioEnProceso.Count > i AndAlso inventarioEnProceso(i) > 0 Then
                g.DrawString(inventarioEnProceso(i).ToString(), New Font("Arial", 8), Brushes.Blue, x, y - 15)
            End If
        Next
    End Sub

    Private Sub DibujarGraficoBalance(sender As Object, e As PaintEventArgs)
        If estaciones.Count = 0 Then
            Dim g As Graphics = e.Graphics
            TextRenderer.DrawText(g, "Configure las estaciones para ver el gráfico de balance", 
                                New Font("Arial", 12), New Point(50, 100), Color.Gray)
            Return
        End If

        DibujarGraficoTiempos(e.Graphics)
    End Sub

    Private Sub DibujarGraficoTiempos(g As Graphics)
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' Configuración del gráfico
        Dim margenX As Integer = 80
        Dim margenY As Integer = 40
        Dim anchoGrafico As Integer = 1120
        Dim altoGrafico As Integer = 170

        ' Encontrar tiempo máximo
        Dim tiempoMaximo As Double = estaciones.Max(Function(est) Convert.ToDouble(est("tiempoCiclo")))
        If tiempoMaximo = 0 Then tiempoMaximo = 1

        ' Dibujar ejes
        Dim penEjes As New Pen(Color.Black, 2)
        g.DrawLine(penEjes, margenX, margenY + altoGrafico, margenX + anchoGrafico, margenY + altoGrafico) ' Eje X
        g.DrawLine(penEjes, margenX, margenY, margenX, margenY + altoGrafico) ' Eje Y

        ' Etiquetas de ejes
        g.DrawString("Estaciones", New Font("Arial", 10), Brushes.Black, CSng(margenX + anchoGrafico / 2), CSng(margenY + altoGrafico + 20))
        g.TranslateTransform(20, margenY + altoGrafico / 2)
        g.RotateTransform(-90)
        g.DrawString("Tiempo de Ciclo (min)", New Font("Arial", 10), Brushes.Black, CSng(0), CSng(0))
        g.ResetTransform()

        ' Dibujar barras
        Dim anchoBarra As Integer = anchoGrafico \ estaciones.Count - 10
        For i As Integer = 0 To estaciones.Count - 1
            Dim tiempoCiclo As Double = Convert.ToDouble(estaciones(i)("tiempoCiclo"))
            Dim alturaBarra As Integer = CInt((tiempoCiclo / tiempoMaximo) * altoGrafico)
            
            Dim x As Integer = margenX + 5 + (i * (anchoBarra + 10))
            Dim y As Integer = margenY + altoGrafico - alturaBarra

            ' Color según si es cuello de botella
            Dim esCuelloBotella As Boolean = resultadosBalance.ContainsKey("cuelloBotella") AndAlso 
                                           Convert.ToInt32(estaciones(i)("numero")) = Convert.ToInt32(resultadosBalance("cuelloBotella"))
            Dim colorBarra As Color = If(esCuelloBotella, Color.Red, Color.LightBlue)

            g.FillRectangle(New SolidBrush(colorBarra), x, y, anchoBarra, alturaBarra)
            g.DrawRectangle(Pens.Black, x, y, anchoBarra, alturaBarra)

            ' Etiqueta de estación
            g.DrawString("E" & (i + 1), New Font("Arial", 9), Brushes.Black, CSng(x + anchoBarra / 4), CSng(margenY + altoGrafico + 5))
            
            ' Valor del tiempo
            g.DrawString(tiempoCiclo.ToString("F1"), New Font("Arial", 8), Brushes.Black, CSng(x), CSng(y - 15))
        Next

        ' Línea de tiempo ciclo teórico si existe
        If resultadosBalance.ContainsKey("tiempoCicloTeorico") Then
            Dim tiempoCicloTeorico As Double = Convert.ToDouble(resultadosBalance("tiempoCicloTeorico"))
            Dim yTeorico As Integer = margenY + altoGrafico - CInt((tiempoCicloTeorico / tiempoMaximo) * altoGrafico)
            
            Dim penTeorico As New Pen(Color.Green, 2)
            penTeorico.DashStyle = DashStyle.Dash
            g.DrawLine(penTeorico, margenX, yTeorico, margenX + anchoGrafico, yTeorico)
            g.DrawString("Tiempo Ciclo Teórico", New Font("Arial", 9), Brushes.Green, CSng(margenX + anchoGrafico - 150), CSng(yTeorico - 15))
        End If

        ' Marcas en el eje Y
        For i As Integer = 0 To 5
            Dim y As Integer = margenY + altoGrafico - (i * altoGrafico / 5)
            Dim valor As Double = (i * tiempoMaximo / 5)
            g.DrawString(valor.ToString("F1"), New Font("Arial", 8), Brushes.Black, CSng(margenX - 40), CSng(y - 5))
        Next
    End Sub

End Class
