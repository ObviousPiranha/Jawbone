using Jawbone;
using System;
using System.Linq;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine(new LoopyList<int>());
            var list = new LoopyList<int>(16);
            Console.WriteLine(list);
            list.PushBack(Enumerable.Range(1, list.Capacity));
            Console.WriteLine(list);
            list.RemoveFront(2);
            list.RemoveBack(2);
            Console.WriteLine(list);
            list.Clear();
            Console.WriteLine(list);
            list.PushBack(Enumerable.Range(1, list.Capacity));
            list.RemoveFront(list.Capacity / 2);
            list.PushBack(Enumerable.Range(99, list.Capacity / 2 - 2));
            Console.WriteLine(list);
            list.PushBack(200, 201);
            Console.WriteLine(list);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
