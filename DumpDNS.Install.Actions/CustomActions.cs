using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;

namespace DumpDNS.Install.Actions
{
    [RunInstaller(true)]
    public class EnvPathInstaller : Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string currentPath = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
            string newEntry = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "WTDawson", "DumpDNS");

            if (!currentPath.Split(';').Contains(newEntry, StringComparer.OrdinalIgnoreCase))
            {
                string newPath = currentPath + ";" + newEntry;
                Environment.SetEnvironmentVariable("Path", newPath, EnvironmentVariableTarget.Machine);
            }
        }
    }
}