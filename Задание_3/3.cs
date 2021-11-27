using System.Threading;
using System;
class Program
{
    static int plus = 0;
    private static object stop = new object();
    static void Main(string[] args)
    {
        for (int i = 0; i < 7; i++)
        {
            Thread myThread = new Thread(Inc);
            myThread.Start();
        }
    }
  
    public static void Inc()
    {
        lock (stop)
        {
            plus++;
            Console.WriteLine(plus);
        }
    }
}