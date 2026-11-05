# Uso de la IA

## 1. Herramienta utilizada

- **Herramienta:** Claude (Anthropic), usado desde Claude Code. Modelo configurado en la sesión: `claude-sonnet-5-5`.
- **Para qué se usó:** generar las implementaciones de los siete algoritmos de ordenamiento (Selection, Bubble, Insertion, Shell, Merge, Quicksort y Radix).
- **Para qué no se usó:** el análisis, la interpretación de los resultados, la tabla de predicciones (Parte 1) y los párrafos de contraste (Parte 4) son del grupo, como exige la tarea.

## 2. Prompt principal

Este es el prompt con el que se generaron los algoritmos (texto completo, tal como se envió):

> Implementa en C# los siguientes algoritmos de ordenamiento desde cero: Selection Sort, Bubble Sort, Insertion Sort, Shell Sort, Merge Sort, Quicksort y Radix Sort. Crea una clase y un archivo independiente para cada algoritmo, con un método `Sort(int[] array)` que ordene el arreglo ascendentemente. Usa implementaciones clásicas, sin optimizaciones adicionales. En Quicksort, utiliza siempre el primer o último elemento como pivote. En Radix Sort, considera enteros no negativos. Entrega el código completo de cada archivo, claro y comentado, e incluye las complejidades temporales y espaciales de cada algoritmo. No implementes el benchmark ni las pruebas todavía.

**Resultado:** siete archivos (`SelectionSort.cs`, `BubbleSort.cs`, `InsertionSort.cs`, `ShellSort.cs`, `MergeSort.cs`, `QuickSort.cs`, `RadixSort.cs`), cada uno con su clase, un método `Sort(int[] array)`, comentarios y las complejidades temporal y espacial.

## 3. Qué se corrigió o ajustó en el código

La IA entregó los siete algoritmos en el primer intento. Al compararlos con `Array.Sort` no se detectó ningún error de lógica, por lo que no fue necesario corregirlos.

> **[COMPLETAR]** Si el grupo modificó algo de los archivos (nombres, comentarios, namespace, estilo), anótenlo aquí. Si no se tocó nada, dejen esta sección tal cual.

## 4. Cómo se verificó el código

Cada algoritmo se comparó contra `Array.Sort`, el algoritmo de referencia de la biblioteca estándar, con arreglos de distintos tamaños y con casos límite (arreglo vacío, un elemento, elementos repetidos, negativos para los algoritmos por comparación y rechazo de negativos en Radix).

> **[COMPLETAR]** Anotar el resultado de ejecutar `dotnet test` en la computadora del grupo.

## 5. Decisiones de implementación que el grupo debe entender

- **Selection Sort:** siempre recorre toda la parte no ordenada, por eso es O(n²) en los tres casos, y hace como máximo n-1 intercambios.
- **Bubble Sort:** es la versión clásica, sin bandera de corte anticipado. Por eso su mejor caso también es O(n²).
- **Insertion Sort:** su mejor caso es O(n) con la entrada ya ordenada y su peor caso es O(n²) con la entrada descendente.
- **Shell Sort:** usa la secuencia original de Shell (n/2, n/4, …, 1). Su rendimiento depende de la secuencia y su peor caso con esta es O(n²).
- **Merge Sort:** es recursivo top-down con un único arreglo auxiliar, así que usa O(n) de memoria extra y es O(n log n) en todos los casos.
- **Quicksort:** usa partición de Lomuto con el último elemento como pivote. El mejor caso se da cuando el pivote parte el arreglo por la mitad, y el peor cuando queda en un extremo.
- **Radix Sort:** es LSD en base 10, con Counting Sort estable por dígito y complejidad O(d·(n+b)). Solo acepta enteros no negativos y lanza una excepción si recibe un negativo. El multiplicador de dígitos es `long` para evitar desbordamiento con valores cercanos a `int.MaxValue`.

## 6. Referencias

- Anthropic. (2026). *Claude* [Modelo de lenguaje]. https://claude.ai
- **[COMPLETAR]** Agregar aquí la bibliografía de teoría que usó el grupo (libro de texto, apuntes del curso, etc.).
