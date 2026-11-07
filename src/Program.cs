using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using BenchmarkOrdenamiento.Benchmark;

namespace BenchmarkOrdenamiento
{
    internal class Program
    {
        // CSV con todas las mediciones individuales (se crea la carpeta si no existe).
        private const string CsvPath = "data/mediciones.csv";

        // Tamaño de pila del hilo que ejecuta el benchmark: 512 MB.
        // Quicksort con pivote fijo recursa a profundidad O(n) con entradas ordenadas o con duplicados,
        // y la pila por defecto (1 MB en Windows) no alcanza. Un StackOverflow en .NET no se puede capturar.
        private const int StackSize = 512 * 1024 * 1024;

        static void Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("ADVERTENCIA: compilación Debug. Para medir use: dotnet run -c Release --project src/BenchmarkOrdenamiento");
#endif
            Thread worker = new Thread(RunBenchmark, StackSize);
            worker.Start();
            worker.Join();
        }

        private static void RunBenchmark()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            using (CsvExporter csv = new CsvExporter(CsvPath))
            {
                BenchmarkRunner runner = new BenchmarkRunner(csv);

                Console.WriteLine("=== Experimento principal ===");
                runner.RunMain();

                Console.WriteLine();
                Console.WriteLine("=== Prueba especial de Radix (n fijo, variando dígitos) ===");
                runner.RunRadixDigits();
            }

            stopwatch.Stop();

            // Datos del entorno, útiles para el informe.
            Console.WriteLine();
            Console.WriteLine("Listo en " + stopwatch.Elapsed + ". Mediciones guardadas en " + CsvPath);
            Console.WriteLine("Sistema operativo: " + RuntimeInformation.OSDescription);
            Console.WriteLine("Runtime: " + RuntimeInformation.FrameworkDescription);
            Console.WriteLine("Procesadores lógicos: " + Environment.ProcessorCount);
        }
    }
}
