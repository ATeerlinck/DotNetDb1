using System.Collections.Generic;
using System;
using System.Globalization;

namespace midterm
{    
    public class Ticket
    {
        public Int32 ticketID {get; set;}
        public string summary {get; set;}
        public string status {get; set;}
        public string priority {get; set;}
        public string submitter {get; set;}
        public string assigned {get; set;}
        public List<string> watchers {get; set;}

        public Ticket(){
            watchers = new List<string>();
        }

        public string watcherDisplay(){
            string watcherList = "";
            for(int i = 0; i<watchers.Count; i++){
                watcherList += watchers[i];
                if(watchers.Count >2 && i!=watchers.Count-1){
                    watcherList += ", ";
                }
                if(i==watchers.Count-2){
                    watcherList += "and ";
                }
            }
            return watcherList;
        }

        public override string ToString()
        {
            string watcherList = watcherDisplay();
            return $"TicketID :{ticketID}, Summary: {summary}, Status: {status}, Priority: {priority}, Submitter: {submitter}, Assigned: {assigned}, Watching: {watcherList}";
        }
    }
    public class Defect : Ticket{
        public string severity {get; set;}
        public override string ToString(){
            string watcherList = watcherDisplay();
            return $"TicketID :{ticketID}, Summary: {summary}, Status: {status}, Priority: {priority}, Submitter: {submitter}, Assigned: {assigned}, Watching: {watcherList}, Severity: {severity}";
        }
    }

    public class Enhancement : Ticket{
        public double cost {get; set;}
        public string reason {get; set;}
        public DateTime estimate {get; set;}
        public override string ToString(){
            string watcherList = watcherDisplay();
            return $"TicketID :{ticketID}, Summary: {summary}, Status: {status}, Priority: {priority}, Submitter: {submitter}, Assigned: {assigned}, Watching: {watcherList}, Cost: {cost.ToString("C3",CultureInfo.CurrentCulture)}, Reason: {reason}, Estimate: {estimate:MMMM dd, yyyy}";
        }
    }

    public class Task : Ticket{
        public string projectName {get; set;}
        public DateTime dueDate {get; set;}
        public override string ToString(){
            string watcherList = watcherDisplay();
            return $"TicketID :{ticketID}, Summary: {summary}, Status: {status}, Priority: {priority}, Submitter: {submitter}, Assigned: {assigned}, Watching: {watcherList}, Project Name: {projectName}, Date Due: {dueDate:MMMM dd, yyyy}";
        }
    }
}