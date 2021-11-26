using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {

        string s1 = "1 2 3 4 5";
        string s2 = "5 7 5 3 1";

        //через LINQ

        // если 2 строка обратна 1, выводим (1), иначе (2) 
        // Reverse(Array) - Изменяет порядок элементов во всем одномерном массиве Array на обратный.
        Console.WriteLine(s1 == new string(s2.Reverse().ToArray()) ? "Строка s2 является обратной строке s1" : "Строка s2 не является обратной строке s1");
        

        // обычным способом

        if (s1.Length != s2.Length)
        {
            Console.WriteLine("Строка s2 не является обратной строке s1");
            return;
        }

        bool flag = true;
        for (int i = 0; i < s1.Length; i++)
        {
            if (s1[i] != s2[s2.Length - i - 1])
            {
                flag = false;
                break;
            }
        }

        if (flag)
            Console.WriteLine("Строка s2 является обратной строке s1");
        else
            Console.WriteLine("Строка s2 не является обратной строке s1");

        Console.ReadKey();
    }
}
