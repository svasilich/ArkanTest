using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ArkanTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var old = Generate(1000000, 10000000, 0, 100000);
            var rem = Generate(1, 1000000, 0, 100000);
            //var old = Generate(100000, 100000, 0, 100000);
            //var rem = Generate(1, 100000, 0, 100000);
            Console.WriteLine(Test(new DataLinq(), old, rem));
            Console.WriteLine(Test(new DataFast(), old, rem));            
            Console.WriteLine(Test(new DataParallel(), old, rem));
            //Console.WriteLine(Test(new DataCompact(), old, rem));
            Console.ReadKey();
        }

        private static int[] Generate(int minCount, int maxCount, int minValue, int maxValue)
        {
            var rnd = new Random();
            var values = new int[rnd.Next(minCount, maxCount + 1)];
            for (var i = 0; i < values.Length; ++i)
                values[i] = rnd.Next(minValue, maxValue);
            return values;
        }

        private static string Test(Data data, int[] values, int[] remove)
        {
            data.init(values);
            var sw = new Stopwatch();
            sw.Start();
            data.cleanup(remove);
            sw.Stop();
            var datTest = new DataLinq();
            datTest.init(values);
            datTest.cleanup(remove);
            var ok = data.IsValid
            && datTest.Values.SequenceEqual(data.Values.Take(data.Count));
            return $"{data.GetType().Name}:\t{(ok ? "OK" : "FAIL")}, { sw.ElapsedTicks} ticks";
        }
    }
}

