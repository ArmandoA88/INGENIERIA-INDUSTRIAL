# 🚀 CÓMO EJECUTAR DESDE VISUAL STUDIO CODE - PASO A PASO

## Método 1: Crear Proyecto .NET desde VSCode

### Paso 1: Instalar Extensiones Necesarias
1. En VSCode, ir a **Extensions** (Ctrl+Shift+X)
2. Buscar e instalar:
   - ✅ **"C# Dev Kit"** (Microsoft)
   - ✅ **".NET Extension Pack"** (Microsoft)
3. Reiniciar VSCode

### Paso 2: Instalar .NET SDK
1. Ir a: https://dotnet.microsoft.com/download
2. Descargar **.NET 8.0 SDK** (o la versión más reciente)
3. Instalar siguiendo el asistente

### Paso 3: Crear Proyecto Windows Forms
1. Abrir **Terminal** en VSCode (Ctrl+`)
2. Navegar a la carpeta donde quieres el proyecto:
   ```bash
   cd "C:\Users\TuUsuario\Desktop"
   ```
3. Crear proyecto Windows Forms:
   ```bash
   dotnet new winforms -n CalculadoraEstudioTiempos
   ```
4. Entrar al proyecto:
   ```bash
   cd CalculadoraEstudioTiempos
   ```
5. Abrir en VSCode:
   ```bash
   code .
   ```

### Paso 4: Reemplazar el Código
1. En VSCode, abrir el archivo **`Form1.vb`**
2. **Seleccionar todo** (Ctrl+A) y **eliminar**
3. **Copiar y pegar** el código completo de `CalculadoraEstudioTiempos.vb`
4. **Guardar** (Ctrl+S)

### Paso 5: Ejecutar
1. En la **Terminal** de VSCode:
   ```bash
   dotnet run
   ```
2. ¡La calculadora se abrirá!

## Método 2: Ejecutar Directamente (Más Simple)

### Paso 1: Crear Archivo Temporal
1. En VSCode, crear nuevo archivo: **`calculadora.vb`**
2. Copiar y pegar el código de `CalculadoraEstudioTiempos.vb`
3. Guardar el archivo

### Paso 2: Compilar y Ejecutar
1. Abrir **Terminal** en VSCode (Ctrl+`)
2. Navegar a donde guardaste el archivo
3. Compilar:
   ```bash
   vbc calculadora.vb /target:winexe /reference:System.Windows.Forms.dll,System.Drawing.dll
   ```
4. Ejecutar:
   ```bash
   calculadora.exe
   ```

## Método 3: Script de PowerShell (Automático)

### Crear Script de Ejecución
1. Crear archivo **`ejecutar.ps1`** en VSCode:

```powershell
# Script para ejecutar la calculadora
Write-Host "Creando proyecto..." -ForegroundColor Green

# Crear directorio del proyecto
$projectDir = "CalculadoraEstudioTiempos"
if (Test-Path $projectDir) {
    Remove-Item $projectDir -Recurse -Force
}

# Crear proyecto Windows Forms
dotnet new winforms -n $projectDir
Set-Location $projectDir

# Copiar código (debes pegar manualmente el código en Form1.vb)
Write-Host "Abre Form1.vb y pega el código de la calculadora" -ForegroundColor Yellow
Write-Host "Presiona Enter cuando hayas pegado el código..."
Read-Host

# Ejecutar
Write-Host "Ejecutando calculadora..." -ForegroundColor Green
dotnet run
```

2. Ejecutar en Terminal:
   ```bash
   powershell -ExecutionPolicy Bypass -File ejecutar.ps1
   ```

## Método 4: Usando C# en lugar de VB.NET

### Si prefieres C# (más compatible con VSCode):
1. Crear proyecto C#:
   ```bash
   dotnet new winforms -n CalculadoraEstudioTiempos
   cd CalculadoraEstudioTiempos
   ```

2. Te puedo convertir el código de VB.NET a C# si prefieres

## ⚠️ Solución de Problemas

### Error: "dotnet no se reconoce"
- Instalar .NET SDK desde: https://dotnet.microsoft.com/download
- Reiniciar VSCode después de instalar

### Error: "vbc no se reconoce"
- Instalar Visual Studio Build Tools
- O usar el método con `dotnet new winforms`

### Error: Extensiones no funcionan
- Verificar que instalaste "C# Dev Kit"
- Reiniciar VSCode
- Verificar que .NET SDK está instalado

### La ventana no se abre
- Verificar que el código se copió completo
- Revisar errores en la Terminal
- Probar con `dotnet build` primero

## 🎯 Método Recomendado para VSCode

**El más fácil es el Método 1:**
1. Instalar extensiones C# Dev Kit
2. Instalar .NET SDK
3. `dotnet new winforms -n CalculadoraEstudioTiempos`
4. Pegar código en Form1.vb
5. `dotnet run`

## 📞 Si Nada Funciona

### Alternativa Rápida:
1. Descargar **Visual Studio Community** (más fácil para Windows Forms)
2. O puedo crear una **versión de consola** más simple
3. O convertir a **C#** que funciona mejor en VSCode

¿Con cuál método quieres que te ayude específicamente?
