' =====================================================
' INGENIERÍA INDUSTRIAL - LANDING PAGE PRINCIPAL
' Suite de Herramientas Integradas
' =====================================================
' Autor: Repositorio Ingeniería Industrial
' Fecha: 2025
' Descripción: Interfaz principal que organiza todas las
'              herramientas y recursos del programa
' =====================================================

Imports System.Diagnostics

Public Class Form1

    ' ===== EVENTOS DEL FORMULARIO =====
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configuración inicial del formulario
        Me.Text = "Ingeniería Industrial - Suite de Herramientas"
        Me.Size = New Size(1200, 800)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 248, 255) ' AliceBlue
        
        ' Inicializar controles
        InicializarInterfaz()
    End Sub

    ' ===== INICIALIZACIÓN DE LA INTERFAZ =====
    Private Sub InicializarInterfaz()
        ' Panel principal con scroll
        Dim panelPrincipal As New Panel With {
            .Dock = DockStyle.Fill,
            .AutoScroll = True,
            .Padding = New Padding(20)
        }
        Me.Controls.Add(panelPrincipal)

        ' === HEADER PRINCIPAL ===
        Dim lblTituloPrincipal As New Label With {
            .Text = "INGENIERÍA INDUSTRIAL",
            .Font = New Font("Arial", 24, FontStyle.Bold),
            .ForeColor = Color.DarkBlue,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panelPrincipal.Controls.Add(lblTituloPrincipal)

        Dim lblSubtitulo As New Label With {
            .Text = "Suite Completa de Herramientas y Recursos",
            .Font = New Font("Arial", 14, FontStyle.Italic),
            .ForeColor = Color.DarkSlateGray,
            .AutoSize = True,
            .Location = New Point(20, 60)
        }
        panelPrincipal.Controls.Add(lblSubtitulo)

        ' Línea separadora
        Dim separador As New Panel With {
            .Size = New Size(1100, 3),
            .Location = New Point(20, 100),
            .BackColor = Color.DarkBlue
        }
        panelPrincipal.Controls.Add(separador)

        ' === SECCIÓN 1: HERRAMIENTAS ===
        CrearSeccionHerramientas(panelPrincipal, 120)

        ' === SECCIÓN 2: METODOLOGÍAS ===
        CrearSeccionMetodologias(panelPrincipal, 320)

        ' === SECCIÓN 3: PROCESOS ===
        CrearSeccionProcesos(panelPrincipal, 520)

        ' === SECCIÓN 4: CASOS DE ESTUDIO ===
        CrearSeccionCasosEstudio(panelPrincipal, 720)

        ' === SECCIÓN 5: RECURSOS ===
        CrearSeccionRecursos(panelPrincipal, 920)

        ' === FOOTER ===
        CrearFooter(panelPrincipal, 1120)
    End Sub

    ' ===== SECCIÓN HERRAMIENTAS =====
    Private Sub CrearSeccionHerramientas(panel As Panel, yPos As Integer)
        ' Título de sección
        Dim lblTitulo As New Label With {
            .Text = "🛠️ HERRAMIENTAS",
            .Font = New Font("Arial", 18, FontStyle.Bold),
            .ForeColor = Color.DarkGreen,
            .AutoSize = True,
            .Location = New Point(20, yPos)
        }
        panel.Controls.Add(lblTitulo)

        ' Panel contenedor para herramientas
        Dim panelHerramientas As New Panel With {
            .Size = New Size(1100, 180),
            .Location = New Point(20, yPos + 40),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        panel.Controls.Add(panelHerramientas)

        ' === CALCULADORAS ===
        Dim gbCalculadoras As New GroupBox With {
            .Text = "📊 Calculadoras",
            .Size = New Size(250, 160),
            .Location = New Point(10, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelHerramientas.Controls.Add(gbCalculadoras)

        Dim btnEstudioTiempos As New Button With {
            .Name = "btnEstudioTiempos",
            .Text = "Estudio de Tiempos",
            .Size = New Size(220, 30),
            .Location = New Point(15, 25),
            .BackColor = Color.LightBlue,
            .FlatStyle = FlatStyle.Flat
        }
        gbCalculadoras.Controls.Add(btnEstudioTiempos)

        Dim btnProductividad As New Button With {
            .Name = "btnProductividad",
            .Text = "Productividad",
            .Size = New Size(220, 30),
            .Location = New Point(15, 60),
            .BackColor = Color.LightGreen,
            .FlatStyle = FlatStyle.Flat
        }
        gbCalculadoras.Controls.Add(btnProductividad)

        Dim btnInventarios As New Button With {
            .Name = "btnInventarios",
            .Text = "Inventarios (EOQ)",
            .Size = New Size(220, 30),
            .Location = New Point(15, 95),
            .BackColor = Color.LightYellow,
            .FlatStyle = FlatStyle.Flat
        }
        gbCalculadoras.Controls.Add(btnInventarios)

        Dim btnCalidad As New Button With {
            .Name = "btnCalidad",
            .Text = "Control de Calidad",
            .Size = New Size(220, 30),
            .Location = New Point(15, 130),
            .BackColor = Color.LightCoral,
            .FlatStyle = FlatStyle.Flat
        }
        gbCalculadoras.Controls.Add(btnCalidad)

        ' === GRÁFICOS Y DIAGRAMAS ===
        Dim gbGraficos As New GroupBox With {
            .Text = "📈 Gráficos y Diagramas",
            .Size = New Size(250, 160),
            .Location = New Point(280, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelHerramientas.Controls.Add(gbGraficos)

        Dim btnDiagramaFlujo As New Button With {
            .Name = "btnDiagramaFlujo",
            .Text = "Diagrama de Flujo",
            .Size = New Size(220, 30),
            .Location = New Point(15, 25),
            .BackColor = Color.Lavender,
            .FlatStyle = FlatStyle.Flat
        }
        gbGraficos.Controls.Add(btnDiagramaFlujo)

        Dim btnDiagramaIshikawa As New Button With {
            .Name = "btnDiagramaIshikawa",
            .Text = "Diagrama Ishikawa",
            .Size = New Size(220, 30),
            .Location = New Point(15, 60),
            .BackColor = Color.LightSteelBlue,
            .FlatStyle = FlatStyle.Flat
        }
        gbGraficos.Controls.Add(btnDiagramaIshikawa)

        Dim btnGraficoPareto As New Button With {
            .Name = "btnGraficoPareto",
            .Text = "Gráfico de Pareto",
            .Size = New Size(220, 30),
            .Location = New Point(15, 95),
            .BackColor = Color.PaleGreen,
            .FlatStyle = FlatStyle.Flat
        }
        gbGraficos.Controls.Add(btnGraficoPareto)

        Dim btnGraficoControl As New Button With {
            .Name = "btnGraficoControl",
            .Text = "Gráficos de Control",
            .Size = New Size(220, 30),
            .Location = New Point(15, 130),
            .BackColor = Color.PeachPuff,
            .FlatStyle = FlatStyle.Flat
        }
        gbGraficos.Controls.Add(btnGraficoControl)

        ' === SIMULADORES ===
        Dim gbSimuladores As New GroupBox With {
            .Text = "🎮 Simuladores",
            .Size = New Size(250, 160),
            .Location = New Point(550, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelHerramientas.Controls.Add(gbSimuladores)

        Dim btnSimulacionColas As New Button With {
            .Name = "btnSimulacionColas",
            .Text = "Teoría de Colas",
            .Size = New Size(220, 30),
            .Location = New Point(15, 25),
            .BackColor = Color.LightPink,
            .FlatStyle = FlatStyle.Flat
        }
        gbSimuladores.Controls.Add(btnSimulacionColas)

        Dim btnSimulacionProduccion As New Button With {
            .Name = "btnSimulacionProduccion",
            .Text = "Línea de Producción",
            .Size = New Size(220, 30),
            .Location = New Point(15, 60),
            .BackColor = Color.LightGoldenrodYellow,
            .FlatStyle = FlatStyle.Flat
        }
        gbSimuladores.Controls.Add(btnSimulacionProduccion)

        Dim btnSimulacionInventario As New Button With {
            .Name = "btnSimulacionInventario",
            .Text = "Gestión de Inventario",
            .Size = New Size(220, 30),
            .Location = New Point(15, 95),
            .BackColor = Color.LightSeaGreen,
            .FlatStyle = FlatStyle.Flat
        }
        gbSimuladores.Controls.Add(btnSimulacionInventario)

        ' === GENERADORES ===
        Dim gbGeneradores As New GroupBox With {
            .Text = "📋 Generadores",
            .Size = New Size(250, 160),
            .Location = New Point(820, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelHerramientas.Controls.Add(gbGeneradores)

        Dim btnGeneradorReportes As New Button With {
            .Name = "btnGeneradorReportes",
            .Text = "Reportes Automáticos",
            .Size = New Size(220, 30),
            .Location = New Point(15, 25),
            .BackColor = Color.Thistle,
            .FlatStyle = FlatStyle.Flat
        }
        gbGeneradores.Controls.Add(btnGeneradorReportes)

        Dim btnGeneradorFormatos As New Button With {
            .Name = "btnGeneradorFormatos",
            .Text = "Formatos Estándar",
            .Size = New Size(220, 30),
            .Location = New Point(15, 60),
            .BackColor = Color.Wheat,
            .FlatStyle = FlatStyle.Flat
        }
        gbGeneradores.Controls.Add(btnGeneradorFormatos)

        ' Configurar eventos
        AddHandler btnEstudioTiempos.Click, AddressOf AbrirCalculadoraTiempos
        AddHandler btnProductividad.Click, AddressOf AbrirCalculadoraProductividad
        AddHandler btnInventarios.Click, AddressOf AbrirCalculadoraInventarios
        AddHandler btnCalidad.Click, AddressOf AbrirCalculadoraCalidad
        AddHandler btnDiagramaFlujo.Click, AddressOf AbrirDiagramaFlujo
        AddHandler btnDiagramaIshikawa.Click, AddressOf AbrirDiagramaIshikawa
        AddHandler btnGraficoPareto.Click, AddressOf AbrirGraficoPareto
        AddHandler btnGraficoControl.Click, AddressOf AbrirGraficoControl
        AddHandler btnSimulacionColas.Click, AddressOf AbrirSimulacionColas
        AddHandler btnSimulacionProduccion.Click, AddressOf AbrirSimulacionProduccion
        AddHandler btnSimulacionInventario.Click, AddressOf AbrirSimulacionInventario
        AddHandler btnGeneradorReportes.Click, AddressOf AbrirGeneradorReportes
        AddHandler btnGeneradorFormatos.Click, AddressOf AbrirGeneradorFormatos
    End Sub

    ' ===== SECCIÓN METODOLOGÍAS =====
    Private Sub CrearSeccionMetodologias(panel As Panel, yPos As Integer)
        Dim lblTitulo As New Label With {
            .Text = "📚 METODOLOGÍAS",
            .Font = New Font("Arial", 18, FontStyle.Bold),
            .ForeColor = Color.DarkOrange,
            .AutoSize = True,
            .Location = New Point(20, yPos)
        }
        panel.Controls.Add(lblTitulo)

        Dim panelMetodologias As New Panel With {
            .Size = New Size(1100, 180),
            .Location = New Point(20, yPos + 40),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        panel.Controls.Add(panelMetodologias)

        ' Estudio de Tiempos
        Dim gbEstudioTiempos As New GroupBox With {
            .Text = "⏱️ Estudio de Tiempos",
            .Size = New Size(350, 160),
            .Location = New Point(10, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelMetodologias.Controls.Add(gbEstudioTiempos)

        Dim btnMetodoWestinghouse As New Button With {
            .Text = "Método Westinghouse",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.LightBlue
        }
        gbEstudioTiempos.Controls.Add(btnMetodoWestinghouse)

        Dim btnMuestreoTrabajo As New Button With {
            .Text = "Muestreo de Trabajo",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightGreen
        }
        gbEstudioTiempos.Controls.Add(btnMuestreoTrabajo)

        Dim btnTiemposPredeterminados As New Button With {
            .Text = "Tiempos Predeterminados (MTM)",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.LightYellow
        }
        gbEstudioTiempos.Controls.Add(btnTiemposPredeterminados)

        ' Mejora de Procesos
        Dim gbMejoraProcesos As New GroupBox With {
            .Text = "🔄 Mejora de Procesos",
            .Size = New Size(350, 160),
            .Location = New Point(380, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelMetodologias.Controls.Add(gbMejoraProcesos)

        Dim btnKaizen As New Button With {
            .Text = "Kaizen",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.Lavender
        }
        gbMejoraProcesos.Controls.Add(btnKaizen)

        Dim btnSixSigma As New Button With {
            .Text = "Six Sigma",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightSteelBlue
        }
        gbMejoraProcesos.Controls.Add(btnSixSigma)

        Dim btnDMAIC As New Button With {
            .Text = "DMAIC",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.PaleGreen
        }
        gbMejoraProcesos.Controls.Add(btnDMAIC)

        ' Calidad
        Dim gbCalidad As New GroupBox With {
            .Text = "✅ Control de Calidad",
            .Size = New Size(350, 160),
            .Location = New Point(750, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelMetodologias.Controls.Add(gbCalidad)

        Dim btnControlEstadistico As New Button With {
            .Text = "Control Estadístico",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.LightPink
        }
        gbCalidad.Controls.Add(btnControlEstadistico)

        Dim btnCapacidadProceso As New Button With {
            .Text = "Capacidad de Proceso",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightGoldenrodYellow
        }
        gbCalidad.Controls.Add(btnCapacidadProceso)

        Dim btnMuestreoAceptacion As New Button With {
            .Text = "Muestreo de Aceptación",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.LightSeaGreen
        }
        gbCalidad.Controls.Add(btnMuestreoAceptacion)
    End Sub

    ' ===== SECCIÓN PROCESOS =====
    Private Sub CrearSeccionProcesos(panel As Panel, yPos As Integer)
        Dim lblTitulo As New Label With {
            .Text = "⚙️ PROCESOS",
            .Font = New Font("Arial", 18, FontStyle.Bold),
            .ForeColor = Color.DarkRed,
            .AutoSize = True,
            .Location = New Point(20, yPos)
        }
        panel.Controls.Add(lblTitulo)

        Dim panelProcesos As New Panel With {
            .Size = New Size(1100, 180),
            .Location = New Point(20, yPos + 40),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        panel.Controls.Add(panelProcesos)

        ' Manufactura Esbelta
        Dim gbLean As New GroupBox With {
            .Text = "🏭 Manufactura Esbelta",
            .Size = New Size(540, 160),
            .Location = New Point(10, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelProcesos.Controls.Add(gbLean)

        Dim btn5S As New Button With {
            .Text = "5S",
            .Size = New Size(120, 30),
            .Location = New Point(15, 25),
            .BackColor = Color.LightBlue
        }
        gbLean.Controls.Add(btn5S)

        Dim btnJIT As New Button With {
            .Text = "Just In Time",
            .Size = New Size(120, 30),
            .Location = New Point(145, 25),
            .BackColor = Color.LightGreen
        }
        gbLean.Controls.Add(btnJIT)

        Dim btnKanban As New Button With {
            .Text = "Kanban",
            .Size = New Size(120, 30),
            .Location = New Point(275, 25),
            .BackColor = Color.LightYellow
        }
        gbLean.Controls.Add(btnKanban)

        Dim btnPoka As New Button With {
            .Text = "Poka-Yoke",
            .Size = New Size(120, 30),
            .Location = New Point(405, 25),
            .BackColor = Color.LightCoral
        }
        gbLean.Controls.Add(btnPoka)

        Dim btnVSM As New Button With {
            .Text = "Value Stream Mapping",
            .Size = New Size(250, 30),
            .Location = New Point(15, 65),
            .BackColor = Color.Lavender
        }
        gbLean.Controls.Add(btnVSM)

        Dim btnSMED As New Button With {
            .Text = "SMED",
            .Size = New Size(120, 30),
            .Location = New Point(275, 65),
            .BackColor = Color.LightSteelBlue
        }
        gbLean.Controls.Add(btnSMED)

        Dim btnTPM As New Button With {
            .Text = "TPM",
            .Size = New Size(120, 30),
            .Location = New Point(405, 65),
            .BackColor = Color.PaleGreen
        }
        gbLean.Controls.Add(btnTPM)

        ' Planificación
        Dim gbPlanificacion As New GroupBox With {
            .Text = "📅 Planificación",
            .Size = New Size(540, 160),
            .Location = New Point(560, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelProcesos.Controls.Add(gbPlanificacion)

        Dim btnMRP As New Button With {
            .Text = "MRP",
            .Size = New Size(120, 30),
            .Location = New Point(15, 25),
            .BackColor = Color.LightPink
        }
        gbPlanificacion.Controls.Add(btnMRP)

        Dim btnERP As New Button With {
            .Text = "ERP",
            .Size = New Size(120, 30),
            .Location = New Point(145, 25),
            .BackColor = Color.LightGoldenrodYellow
        }
        gbPlanificacion.Controls.Add(btnERP)

        Dim btnCapacidad As New Button With {
            .Text = "Planificación Capacidad",
            .Size = New Size(250, 30),
            .Location = New Point(275, 25),
            .BackColor = Color.LightSeaGreen
        }
        gbPlanificacion.Controls.Add(btnCapacidad)

        Dim btnPronosticos As New Button With {
            .Text = "Pronósticos",
            .Size = New Size(120, 30),
            .Location = New Point(15, 65),
            .BackColor = Color.Thistle
        }
        gbPlanificacion.Controls.Add(btnPronosticos)

        Dim btnProgramacion As New Button With {
            .Text = "Programación Producción",
            .Size = New Size(250, 30),
            .Location = New Point(145, 65),
            .BackColor = Color.Wheat
        }
        gbPlanificacion.Controls.Add(btnProgramacion)
    End Sub

    ' ===== SECCIÓN CASOS DE ESTUDIO =====
    Private Sub CrearSeccionCasosEstudio(panel As Panel, yPos As Integer)
        Dim lblTitulo As New Label With {
            .Text = "📖 CASOS DE ESTUDIO",
            .Font = New Font("Arial", 18, FontStyle.Bold),
            .ForeColor = Color.DarkMagenta,
            .AutoSize = True,
            .Location = New Point(20, yPos)
        }
        panel.Controls.Add(lblTitulo)

        Dim panelCasos As New Panel With {
            .Size = New Size(1100, 180),
            .Location = New Point(20, yPos + 40),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        panel.Controls.Add(panelCasos)

        ' Casos por industria
        Dim gbIndustrias As New GroupBox With {
            .Text = "🏭 Por Industria",
            .Size = New Size(350, 160),
            .Location = New Point(10, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelCasos.Controls.Add(gbIndustrias)

        Dim btnManufactura As New Button With {
            .Text = "Manufactura",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.LightBlue
        }
        gbIndustrias.Controls.Add(btnManufactura)

        Dim btnServicios As New Button With {
            .Text = "Servicios",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightGreen
        }
        gbIndustrias.Controls.Add(btnServicios)

        Dim btnLogistica As New Button With {
            .Text = "Logística",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.LightYellow
        }
        gbIndustrias.Controls.Add(btnLogistica)

        ' Casos por metodología
        Dim gbMetodologia As New GroupBox With {
            .Text = "📊 Por Metodología",
            .Size = New Size(350, 160),
            .Location = New Point(380, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelCasos.Controls.Add(gbMetodologia)

        Dim btnCasoLean As New Button With {
            .Text = "Implementación Lean",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.Lavender
        }
        gbMetodologia.Controls.Add(btnCasoLean)

        Dim btnCasoSixSigma As New Button With {
            .Text = "Proyecto Six Sigma",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightSteelBlue
        }
        gbMetodologia.Controls.Add(btnCasoSixSigma)

        Dim btnCasoKaizen As New Button With {
            .Text = "Evento Kaizen",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.PaleGreen
        }
        gbMetodologia.Controls.Add(btnCasoKaizen)

        ' Simulaciones
        Dim gbSimulaciones As New GroupBox With {
            .Text = "🎮 Simulaciones",
            .Size = New Size(350, 160),
            .Location = New Point(750, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelCasos.Controls.Add(gbSimulaciones)

        Dim btnSimulacionCompleta As New Button With {
            .Text = "Fábrica Virtual",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.LightPink
        }
        gbSimulaciones.Controls.Add(btnSimulacionCompleta)

        Dim btnJuegoRoles As New Button With {
            .Text = "Juego de Roles",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightGoldenrodYellow
        }
        gbSimulaciones.Controls.Add(btnJuegoRoles)

        Dim btnEscenarios As New Button With {
            .Text = "Análisis de Escenarios",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.LightSeaGreen
        }
        gbSimulaciones.Controls.Add(btnEscenarios)
    End Sub

    ' ===== SECCIÓN RECURSOS =====
    Private Sub CrearSeccionRecursos(panel As Panel, yPos As Integer)
        Dim lblTitulo As New Label With {
            .Text = "📚 RECURSOS",
            .Font = New Font("Arial", 18, FontStyle.Bold),
            .ForeColor = Color.DarkCyan,
            .AutoSize = True,
            .Location = New Point(20, yPos)
        }
        panel.Controls.Add(lblTitulo)

        Dim panelRecursos As New Panel With {
            .Size = New Size(1100, 180),
            .Location = New Point(20, yPos + 40),
            .BorderStyle = BorderStyle.FixedSingle,
            .BackColor = Color.White
        }
        panel.Controls.Add(panelRecursos)

        ' Biblioteca
        Dim gbBiblioteca As New GroupBox With {
            .Text = "📖 Biblioteca",
            .Size = New Size(350, 160),
            .Location = New Point(10, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelRecursos.Controls.Add(gbBiblioteca)

        Dim btnLibros As New Button With {
            .Text = "Libros y Artículos",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.LightBlue
        }
        gbBiblioteca.Controls.Add(btnLibros)

        Dim btnNormas As New Button With {
            .Text = "Normas y Estándares",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightGreen
        }
        gbBiblioteca.Controls.Add(btnNormas)

        Dim btnGlosario As New Button With {
            .Text = "Glosario de Términos",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.LightYellow
        }
        gbBiblioteca.Controls.Add(btnGlosario)

        ' Plantillas
        Dim gbPlantillas As New GroupBox With {
            .Text = "📋 Plantillas",
            .Size = New Size(350, 160),
            .Location = New Point(380, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelRecursos.Controls.Add(gbPlantillas)

        Dim btnFormatos As New Button With {
            .Text = "Formatos Estándar",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.Lavender
        }
        gbPlantillas.Controls.Add(btnFormatos)

        Dim btnCheckLists As New Button With {
            .Text = "Check Lists",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightSteelBlue
        }
        gbPlantillas.Controls.Add(btnCheckLists)

        Dim btnPlantillasReporte As New Button With {
            .Text = "Plantillas de Reporte",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.PaleGreen
        }
        gbPlantillas.Controls.Add(btnPlantillasReporte)

        ' Videos y Tutoriales
        Dim gbTutoriales As New GroupBox With {
            .Text = "🎥 Videos y Tutoriales",
            .Size = New Size(350, 160),
            .Location = New Point(750, 10),
            .Font = New Font("Arial", 10, FontStyle.Bold)
        }
        panelRecursos.Controls.Add(gbTutoriales)

        Dim btnVideosTutoriales As New Button With {
            .Text = "Videos Explicativos",
            .Size = New Size(320, 25),
            .Location = New Point(15, 25),
            .BackColor = Color.LightPink
        }
        gbTutoriales.Controls.Add(btnVideosTutoriales)

        Dim btnCursosOnline As New Button With {
            .Text = "Cursos Online",
            .Size = New Size(320, 25),
            .Location = New Point(15, 55),
            .BackColor = Color.LightGoldenrodYellow
        }
        gbTutoriales.Controls.Add(btnCursosOnline)

        Dim btnWebinars As New Button With {
            .Text = "Webinars",
            .Size = New Size(320, 25),
            .Location = New Point(15, 85),
            .BackColor = Color.LightSeaGreen
        }
        gbTutoriales.Controls.Add(btnWebinars)
    End Sub

    ' ===== FOOTER =====
    Private Sub CrearFooter(panel As Panel, yPos As Integer)
        Dim panelFooter As New Panel With {
            .Size = New Size(1100, 80),
            .Location = New Point(20, yPos),
            .BackColor = Color.DarkBlue
        }
        panel.Controls.Add(panelFooter)

        Dim lblFooter As New Label With {
            .Text = "Ingeniería Industrial - Suite de Herramientas © 2025",
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .Location = New Point(20, 20)
        }
        panelFooter.Controls.Add(lblFooter)

        Dim lblVersion As New Label With {
            .Text = "Versión 1.0 - Desarrollado con Visual Basic .NET",
            .Font = New Font("Arial", 10, FontStyle.Italic),
            .ForeColor = Color.LightGray,
            .AutoSize = True,
            .Location = New Point(20, 45)
        }
        panelFooter.Controls.Add(lblVersion)

        ' Botones de acción del footer
        Dim btnAcercaDe As New Button With {
            .Text = "Acerca de",
            .Size = New Size(100, 30),
            .Location = New Point(800, 15),
            .BackColor = Color.White,
            .ForeColor = Color.DarkBlue
        }
        panelFooter.Controls.Add(btnAcercaDe)

        Dim btnSalir As New Button With {
            .Text = "Salir",
            .Size = New Size(100, 30),
            .Location = New Point(920, 15),
            .BackColor = Color.Red,
            .ForeColor = Color.White
        }
        panelFooter.Controls.Add(btnSalir)

        AddHandler btnAcercaDe.Click, AddressOf MostrarAcercaDe
        AddHandler btnSalir.Click, AddressOf SalirAplicacion
    End Sub

    ' ===== MÉTODOS DE EVENTOS =====
    Private Sub AbrirCalculadoraTiempos(sender As Object, e As EventArgs)
        Try
            ' Crear y mostrar la calculadora directamente
            Dim calculadoraForm As New FormCalculadoraTiempos()
            calculadoraForm.Show()
        Catch ex As Exception
            MessageBox.Show("Error al abrir la calculadora:" & vbCrLf & vbCrLf &
                          ex.Message,
                          "Error - Calculadora de Estudio de Tiempos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub AbrirCalculadoraProductividad(sender As Object, e As EventArgs)
        MessageBox.Show("Calculadora de Productividad" & vbCrLf & vbCrLf &
                      "Esta herramienta permite calcular diferentes métricas de productividad:" & vbCrLf &
                      "• Productividad laboral" & vbCrLf &
                      "• Productividad de materiales" & vbCrLf &
                      "• Productividad de maquinaria" & vbCrLf &
                      "• Eficiencia global (OEE)" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Calculadora de Productividad", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirCalculadoraInventarios(sender As Object, e As EventArgs)
        MessageBox.Show("Calculadora de Inventarios (EOQ)" & vbCrLf & vbCrLf &
                      "Esta herramienta permite calcular:" & vbCrLf &
                      "• Cantidad económica de pedido (EOQ)" & vbCrLf &
                      "• Punto de reorden" & vbCrLf &
                      "• Stock de seguridad" & vbCrLf &
                      "• Costos de inventario" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Calculadora de Inventarios", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirCalculadoraCalidad(sender As Object, e As EventArgs)
        MessageBox.Show("Calculadora de Control de Calidad" & vbCrLf & vbCrLf &
                      "Esta herramienta permite calcular:" & vbCrLf &
                      "• Límites de control (UCL, LCL)" & vbCrLf &
                      "• Índices de capacidad (Cp, Cpk)" & vbCrLf &
                      "• Planes de muestreo" & vbCrLf &
                      "• Análisis de variabilidad" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Calculadora de Control de Calidad", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirDiagramaFlujo(sender As Object, e As EventArgs)
        MessageBox.Show("Generador de Diagramas de Flujo" & vbCrLf & vbCrLf &
                      "Herramienta para crear diagramas de flujo de procesos:" & vbCrLf &
                      "• Símbolos estándar ANSI" & vbCrLf &
                      "• Conexiones automáticas" & vbCrLf &
                      "• Exportación a PDF/PNG" & vbCrLf &
                      "• Plantillas predefinidas" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Diagrama de Flujo", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirDiagramaIshikawa(sender As Object, e As EventArgs)
        MessageBox.Show("Generador de Diagrama Ishikawa" & vbCrLf & vbCrLf &
                      "Herramienta para análisis de causa-efecto:" & vbCrLf &
                      "• 6M (Método, Máquina, Material, Mano de obra, Medición, Medio ambiente)" & vbCrLf &
                      "• Personalización de categorías" & vbCrLf &
                      "• Exportación de diagramas" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Diagrama Ishikawa", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirGraficoPareto(sender As Object, e As EventArgs)
        MessageBox.Show("Generador de Gráfico de Pareto" & vbCrLf & vbCrLf &
                      "Herramienta para análisis 80/20:" & vbCrLf &
                      "• Ordenamiento automático" & vbCrLf &
                      "• Línea de porcentaje acumulado" & vbCrLf &
                      "• Identificación de causas vitales" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Gráfico de Pareto", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirGraficoControl(sender As Object, e As EventArgs)
        MessageBox.Show("Generador de Gráficos de Control" & vbCrLf & vbCrLf &
                      "Herramienta para control estadístico:" & vbCrLf &
                      "• Gráficos X-R, X-S" & vbCrLf &
                      "• Gráficos p, np, c, u" & vbCrLf &
                      "• Detección de patrones" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Gráficos de Control", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirSimulacionColas(sender As Object, e As EventArgs)
        MessageBox.Show("Simulador de Teoría de Colas" & vbCrLf & vbCrLf &
                      "Simulación de sistemas de espera:" & vbCrLf &
                      "• Modelos M/M/1, M/M/c" & vbCrLf &
                      "• Análisis de tiempos de espera" & vbCrLf &
                      "• Optimización de servidores" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Simulación de Colas", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirSimulacionProduccion(sender As Object, e As EventArgs)
        MessageBox.Show("Simulador de Línea de Producción" & vbCrLf & vbCrLf &
                      "Simulación de procesos productivos:" & vbCrLf &
                      "• Balance de línea" & vbCrLf &
                      "• Identificación de cuellos de botella" & vbCrLf &
                      "• Análisis de capacidad" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Simulación de Producción", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirSimulacionInventario(sender As Object, e As EventArgs)
        MessageBox.Show("Simulador de Gestión de Inventario" & vbCrLf & vbCrLf &
                      "Simulación de políticas de inventario:" & vbCrLf &
                      "• Modelos de demanda variable" & vbCrLf &
                      "• Análisis de costos" & vbCrLf &
                      "• Optimización de políticas" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Simulación de Inventario", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirGeneradorReportes(sender As Object, e As EventArgs)
        MessageBox.Show("Generador de Reportes Automáticos" & vbCrLf & vbCrLf &
                      "Creación automática de reportes:" & vbCrLf &
                      "• Reportes de productividad" & vbCrLf &
                      "• Análisis de tiempos" & vbCrLf &
                      "• Reportes de calidad" & vbCrLf &
                      "• Exportación a PDF/Excel" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Generador de Reportes", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AbrirGeneradorFormatos(sender As Object, e As EventArgs)
        MessageBox.Show("Generador de Formatos Estándar" & vbCrLf & vbCrLf &
                      "Plantillas de formatos industriales:" & vbCrLf &
                      "• Hojas de estudio de tiempos" & vbCrLf &
                      "• Formatos de control de calidad" & vbCrLf &
                      "• Check lists de auditoría" & vbCrLf &
                      "• Formatos de mejora continua" & vbCrLf & vbCrLf &
                      "Estado: 🚧 En desarrollo",
                      "Generador de Formatos", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MostrarAcercaDe(sender As Object, e As EventArgs)
        MessageBox.Show("INGENIERÍA INDUSTRIAL - Suite de Herramientas" & vbCrLf & vbCrLf &
                      "Versión: 1.0" & vbCrLf &
                      "Desarrollado en: Visual Basic .NET" & vbCrLf &
                      "Fecha: 2025" & vbCrLf & vbCrLf &
                      "Esta suite integra todas las herramientas necesarias para:" & vbCrLf &
                      "• Estudio de tiempos y movimientos" & vbCrLf &
                      "• Control de calidad" & vbCrLf &
                      "• Mejora de procesos" & vbCrLf &
                      "• Análisis de productividad" & vbCrLf &
                      "• Simulación de sistemas" & vbCrLf & vbCrLf &
                      "Repositorio: https://github.com/ArmandoA88/INGENIERIA-INDUSTRIAL",
                      "Acerca de", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub SalirAplicacion(sender As Object, e As EventArgs)
        If MessageBox.Show("¿Está seguro de que desea salir?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

End Class
