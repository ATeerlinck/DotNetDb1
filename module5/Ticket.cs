using System.Collections.Generic;
using System;

namespace module5
{    
    public class Ticket
    {
        public Int32 ticketID {get; set;}
        public string summary {get; set;}
        public string status {get; set;}
        public string priority {get; set;}
        public string submitter {get; set;}
        public string assigned {get; set;}
        List<string> watchers {get; set;}

        public Ticket(){
            watchers = new List<string>();
        }

        public override string ToString()
        {
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
                            Console.WriteLine("TicketID:{0}, Summary:{1}, Status:{2}, Priority:{3}, Submitter:{4}, Assigned:{5}, Watching:{6}",ticketID,summary,status,priority,submitter,assigned,watcherList);
            return base.ToString();
        }
    }
}