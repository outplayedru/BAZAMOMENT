using System;
using System.Collections.Generic;
using System.Linq;


class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>() { 1, 5, 32, 13, 90, 9, 291, 1, 0, 73 };

        Console.WriteLine("Массив целых чисел: ");
        foreach (var i in numbers)
        {
            Console.WriteLine(i);
        }

        var query = numbers.OrderBy(n => Math.Abs(n%2)).ThenBy(n => n);

        Console.WriteLine("\nГруппирование по четности и отсортировка по возрастанию:");

        foreach (var i in query)
        {
            Console.WriteLine(i);
        }
        Console.ReadKey();
    }
}