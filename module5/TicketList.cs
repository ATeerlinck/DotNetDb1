using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using NLog.Web;
namespace module5
{
    public class TicketList
    {
        public string file {get; set;}
        public List<Ticket> Tickets {get; set;}
        public static NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

        public TicketList(string fileName){
            file = fileName;
            Tickets = new List<Ticket>();
            Tickets = CreateTicketList(file);
        }

        public static List<Ticket> CreateTicketList(string file){
            List<Ticket> Tickets = new List<Ticket>();
            try{
                StreamReader sr = new StreamReader(file);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] ticket = line.Split(",");
                    string[] watchers = ticket[6].Split("|");
                    Ticket newTicket = new Ticket();
                    newTicket.ticketID = Convert.ToInt32(ticket[0]);
                    newTicket.summary = ticket[1];
                    newTicket.status = ticket[2];
                    newTicket.priority = ticket[3];
                    newTicket.submitter = ticket[4];
                    newTicket.assigned = ticket[5];
                    newTicket.watchers = ticket[6].Split("|").ToList();
                    Tickets.Add(newTicket);
                }
                sr.Close();
            }
            catch (Exception e){
                logger.Error(e.Message);
            }
            return Tickets;
        }
        public static void CreateTicket(string file){
            StreamWriter sw = new StreamWriter(file, true);
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
                Console.WriteLine("Enter the ticket's assigned person.");
                string assigned = Console.ReadLine();
                List<string> watchers = new List<string>();
                string answe;
                do{
                    Console.WriteLine("Enter a ticket wathcer.");
                    string watcher = Console.ReadLine();
                    watchers.Add(watcher);
                    Console.WriteLine("Is there another a watcher? (Y/N)");
                    answe = Console.ReadLine().ToUpper();
                } while(answe=="Y");
                sw.Write("\n{0},{1},{2},{3},{4},{5},{6}", ticketID, summary, status, priority, submitter, assigned, string.Join("|",watchers));
                Console.WriteLine("Enter a new yticket (Y/N)?");
                string answer = Console.ReadLine().ToUpper();
                if (answer != "Y") { break; }
            }
            sw.Close();
        }
    }
}