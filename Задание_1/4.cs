using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
	public static void Main (string[] args)
	{
		List<string> stringList = new List<string>();
		Console.WriteLine("Insert for List<String>: " + MeasureListTime(stringList, "test_string", true));
		Console.WriteLine("Access for List<String>: " + MeasureListTime(stringList, null, false));
		Console.WriteLine();

		ArrayList stringArray = new ArrayList();
		Console.WriteLine("Insert for string ArrayList: " + MeasureArrayListTime(stringArray, "test_string", true));
		Console.WriteLine("Access for string ArrayList: " + MeasureArrayListTime(stringArray, "", false));
		Console.WriteLine();

		List<int> intList = new List<int>();
		Console.WriteLine("Insert for List<int>: " + MeasureListTime(intList, 69, true));
		Console.WriteLine("Access for List<int>: " + MeasureListTime(intList, 0, false));
		Console.WriteLine();

		ArrayList intArray = new ArrayList();
		Console.WriteLine("Insert for int ArrayList: " + MeasureArrayListTime(intArray, 69, true));
		Console.WriteLine("Access for int ArrayList: " + MeasureArrayListTime(intArray, 0, false));
	}

	// один метод с аргументами <L, V> where L : IList почему то уничтожает время выполнения для List<> поэтому разделяем на два

	public static TimeSpan MeasureListTime<V>(List<V> list, V value, bool insert)
	{
		V testObj;	

		Stopwatch timer = new Stopwatch();
		timer.Start();

		for(int i = 0; i < 1000000; ++i)
		{
			if(!insert)
			{
				testObj = list[i];
			}
			else
			{
				list.Add(value);
			}
		}

		timer.Stop();

		return timer.Elapsed;
	}

	public static TimeSpan MeasureArrayListTime(ArrayList list, object value, bool insert)
	{
		object testObj;

		Stopwatch timer = new Stopwatch();
		timer.Start();

		for(int i = 0; i < 1000000; ++i)
		{
			if(!insert)
			{
				testObj = list[i];
			}
			else
			{
				list.Add(value);
			}
		}

		timer.Stop();

		return timer.Elapsed;
	}
}