
    using System;
    using System.Collections.Generic;
    using System.Linq;

namespace MoveNugetFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PLEASE TYPE THE PATH TO NUGET PACKAGES FOLDER YOU WOULD LIKE TO IMPORT AND HIT ENTER");
            string addPackages = Console.ReadLine();
            Console.Clear();
            if (!System.IO.Directory.Exists(addPackages))
            {
                Console.WriteLine("The directory path is inaccesible or does not exist.");
                Console.ReadKey();
                Console.WriteLine("The folder will default to \"\\\\vaprd-rp-01\\Depts\\Business Automation\\Robotics\\Nuget Packages Repository\\Packages To Add\" -- Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                addPackages = @"\\vaprd-rp-01\Depts\Business Automation\Robotics\Nuget Packages Repository\Packages To Add";
            }

            string corePackages = @"\\vaprd-rp-01\Depts\Business Automation\Robotics\Nuget Packages Repository\nuget";
            List<string> toAddFiles = new List<string>();
            List<string> nugetPackages = new List<string>();
            var tAddFiles = System.IO.Directory.GetDirectories(addPackages);
            toAddFiles.AddRange(tAddFiles);
            var currentFiles = System.IO.Directory.GetDirectories(corePackages);
            nugetPackages.AddRange(currentFiles);

            foreach(string dir in toAddFiles)
            {
                Console.WriteLine("Checking if " + System.IO.Path.GetFileName(dir) + " exists in " + corePackages);
                bool dirExists = nugetPackages.Any(s => System.IO.Path.GetFileName(s) == System.IO.Path.GetFileName(dir));
                if(dirExists == false)
                {
                    Console.WriteLine(dirExists.ToString());
                }
                if(dirExists == true)
                {
                    AddOnlyNeededVersions(dir, nugetPackages);
                }
                else
                {
                    Console.WriteLine("Trying to move " + dir + " to: " + corePackages);
                    System.IO.Directory.Move(dir, corePackages+"\\"+System.IO.Path.GetFileName(dir));
                }
                try
                {
                    if (System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.Delete(dir, true);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error deleting " + dir + " -- " + e.Message.ToString());
                }
            }
        }

        public static void AddOnlyNeededVersions(string dir, List<string> nugetPackages)
        {
            var coreNugetDir = nugetPackages.First(s => s.Contains(System.IO.Path.GetFileName(dir)));
            string[] cND = System.IO.Directory.GetDirectories(coreNugetDir);
            List<string> coreNugetSubDir = new List<string>();
            coreNugetSubDir.AddRange(cND);

            string[] tAddVersions = System.IO.Directory.GetDirectories(dir);
            List<string> toAddVersions = new List<string>();
            toAddVersions.AddRange(tAddVersions);
            foreach(string toAddSub in toAddVersions)
            {
                bool dirExists = coreNugetSubDir.Exists(s => s.Contains(System.IO.Path.GetFileName(toAddSub)));
                if(dirExists == false)
                {
                    Console.WriteLine("Trying to move " + toAddSub + " to: " + coreNugetDir);
                    System.IO.Directory.Move(toAddSub, coreNugetDir+"\\"+System.IO.Path.GetFileName(toAddSub));
                }
            }
        }
    }
}
