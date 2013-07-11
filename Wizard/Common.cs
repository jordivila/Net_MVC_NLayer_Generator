using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EnvDTE80;

namespace Wizard
{
    public class GlobalData
    {
        public string NuGetPackagesFolderName = "packages";
        public string CustomNamespace { get; set; }
        public readonly string CustomNamespaceKey = "$customNamespace$";
        public DirectoryInfo PackagesDirectory { get; private set; }
        public DirectoryInfo SolutionDirectory { get; private set; }
        public DirectoryInfo PackagesDirectoryRepository { get; private set; }
        public DTE2 dte { get; set; }

        public GlobalData(object automationObjet, Dictionary<string, string> replacementsDictionary, object[] customParams)
        {
            this.CustomNamespace = replacementsDictionary["$projectname$"].Replace(" ", "_");
            this.SolutionDirectory = new DirectoryInfo((string)replacementsDictionary["$destinationdirectory$"]);
            if (this.SolutionDirectory.Parent.GetDirectories(this.NuGetPackagesFolderName).Count() == 0)
            {
                this.PackagesDirectory = this.SolutionDirectory.Parent.CreateSubdirectory(this.NuGetPackagesFolderName);
            }
            else
            {
                this.PackagesDirectory = this.SolutionDirectory.Parent.GetDirectories(this.NuGetPackagesFolderName).First();
            }

            this.PackagesDirectoryRepository = this.DirectoryGetPackagesRepository(customParams);
        }

        private bool DirectoryHasPackages(DirectoryInfo current)
        {
            return (current.GetDirectories(this.NuGetPackagesFolderName).Count() == 1);
        }
        public string DirectoryGetPackagesLevel(DirectoryInfo current)
        {
            string result = string.Empty;
            string packagesDirectoryLevel = @".\";

            if (!this.DirectoryHasPackages(current))
            {
                while (current.Parent != null)
                {
                    packagesDirectoryLevel += @"..\";

                    if (!this.DirectoryHasPackages(current.Parent))
                    {
                        current = current.Parent;
                    }
                    else
                    {
                        result = packagesDirectoryLevel;
                        break;
                    }
                }
            }

            return result;
        }
        private DirectoryInfo DirectoryGetPackagesRepository(object[] customParams)
        {
            DirectoryInfo result = null;
            DirectoryInfo current = new FileInfo((string)customParams[0]).Directory;
            while (current.Parent != null)
            {
                if (!this.DirectoryHasPackages(current.Parent))
                {
                    current = current.Parent;
                }
                else
                {
                    result = current.Parent.GetDirectories(this.NuGetPackagesFolderName).First();
                    break;
                }
            }
            return result;
        }
        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
