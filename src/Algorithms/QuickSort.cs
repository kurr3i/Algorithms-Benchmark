
using System;

namespace Sorting
{
    /// <summary>
    /// Quicksort clásico con partición de Lomuto y el elemento CENTRAL como pivote.
    ///
    /// Idea: elegir un pivote, reubicar los elementos menores o iguales a su
    /// izquierda y los mayores a su derecha, y ordenar recursivamente ambos lados.
    ///
    /// Complejidad temporal:
    ///   - Mejor caso:  O(n log n)
    ///   - Caso medio:  O(n log n)
    ///   - Peor caso:   O(n²)
    /// Complejidad espacial: O(log n) de pila de recursión en promedio;
    ///   O(n) en el peor caso.
    /// Estable: No.
    /// </summary>
    public class QuickSort
    {
        /// <summary>Ordena el arreglo de forma ascendente.</summary>
        public void Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length < 2) return;

            Sort(array, 0, array.Length - 1);
        }

        /// <summary>Ordena recursivamente el subarreglo array[low..high].</summary>
        private void Sort(int[] array, int low, int high)
        {
            if (low >= high) return;

            int pivotIndex = Partition(array, low, high);

            Sort(array, low, pivotIndex - 1);
            Sort(array, pivotIndex + 1, high);
        }

        /// <summary>
        /// Partición de Lomuto: usa el elemento CENTRAL como pivote
        /// y devuelve su posición final. Tras ejecutarse, todo lo que está
        /// a la izquierda es menor o igual al pivote y todo lo que está
        /// a la derecha es mayor.
        /// </summary>
        private int Partition(int[] array, int low, int high)
        {
            int middle = low + (high - low) / 2; // CAMBIO: calcular índice central
            Swap(array, middle, high);           // CAMBIO: mover pivote central al final

            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (array[j] <= pivot)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            // Colocar el pivote justo después de la zona de menores o iguales.
            Swap(array, i + 1, high);
            return i + 1;
        }

        private void Swap(int[] array, int a, int b)
        {
            int temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }
    }
}
