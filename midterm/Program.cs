using System;
using System.IO;
using System.Collections.Generic;
namespace midterm
{
    class Program
    {
        static void Main(string[] args)
        {   
            string file1 = "tickets.csv";
            string file2 = "enhancements.csv";
            string file3 = "task.csv";
            string choice;
            TicketList list = new TicketList(file1, file2, file3);
            do
            {
                Console.WriteLine("1) Read data from file.");
                Console.WriteLine("2) Create file from data.");
                Console.WriteLine("Enter any other key to exit.");
                choice = Console.ReadLine();

                if (choice == "1")
                {
                    string answer;
                    do
                    {
                        list.defects = TicketList.CreateDefectList(file1, list.defects);
                        list.enhancements = TicketList.CreateEnhancementList(file2, list.enhancements);
                        list.tasks = TicketList.CreateTaskList(file3, list.tasks);
                        Console.WriteLine("What ticket type would you like to see?\n1. Defect\n2. Enhancement\n3. Task\n4. All");
                        answer = Console.ReadLine();
                        if(answer == "1"){
                            if (File.Exists(file1))
                            {
                                foreach(Defect defect in list.defects){
                                        Console.WriteLine(defect);
                                    }
                            }
                            else
                            {
                                Console.WriteLine($"File \"{file1}\" does not exist");
                            }
                        }
                        else if(answer == "2"){
                            if (File.Exists(file2))
                            {
                                foreach(Enhancement enhancement in list.enhancements){
                                        Console.WriteLine(enhancement);
                                    }
                            }
                            else
                            {
                                Console.WriteLine($"File \"{file2}\" does not exist");
                            }
                        }
                        else if(answer == "3"){
                            if (File.Exists(file3))
                            {
                                foreach(Task task in list.tasks){
                                        Console.WriteLine(task);
                                    }
                            }
                            else
                            {
                                Console.WriteLine($"File \"{file3}\" does not exist");
                            }
                        }
                        else if(answer == "4"){
                            if (File.Exists(file1))
                            {
                                foreach(Defect defect in list.defects){
                                        Console.WriteLine(defect);
                                    }
                            }
                            else
                            {
                                Console.WriteLine($"File \"{file1}\" does not exist");
                            }
                            
                            if (File.Exists(file2))
                            {
                                foreach(Enhancement enhancement in list.enhancements){
                                        Console.WriteLine(enhancement);
                                    }
                            }
                            else
                            {
                                Console.WriteLine($"File \"{file2}\" does not exist");
                            }
                            
                            if (File.Exists(file3))
                            {
                                foreach(Task task in list.tasks){
                                        Console.WriteLine(task);
                                    }
                            }
                            else
                            {
                                Console.WriteLine($"File \"{file3}\" does not exist");
                            }
                        }
                        else{
                            Console.WriteLine("You didn't select an option");
                        }
                        Console.WriteLine("Would you like to display more? (Y/N)");
                        answer = Console.ReadLine().ToUpper();
                    } while(answer == "Y");
                }
                else if (choice == "2")
                {
                    string answer;
                    do{
                        Console.WriteLine("What ticket type would you like to see?\n1. Defect\n2. Enhancement\n3. Task\n4. All");
                        answer = Console.ReadLine();
                        if(answer == "1"){
                            TicketList.CreateTicket(file1, "defect");
                        }
                        else if(answer == "2"){
                            TicketList.CreateTicket(file2, "enhancement");
                        }
                        else if(answer == "3"){
                            TicketList.CreateTicket(file3, "task");
                        }
                        else{
                            Console.WriteLine("You didn't select an option");
                        }
                        Console.WriteLine("Would you like to create a ticket of another type? (Y/N)");
                        answer = Console.ReadLine().ToUpper();
                    } while(answer == "Y");
                }
            } while (choice == "1" || choice == "2");
        }
    }
}