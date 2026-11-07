using System;

namespace Sorting
{
    /// <summary>
    /// Radix Sort LSD (Least Significant Digit) en base 10, para enteros NO negativos.
    ///
    /// Idea: ordenar por cada dígito, desde el menos significativo (unidades)
    /// hasta el más significativo, usando en cada paso un Counting Sort estable.
    /// No compara elementos entre sí.
    ///
    /// Complejidad temporal:
    ///   - Mejor / medio / peor caso: O(d · (n + b))
    ///     donde d = número de dígitos del mayor valor y b = base (10).
    ///     Con b constante equivale a O(d · n).
    /// Complejidad espacial: O(n + b) (arreglo de salida + contadores).
    /// Estable: Sí.
    ///
    /// Precondición: todos los elementos deben ser >= 0.
    /// </summary>
    public class RadixSort
    {
        private const int Base = 10;

        /// <summary>Ordena el arreglo de forma ascendente (solo enteros no negativos).</summary>
        public void Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length < 2) return;

            // Hallar el máximo para saber cuántos dígitos procesar; validar no negativos.
            int max = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < 0)
                    throw new ArgumentException("Radix Sort solo admite enteros no negativos.", nameof(array));
                if (array[i] > max)
                    max = array[i];
            }

            // exp = 1, 10, 100, ... (long para evitar desbordamiento con valores cercanos a int.MaxValue).
            for (long exp = 1; max / exp > 0; exp *= Base)
            {
                CountingSortByDigit(array, exp);
            }
        }

        /// <summary>Counting Sort estable según el dígito (value / exp) % 10.</summary>
        private void CountingSortByDigit(int[] array, long exp)
        {
            int n = array.Length;
            int[] output = new int[n];
            int[] count = new int[Base];

            // 1. Contar la frecuencia de cada dígito.
            for (int i = 0; i < n; i++)
            {
                int digit = (int)((array[i] / exp) % Base);
                count[digit]++;
            }

            // 2. Acumular: count[d] pasa a ser la posición final (exclusiva) del dígito d.
            for (int d = 1; d < Base; d++)
                count[d] += count[d - 1];

            // 3. Construir la salida recorriendo de derecha a izquierda para mantener estabilidad.
            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (int)((array[i] / exp) % Base);
                output[--count[digit]] = array[i];
            }

            // 4. Copiar el resultado de vuelta al arreglo original.
            Array.Copy(output, array, n);
        }
    }
}
