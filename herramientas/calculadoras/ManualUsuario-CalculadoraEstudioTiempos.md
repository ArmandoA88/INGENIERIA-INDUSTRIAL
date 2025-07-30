# Manual de Usuario - Calculadora de Estudio de Tiempos

## 📋 Descripción General

La Calculadora de Estudio de Tiempos es una aplicación desarrollada en Visual Basic .NET que permite a los ingenieros industriales calcular tiempos estándar de manera rápida y precisa utilizando el método Westinghouse.

## 🚀 Instalación

### Requisitos del Sistema
- **Sistema Operativo**: Windows 10 o superior
- **Framework**: .NET Framework 4.7.2 o superior
- **IDE**: Visual Studio 2019 o superior (para desarrollo)
- **Memoria RAM**: Mínimo 4 GB
- **Espacio en disco**: 50 MB libres

### Pasos de Instalación
1. Descargar el archivo `CalculadoraEstudioTiempos.vb`
2. Abrir Visual Studio
3. Crear nuevo proyecto:
   - File → New → Project
   - Seleccionar "Windows Forms App (.NET Framework)"
   - Nombrar el proyecto: "CalculadoraEstudioTiempos"
4. Reemplazar el código del Form1.vb con el código descargado
5. Compilar el proyecto (Build → Build Solution)
6. Ejecutar la aplicación (F5 o Debug → Start Debugging)

## 🖥️ Interfaz de Usuario

### Ventana Principal
La aplicación se divide en 5 secciones principales:

#### 1. Tiempos Observados
- **Campo de entrada**: Para ingresar cada tiempo observado en minutos
- **Botón Agregar**: Añade el tiempo a la lista
- **Lista de tiempos**: Muestra todos los tiempos ingresados
- **Botón Eliminar**: Remueve el tiempo seleccionado
- **Botón Limpiar**: Elimina todos los tiempos

#### 2. Factor de Calificación (Método Westinghouse)
- **Habilidad**: Evalúa la destreza del operador
- **Esfuerzo**: Evalúa la voluntad de trabajar del operador
- **Condiciones**: Evalúa las condiciones del ambiente de trabajo
- **Consistencia**: Evalúa la uniformidad en los tiempos
- **Factor Total**: Muestra el factor de calificación calculado

#### 3. Suplementos
- **Necesidades personales**: Tiempo para necesidades básicas (5-7%)
- **Fatiga básica**: Tiempo de recuperación básica (4%)
- **Suplementos variables**: Factores adicionales según condiciones

#### 4. Resultados
- **Tiempo Promedio Observado**: Media aritmética de los tiempos
- **Tiempo Normal**: Tiempo observado × Factor de calificación
- **Tiempo Estándar**: Tiempo normal × (1 + % suplementos)
- **Productividad Estándar**: Unidades por hora que se pueden producir

#### 5. Información del Estudio
- **Operación**: Nombre de la operación estudiada
- **Operador**: Nombre del trabajador observado
- **Fecha**: Fecha del estudio

## 📊 Procedimiento de Uso

### Paso 1: Ingresar Tiempos Observados
1. En el campo "Tiempo observado", escribir el tiempo en minutos (ej: 2.45)
2. Hacer clic en "Agregar" o presionar Enter
3. Repetir para cada observación (mínimo 5-10 observaciones recomendadas)
4. Los tiempos aparecerán en la lista numerados

**Ejemplo:**
```
Obs 1: 2.450 min
Obs 2: 2.380 min
Obs 3: 2.520 min
Obs 4: 2.410 min
Obs 5: 2.470 min
```

### Paso 2: Configurar Factor de Calificación
Seleccionar la opción apropiada para cada factor:

#### Habilidad del Operador
- **Superior (+15%)**: Operador excepcional, muy experimentado
- **Excelente (+11%)**: Operador muy hábil
- **Buena (+6%)**: Operador hábil
- **Promedio (0%)**: Operador con habilidad normal
- **Regular (-10%)**: Operador con poca experiencia
- **Deficiente (-16%)**: Operador inexperto

#### Esfuerzo del Operador
- **Excesivo (+13%)**: Ritmo insostenible a largo plazo
- **Excelente (+10%)**: Muy buen ritmo de trabajo
- **Bueno (+5%)**: Buen ritmo de trabajo
- **Promedio (0%)**: Ritmo normal
- **Regular (-4%)**: Ritmo lento
- **Deficiente (-8%)**: Ritmo muy lento

#### Condiciones de Trabajo
- **Ideales (+6%)**: Condiciones perfectas
- **Excelentes (+4%)**: Muy buenas condiciones
- **Buenas (+2%)**: Buenas condiciones
- **Promedio (0%)**: Condiciones normales
- **Regulares (-3%)**: Condiciones por debajo del promedio
- **Deficientes (-7%)**: Malas condiciones

#### Consistencia
- **Perfecta (+4%)**: Tiempos muy uniformes
- **Excelente (+3%)**: Tiempos uniformes
- **Buena (+1%)**: Poca variación
- **Promedio (0%)**: Variación normal
- **Regular (-2%)**: Variación considerable
- **Deficiente (-4%)**: Mucha variación

### Paso 3: Ajustar Suplementos
Modificar los porcentajes según las condiciones específicas:

#### Suplementos Constantes
- **Necesidades personales**: 5-7% (valor típico: 5%)
- **Fatiga básica**: 4% (estándar)

#### Suplementos Variables (ejemplos)
- **Trabajo de pie**: +2%
- **Postura incómoda**: +0-7%
- **Uso de fuerza**: +0-17%
- **Mala iluminación**: +0-5%
- **Condiciones atmosféricas**: +0-10%
- **Concentración**: +0-5%
- **Ruido**: +0-5%

### Paso 4: Calcular Resultados
1. Hacer clic en "CALCULAR TIEMPO ESTÁNDAR"
2. Revisar los resultados mostrados
3. Verificar que los valores sean razonables

### Paso 5: Documentar el Estudio
1. Completar la información del estudio:
   - Nombre de la operación
   - Nombre del operador
   - Fecha del estudio
2. Usar los botones de acción según necesidad

## 🧮 Fórmulas Utilizadas

### Tiempo Normal
```
Tiempo Normal = Tiempo Promedio Observado × Factor de Calificación
```

### Tiempo Estándar
```
Tiempo Estándar = Tiempo Normal × (1 + Porcentaje de Suplementos)
```

### Productividad Estándar
```
Productividad = 60 minutos/hora ÷ Tiempo Estándar
```

### Factor de Calificación
```
Factor = 1 + (% Habilidad + % Esfuerzo + % Condiciones + % Consistencia) / 100
```

## 📝 Ejemplo Práctico

### Datos de Entrada
- **Operación**: Ensamble de componente electrónico
- **Operador**: Juan Pérez
- **Tiempos observados**: 2.45, 2.38, 2.52, 2.41, 2.47, 2.39, 2.48 minutos
- **Calificación**: Habilidad Buena (+6%), Esfuerzo Promedio (0%), Condiciones Buenas (+2%), Consistencia Buena (+1%)
- **Suplementos**: Necesidades 5%, Fatiga 4%, Variables 6%

### Cálculos
1. **Tiempo Promedio**: (2.45+2.38+2.52+2.41+2.47+2.39+2.48) ÷ 7 = 2.44 min
2. **Factor de Calificación**: 1 + (6+0+2+1)/100 = 1.09
3. **Tiempo Normal**: 2.44 × 1.09 = 2.66 min
4. **Suplementos**: (5+4+6)/100 = 0.15 (15%)
5. **Tiempo Estándar**: 2.66 × (1+0.15) = 3.06 min
6. **Productividad**: 60 ÷ 3.06 = 19.6 unidades/hora

## ⚠️ Consideraciones Importantes

### Buenas Prácticas
- **Número de observaciones**: Mínimo 5, recomendado 10-15
- **Condiciones consistentes**: Mantener las mismas condiciones durante todas las observaciones
- **Operador representativo**: Seleccionar operador con habilidad promedio
- **Método estandarizado**: Asegurar que el método esté bien definido

### Limitaciones
- **Solo para trabajos repetitivos**: No aplicable a trabajos únicos
- **Requiere operador cooperativo**: El operador debe trabajar normalmente
- **Subjetividad en calificación**: La evaluación del factor puede variar entre analistas
- **Condiciones estables**: Los resultados son válidos solo bajo condiciones similares

### Errores Comunes
- **Muy pocas observaciones**: Reduce la confiabilidad
- **Calificación incorrecta**: Afecta significativamente el resultado
- **Suplementos inadecuados**: Pueden ser muy altos o muy bajos
- **Método no estandarizado**: Genera variabilidad excesiva

## 🔧 Solución de Problemas

### Error: "Tiempo válido mayor a 0"
- **Causa**: Se ingresó un valor negativo o cero
- **Solución**: Ingresar solo valores positivos en minutos

### Factor de calificación no se actualiza
- **Causa**: No se han seleccionado todas las opciones
- **Solución**: Verificar que todos los ComboBox tengan una selección

### Resultados parecen incorrectos
- **Causa**: Datos de entrada erróneos o calificación inadecuada
- **Solución**: Revisar tiempos observados y factores de calificación

## 📞 Soporte Técnico

Para problemas técnicos o consultas sobre el uso de la calculadora:
- Revisar este manual
- Consultar la documentación de estudio de tiempos
- Contactar al equipo de desarrollo a través del repositorio

## 📚 Referencias

- Niebel, B. (2019). Ingeniería Industrial: Métodos, Estándares y Diseño del Trabajo
- Sistema de Calificación Westinghouse
- Normas de suplementos de tiempo ILO (International Labour Organization)
- [Metodología de Estudio de Tiempos](../../metodologias/estudio-de-tiempos/)

---

**Versión**: 1.0  
**Fecha**: 2025  
**Autor**: Repositorio Ingeniería Industrial
