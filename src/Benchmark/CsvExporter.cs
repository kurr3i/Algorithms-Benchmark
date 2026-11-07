using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace BenchmarkOrdenamiento.Benchmark
{
    /// <summary>
    /// Escribe las mediciones individuales a un archivo CSV, una fila por repetición
    /// (la mediana NO se calcula aquí: se obtiene después, en el análisis).
    /// Escribe de forma incremental: si una ejecución larga se interrumpe, lo medido se conserva.
    /// </summary>
    public class CsvExporter : IDisposable
    {
        private readonly StreamWriter _writer;

        public CsvExporter(string path)
        {
            string directory = Path.GetDirectoryName(Path.GetFullPath(path));
            Directory.CreateDirectory(directory);

            _writer = new StreamWriter(path, false, new UTF8Encoding(false));
            _writer.WriteLine("experiment,algorithm,input_type,n,digits,repetition,time_ns");
        }

        /// <summary>Escribe una medición individual.</summary>
        /// <param name="experiment">"main" (experimento principal) o "radix_digits" (prueba especial de Radix).</param>
        /// <param name="algorithm">Nombre del algoritmo.</param>
        /// <param name="inputType">Tipo de entrada (Random, Ascending, Descending, Duplicates).</param>
        /// <param name="n">Tamaño del arreglo.</param>
        /// <param name="digits">Dígitos de los valores; solo en la prueba de Radix (null en el resto).</param>
        /// <param name="repetition">Número de repetición (1..5); el calentamiento no se registra.</param>
        /// <param name="timeNs">Tiempo de ordenamiento en nanosegundos.</param>
        public void Write(string experiment, string algorithm, string inputType, int n, int? digits, int repetition, long timeNs)
        {
            // Cultura invariante: sin separadores de miles, sin importar la configuración regional.
            string digitsText = digits.HasValue ? digits.Value.ToString(CultureInfo.InvariantCulture) : "";

            _writer.WriteLine(string.Join(",",
                experiment,
                algorithm,
                inputType,
                n.ToString(CultureInfo.InvariantCulture),
                digitsText,
                repetition.ToString(CultureInfo.InvariantCulture),
                timeNs.ToString(CultureInfo.InvariantCulture)));
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
