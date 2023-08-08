using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOK
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
                _menuItems.Add(new ShowExistingProjects(_manager));
                _menuItems.Add(new EditExistingProjectMenu(_manager));
                _menuItems.Add(new RemoveExistingProjectMenu(_manager));
            }
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Shape Manager Menu";
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
            _menuItems.Add(new ExitMenuItem(this));
        }

        public override string MenuText()
        {
            return "Display an existing project";
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

    class EditExistingProjectMenu : ConsoleMenu // allows for browsing of projects to check and eddit transactions
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
            return "Edit an existing shape menu";
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
            return "Remove existing shape menu";
        }

        public override void Select()
        {
            base.Select();
        }
    }

    #endregion



    #region Transaction Menus and MenuItems


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
            Console.WriteLine("ID  Type  Value");
            foreach (transactions t in Projects._transactions)
            {
                Console.WriteLine(Projects.Project_ID + "   " +  t.transaction.ToString());
            }
            Projects.Get_Total_Profit();

            _menuItems.Add(new ViewProjectProfits(Projects));
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
            return " View Project " + Projects.Project_ID + " Transactions";
        }
    }

    class ViewProjectProfits : MenuItem // allows for project profits viewing
    {
        private Projects Projects;

        public ViewProjectProfits(Projects projects)
        {
            Projects = projects;
        }
        public override void Select()
        {

            Console.WriteLine("total sales for project " + Projects.Project_ID);
            Console.WriteLine(Projects.Get_Total_Profit());

        }
        public override string MenuText()
        {
            return " View Project Profits";
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
            
            Console.WriteLine("total sales for project " + Projects.Project_ID);
            Console.WriteLine( Projects.Get_Total_Sales() );
            
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

            Console.WriteLine("total Purchases for project " + Projects.Project_ID);
            Console.WriteLine(  Projects.Get_Total_Purchases()  );

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


