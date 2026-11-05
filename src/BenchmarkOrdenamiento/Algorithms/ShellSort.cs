using System;

namespace Sorting
{
    /// <summary>
    /// Shell Sort, con la secuencia de saltos original de Shell (n/2, n/4, ..., 1).
    ///
    /// Idea: generalización de Insertion Sort que compara y mueve elementos
    /// separados por un "salto" (gap). Se reduce el salto progresivamente hasta 1,
    /// momento en que el arreglo ya está casi ordenado y la última pasada es barata.
    ///
    /// Complejidad temporal (con la secuencia original de Shell):
    ///   - Mejor caso:  O(n log n)
    ///   - Caso medio:  depende de la secuencia (aprox. entre O(n^1.3) y O(n²))
    ///   - Peor caso:   O(n²)
    /// Complejidad espacial: O(1) (ordena in-place).
    /// Estable: No.
    /// </summary>
    public class ShellSort
    {
        /// <summary>Ordena el arreglo de forma ascendente.</summary>
        public void Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            int n = array.Length;

            // Secuencia de Shell: gap = n/2, luego se divide a la mitad en cada ronda.
            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                // Insertion Sort sobre los sub-arreglos de elementos separados por gap.
                for (int i = gap; i < n; i++)
                {
                    int key = array[i];
                    int j = i;

                    // Desplazar elementos mayores que key, saltando de gap en gap.
                    while (j >= gap && array[j - gap] > key)
                    {
                        array[j] = array[j - gap];
                        j -= gap;
                    }

                    array[j] = key;
                }
            }
        }
    }
}
