using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkanTest
{
    /// <summary>
     /// Реализация с минимальными затратами памяти.
     /// </summary>
    public class DataCompact : Data
    {
        public override void cleanup(IEnumerable<int> values)
        {
            Lock.EnterWriteLock();
            try
            {
                int count = Count;
                int i = 0;
                while (i < count)
                {
                    if (!values.Contains(Values[i].Value))
                    {
                        ++i;
                    }
                    else
                    {
                        for (var j = i + 1; j < count; ++j)
                            Values[j - 1] = Values[j];
                        Values[count - 1] = null;
                        --count;                        
                    }
                }

                Count = count;

            }
            finally
            {
                Lock.ExitWriteLock();
            }

        }
    }
}
