using System;

namespace Sorting
{
    /// <summary>
    /// Merge Sort (ordenamiento por mezcla), versión recursiva top-down.
    ///
    /// Idea: dividir el arreglo por la mitad, ordenar recursivamente cada mitad y
    /// mezclar (merge) las dos mitades ordenadas en una sola secuencia ordenada.
    ///
    /// Complejidad temporal:
    ///   - Mejor caso:  O(n log n)
    ///   - Caso medio:  O(n log n)
    ///   - Peor caso:   O(n log n)
    /// Complejidad espacial: O(n) por el arreglo auxiliar
    ///   (más O(log n) de pila de recursión).
    /// Estable: Sí.
    /// </summary>
    public class MergeSort
    {
        /// <summary>Ordena el arreglo de forma ascendente.</summary>
        public void Sort(int[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Length < 2) return;

            // Arreglo auxiliar reutilizado en todas las mezclas.
            int[] aux = new int[array.Length];
            Sort(array, aux, 0, array.Length - 1);
        }

        /// <summary>Ordena recursivamente el subarreglo array[left..right].</summary>
        private void Sort(int[] array, int[] aux, int left, int right)
        {
            // Caso base: 0 o 1 elemento ya está ordenado.
            if (left >= right) return;

            int mid = left + (right - left) / 2; // evita desbordamiento de (left + right)

            Sort(array, aux, left, mid);       // ordenar mitad izquierda
            Sort(array, aux, mid + 1, right);  // ordenar mitad derecha
            Merge(array, aux, left, mid, right);
        }

        /// <summary>Mezcla las mitades ordenadas array[left..mid] y array[mid+1..right].</summary>
        private void Merge(int[] array, int[] aux, int left, int mid, int right)
        {
            // Copiar el segmento a mezclar al arreglo auxiliar.
            for (int k = left; k <= right; k++)
                aux[k] = array[k];

            int i = left;      // puntero a la mitad izquierda
            int j = mid + 1;   // puntero a la mitad derecha

            for (int k = left; k <= right; k++)
            {
                if (i > mid)
                    array[k] = aux[j++];            // izquierda agotada
                else if (j > right)
                    array[k] = aux[i++];            // derecha agotada
                else if (aux[j] < aux[i])
                    array[k] = aux[j++];            // el de la derecha es estrictamente menor
                else
                    array[k] = aux[i++];            // empate o izquierda menor: conserva estabilidad
            }
        }
    }
}
