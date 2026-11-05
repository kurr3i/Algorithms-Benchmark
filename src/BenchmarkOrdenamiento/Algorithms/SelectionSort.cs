using System;

namespace Sorting
{
    /// <summary>
    /// Selection Sort (ordenamiento por selección).
    ///
    /// Idea: en cada pasada se busca el elemento mínimo de la parte no ordenada
    /// y se intercambia con el primer elemento de esa parte.
    ///
    /// Complejidad temporal:
    ///   - Mejor caso:  O(n²)  (siempre recorre toda la parte no ordenada)
    ///   - Caso medio:  O(n²)
    ///   - Peor caso:   O(n²)
    /// Complejidad espacial: O(1) (ordena in-place).
    /// Estable: No. Realiza como máximo n-1 intercambios.
    /// </summary>
    public class SelectionSort
    {
        /// <summary>Ordena el arreglo de forma ascendente.</summary>
        public void Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            int n = array.Length;

            // i marca el límite entre la parte ordenada [0..i-1] y la no ordenada [i..n-1].
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                // Buscar el índice del mínimo en la parte no ordenada.
                for (int j = i + 1; j < n; j++)
                {
                    if (array[j] < array[minIndex])
                        minIndex = j;
                }

                // Colocar el mínimo en su posición definitiva.
                int temp = array[i];
                array[i] = array[minIndex];
                array[minIndex] = temp;
            }
        }
    }
}
