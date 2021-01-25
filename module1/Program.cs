using System;
using System.IO;
namespace module1
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = "tickets.csv";
            string choice;
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
                            string line = sr.ReadLine();
                            string[] ticket = line.Split(",");
                            string[] watchers = ticket[6].Split("|");
                            string watcherList = "";
                            for(int i = 0; i<watchers.Length; i++){
                                watcherList += watchers[i];
                                if(watchers.Length >2 && i!=watchers.Length-1){
                                    watcherList += ", ";
                                }
                                if(i==watchers.Length-2){
                                    watcherList += "and ";
                                }
                            }
                            ticket[6] = watcherList;
                            Console.WriteLine("TicketID:{0}, Summary:{1}, Status:{2}, Priority:{3}, Submitter:{4}, Assigned:{5}, Watching:{6}",ticket[0],ticket[1],ticket[2],ticket[3],ticket[4],ticket[5],ticket[6]);
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
                    for (int i = 0; i < 7; i++)
                    {
                        Console.WriteLine("Enter a ticket (Y/N)?");
                        string answer = Console.ReadLine().ToUpper();
                        if (answer != "Y") { break; }
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
                    }
                    sw.Close();
                }
            } while (choice == "1" || choice == "2");
        }
    }
}