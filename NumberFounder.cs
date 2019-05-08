using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskResolve
{   
    /*
     * Этапы:
        1. Генерация исходного массива чисел [0...N], исключая 2 случайных числа (X, Y) в разных половинах массива
        2. Вычисления. 
        Система уравнений с 2-мя неизвестными.
        sum + X + Y = sumN
        sum2 * X^2 * Y^2 = sumN2
        => сводится к квадратному уравнению, решениями которого являются 2 пропущенных числа.
    */
    public class NumberFounder
    {
        const int NCOUNT = 2;
        private List<int> array;
        private List<int> missingNumbers;

        public List<int> Array
        {
            get
            {
                return array;
            }
        }
        
        /// <param name="count">Размер массива</param>
        public NumberFounder(int count)
        {
            if (count > 2)
            {
                array = new List<int>(count - NCOUNT);
                Random rand = new Random();
                //в качестве пропущенных выбираются случайные числа из разных половин массива
                missingNumbers = new List<int>(NCOUNT)
                {
                    rand.Next(0, (int)count/2),
                    rand.Next((int)count/2 + 1, count - 1)
                };
                for (int i = 0; i < count; i++)
                {
                    if (!missingNumbers.Contains(i))
                    {
                        array.Add(i);
                    }
                }
                // в условии не указано, должен ли быть отсортирован исходный массив 
                // поэтому для корректности задачи элементы заполненного массива перемешиваются в случайном порядке
                array = array.OrderBy(c => rand.Next()).ToList();
            }
            else
            {
                throw new ArgumentOutOfRangeException("count", "count must be greater then 2");
            }
            
        }

        public Tuple<int, int> CalcMissingNumbers()
        {
            if (array is null)
            {
                throw new Exception("Array is not initialized");
            }
            // сумма исходного диапазона чисел и сумма всего ряда
            int sum = 0, sumN = 0;
            // сумма квадратов исходного диапазона чисел и всего ряда
            double sum2 = 1, sumN2 = 1;
            // говорит о том, что в исходной последовательности присутствует 0
            bool zeroPresence = false;
            foreach (int number in array)
            {
                if (number == 0)
                {
                    zeroPresence = true;
                }
                else
                {
                    sum += number;
                    sum2 += Math.Pow(number, 2);
                }
            }
            sumN = sum;
            sumN2 = sum2;
            /* 
             * на данном этапе, если zeroPresence == false, уже известен один пропущенный элемент 
             * случай, когда 0 не присутствует в исходном массиве, можно считать вырожденным для данного алгоритма 
             * => произведение считать не нужно, но для сокращения объема кода и общей концепции произведение всё же вычисляется
            */
            foreach (int missingNumber in missingNumbers)
            {
                if (missingNumber != 0)
                {
                    sumN += missingNumber;
                    sumN2 += Math.Pow(missingNumber, 2);
                }
            }
            // рассматривается вырожденный случай (нужно найти одно число, второе известно)
            if (!zeroPresence)
            {
                return new Tuple<int, int>(0, sumN - sum);
            }
            // коэффициенты квадратного уравнения
            double a = 2, b = -2*(sumN - sum), c = Math.Pow((sumN - sum), 2) - sumN2 + sum2;
            double d = Math.Pow(b, 2) - 4 * a * c;

            if (d < 0)
            {
                return null;
            }
            return new Tuple<int, int>((int)((-b - Math.Sqrt(d)) / (2 * a)), (int)((-b + Math.Sqrt(d)) / (2 * a)));
        }
        
    }
}
