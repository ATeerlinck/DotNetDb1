using System;

namespace module_2
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime today = DateTime.Now;
            Console.WriteLine("1. {0:MMMM} {0:dd}, {0:yyyy} ", today);
            Console.WriteLine("2. {0:yyyy}.{0:mm}.{0:dd} ", today);
            Console.WriteLine("3. Day {0:dd} of {0:MMMM},{0:yyyy} ", today);
            Console.WriteLine("4. Year: {0:yyyy}, Month: {0:MM}, Day:{0:dd} ", today);
            Console.WriteLine("5. {0:dddd} ", today);
            Console.WriteLine("6. {0:hh:mm tt} ", today);
            Console.WriteLine("7. h:{0:hh}, m:{0:mm}, s:{0:tt} ", today);
            Console.WriteLine("8. {0:yyyy.mm.dd.hh.mm.tt} ", today);
            
            Console.WriteLine("-------------HERE LIES THE SPLIT------------- ");
            
            Console.WriteLine($"1. {today:MMMM} {today:dd}, {today:yyyy} ");
            Console.WriteLine($"2. {today:yyyy}.{today:mm}.{today:dd} ");
            Console.WriteLine($"3. Day {today:dd} of {today:MMMM},{today:yyyy} ");
            Console.WriteLine($"4. Year: {today:yyyy}, Month: {today:MM}, Day:{today:dd} ");
            Console.WriteLine($"5. {today:dddd} ");
            Console.WriteLine($"6. {today:hh:mm tt} ");
            Console.WriteLine($"7. h:{today:hh}, m:{today:mm}, s:{today:tt} ");
            Console.WriteLine($"8. {today:yyyy.mm.dd.hh.mm.tt} ");

            

        }
    }
}
