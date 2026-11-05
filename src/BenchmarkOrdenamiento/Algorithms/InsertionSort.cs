using System;

namespace Sorting
{
    /// <summary>
    /// Insertion Sort (ordenamiento por inserción).
    ///
    /// Idea: se mantiene un prefijo ordenado; cada nuevo elemento se inserta en
    /// su posición correcta desplazando hacia la derecha los elementos mayores.
    ///
    /// Complejidad temporal:
    ///   - Mejor caso:  O(n)   (arreglo ya ordenado)
    ///   - Caso medio:  O(n²)
    ///   - Peor caso:   O(n²)  (arreglo en orden inverso)
    /// Complejidad espacial: O(1) (ordena in-place).
    /// Estable: Sí.
    /// </summary>
    public class InsertionSort
    {
        /// <summary>Ordena el arreglo de forma ascendente.</summary>
        public void Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            int n = array.Length;

            // El elemento en [0] se considera un prefijo ordenado de tamaño 1.
            for (int i = 1; i < n; i++)
            {
                int key = array[i]; // elemento a insertar
                int j = i - 1;

                // Desplazar a la derecha los elementos del prefijo mayores que key.
                while (j >= 0 && array[j] > key)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                // Colocar key en el hueco que quedó.
                array[j + 1] = key;
            }
        }
    }
}
