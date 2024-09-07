using Piranha.Jawbone;
using Piranha.Jawbone.Net;
using Piranha.Jawbone.Sdl2;
using System;
using System.Runtime.InteropServices;

namespace Piranha.Sandbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine(Alignment.Of<AddressV6>());
            Console.WriteLine("Enter address text.");

            while (true)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    break;

                if (AddressV6.TryParse(input, null, out var address))
                    Console.WriteLine("Successfully parsed " + address);
                else
                    Console.WriteLine("Failed to parse " + input);
            }

            Console.WriteLine("Exiting...");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine(ex);
            Console.WriteLine();
        }
    }
}
