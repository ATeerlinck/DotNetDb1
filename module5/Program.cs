using System;
using System.IO;
using System.Collections.Generic;
namespace module5
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "tickets.csv";
            string choice;
            TicketList list = new TicketList(file);
            do
            {
                Console.WriteLine("1) Read data from file.");
                Console.WriteLine("2) Create file from data.");
                Console.WriteLine("Enter any other key to exit.");
                choice = Console.ReadLine();

                if (choice == "1")
                {
                    if (File.Exists(file))
                    {
                        foreach(Ticket ticket in list.Tickets){
                                Console.WriteLine(ticket);
                            }
                    }
                    else
                    {
                        Console.WriteLine("File does not exist");
                    }
                }
                else if (choice == "2")
                {
                    TicketList.CreateTicket(file);
                    list.Tickets = TicketList.CreateTicketList(file);
                }
            } while (choice == "1" || choice == "2");
        }
    }
}