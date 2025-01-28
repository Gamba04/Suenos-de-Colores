# Prueba Técnica - Sueños de Colores

Este es un proyecto desarrollado como prueba de capacidades de desarrollo en Unity, aplicado a los objetivos generales del proyecto Sueños de Colores por parte del equipo ASEIS.

## Paquetes de terceros
Ninguno.

Únicamente se utilizaron algunos archivos de código personales de proyectos anteriores.

## Selección de archivos
Para la implementación del cuadro de diálogo del explorador de archivos, se utilizó un llamado directo a la API de Win32 mediante [P/Invoke](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke), invocando la librería `Comdlg32.dll`.

## Colores predominantes
La elección de los colores predominantes consiste en encontrar los `n` primeros colores con mayor cantidad de veces repetidas en la imágen (en este caso, los primeros 3).

## Exportación a Excel
El formato utilizado para la exportación fue CSV, ya que es un formato liviano y fácilmente interpretado por Microsoft Excel.

La escritura del archivo se llevó a cabo con la herramienta `StreamWriter` de la librería `System.IO`.
