using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace HotelOpgaveEntity
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1.List all\n2.List hotels, rooms and reservations\n3.Add new guest\n4.New Booking\n5.Edit hotel\n6.Edit guest\n0.Exit\nPlease enter your choice:");
            String choice = Console.ReadLine();
            Console.Clear();
            while (choice != "0")
            {
                switch (choice)
                {
                    case "1":
                        ListAll();
                        break;
                    case "2":
                        ListHotelsAndRooms();
                        break;
                    case "3":
                        InsertNewGuest();
                        break;
                    case "4":
                        NewBooking();
                        break;
                    case "5":
                        EditHotel();
                        break;
                    case "6":
                        EditGuest();
                        break;
                    default:
                        Console.WriteLine("Command unknown");
                        break;
                }
                Console.WriteLine("1.List all\n2.List hotels, rooms and reservations\n3.Add new guest\n4.New Booking\n5.Edit hotel\n6.Edit guest\n0.Exit\nPlease enter your choice:");
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
            }

            Console.WriteLine("\nPress any key to return..");
            Console.ReadKey();
            Console.Clear();
        }

        private static void ListHotelsAndRooms()
        {
            using(var context = new HotelContext())
            {
                var HotelAndRoom = context.Hotel.Include(x => x.Room);

                foreach (var item in HotelAndRoom)
                {
                    Console.WriteLine(item);
                    Console.WriteLine(String.Join("\n", item.Room));

                    var RoomAndBooking = context.Booking.Where(x => x.Hotel_No == item.Hotel_No);
                    foreach (var item2 in RoomAndBooking)
                    {
                        Console.WriteLine(item2);
                    }
                }

            }
            Console.WriteLine("\nPress any key to return..");
            Console.ReadKey();
            Console.Clear();
        }

        private static void InsertNewGuest()
        {
            using(var context = new HotelContext())
            {
                Console.Write("Guest name: ");
                String guestName = Console.ReadLine();
                Console.Write("Guest address: ");
                String guestAddress = Console.ReadLine();

                var getGuestNo = context.Guest.OrderByDescending(x => x.Guest_No).First();

                Guest newGuest = new Guest { Guest_No = getGuestNo.Guest_No + 1, Name = guestName, Address = guestAddress };
                context.Guest.Add(newGuest);

                context.SaveChanges();

                Console.Write("A new guest has been added: ");
                Console.WriteLine(newGuest);
            }
            Console.WriteLine("\nPress any key to return..");
            Console.ReadKey();
            Console.Clear();
        }

        private static void NewBooking()
        {
            using(var context = new HotelContext())
            {
                int guestno = 0;
                int hotelno = 0;
                int roomno = 0;
                DateTime datefrom = DateTime.Now;
                DateTime dateto = DateTime.Now;

                Console.WriteLine("What hotel do you wish to make a booking for?:");
                var AllHotels = context.Hotel;

                foreach (var item in AllHotels)
                {
                    Console.WriteLine($"{item.Hotel_No}.{item.Name}");
                }

                String choice = Console.ReadLine();
                Int32.TryParse(choice, out hotelno);
                Console.Clear();

                Console.WriteLine("What room do you want to book: ");
                var RoomInHotel = context.Room.Where(x => x.Hotel_No == hotelno);

                foreach (var item in RoomInHotel)
                {
                    Console.WriteLine($"{item.Room_No}.Type: {item.Types} | Price: {item.Price}");
                }
                choice = Console.ReadLine();
                Int32.TryParse(choice, out roomno);
                Console.Clear();


                Console.WriteLine("What guest is booking the reservation?");
                Console.Write("Guest name?: ");
                String gName = Console.ReadLine();
                gName = gName.First().ToString().ToUpper() + gName.Substring(1);

                var getGuestNo = context.Guest.Where(x => x.Name == gName);

                if(getGuestNo.Count() == 0)
                {
                    Console.WriteLine("No guest with that name was found!");
                }
                else if(getGuestNo.Count() > 1)
                {
                    Console.WriteLine("There was found more than one guest with that name. Choose one:");
                    foreach (var item in getGuestNo)
                    {
                        Console.WriteLine($"{item.Guest_No}.{item.Name} | {item.Address}");
                    }
                    Console.Write("Choose a number: ");
                    choice = Console.ReadLine();
                    Int32.TryParse(choice, out guestno);
                }
                else
                {
                    guestno = getGuestNo.First().Guest_No;
                }

                Console.Clear();

                bool datefromtimeset = false;

                while(!datefromtimeset)
                {
                    Console.WriteLine("Date from (Use the format mm/dd/yyyy):");
                    choice = Console.ReadLine();
                    if (DateTime.TryParseExact(choice, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datefrom))
                    {
                        datefromtimeset = true;
                        Console.Clear();
                    }
                }

                bool datetotimeset = false;

                while (!datetotimeset)
                {
                    Console.WriteLine("Date to (Use the format mm/dd/yyyy):");
                    choice = Console.ReadLine();
                    if (DateTime.TryParseExact(choice, "M/d/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateto))
                    {
                        datetotimeset = true;
                        Console.Clear();
                    }
                }

                Booking newBooking = new Booking { Hotel_No = hotelno, Guest_No = guestno, Room_No = roomno, Date_From = datefrom, Date_To = dateto };
                context.Booking.Add(newBooking);
                context.SaveChanges();

                Console.WriteLine("Your booking was complete:");
                Console.WriteLine(newBooking);

            }
            Console.WriteLine("\nPress any key to return..");
            Console.ReadKey();
            Console.Clear();
        }

        private static void EditHotel()
        {
            using(var context = new HotelContext())
            {
                Console.WriteLine("What hotel want to edit?:");
                var AllHotels = context.Hotel;

                foreach (var item in AllHotels)
                {
                    Console.WriteLine($"{item.Hotel_No}.{item.Name}");
                }

                String choice = Console.ReadLine();
                int hotelno;
                Int32.TryParse(choice, out hotelno);
                Console.Clear();

                var original = context.Hotel.Find(hotelno);
                Console.WriteLine("You are editing" + original + "\n");
                Console.WriteLine("What do you want to edit?:\n1.Hotel name\n2.Hotel address\n3.Both\n");
                choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        Console.Write("Please write a new name for the hotel: ");
                        String newname = Console.ReadLine();
                        original.Name = newname;
                        break;
                    case "2":
                        Console.Write("Please write a new address for the hotel: ");
                        String newaddress = Console.ReadLine();
                        original.Address = newaddress;
                        break;
                    case "3":
                        Console.Write("Please write a new name for the hotel: ");
                        newaddress = Console.ReadLine();
                        Console.Write("\nPlease write a new address for the hotel: ");
                        newname = Console.ReadLine();
                        original.Name = newname;
                        original.Address = newaddress;
                        break;
                    default:
                        break;
                }

                context.SaveChanges();
                Console.WriteLine("The edit was complete\nThe new hotel:");
                Console.WriteLine(original);
            }

            Console.WriteLine("\nPress any key to return..");
            Console.ReadKey();
            Console.Clear();
        }

        public static void EditGuest()
        {
            using(var context = new HotelContext())
            {
                int guestno = 0;

                Console.WriteLine("What guest do you want to edit?");
                Console.Write("Guest name?: ");
                String gName = Console.ReadLine();
                gName = gName.First().ToString().ToUpper() + gName.Substring(1);

                var getGuestNo = context.Guest.Where(x => x.Name == gName);

                if (getGuestNo.Count() == 0)
                {
                    Console.WriteLine("No guest with that name was found!");
                }
                else if (getGuestNo.Count() > 1)
                {
                    Console.WriteLine("There was found more than one guest with that name. Choose one:");
                    foreach (var item in getGuestNo)
                    {
                        Console.WriteLine($"{item.Guest_No}.{item.Name} | {item.Address}");
                    }
                    Console.Write("Choose a number: ");
                    String choice2 = Console.ReadLine();
                    Int32.TryParse(choice2, out guestno);
                }
                else
                {
                    guestno = getGuestNo.First().Guest_No;
                }

                var original = context.Guest.Find(guestno);

                Console.Clear();
                Console.WriteLine("What do you want to edit?\n1.Guest name\n2.Guest address\n3.Both");
                String choice = Console.ReadLine();
                Console.Clear();

                switch(choice)
                {
                    case "1":
                        Console.WriteLine("Write a new name: ");
                        String newname = Console.ReadLine();
                        original.Name = newname;
                        break;
                    case "2":
                        Console.WriteLine("Write a new address: ");
                        String newaddress = Console.ReadLine();
                        original.Address = newaddress;
                        break;
                    case "3":
                        Console.WriteLine("Write a new name: ");
                        newname = Console.ReadLine();
                        Console.WriteLine("Write a new address: ");
                        newaddress = Console.ReadLine();
                        original.Name = newname;
                        original.Address = newaddress;
                        break;
                    default:
                        break;
                }

                context.SaveChanges();
                Console.WriteLine("The edit was complete\nThe guest:");
                Console.WriteLine(original);
            }

            Console.WriteLine("\nPress any key to return..");
            Console.ReadKey();
            Console.Clear();
        
        }
    }
}
