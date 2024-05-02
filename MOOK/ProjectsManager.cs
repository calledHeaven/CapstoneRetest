using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MOOK // note: remember to add readlines to data viewing sections, so you actualy hav a chance to read the data
{
    // based on the OO Menu system provided by Hull University
    #region Project Manager Menus and MenuItems

    class ProjectManagerMenu : ConsoleMenu
    {
        private ProjectsManager _manager;

        public ProjectManagerMenu(ProjectsManager manager)
        {
            _manager = manager;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();
            _menuItems.Add(new AddNewProjectMenu(_manager));
            if (_manager._projects.Count > 0)
            {
                _menuItems.Add(new CreateNewProjects(_manager));
                _menuItems.Add(new ShowExistingProjects(_manager));
                _menuItems.Add(new EditExistingProjectMenu(_manager));
                _menuItems.Add(new RemoveExistingProjectMenu(_manager));
            }
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Buisness Manager Menu";
        }
    }

    class CreateNewProjects : ConsoleMenu //creates list of existing projects, that can be viewed and selected to view transactions
    {
        ProjectsManager _manager;

        public CreateNewProjects(ProjectsManager manager)
        {
            _manager = manager;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();

            _menuItems.Add(new CreateProjectRenovation(_manager));
            _menuItems.Add(new CreateProjectNewbuild(_manager));

            
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Create a new project";
        }
    }

    class ShowExistingProjects : ConsoleMenu //creates list of existing projects, that can be viewed and selected to view transactions
    {
        ProjectsManager _manager;

        public ShowExistingProjects(ProjectsManager manager)
        {
            _manager = manager;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();

            
             foreach (Projects p in _manager._projects)
             {
                 switch (p)
                 {
                     case Projects:
                         _menuItems.Add(new ViewProjectTransactions(p));
                         break;
                 }
             }
            _menuItems.Add(new TotalPortfolio(_manager));
             _menuItems.Add(new ExitMenuItem(this));
         }

         public override string MenuText()
         {
             return "Display an existing Project";
         }
     }

     class AddNewProjectMenu : MenuItem //allows for file reading that will create new projects as needed
     {
         ProjectsManager _manager;

         public AddNewProjectMenu(ProjectsManager manager)
         {
             _manager = manager;
         }

         public override void Select()
         {
             string FileName;
             int FileType;
             FileName = FileReader.GetFileName();

             FileType = FileReader.Get_Input_Type(FileName);

             switch (FileType)
             {
                 case 1:
                     FileReader.Read_Input_To_Transaction_Type1(_manager, FileName);
                     break;

                 case 2:
                     FileReader.Read_Input_To_Transaction_Type2(_manager, FileName);
                     break;

                 case 3:
                     Console.WriteLine("Error in File, Please check File Transcript and try again");
                     break;


             }

         }

         public override string MenuText()
         {
             return "Read Text File to generate a project";
         }
     }

     class EditExistingProjectMenu : ConsoleMenu // allows for browsing of projects to check and edit transactions
     {
         private ProjectsManager _manager;

         public EditExistingProjectMenu(ProjectsManager manager)
         {
             _manager = manager;
         }

         public override void CreateMenu()
         {
             _menuItems.Clear();
             foreach (Projects project in _manager._projects)
             {
                 switch (project)
                 {
                     case Projects:
                         _menuItems.Add(new EditExistingProjectMenuItem(project));
                         break;
                 }
             }
             _menuItems.Add(new ExitMenuItem(this));

         }

         public override string MenuText()
         {
             return "Edit an existing project";
         }
     }

     class EditExistingProjectMenuItem : MenuItem //allows for selection of projects to edit
     {
         private Projects Projects;

         public EditExistingProjectMenuItem(Projects projects)
         {
             Projects = projects;
         }

         public override string MenuText()
         {
             return "add transaction to project " + Projects.Project_ID + " manualy";
         }

         public override void Select()
         {
             char Type;
             float value;

             Console.WriteLine("please enter transaction type:  \n enter in the format 'L' for land, 'P; for purchase, and so on");
             Type = Convert.ToChar((Console.ReadLine().ToUpper()));

             /*
             while (Type != 'L' || Type != 'R' || Type != 'P' || Type != 'S' )
             {
                 Console.WriteLine("invalid input, please try again");
                 Console.WriteLine("please enter transaction type:  \n enter in the format 'L' for land, 'P; for purchase, and so on");
                 Type = Convert.ToChar((Console.ReadLine().ToUpper()));
             }
             */
            Console.WriteLine("please enter transaction Value");
            value =  float.Parse(Console.ReadLine());

            while (value < 0 )
            {
                Console.WriteLine("invalid input, please try again");
                Console.WriteLine("please enter transaction Value");
                value = float.Parse(Console.ReadLine());
            }

            transactions transactions = new transactions(Type, value);
            Projects.addTransaction(transactions);
        }
    }

    class RemoveExistingProjectMenuItem : MenuItem // creates menu for project removal
    {
        private Projects Projects;
        private ProjectsManager _manager;

        public RemoveExistingProjectMenuItem(Projects projects, ProjectsManager projectManager)
        {
            Projects = projects;
            _manager = projectManager;
        }

        public override string MenuText()
        {
            return Convert.ToString(Projects.Project_ID);
        }

        public override void Select()
        {
            for (int i = 0; i < _manager._projects.Count; i++)
            {
                if (_manager._projects[i] == Projects)
                {
                    _manager._projects.RemoveAt(i);
                }
            }
        }
    }

    class RemoveExistingProjectMenu : ConsoleMenu // menu ofprojects to remove
    {
        private ProjectsManager _manager;
        public RemoveExistingProjectMenu(ProjectsManager manager)
        {
            _manager = manager;
        }

        public override void CreateMenu()
        {
            _menuItems.Clear();
            foreach (Projects projects in _manager._projects)
            {
                _menuItems.Add(new RemoveExistingProjectMenuItem(projects, _manager));
            }
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Remove existing Project menu";
        }

        public override void Select()
        {
            base.Select();
        }
    }

    #endregion



    #region Transaction Menus and MenuItems

    class TotalPortfolio : MenuItem //displays all transactions in the portfolio
    {
        private ProjectsManager _manager;

        public TotalPortfolio(ProjectsManager projectsManager)
        {
            _manager = projectsManager;
        }

        public override string MenuText()
        {
            return ("show total of portfolio");
        }

        public override void Select()
        {
            
             double PortfolioProfit = 0;
             double PortfolioRefund = 0;
             double PortfolioSales = 0;
             double PortfolioPurchases = 0;
             foreach (Projects p in _manager._projects)
             {
                 PortfolioProfit += p.Get_Total_Profit();
                 PortfolioRefund += p.VATRefund(PortfolioProfit);
                 PortfolioSales += p.Get_Total_Sales();
                 PortfolioPurchases += p.Get_Total_Purchases();
             }
            Console.WriteLine("total portfolio profit " + PortfolioProfit + "\ntotal Portfolio refund " + PortfolioRefund + "\ntotal Portfolio sales " + PortfolioSales + "\ntotal Portfolio Purchases " + PortfolioPurchases);
            Console.ReadLine();
            
        }
    }

    class CreateProjectNewbuild : MenuItem //displays all transactions in a project
    {
        private ProjectsManager _manager;

        public CreateProjectNewbuild(ProjectsManager projectsManager)
        {
            _manager = projectsManager;
        }

        public override string MenuText()
        {
            return ("Create a Newbuild Project");
        }

        public override void Select()
        {
            bool NumInUse = false;
            Console.WriteLine("Please enter new project Number");
            int NewProjectNumber = Convert.ToInt16(Console.ReadLine());

            foreach (Projects projects in _manager._projects)
            {
                if (projects.Project_ID == NewProjectNumber)
                {
                    NumInUse = true;
                }
            }
            if (NumInUse == true)
            {
                Console.WriteLine(" this project number is currently in use, please use another");
            }
            else
            {
                Projects projects = new Projects(NewProjectNumber, true);
                _manager.addProjects(projects);
                Console.WriteLine("new project has been created");
                Console.ReadLine();
            }

        }
    }

    class CreateProjectRenovation : MenuItem //displays all transactions in a project
    {
        private ProjectsManager _manager;

        public CreateProjectRenovation(ProjectsManager projectsManager)
        {
            _manager = projectsManager;
        }

        public override string MenuText()
        {
            return ("Create a New Renovation Project");
        }

        public override void Select()
        {
            bool NumInUse = false;
            Console.WriteLine("Please enter new project Number");
            int NewProjectNumber = Convert.ToInt16(Console.ReadLine());

            foreach (Projects projects in _manager._projects)
            {
                if (projects.Project_ID == NewProjectNumber)
                {
                    NumInUse = true;
                }
            }
            if (NumInUse == true)
            {
                Console.WriteLine(" this project number is currently in use, please use another");
            }
            else
            {
                Projects projects = new Projects(NewProjectNumber, false);
                _manager.addProjects(projects);
                Console.WriteLine("new project has been created");
                Console.ReadLine();
            }

        }
    }

    class DisplayProjectTransactions : MenuItem //displays all transactions in a project
    {
        private Projects _projects;

        public DisplayProjectTransactions(Projects projects)
        {
            _projects = projects;
        }

        public override string MenuText()
        {
            return ("Project " + _projects.Project_ID + " transactions");
        }

        public override void Select()
        {
            foreach (transactions t in _projects._transactions)
            {
                Console.WriteLine(t.transaction);
            }
            Console.ReadLine();


        }
    }

    class ViewProjectTransactions : ConsoleMenu //displays transactions and allows for viewing of project information
    {
        private Projects Projects;

        public ViewProjectTransactions(Projects projects)
        {
            Projects = projects;
        }
        public override void CreateMenu()
        {
            _menuItems.Clear();
            foreach (transactions t in Projects._transactions)
            Projects.Get_Total_Profit();
            _menuItems.Add(new DisplayProjectTransactions(Projects));
            _menuItems.Add(new ViewProjectSales(Projects));
            _menuItems.Add(new ViewProjectPurchases(Projects));
            if (Projects.VAT_available == true) 
            {
                _menuItems.Add(new ViewProjectVAT(Projects));
            }
            _menuItems.Add(new ExitMenuItem(this));
        }
        public override string MenuText()
        {
            return Projects.Project_ID + " Sales (" + Projects.Get_Total_Sales() + ")  Purchases ("  + Projects.Get_Total_Purchases() + ")  Refund (" + Projects.VATRefund(Projects.Get_Total_Profit()) + ")  Profit (" + Projects.Get_Total_Profit() + ")" ; 
        }
    }

    class ViewProjectSales : MenuItem // alows for project sales viewing
    {
        private Projects Projects;

        public ViewProjectSales(Projects projects)
        {
            Projects = projects;
        }
        public override void Select()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Type  Value");
            foreach (var transaction in Projects._transactions) 
            {
                if (transaction.Transaction_Type == 'S')
                    Console.WriteLine("s    " + transaction.Transaction_Value);
            }

            Console.WriteLine(" ");
            Console.WriteLine("total sales for project " + Projects.Project_ID);
            Console.WriteLine( Projects.Get_Total_Sales() );
            Console.ReadLine();
        }
        public override string MenuText()
        {
            return " View Project Sales";
        }
    }
    class ViewProjectPurchases : MenuItem // allows for project purchase viewing
    {
        private Projects Projects;

        public ViewProjectPurchases(Projects projects)
        {
            Projects = projects;
        }
        public override void Select()
        {

            Console.WriteLine(" ");
            Console.WriteLine("Type  Value");
            foreach (var transaction in Projects._transactions)
            {
                if (transaction.Transaction_Type == 'P')
                    Console.WriteLine("P    " + transaction.Transaction_Value);
                else if (transaction.Transaction_Type == 'R')
                    Console.WriteLine("R    " + transaction.Transaction_Value);
                else if (transaction.Transaction_Type == 'L')
                    Console.WriteLine("L    " + transaction.Transaction_Value);
            }

            Console.WriteLine(" ");
            Console.WriteLine("total Purchases for project " + Projects.Project_ID);
            Console.WriteLine(Projects.Get_Total_Purchases());
            Console.ReadLine();


        }
        public override string MenuText()
        {
            return " View Project Purchases";
        }
    }
    class ViewProjectVAT : MenuItem // allows for VAT refund amount viewing if eligable projects
    {
        private Projects Projects;

        public ViewProjectVAT(Projects projects)
        {
            Projects = projects;
        }
        public override void Select()
        {

            Console.WriteLine("VAT calculations for " + Projects.Project_ID);
            Console.WriteLine(Projects.VATRefund(Projects.Get_Total_Profit()));

        }
        public override string MenuText()
        {
            return " View Project VAT refund";
        }
    }
        #endregion

}


