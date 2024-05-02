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

        public static string GetFileName() // gets file name and returns as a string for other methods later use
        {
            string Filename;
            try
            {
                
                Console.WriteLine( "\nPlease enter the file directory of the file, e.g C:\\Users\\HomeUser\\Documents\\Visual Studio 2022\\coursework");
                Console.WriteLine("if this is a struggle, locate the files in the file explorer, right click and seslect 'Copy as text', and dont forget to remove the quotation marks around the file");
                Filename = Console.ReadLine();

                while (Filename == null)
                {
                    Console.WriteLine("error in entering file location, please try again");
                }

                StreamReader sr_test = new StreamReader(Filename);

            }
            catch
            (Exception e)
            {
                Console.WriteLine("Filename enter incorrectly, please try again");
                Console.WriteLine("Please enter the file directory of the file, e.g C:\\Users\\HomeUser\\Documents\\Visual Studio 2022\\coursework");
                Console.WriteLine("if this is a struggle, locate the files in the file explorer, right click and seslect 'Copy as text'");
                Filename = Console.ReadLine();
                return Filename;
            }

            return Filename;
        }
        

        public static void Read_Input_To_Transaction_Type1(ProjectsManager projectsManager, string FileName ) // reads files of the first type (comma seperated files), and splits it into sections that can easily be input into the transactions constructor, and will automaticaly enter it into projects, or creates them if they do not already exist
        {
            bool Project_Exists = false;
            string lines;
            try
            {
                StreamReader sr_test = new StreamReader(FileName);
            }
            catch (Exception e) 
            {
                Console.WriteLine("errer in file name please try again");
            }
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
                        projects.addTransaction(transactions);
                    }
                    else
                    {
                        transactions transactions = new transactions(T_Type, T_Value);
                        Projects projects = new Projects(P_ID, false);
                        projectsManager.addProjects(projects);
                        projects.addTransaction(transactions);
                    }
                }
            }
        }

        public static void Read_Input_To_Transaction_Type2(ProjectsManager projectsManager, string FileName) // an adjustment of the first read to input method adjusted to read the second file type(full type files)
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
                float T_Value = float.Parse(splitstring[1] = splitstring[1].Remove(splitstring[1].Length -1));

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
                        projects.addTransaction(transactions);
                    }
                    else
                    {
                        transactions transactions = new transactions(T_Type, T_Value);
                        Projects projects = new Projects(P_ID, false);
                        projectsManager.addProjects(projects);
                        projects.addTransaction(transactions);
                    }
                }
            }
        }

        public static char Get_Transaction_Type(string Type_Unsorted) // gets Transaction types from type 2 files, and converts them to charicters that can be input into the transactions constructors
        {
            Type_Unsorted = Type_Unsorted.ToUpper();
            if (Type_Unsorted == "LAND")
            {
                return ('L');
            }
            else if (Type_Unsorted == "RENOVATION")
            {
                return ('R');
            }
            else if (Type_Unsorted == "PURCHASE")
            {
                return ('P');
            }
            else if (Type_Unsorted == "SALE")
            {
                return ('S');
            }
            else
            {
                return ('E');
            }
        }

        public static int Get_Input_Type(string FileName) // checks file type, whist adding in a third section to demonstrait erronius file type readings
        {
            string lines;
            try
            {
                StreamReader sr_Test = new StreamReader(FileName);
            }
            catch
            (Exception e)
            {
                Console.WriteLine("Filename enter incorrectly, please try again");
                
            }

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
    }
}
