using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

class Worker
{
    public string Name;
    public int Age;
}

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Worker> workers = new List<Worker>()
            {
                new Worker { Name = "Tom", Age = 33 },
                new Worker { Name = "Bob", Age = 30 },
                new Worker { Name = "Tom", Age = 21 },
                new Worker { Name = "Sam", Age = 43 }
            };

            var sortedByName = workers.OrderBy(u => u.Name);
            foreach (Worker u in sortedByName)
            {
                Console.Write(u.Name);
                Console.Write(" ");
                Console.Write(u.Age);
                Console.WriteLine(" ");
            }

            Console.WriteLine(" ");

            var sortedByAge = workers.OrderByDescending(u => u.Age);
            foreach (Worker u in sortedByAge)
            {
                Console.Write(u.Name);
                Console.Write(" ");
                Console.Write(u.Age);
                Console.WriteLine(" ");
            }       
        }
    }
}
