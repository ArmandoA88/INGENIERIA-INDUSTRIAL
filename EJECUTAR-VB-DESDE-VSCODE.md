# 🖥️ EJECUTAR INTERFAZ GRÁFICA DE VISUAL BASIC DESDE VSCODE

## 🎯 Para la Interfaz con Objetos Visuales (Botones, TextBox, etc.)

### Método 1: Usar Visual Studio Community (Recomendado)

#### Paso 1: Descargar Visual Studio Community
1. Ir a: https://visualstudio.microsoft.com/es/downloads/
2. Descargar **Visual Studio Community 2022** (GRATIS)
3. Durante instalación seleccionar:
   - ✅ **Desarrollo de escritorio de .NET**
   - ✅ **Visual Basic**

#### Paso 2: Crear Proyecto Windows Forms
1. Abrir Visual Studio Community
2. **Crear nuevo proyecto**
3. Seleccionar **"Aplicación de Windows Forms (.NET Framework)"**
4. Seleccionar **Visual Basic** como lenguaje
5. Nombrar: `CalculadoraEstudioTiempos`

#### Paso 3: Usar el Código VB
1. Ir a **Explorador de soluciones**
2. Doble clic en **Form1.vb**
3. **Seleccionar todo** el código (Ctrl+A)
4. **Eliminar** y **pegar** el código de `CalculadoraEstudioTiempos.vb`
5. Presionar **F5** para ejecutar

### Método 2: Desde VSCode (Más Complejo)

#### Paso 1: Instalar Extensiones
1. En VSCode: **Extensions** (Ctrl+Shift+X)
2. Instalar:
   - ✅ **"VB.NET"**
   - ✅ **"Visual Basic .NET"**

#### Paso 2: Crear Proyecto VB.NET
```bash
# En terminal de VSCode:
dotnet new winforms -lang VB -n CalculadoraVB
cd CalculadoraVB
```

#### Paso 3: Reemplazar Form1.vb
1. Abrir `Form1.vb`
2. Reemplazar con el código de `CalculadoraEstudioTiempos.vb`
3. Ejecutar: `dotnet run`

## 🖼️ Lo que Verás con la Interfaz Gráfica:

### Ventana Principal (800x600 pixels):
- **Título**: "Calculadora de Estudio de Tiempos - Ingeniería Industrial"
- **5 Secciones organizadas visualmente**

### Sección 1: Tiempos Observados
- 📝 **TextBox** para ingresar tiempos
- 🔘 **Botón "Agregar"** 
- 📋 **ListBox** con todos los tiempos
- 🗑️ **Botón "Eliminar Seleccionado"**
- 🧹 **Botón "Limpiar Todo"**

### Sección 2: Factor de Calificación
- 📊 **4 ComboBox** para método Westinghouse:
  - Habilidad (Superior, Excelente, Buena, etc.)
  - Esfuerzo (Excesivo, Excelente, Bueno, etc.)
  - Condiciones (Ideales, Excelentes, Buenas, etc.)
  - Consistencia (Perfecta, Excelente, Buena, etc.)
- 📈 **Label** mostrando factor total calculado

### Sección 3: Suplementos
- 📝 **3 TextBox** para porcentajes:
  - Necesidades personales (5%)
  - Fatiga básica (4%)
  - Suplementos variables (6%)

### Sección 4: Resultados
- 📊 **4 Labels** con resultados:
  - Tiempo Promedio Observado
  - Tiempo Normal
  - **Tiempo Estándar** (destacado en verde)
  - Productividad Estándar
- 🔵 **Botón "CALCULAR TIEMPO ESTÁNDAR"** (azul, grande)

### Sección 5: Información del Estudio
- 📝 **TextBox** para nombre de operación
- 📝 **TextBox** para nombre del operador
- 📅 **DateTimePicker** para fecha

### Botones de Acción:
- 💾 **"Guardar Estudio"** (verde)
- 🆕 **"Nuevo Estudio"**
- 📤 **"Exportar Reporte"**
- ❓ **"Ayuda"**

## 🎨 Características Visuales:

✅ **Colores profesionales**: Azul, verde, amarillo  
✅ **Fuentes apropiadas**: Arial, tamaños variables  
✅ **Organización clara**: 5 secciones bien definidas  
✅ **Validaciones visuales**: Mensajes de error/éxito  
✅ **Interfaz intuitiva**: Fácil de usar  

## ⚡ Ejecución Rápida:

### Opción A: Visual Studio Community
```
1. Descargar VS Community
2. Crear proyecto Windows Forms VB
3. Pegar código de CalculadoraEstudioTiempos.vb
4. F5 para ejecutar
```

### Opción B: Desde VSCode
```bash
dotnet new winforms -lang VB -n CalculadoraVB
cd CalculadoraVB
# Reemplazar Form1.vb con el código
dotnet run
```

## 🔧 Si Hay Problemas:

**Error "Windows Forms no disponible":**
- Instalar .NET Framework 4.7.2+
- Usar Visual Studio Community (más fácil)

**Error de compilación VB:**
- Verificar sintaxis de Visual Basic
- Usar Visual Studio Community (mejor soporte VB)

## 📱 Resultado Final:

¡Tendrás una **ventana gráfica profesional** con:
- Botones clickeables
- Campos de texto
- Listas desplegables
- Resultados en tiempo real
- Interfaz visual completa!

¿Quieres que te ayude con Visual Studio Community o prefieres intentar desde VSCode?
