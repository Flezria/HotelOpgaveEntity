using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace HotelOpgaveEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.List all\nPlease enter your choice:");
            String choice = Console.ReadLine();
            Console.Clear();
            while (choice != "0")
            {
                switch (choice)
                {
                    case "1":
                        ListAll();
                        break;
                    default:
                        Console.WriteLine("Command unknown");
                        break;
                }
                Console.WriteLine("1.List all\nPlease enter your choice:");
                choice = Console.ReadLine();
                Console.Clear();
            }
            Environment.Exit(0);
        }

        private static void ListAll()
        {
            using(var context = new HotelContext())
            {
                var AllHotels = context.Hotel;

                Console.WriteLine("Hotels:\n");
                foreach (var item in AllHotels)
                {
                    Console.WriteLine(item);
                }

                var AllCustomers = context.Guest;

                Console.WriteLine("\nGuests:\n");
                foreach (var item in AllCustomers)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\nPress any key to return..");
                Console.ReadKey();
                Console.Clear();
            }
        }

        

    }
}
