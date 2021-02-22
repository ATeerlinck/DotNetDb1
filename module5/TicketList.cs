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
        }
    }
}