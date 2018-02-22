using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slae_project
{
    public class Asynchronized
    {
        async Task myMethod()
        {
            await SomeCycleAsync();
            Console.WriteLine("выполнился цикл-2");
        }

        async Task SomeCycleAsync()
        {
            Console.WriteLine("стартует цикл");
            // это запускает длинное вычисление на пуле потоков
            //var result = await Task.Run(ResultOfCycle);
            //Console.WriteLine("выполнился цикл, результат: " + result);
        }

        int ResultOfCycle()
        {
            int sum = 0;
            for (int i = 0; i < 1000000000; i++)
                sum += i;
            return sum;
        }

        public async void Start()
        {
            
            await myMethod();
        }
    }
}
