using Jawbone;
using System;
using System.Linq;
using System.Text;

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

            var array = Enumerable.Range(0, 16).ToArray();
            var n = 4;
            var dualSpan = new DualSpan<int>(array.AsSpan(0, n), array.AsSpan(n));
            Dump(dualSpan);
            dualSpan.Reverse();
            Dump(dualSpan);
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }

    static void Dump(DualSpan<int> dualSpan)
    {
        var builder = new StringBuilder();
        builder.Append(dualSpan[0]);

        for (int i = 1; i < dualSpan.Length; ++i)
            builder.Append(", ").Append(dualSpan[i]);
        Console.WriteLine(builder);
    }
}
