using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using NLog.Web;
using System.Globalization;
namespace midterm
{
    public class module9assignment
    {
        public string file1 {get; set;}
        public string file2 {get; set;}
        public string file3 {get; set;}
        public List<Defect> defects {get; set;}
        public List<Enhancement> enhancements {get; set;}
        public List<Task> tasks {get; set;}
        public static NLog.Logger logger = NLog.Web.NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

        public TicketList(string File1, string File2, string File3){
            file1 = File1;
            file2 = File2;
            file3 = File3;
            defects = new List<Defect>();
            enhancements = new List<Enhancement>();
            tasks = new List<Task>();
            CreateDefectList(file1, defects);
            CreateEnhancementList(file2, enhancements);
            CreateTaskList(file3, tasks);
        }

        public static List<Defect> CreateDefectList(string file1, List<Defect> defects){
            defects = new List<Defect>();
            try{
                if (File.Exists(file1)){    
                    StreamReader sr = new StreamReader(file1);
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] ticket = line.Split(",");
                        Defect newTicket = new Defect();
                        newTicket.ticketID = Convert.ToInt32(ticket[0]);
                        newTicket.summary = ticket[1];
                        newTicket.status = ticket[2];
                        newTicket.priority = ticket[3];
                        newTicket.submitter = ticket[4];
                        newTicket.assigned = ticket[5];
                        newTicket.watchers = ticket[6].Split("|").ToList();
                        newTicket.severity = ticket[7];
                        defects.Add(newTicket);
                    }
                    sr.Close();
                }  
            }
            catch (Exception e){
                logger.Error(e.Message);
            }
            return defects;
        }
        public static List<Enhancement> CreateEnhancementList(string file2, List<Enhancement> enhancements){
            enhancements = new List<Enhancement>();
            try{    
                if (File.Exists(file2)){
                    StreamReader sr = new StreamReader(file2);
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] ticket = line.Split(",");
                        Enhancement newTicket = new Enhancement();
                        newTicket.ticketID = Convert.ToInt32(ticket[0]);
                        newTicket.summary = ticket[1];
                        newTicket.status = ticket[2];
                        newTicket.priority = ticket[3];
                        newTicket.submitter = ticket[4];
                        newTicket.assigned = ticket[5];
                        newTicket.watchers = ticket[6].Split("|").ToList();
                        newTicket.cost = Double.Parse(ticket[7]);
                        newTicket.reason = ticket[8];
                        newTicket.estimate = DateTime.Parse(ticket[9]);
                        enhancements.Add(newTicket);
                    }
                    sr.Close();
                }
            }
            catch (Exception e){
                logger.Error(e.Message);
            }
            return enhancements;
        }
        public static List<Task> CreateTaskList(string file3,  List<Task> tasks){
            tasks = new List<Task>();
            try{    
                if (File.Exists(file3)){
                    StreamReader sr = new StreamReader(file3);
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] ticket = line.Split(",");
                        Task newTicket = new Task();
                        newTicket.ticketID = Convert.ToInt32(ticket[0]);
                        newTicket.summary = ticket[1];
                        newTicket.status = ticket[2];
                        newTicket.priority = ticket[3];
                        newTicket.submitter = ticket[4];
                        newTicket.assigned = ticket[5];
                        newTicket.watchers = ticket[6].Split("|").ToList();
                        newTicket.projectName = ticket[7];
                        newTicket.dueDate = DateTime.Parse(ticket[8]);
                        tasks.Add(newTicket);
                    }
                    sr.Close();
                }
            }
            catch (Exception e){
                logger.Error(e.Message);
            }
            return tasks;
        }
        public static void CreateTicket(string file, string type){
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
                if(type.ToLower()=="defect"){
                    Console.WriteLine("How severe is this issue?");
                    string severity = Console.ReadLine();
                    sw.Write("\n{0},{1},{2},{3},{4},{5},{6},{7}", ticketID, summary, status, priority, submitter, assigned, string.Join("|",watchers), severity);
                }
                if(type.ToLower()=="enhancement"){
                    Console.WriteLine("How much does this cost?");
                    double cost = Convert.ToInt64(Console.ReadLine());
                    Console.WriteLine("Why is it needed?");
                    string reason = Console.ReadLine();
                    Console.WriteLine("When are we going to get it? (mm/dd/yyyy)");
                    DateTime estimate = DateTime.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
                    sw.Write("\n{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}", ticketID, summary, status, priority, submitter, assigned, string.Join("|",watchers), cost, reason, estimate);
                }
                if(type.ToLower()=="task"){
                    Console.WriteLine("What is the Project Name?");
                    string projectName = Console.ReadLine();
                    Console.WriteLine("When should it be done? (mm/dd/yyyy)");
                    DateTime dueDate = DateTime.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
                    sw.Write("\n{0},{1},{2},{3},{4},{5},{6},{7},{8}", ticketID, summary, status, priority, submitter, assigned, string.Join("|",watchers), projectName, dueDate);
                }
                Console.WriteLine($"Enter a new {type} ticket (Y/N)?");
                string answer = Console.ReadLine().ToUpper();
                if (answer != "Y") { break; }
            }
            sw.Close();
        }
    }
}