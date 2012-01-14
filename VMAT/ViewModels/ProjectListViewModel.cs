using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VMAT.Models;

namespace VMAT.ViewModels
{
    public class ProjectListViewModel
    {
        /// <summary>
        /// Pull all of the information for each virtual machine. Parse the machine
        /// and project name and fill in any other derived information. Group the
        /// machines into their respective projects.
        /// </summary>
        /// <returns>A list of project items and information</returns>
        public static List<Project> GetProjectInfo()
        {
            VirtualMachineManager manager = new VirtualMachineManager();
            List<Project> projects = new List<Project>();

            projects.Add(new Project("1234"));

            foreach (string imageName in manager.GetRegisteredVMImagePaths())
            {
                VirtualMachine vm = new RunningVirtualMachine(imageName);
                projects[0].AddVirtualMachine(vm);
            }

            return projects;
        }
    }
}