using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>
             {
                "Первое",
                "Второе",
                "Второе",
                "Второе",
                "Третье",
                "Третье",
                "Четвертое",
                "Пятое",
                "Пятое",
                "Пятое",
                "Пятое",
            };

            var newlist = list.GroupBy(g => g).Where(g => g.Count() == 3).Select(g => g.Key);
            foreach( string s in newlist)
                Console.WriteLine(s);
        }
    }
}
