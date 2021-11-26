using System;
using System.Collections.Generic;
using System.Threading;

class Program {

  public static bool done = false; 

  static void Help() {
    
    Console.WriteLine ("(c)\tочистка экрана");
    Console.WriteLine ("(h)\t(эта) подсказка");
    Console.WriteLine ("(q)\tзавершение работы");
    Console.WriteLine ();
    Console.WriteLine ("(t)\tзапуск потоков");
    Console.WriteLine ("(b)\tсделать потоки фоновыми");
    Console.WriteLine ("(f)\tсделать потоки активными");
    Console.WriteLine ();
  }

  public static void Action() 
  {
    while(!done) {
      Console.WriteLine("{0} : является фоновым = {1}",
                Thread.CurrentThread.Name, 
                Thread.CurrentThread.IsBackground);
      Thread.Sleep(1000);
    }
    
    Random rnd = new Random();
    Int32 delay = rnd.Next(1000,10000);
    Console.WriteLine("{0} будет завершен через {1} секунд",
                Thread.CurrentThread.Name, 
                delay/1000);
      
    Thread.Sleep(delay);
      
    Console.WriteLine("Поток {0} завершен.",Thread.CurrentThread.Name);
    
  }

  static void Main() {

    List<Thread> threads = new List<Thread>();

    string input = "h";

  
    do {
    
      switch (input)
        {
          case "b":
            foreach(Thread t in threads) t.IsBackground = true;
            break;
          
          case "c":
            Console.Clear();
            break;

          case "f":
            foreach(Thread t in threads) t.IsBackground = false;
            break;

          case "h":
            Help();
            break;
          
          case "q":
            done = true;

            foreach(Thread t in threads) t.Join();
            continue;

          case "t":
            for( int i=0; i<3; i++) 
            {
              Thread t = new Thread(new ThreadStart(Action));
              t.Name = "поток " + i.ToString();
              t.Start();
              threads.Add(t);
            }
            break;
          
          case null:
            break;
        }

      input = Console.ReadLine();
    
    } while (!done);
    
    Console.WriteLine("Основной поток завершен.");
  }  
}