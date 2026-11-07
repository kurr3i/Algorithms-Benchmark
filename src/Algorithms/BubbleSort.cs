using System;

namespace Sorting
{
    /// <summary>
    /// Bubble Sort (ordenamiento de burbuja), versión clásica.
    ///
    /// Idea: se recorre el arreglo comparando pares adyacentes e intercambiándolos
    /// si están en el orden incorrecto. En cada pasada el mayor elemento "sube"
    /// hasta su posición final, al final de la parte no ordenada.
    ///
    /// Complejidad temporal:
    ///   - Mejor caso:  O(n²)  (versión clásica, sin bandera de corte anticipado)
    ///   - Caso medio:  O(n²)
    ///   - Peor caso:   O(n²)
    /// Complejidad espacial: O(1) (ordena in-place).
    /// Estable: Sí.
    /// </summary>
    public class BubbleSort
    {
        /// <summary>Ordena el arreglo de forma ascendente.</summary>
        public void Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            int n = array.Length;

            // Cada pasada i deja los i+1 mayores en su lugar final.
            for (int i = 0; i < n - 1; i++)
            {
                // El rango se reduce porque los últimos i elementos ya están ordenados.
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        // Intercambiar vecinos fuera de orden.
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
    }
}
