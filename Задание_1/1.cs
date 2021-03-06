using System;
using System.Collections;
using System.Collections.Generic;

namespace Task1
{
    public class Animal : IComparable <Animal>
    {
        public int Age { get; set; }
        public string Name { get; set; }
        
        /*
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            Animal animal = obj as Animal;
            if (animal != null)
            {
                return this.Age.CompareTo(animal.Age);
            }
            else
            {
                throw new ArgumentException("Object is not a animal");
            }
        }
       */
        
        public int CompareTo(Animal animal)
        {
            if (animal == null)
            {
                return 1;
            }
            Animal Animal = animal as Animal;
            if (animal != null)
            {
                return this.Age.CompareTo(animal.Age);
            }
            else
            {
                throw new ArgumentException("Object is not a animal");
            }
        }
        
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Animal> List = new List<Animal>();
            List.Add(new Animal() { Name = "Lion", Age = 4 });
            List.Add(new Animal() { Name = "Zebra", Age = 5 });

            Console.WriteLine("Элементы списка:");


            foreach (Animal i in List)
            {
                Console.WriteLine("Animal: " + i.Name + ", age: " + i.Age);
            }

            Console.WriteLine(" ");

            List.Sort();

            Console.WriteLine("После сортировки:");

            foreach (Animal i in List)
            {
                Console.WriteLine("Animal: " + i.Name + ", age: " + i.Age);
            }

            Console.ReadKey();

        }
    }
}