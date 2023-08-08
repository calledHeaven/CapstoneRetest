// See https://aka.ms/new-console-template for more information
using MOOK;
using System.Runtime.InteropServices;

ProjectsManager manager = new ProjectsManager();

new ProjectManagerMenu(manager).Select();

