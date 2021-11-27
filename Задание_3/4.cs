using System;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        private static Semaphore _pool;
        private static int _padding;

        public static void Main()
        {
            _pool = new Semaphore(0, 3);
            for (int i = 0; i < 3; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(MyThreads));
                t.Start(i);
                t.Name = "thread : " + i.ToString();
            }   
            _pool.Release(3);   
        }
        
        private static void MyThreads(object threadN)
        { 
            Console.WriteLine($"Start {Thread.CurrentThread.Name}");
            _pool.WaitOne();
            int padding = Interlocked.Add(ref _padding, 1000);
            Thread.Sleep(1000 + padding); 
            Console.WriteLine($"Thread {threadN} = " + _pool.Release());
        }
    }
}
