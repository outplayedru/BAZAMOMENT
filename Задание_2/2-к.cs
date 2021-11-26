using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

class Worker
{
    public string Name;
    public int Salary;
}

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            var workers = new[]
               {
                    new Worker {Name = "Иванов", Salary = 20000},
                    new Worker {Name = "Петров", Salary = 7500},
                    new Worker {Name = "Иванов", Salary = 5000},
                    new Worker {Name = "Сидоров", Salary = 3777},
                    new Worker {Name = "Сидоров", Salary = 10000},
                    new Worker {Name = "Петров", Salary = 33000},
                };

            var grouped = workers
                 .GroupBy(worker => worker.Name)
                 .Select(g => new Worker { Name = g.Key, Salary = g.Sum(worker => worker.Salary) });

            foreach (var worker in grouped)
                Console.WriteLine("Name: {0}, Salary {1}", worker.Name, worker.Salary);
        }
    }
}