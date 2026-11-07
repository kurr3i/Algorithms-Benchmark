using System;

namespace BenchmarkOrdenamiento.Benchmark
{
    /// <summary>Los cuatro tipos de entrada que pide la tarea.</summary>
    public enum InputType
    {
        /// <summary>Valores aleatorios en [0, 999 999].</summary>
        Random,

        /// <summary>Valores aleatorios ya ordenados ascendentemente.</summary>
        Ascending,

        /// <summary>Valores aleatorios ordenados descendentemente.</summary>
        Descending,

        /// <summary>Muchos duplicados: valores en [0, 10].</summary>
        Duplicates
    }

    /// <summary>
    /// Genera las entradas de los experimentos. Es determinista: la misma
    /// combinación (tipo, n, semilla) produce siempre exactamente el mismo arreglo,
    /// de modo que todos los algoritmos se miden con la misma entrada.
    /// </summary>
    public static class InputGenerator
    {
        /// <summary>Valor máximo (inclusive) de los arreglos del experimento principal.</summary>
        public const int MaxValue = 999999;

        /// <summary>Valor máximo (inclusive) del tipo "muchos duplicados".</summary>
        public const int DuplicatesMaxValue = 10;

        /// <summary>Genera un arreglo de n enteros del tipo indicado.</summary>
        public static int[] Generate(InputType type, int n, int seed)
        {
            Random rng = new Random(seed);
            int[] array = new int[n];

            switch (type)
            {
                case InputType.Random:
                    Fill(array, rng, MaxValue);
                    break;

                case InputType.Ascending:
                    Fill(array, rng, MaxValue);
                    Array.Sort(array); // preparación de la entrada, fuera de lo medido
                    break;

                case InputType.Descending:
                    Fill(array, rng, MaxValue);
                    Array.Sort(array);
                    Array.Reverse(array);
                    break;

                case InputType.Duplicates:
                    Fill(array, rng, DuplicatesMaxValue);
                    break;
            }

            return array;
        }

        /// <summary>
        /// Genera n valores aleatorios de hasta <paramref name="digits"/> dígitos (0 .. 10^digits - 1).
        /// Garantiza que al menos un elemento valga el máximo (999, 999 999, 999 999 999),
        /// para que Radix procese siempre exactamente d dígitos (prueba especial de Radix).
        /// </summary>
        public static int[] GenerateWithDigits(int n, int digits, int seed)
        {
            int maxValue = 1;
            for (int i = 0; i < digits; i++)
            {
                maxValue *= 10;
            }
            maxValue -= 1;

            Random rng = new Random(seed);
            int[] array = new int[n];
            Fill(array, rng, maxValue);
            array[rng.Next(n)] = maxValue;
            return array;
        }

        private static void Fill(int[] array, Random rng, int maxInclusive)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rng.Next(0, maxInclusive + 1);
            }
        }
    }
}
