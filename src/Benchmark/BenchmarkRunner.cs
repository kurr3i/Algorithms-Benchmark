using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sorting;

namespace BenchmarkOrdenamiento.Benchmark
{
    /// <summary>
    /// Ejecuta los experimentos: mide únicamente el ordenamiento (sin la generación de la entrada),
    /// usa la misma entrada para todos los algoritmos y guarda cada repetición en el CSV.
    /// </summary>
    public class BenchmarkRunner
    {
        // ---- Parámetros del experimento (siguen la especificación de la tarea) ----

        private const int Repetitions = 5;             // repeticiones registradas por medición
        private const int Warmups = 1;                 // ejecuciones de calentamiento (JIT) que se descartan
        private const int Seed = 20261028;             // semilla fija: el experimento es reproducible
        private const int StartN = 1000;               // primer tamaño; cada medición siguiente duplica n
        private const int QuadraticMaxN = 16000;       // tamaño máximo de Selection, Bubble e Insertion
        private const int FastMaxN = 1024000;          // tamaño máximo de Shell, Merge, Quicksort y Radix (>= 1 000 000)

        // Quicksort con pivote fijo es O(n²) y recursa a profundidad O(n) en entradas ordenadas,
        // descendentes y con duplicados: en esos casos se limita n para evitar el desbordamiento de pila.
        private const int QuickSortUnsortedMaxN = 64000;

        private const int RadixN = 100000;                              // n fijo de la prueba especial de Radix
        private static readonly int[] RadixDigits = { 3, 6, 9 };        // dígitos a probar en esa prueba

        /// <summary>Un algoritmo a medir con sus tamaños máximos.</summary>
        private class AlgorithmSpec
        {
            public string Name;
            public Action<int[]> Sort;
            public int MaxNRandom;   // tamaño máximo con entrada aleatoria
            public int MaxNOther;    // tamaño máximo con los demás tipos de entrada

            public AlgorithmSpec(string name, Action<int[]> sort, int maxNRandom, int maxNOther)
            {
                Name = name;
                Sort = sort;
                MaxNRandom = maxNRandom;
                MaxNOther = maxNOther;
            }
        }

        private readonly CsvExporter _csv;

        public BenchmarkRunner(CsvExporter csv)
        {
            _csv = csv;
        }

        private List<AlgorithmSpec> BuildAlgorithms()
        {
            List<AlgorithmSpec> algorithms = new List<AlgorithmSpec>();
            algorithms.Add(new AlgorithmSpec("Selection", new SelectionSort().Sort, QuadraticMaxN, QuadraticMaxN));
            algorithms.Add(new AlgorithmSpec("Bubble", new BubbleSort().Sort, QuadraticMaxN, QuadraticMaxN));
            algorithms.Add(new AlgorithmSpec("Insertion", new InsertionSort().Sort, QuadraticMaxN, QuadraticMaxN));
            algorithms.Add(new AlgorithmSpec("Shell", new ShellSort().Sort, FastMaxN, FastMaxN));
            algorithms.Add(new AlgorithmSpec("Merge", new MergeSort().Sort, FastMaxN, FastMaxN));
            algorithms.Add(new AlgorithmSpec("Quicksort", new QuickSort().Sort, FastMaxN, QuickSortUnsortedMaxN));
            algorithms.Add(new AlgorithmSpec("Radix", new RadixSort().Sort, FastMaxN, FastMaxN));
            return algorithms;
        }

        /// <summary>Experimento principal: 4 tipos de entrada × tamaños que se duplican × 7 algoritmos.</summary>
        public void RunMain()
        {
            List<AlgorithmSpec> algorithms = BuildAlgorithms();

            foreach (InputType type in Enum.GetValues(typeof(InputType)))
            {
                for (int n = StartN; n <= FastMaxN; n *= 2)
                {
                    // La entrada se genera UNA vez por (tipo, n) y se comparte entre todos los algoritmos.
                    int[] input = InputGenerator.Generate(type, n, Seed ^ n);
                    int[] expected = Reference(input);
                    int[] work = new int[n];

                    foreach (AlgorithmSpec algorithm in algorithms)
                    {
                        int maxN = type == InputType.Random ? algorithm.MaxNRandom : algorithm.MaxNOther;
                        if (n > maxN)
                        {
                            continue; // este algoritmo no se prueba con un n tan grande
                        }

                        Measure("main", algorithm, type.ToString(), n, null, input, expected, work);
                    }

                    _csv.Flush();
                }
            }
        }

        /// <summary>Prueba especial de Radix: n fijo, valores con 3, 6 y 9 dígitos.</summary>
        public void RunRadixDigits()
        {
            AlgorithmSpec radix = new AlgorithmSpec("Radix", new RadixSort().Sort, RadixN, RadixN);

            foreach (int digits in RadixDigits)
            {
                int[] input = InputGenerator.GenerateWithDigits(RadixN, digits, Seed ^ (digits * 7919));
                int[] expected = Reference(input);
                int[] work = new int[RadixN];

                Measure("radix_digits", radix, "Random", RadixN, digits, input, expected, work);
                _csv.Flush();
            }
        }

        /// <summary>Resultado esperado: la entrada ordenada con Array.Sort (algoritmo de referencia de la biblioteca estándar).</summary>
        private static int[] Reference(int[] input)
        {
            int[] copy = (int[])input.Clone();
            Array.Sort(copy);
            return copy;
        }

        /// <summary>
        /// Mide un algoritmo sobre una entrada: primero el calentamiento (descartado) y luego las
        /// repeticiones registradas. En cada ejecución la entrada se copia ANTES de iniciar el
        /// cronómetro, y el resultado se valida contra Array.Sort DESPUÉS de detenerlo.
        /// </summary>
        private void Measure(string experiment, AlgorithmSpec algorithm, string inputType, int n, int? digits,
                             int[] input, int[] expected, int[] work)
        {
            double nsPerTick = 1000000000.0 / Stopwatch.Frequency;
            List<long> times = new List<long>();

            for (int run = 0; run < Warmups + Repetitions; run++)
            {
                Array.Copy(input, work, n); // restaurar la entrada (fuera del tiempo medido)

                GC.Collect();
                GC.WaitForPendingFinalizers();

                long start = Stopwatch.GetTimestamp();
                algorithm.Sort(work);
                long end = Stopwatch.GetTimestamp();

                // Verificación de correctitud contra el algoritmo de referencia, fuera del tiempo medido.
                for (int i = 0; i < n; i++)
                {
                    if (work[i] != expected[i])
                    {
                        throw new InvalidOperationException(
                            algorithm.Name + " produjo un resultado incorrecto (tipo=" + inputType + ", n=" + n + ").");
                    }
                }

                if (run < Warmups)
                {
                    continue; // calentamiento: no se registra
                }

                long timeNs = (long)Math.Round((end - start) * nsPerTick);
                times.Add(timeNs);
                _csv.Write(experiment, algorithm.Name, inputType, n, digits, run - Warmups + 1, timeNs);
            }

            // Solo informativo en consola; la mediana oficial se calcula en el análisis.
            times.Sort();
            double medianMs = times[times.Count / 2] / 1000000.0;
            string digitsText = digits.HasValue ? " d=" + digits.Value : "";
            Console.WriteLine("[" + inputType.PadRight(10) + "] n=" + n.ToString().PadLeft(8) + digitsText + "  "
                              + algorithm.Name.PadRight(10) + " mediana ~ " + medianMs.ToString("F3") + " ms");
        }
    }
}
