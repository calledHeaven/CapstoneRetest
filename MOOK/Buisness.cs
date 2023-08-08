using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOOK
{

    public class ProjectsManager
    {
        public List<Projects> _projects { get; private set; }

        public ProjectsManager()
        {
            _projects = new List<Projects>();
        }

        public void addProjects(Projects projects)
        {
            _projects.Add(projects);

        }

        public bool checkForProjects(int ProjectID) 
        { 
            bool PrjoectExsists = false;
            foreach (Projects project in _projects) 
            {
                if (project.Project_ID == ProjectID)
                {
                    PrjoectExsists = true;
                    return PrjoectExsists;
                    break;
                }
            }
            return PrjoectExsists;
        }

        
    }


}
