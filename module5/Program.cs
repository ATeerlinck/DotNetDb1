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
            List<Ticket> Tickets = new List<Ticket>();
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
                        StreamReader sr = new StreamReader(file);
                        while (!sr.EndOfStream)
                        {
                            foreach(Ticket ticket in Tickets){
                                Console.WriteLine(ticket);
                            }
                            
                        }
                        sr.Close();

                    }
                    else
                    {
                        Console.WriteLine("File does not exist");
                    }
                }
                else if (choice == "2")
                {
                    StreamWriter sw = new StreamWriter(file);
                    while(true)
                    {
                        Console.WriteLine("Enter the ticket ID.");
                        int ticketID = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter the ticket summary.");
                        string summary = Console.ReadLine();
                        Console.WriteLine("Enter the ticket status.");
                        string status = Console.ReadLine();
                        Console.WriteLine("Enter the ticket priority.");
                        string priority = Console.ReadLine();
                        Console.WriteLine("Enter the ticket submitter.");
                        string submitter = Console.ReadLine();
                        Console.WriteLine("Enter the ticket's aassigned person.");
                        string assigned = Console.ReadLine();
                        Console.WriteLine("Enter the ticket watcher(s)(separate each watcher with a '|').");
                        string watchers = Console.ReadLine();
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6}", ticketID, summary, status, priority, submitter, assigned, watchers);
                        Console.WriteLine("Enter a new yticket (Y/N)?");
                        string answer = Console.ReadLine().ToUpper();
                        if (answer != "Y") { break; }
                    }
                    sw.Close();
                }
            } while (choice == "1" || choice == "2");
        }
    }
}