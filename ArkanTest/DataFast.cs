using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanTest
{
    /// <summary>
     /// Максимально быстрая реализация – без ограничений по памяти.
     /// </summary>
    public class DataFast : Data
    {
        public override void cleanup(IEnumerable<int> values)
        {
            var set = new HashSet<int>(values);
            var result = new int?[Values.Length];
            Lock.EnterWriteLock();
            try
            {
                int count = QuickSelectValues(set, result);

                Count = count;
                Values = result;        
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        protected int QuickSelectValues(HashSet<int> exceptIt, int?[] result)
        {
            int i = 0;
            int resultPos = 0;
            while (i < Count)
            {
                // Найти первый допустимый элемент.
                int first = i;
                while (first < Count && exceptIt.Contains(Values[first].Value))
                    ++first;

                if (first == Count)
                    break; // Больше нечего удалять.

                // Найти первый недопустимый элемент.
                int last = first + 1;

                while (last < Count && !exceptIt.Contains(Values[last].Value))
                    ++last;

                // Скопировать полученный подмассив в result.
                int length = last - first;
                Array.Copy(Values, first, result, resultPos, length);
                resultPos += length;
                i = last + 1;
            }

            return resultPos;
        }
    }
}
