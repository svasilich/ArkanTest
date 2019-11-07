using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanTest
{

    /// <summary>
        /// Реализация с минимальными затратами памяти и минимально короткими
        /// блокировками параллельных потоков-читателей.
    /// </summary>    
    public class DataParallel : DataFast
    {
        // Поскольку ограничений по памяти на эту реализацию нет,
        // воспользуемся уже готовой быстрой сортировкой.
        // Для этого, отнаследуемся от DataFast.

        public override void cleanup(IEnumerable<int> values)
        {
            var set = new HashSet<int>(values);
            var result = new int?[Values.Length];
            int count = 0;
            // На этом этапе мы не изменяем исходные данные,
            // поэтому можем позволить чтение другим потокам.
            Lock.EnterReadLock();
            try
            {
                count = QuickSelectValues(set, result);
            }
            finally
            {
                Lock.ExitReadLock();
            }

            // А теперь изменим наши данные.
            Lock.EnterWriteLock();
            try
            {
                Count = count;
                Values = result;
            }
            finally
            {
                Lock.ExitWriteLock();
            }

            // Всё готово. Осталось подменить исходные
        }
    }
}
