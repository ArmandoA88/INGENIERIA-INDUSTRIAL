// =====================================================
// CALCULADORA SIMPLE DE ESTUDIO DE TIEMPOS - C#
// Para ejecutar fácilmente desde VSCode
// =====================================================

using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculadoraEstudioTiempos
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Calculadora de Estudio de Tiempos - Ingeniería Industrial";
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║        CALCULADORA DE ESTUDIO DE TIEMPOS                ║");
            Console.WriteLine("║              Ingeniería Industrial                      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();

            var calculadora = new CalculadoraEstudioTiempos();
            calculadora.Ejecutar();
        }
    }

    public class CalculadoraEstudioTiempos
    {
        private List<double> tiemposObservados = new List<double>();
        private double factorCalificacion = 1.0;
        private double suplementos = 0.15; // 15% por defecto

        public void Ejecutar()
        {
            bool continuar = true;
            
            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();
                
                switch (opcion)
                {
                    case "1":
                        IngresarTiempos();
                        break;
                    case "2":
                        ConfigurarFactorCalificacion();
                        break;
                    case "3":
                        ConfigurarSuplementos();
                        break;
                    case "4":
                        CalcularTiempoEstandar();
                        break;
                    case "5":
                        MostrarResultadosDetallados();
                        break;
                    case "6":
                        LimpiarDatos();
                        break;
                    case "7":
                        MostrarAyuda();
                        break;
                    case "0":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente de nuevo.");
                        break;
                }
                
                if (continuar)
                {
                    Console.WriteLine("\nPresione Enter para continuar...");
                    Console.ReadLine();
                }
            }
            
            Console.WriteLine("¡Gracias por usar la Calculadora de Estudio de Tiempos!");
        }

        private void MostrarMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("                    MENÚ PRINCIPAL");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Console.WriteLine("1. Ingresar tiempos observados");
            Console.WriteLine("2. Configurar factor de calificación");
            Console.WriteLine("3. Configurar suplementos");
            Console.WriteLine("4. Calcular tiempo estándar");
            Console.WriteLine("5. Ver resultados detallados");
            Console.WriteLine("6. Limpiar todos los datos");
            Console.WriteLine("7. Ayuda");
            Console.WriteLine("0. Salir");
            Console.WriteLine();
            
            // Mostrar estado actual
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Estado actual:");
            Console.WriteLine($"- Tiempos ingresados: {tiemposObservados.Count}");
            Console.WriteLine($"- Factor de calificación: {factorCalificacion:F3}");
            Console.WriteLine($"- Suplementos: {suplementos * 100:F1}%");
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("Seleccione una opción: ");
        }

        private void IngresarTiempos()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("              INGRESAR TIEMPOS OBSERVADOS");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Console.WriteLine("Ingrese los tiempos observados en minutos.");
            Console.WriteLine("Escriba 'fin' para terminar o 'ver' para mostrar la lista actual.\n");
            
            MostrarTiemposActuales();
            
            while (true)
            {
                Console.Write($"Tiempo {tiemposObservados.Count + 1}: ");
                string input = Console.ReadLine().ToLower();
                
                if (input == "fin")
                    break;
                    
                if (input == "ver")
                {
                    MostrarTiemposActuales();
                    continue;
                }
                
                if (double.TryParse(input, out double tiempo) && tiempo > 0)
                {
                    tiemposObservados.Add(tiempo);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"✓ Tiempo {tiempo:F3} min agregado.");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("✗ Error: Ingrese un número válido mayor a 0.");
                    Console.ResetColor();
                }
            }
        }

        private void MostrarTiemposActuales()
        {
            if (tiemposObservados.Count > 0)
            {
                Console.WriteLine("\nTiempos actuales:");
                for (int i = 0; i < tiemposObservados.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {tiemposObservados[i]:F3} min");
                }
                if (tiemposObservados.Count > 1)
                {
                    Console.WriteLine($"  Promedio: {tiemposObservados.Average():F3} min");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("No hay tiempos ingresados aún.\n");
            }
        }

        private void ConfigurarFactorCalificacion()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("           CONFIGURAR FACTOR DE CALIFICACIÓN");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Console.WriteLine("Método Westinghouse - Seleccione para cada factor:\n");
            
            // Habilidad
            Console.WriteLine("1. HABILIDAD del operador:");
            Console.WriteLine("   1) Superior (+15%)    4) Promedio (0%)");
            Console.WriteLine("   2) Excelente (+11%)   5) Regular (-10%)");
            Console.WriteLine("   3) Buena (+6%)        6) Deficiente (-16%)");
            Console.Write("   Selección: ");
            int habilidad = LeerOpcion(1, 6, 4);
            
            // Esfuerzo
            Console.WriteLine("\n2. ESFUERZO del operador:");
            Console.WriteLine("   1) Excesivo (+13%)     4) Promedio (0%)");
            Console.WriteLine("   2) Excelente (+10%)    5) Regular (-4%)");
            Console.WriteLine("   3) Bueno (+5%)         6) Deficiente (-8%)");
            Console.Write("   Selección: ");
            int esfuerzo = LeerOpcion(1, 6, 4);
            
            // Condiciones
            Console.WriteLine("\n3. CONDICIONES de trabajo:");
            Console.WriteLine("   1) Ideales (+6%)       4) Promedio (0%)");
            Console.WriteLine("   2) Excelentes (+4%)    5) Regulares (-3%)");
            Console.WriteLine("   3) Buenas (+2%)        6) Deficientes (-7%)");
            Console.Write("   Selección: ");
            int condiciones = LeerOpcion(1, 6, 4);
            
            // Consistencia
            Console.WriteLine("\n4. CONSISTENCIA de tiempos:");
            Console.WriteLine("   1) Perfecta (+4%)      4) Promedio (0%)");
            Console.WriteLine("   2) Excelente (+3%)     5) Regular (-2%)");
            Console.WriteLine("   3) Buena (+1%)         6) Deficiente (-4%)");
            Console.Write("   Selección: ");
            int consistencia = LeerOpcion(1, 6, 4);
            
            // Calcular factor
            double[] valoresHabilidad = { 15, 11, 6, 0, -10, -16 };
            double[] valoresEsfuerzo = { 13, 10, 5, 0, -4, -8 };
            double[] valoresCondiciones = { 6, 4, 2, 0, -3, -7 };
            double[] valoresConsistencia = { 4, 3, 1, 0, -2, -4 };
            
            double porcentajeTotal = valoresHabilidad[habilidad - 1] + 
                                   valoresEsfuerzo[esfuerzo - 1] + 
                                   valoresCondiciones[condiciones - 1] + 
                                   valoresConsistencia[consistencia - 1];
            
            factorCalificacion = 1.0 + (porcentajeTotal / 100.0);
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Factor de calificación configurado: {factorCalificacion:F3}");
            Console.WriteLine($"  (Porcentaje total: {porcentajeTotal:+0;-0}%)");
            Console.ResetColor();
        }

        private void ConfigurarSuplementos()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("                CONFIGURAR SUPLEMENTOS");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Console.WriteLine("Ingrese los porcentajes de suplementos:\n");
            
            Console.Write("Necesidades personales (5-7%, recomendado 5%): ");
            double necesidades = LeerPorcentaje(5.0);
            
            Console.Write("Fatiga básica (estándar 4%): ");
            double fatiga = LeerPorcentaje(4.0);
            
            Console.Write("Suplementos variables (0-20%, recomendado 6%): ");
            double variables = LeerPorcentaje(6.0);
            
            suplementos = (necesidades + fatiga + variables) / 100.0;
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ Suplementos configurados: {suplementos * 100:F1}%");
            Console.WriteLine($"  Desglose: {necesidades}% + {fatiga}% + {variables}% = {(necesidades + fatiga + variables):F1}%");
            Console.ResetColor();
        }

        private void CalcularTiempoEstandar()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("               CALCULAR TIEMPO ESTÁNDAR");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            if (tiemposObservados.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("✗ Error: Debe ingresar al menos un tiempo observado.");
                Console.ResetColor();
                return;
            }
            
            // Cálculos
            double tiempoPromedio = tiemposObservados.Average();
            double tiempoNormal = tiempoPromedio * factorCalificacion;
            double tiempoEstandar = tiempoNormal * (1 + suplementos);
            double productividad = 60.0 / tiempoEstandar;
            
            // Mostrar resultados
            Console.WriteLine("RESULTADOS DEL CÁLCULO:");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Número de observaciones: {tiemposObservados.Count}");
            Console.WriteLine($"Tiempo promedio observado: {tiempoPromedio:F3} min");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Factor de calificación: {factorCalificacion:F3}");
            Console.WriteLine($"Tiempo normal: {tiempoNormal:F3} min");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Suplementos: {suplementos * 100:F1}%");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"TIEMPO ESTÁNDAR: {tiempoEstandar:F3} min");
            Console.WriteLine($"PRODUCTIVIDAD ESTÁNDAR: {productividad:F1} unidades/hora");
            Console.ResetColor();
            
            Console.WriteLine();
            Console.WriteLine("FÓRMULAS UTILIZADAS:");
            Console.WriteLine($"• Tiempo Normal = {tiempoPromedio:F3} × {factorCalificacion:F3} = {tiempoNormal:F3} min");
            Console.WriteLine($"• Tiempo Estándar = {tiempoNormal:F3} × (1 + {suplementos:F3}) = {tiempoEstandar:F3} min");
            Console.WriteLine($"• Productividad = 60 ÷ {tiempoEstandar:F3} = {productividad:F1} unid/hora");
        }

        private void MostrarResultadosDetallados()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("              RESULTADOS DETALLADOS");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            if (tiemposObservados.Count == 0)
            {
                Console.WriteLine("No hay datos para mostrar. Ingrese tiempos observados primero.");
                return;
            }
            
            // Estadísticas de tiempos
            double min = tiemposObservados.Min();
            double max = tiemposObservados.Max();
            double promedio = tiemposObservados.Average();
            double rango = max - min;
            
            Console.WriteLine("ESTADÍSTICAS DE TIEMPOS OBSERVADOS:");
            Console.WriteLine($"• Número de observaciones: {tiemposObservados.Count}");
            Console.WriteLine($"• Tiempo mínimo: {min:F3} min");
            Console.WriteLine($"• Tiempo máximo: {max:F3} min");
            Console.WriteLine($"• Tiempo promedio: {promedio:F3} min");
            Console.WriteLine($"• Rango: {rango:F3} min");
            
            if (tiemposObservados.Count > 1)
            {
                double varianza = tiemposObservados.Select(x => Math.Pow(x - promedio, 2)).Sum() / (tiemposObservados.Count - 1);
                double desviacion = Math.Sqrt(varianza);
                Console.WriteLine($"• Desviación estándar: {desviacion:F3} min");
                Console.WriteLine($"• Coeficiente de variación: {(desviacion / promedio) * 100:F1}%");
            }
            
            Console.WriteLine("\nTODOS LOS TIEMPOS:");
            for (int i = 0; i < tiemposObservados.Count; i++)
            {
                Console.WriteLine($"  {i + 1,2}. {tiemposObservados[i]:F3} min");
            }
            
            // Cálculos finales si hay datos completos
            if (tiemposObservados.Count > 0)
            {
                double tiempoNormal = promedio * factorCalificacion;
                double tiempoEstandar = tiempoNormal * (1 + suplementos);
                double productividad = 60.0 / tiempoEstandar;
                
                Console.WriteLine("\nRESUMEN DE CÁLCULOS:");
                Console.WriteLine($"• Factor de calificación: {factorCalificacion:F3}");
                Console.WriteLine($"• Suplementos: {suplementos * 100:F1}%");
                Console.WriteLine($"• Tiempo normal: {tiempoNormal:F3} min");
                Console.WriteLine($"• Tiempo estándar: {tiempoEstandar:F3} min");
                Console.WriteLine($"• Productividad: {productividad:F1} unidades/hora");
            }
        }

        private void LimpiarDatos()
        {
            Console.Clear();
            Console.WriteLine("¿Está seguro de que desea limpiar todos los datos? (s/n): ");
            string respuesta = Console.ReadLine().ToLower();
            
            if (respuesta == "s" || respuesta == "si" || respuesta == "sí")
            {
                tiemposObservados.Clear();
                factorCalificacion = 1.0;
                suplementos = 0.15;
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ Todos los datos han sido limpiados.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
            }
        }

        private void MostrarAyuda()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.WriteLine("                        AYUDA");
            Console.WriteLine("═══════════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Console.WriteLine("CALCULADORA DE ESTUDIO DE TIEMPOS");
            Console.WriteLine();
            Console.WriteLine("PASOS PARA USAR:");
            Console.WriteLine("1. Ingresar tiempos observados (mínimo 5 recomendado)");
            Console.WriteLine("2. Configurar factor de calificación (método Westinghouse)");
            Console.WriteLine("3. Configurar suplementos según condiciones de trabajo");
            Console.WriteLine("4. Calcular tiempo estándar");
            Console.WriteLine();
            Console.WriteLine("FÓRMULAS:");
            Console.WriteLine("• Tiempo Normal = Tiempo Observado × Factor de Calificación");
            Console.WriteLine("• Tiempo Estándar = Tiempo Normal × (1 + % Suplementos)");
            Console.WriteLine("• Productividad = 60 / Tiempo Estándar (unidades/hora)");
            Console.WriteLine();
            Console.WriteLine("FACTOR DE CALIFICACIÓN WESTINGHOUSE:");
            Console.WriteLine("Evalúa 4 aspectos: Habilidad, Esfuerzo, Condiciones, Consistencia");
            Console.WriteLine("Cada aspecto puede sumar o restar porcentaje al factor base (1.0)");
            Console.WriteLine();
            Console.WriteLine("SUPLEMENTOS TÍPICOS:");
            Console.WriteLine("• Necesidades personales: 5-7%");
            Console.WriteLine("• Fatiga básica: 4%");
            Console.WriteLine("• Variables: 0-20% (según condiciones específicas)");
        }

        private int LeerOpcion(int min, int max, int defecto)
        {
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return defecto;
                
            if (int.TryParse(input, out int opcion) && opcion >= min && opcion <= max)
                return opcion;
                
            Console.WriteLine($"Opción no válida. Usando valor por defecto: {defecto}");
            return defecto;
        }

        private double LeerPorcentaje(double defecto)
        {
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return defecto;
                
            if (double.TryParse(input, out double valor) && valor >= 0 && valor <= 50)
                return valor;
                
            Console.WriteLine($"Valor no válido. Usando valor por defecto: {defecto}%");
            return defecto;
        }
    }
}
