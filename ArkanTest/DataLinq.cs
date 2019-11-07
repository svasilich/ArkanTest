using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanTest
{
    /// <summary>
     /// Эталонная реализация простым перебором.
     /// </summary>
     /// <remarks>
     /// Эта реализация используется для проверки других реализаций.
     /// </remarks>
    public class DataLinq : Data
    {
        public override void cleanup(IEnumerable<int> values)
        {
            var set = new HashSet<int>(values);
            Lock.EnterWriteLock();
            try
            {
                Values = Values.Where(v => !set.Contains((int)v)).ToArray();
                Count = Values.Count();
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }
    }
}
