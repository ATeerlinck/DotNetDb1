using System;
using System.IO;

namespace module2assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            // ask for input
            Console.WriteLine("Enter 1 to create data file.");
            Console.WriteLine("Enter 2 to parse data.");
            Console.WriteLine("Enter anything else to quit.");
            // input response
            string resp = Console.ReadLine();

            if (resp == "1")
            {
                // create data file

                 // ask a question
                Console.WriteLine("How many weeks of data is needed?");
                // input the response (convert to int)
                int weeks = int.Parse(Console.ReadLine());

                 // determine start and end date
                DateTime today = DateTime.Now;
                // we want full weeks sunday - saturday
                DateTime dataEndDate = today.AddDays(-(int)today.DayOfWeek);
                // subtract # of weeks from endDate to get startDate
                DateTime dataDate = dataEndDate.AddDays(-(weeks * 7));
                
                // random number generator
                Random rnd = new Random();

                // create file
                StreamWriter sw = new StreamWriter("data.txt");
                // loop for the desired # of weeks
                while (dataDate < dataEndDate)
                {
                    // 7 days in a week
                    int[] hours = new int[7];
                    for (int i = 0; i < hours.Length; i++)
                    {
                        // generate random number of hours slept between 4-12 (inclusive)
                        hours[i] = rnd.Next(4, 13);
                    }
                    // M/d/yyyy,#|#|#|#|#|#|#
                    //Console.WriteLine($"{dataDate:M/d/yy},{string.Join("|", hours)}");
                    sw.WriteLine($"{dataDate:M/d/yyyy},{string.Join("|", hours)}");
                    // add 1 week to date
                    dataDate = dataDate.AddDays(7);
                }
                sw.Close();
            }
            else if (resp == "2")
            {
                if (File.Exists("data.txt"))
                    {
                        StreamReader sr = new StreamReader("data.txt");
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] split = line.Split(",");
                            DateTime date = Convert.ToDateTime(split[0]);
                            string[] hours = split[1].Split("|");
                            int total = 0;
                            foreach (string day in hours){
                                total += Convert.ToInt32(day);
                            }
                            double Avg = (double)total/7;
                            Console.WriteLine($"End of the Week of {date:MMM, dd, yyyy}\n Mo Tu We Th Fr Sa Su Tot Avg\n -- -- -- -- -- -- -- --- ---\n {hours[1],-2} {hours[2],-2} {hours[3],-2} {hours[4],-2} {hours[5],-2} {hours[6],-2} {hours[0],-2} {total,-3} {Avg:0.#}");
                        }
                        sr.Close();

                    }
                    else
                    {
                        Console.WriteLine("File does not exist");
                    }
            }
        }
    }
}
