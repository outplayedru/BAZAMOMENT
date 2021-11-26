using System;
using System.Numerics;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

class Program
{
	public static object SumLock = new object();
	public static BigInteger Sum = new BigInteger(0);

	public static void DoSum(dynamic args)
	{
		int min = args.min;
		int max = args.max;

		BigInteger sum = new BigInteger(0);

		for (int i = min; i <= max; ++i)
		{
			sum += i;
		}

		lock(SumLock)
		{
			Sum += sum;
		}
	}

	public static void Main(string[] args)
	{
		Console.WriteLine("Enter number of threads");
		int numThreads = Int32.Parse(Console.ReadLine());

		int maxSum = 1000000;
		int range = maxSum / numThreads;

		List<Thread> threads = new List<Thread>();

		int lastN = 1;

		Stopwatch timer = new Stopwatch();
		timer.Start();

		for (int n = 0; n < numThreads; ++n)
		{
			Thread thread = new Thread(DoSum);
			threads.Add(thread);
			thread.Start(new { min = lastN, max = n == numThreads - 1 ? maxSum : lastN + range - 1 });

			lastN += range;
		}

		for (int n = 0; n < threads.Count; ++n)
		{
			threads[n].Join();
		}

		timer.Stop();

		Console.WriteLine(Sum + ", " + timer.Elapsed);
	}
}