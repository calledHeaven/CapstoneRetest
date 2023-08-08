using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MOOK
{
    public abstract class FileReader
    {

        public static string GetFileName()
        {
            string Filename;
            Console.WriteLine("Please enter the file directory of the file, e.g C:\\Users\\HomeUser\\Documents\\Visual Studio 2022\\coursework");
            Console.WriteLine("if this is a struggle, locate the files in the file explorer, right click and seslect 'Copy as text'");
            Filename = Console.ReadLine();

            while (Filename == null) 
            {
                Console.WriteLine("error in netering file location, please try again");
            }

            return Filename;
        }
        

        public static void Read_Input_To_Transaction_Type1(ProjectsManager projectsManager, string FileName ) 
        {
            bool Project_Exists = false;
            string lines;
            StreamReader sr = new StreamReader(FileName);
            while ((lines = sr.ReadLine()) != null)
            {
                string[] splitstring = lines.Split(",");

                int P_ID = Convert.ToInt32(splitstring[0]);
                char T_Type = Convert.ToChar(splitstring[1]);
                float T_Value = Convert.ToSingle(splitstring[2]);

                Project_Exists = projectsManager.checkForProjects(P_ID);

                if (Project_Exists == true)
                {
                    transactions transactions = new transactions(T_Type, T_Value);
                    foreach (Projects p in projectsManager._projects)
                    {
                        if (p.Project_ID == P_ID)
                        {
                            p.addTransaction(transactions);
                        }
                    }
                }
                else
                {
                    string Type_To_Upper = T_Type.ToString();
                    Type_To_Upper = Type_To_Upper.ToUpper();
                    if (Type_To_Upper == "L")
                    {
                        transactions transactions = new transactions(T_Type, T_Value);
                        Projects projects = new Projects(P_ID, true);
                        projectsManager.addProjects(projects);
                    }
                    else
                    {
                        transactions transactions = new transactions(T_Type, T_Value);
                        Projects projects = new Projects(P_ID, false);
                        projectsManager.addProjects(projects);
                    }
                }
            }
        }

        public static void Read_Input_To_Transaction_Type2(ProjectsManager projectsManager, string FileName) // change to read second file type
        {
            bool Project_Exists = false;
            string lines;
            StreamReader sr = new StreamReader(FileName);
            while ((lines = sr.ReadLine()) != null)
            {
                string[] splitstring = lines.Split("(");

                char T_Type = Get_Transaction_Type(splitstring[0]);

                splitstring = splitstring[1].Split(")");
                int P_ID = Convert.ToInt32(splitstring[0]);

                splitstring = lines.Split(" = ");
                float T_Value = Convert.ToSingle(splitstring[1]);

                Project_Exists = projectsManager.checkForProjects(P_ID);

                if (Project_Exists == true)
                {
                    transactions transactions = new transactions(T_Type, T_Value);
                    foreach (Projects p in projectsManager._projects)
                    {
                        if (p.Project_ID == P_ID)
                        {
                            p.addTransaction(transactions);
                        }
                    }
                }
                else
                {
                    string Type_To_Upper = T_Type.ToString();
                    Type_To_Upper = Type_To_Upper.ToUpper();
                    if (Type_To_Upper == "L")
                    {
                        transactions transactions = new transactions(T_Type, T_Value);
                        Projects projects = new Projects(P_ID, true);
                        projectsManager.addProjects(projects);
                    }
                    else
                    {
                        transactions transactions = new transactions(T_Type, T_Value);
                        Projects projects = new Projects(P_ID, false);
                        projectsManager.addProjects(projects);
                    }
                }
            }
        }

        public static char Get_Transaction_Type(string Type_Unsorted) 
        {
            Type_Unsorted = Type_Unsorted.ToUpper();
            if (Type_Unsorted == "Land")
            {
                return ('L');
            }
            else if (Type_Unsorted == "Renovation")
            {
                return ('R');
            }
            else if (Type_Unsorted == "Purchase")
            {
                return ('P');
            }
            else if (Type_Unsorted == "Sale")
            {
                return ('S');
            }
            else
            {
                return ('E');
            }
        }

        public static int Get_Input_Type(string FileName)
        {
            string lines;
            StreamReader sr = new StreamReader(FileName);
            lines = sr.ReadLine();
            if (Char.IsDigit(lines[0]) == true) 
            {
                return 1;
            }
            else if (Char.IsLetter(lines[0]) == true) 
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
       
        public static void ChooseReadtype(int FileType, ProjectsManager projectsManager, string FileName)
        {
            if (FileType == 1)
                Read_Input_To_Transaction_Type1(projectsManager, FileName);
        }

        
    }
}
