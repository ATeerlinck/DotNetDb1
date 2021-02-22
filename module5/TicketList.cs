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
    }
}