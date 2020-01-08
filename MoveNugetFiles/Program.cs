
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Security.Principal;
    using System.Threading;

namespace MoveNugetFiles
{
    class Program
    {
        public static int consoleLineNumber = 0;
        public static int filesMoved = 0;
        public static int directoriesAffected = 0;
        static void Main(string[] args)
        {
            bool test = false;
            if (test == false)
            {
                while (true)
                {
                    //Intro on App Init
                    ConsoleWrite("-------------------------------------------------------", ref consoleLineNumber);
                    ConsoleWrite("------ Welcome to the Spee-D Nuget Crossover App ------", ref consoleLineNumber);
                    ConsoleWrite("-------------------------------------------------------", ref consoleLineNumber);
                    ConsoleWrite("\n\nPress Any Key to Continue. . .", ref consoleLineNumber);
                    Console.ReadKey();
                    ConsoleClear(ref consoleLineNumber);

                    //Destination Folder Instructions
                    ConsoleWrite("Please provide the source directory that you would like to use for the .nuget folders...\n", ref consoleLineNumber);
                    ConsoleWrite(@"NOTE: If your nuget directories are located in C:\Users\<USER>\.nuget (i.e. C:\Users\<USER>\.nuget\Newtonsoft)", ref consoleLineNumber);
                    ConsoleWrite("\tPlease only reference up to the '\\.nuget' folder unless you wish to copy a specific nuget package.\n\n", ref consoleLineNumber);
                    string source = Console.ReadLine();
                    bool access = false;
                    while (true)
                    {
                        access = CheckPerms(source);
                        if (access == true)
                        {
                            break;
                        }
                        else
                        {
                            ConsoleClear(ref consoleLineNumber);
                            ConsoleWrite("ERROR: The program/user trying to access " + source + " does not have sufficient permissions to complete the process.\nPlease be sure you are selecting the correct source directory or check the permissions with your administrator.", ref consoleLineNumber);
                            ConsoleWrite("\tPlease enter the source directory you'd like to use for the program...\n\n", ref consoleLineNumber);
                            source = Console.ReadLine();
                        }
                    }
                    
                    ConsoleClear(ref consoleLineNumber);
                    ConsoleWrite("Please provide the destination directory that you would like to use for the .nuget packages...\n", ref consoleLineNumber);
                    ConsoleWrite(@"NOTE: Unless you are moving specific nuget packages, this directory should be the '\.nuget' directory", ref consoleLineNumber);
                    ConsoleWrite("\n\n", ref consoleLineNumber);
                    string destination = Console.ReadLine();
                    access = false;
                    while (true)
                    {
                        access = CheckPerms(destination);
                        if (access == true)
                        {
                            break;
                        }
                        else
                        {
                            ConsoleClear(ref consoleLineNumber);
                            ConsoleWrite("ERROR: The program/user trying to access " + destination + " does not have sufficient permissions to complete the process.\nPlease be sure you are selecting the correct destination directory or check the permissions with your administrator.", ref consoleLineNumber);
                            ConsoleWrite("\tPlease enter the source directory you'd like to use for the program...\n\n", ref consoleLineNumber);
                            destination = Console.ReadLine();
                        }
                    }
                    ConsoleClear(ref consoleLineNumber);


                    ConsoleWrite("Would you like the program to overwrite duplicate files?", ref consoleLineNumber);
                    ConsoleWrite("\tThis could potentially have an impact on existing processes if the libraries are user created.\n\tUser created libraries run the risk of being version controlled imporoperly and may operate differently on various machines.", ref consoleLineNumber);
                    ConsoleWrite("\n[Y] Overwrite\n[N] Do Not Overwrite", ref consoleLineNumber);
                    ConsoleKeyInfo userChoice = Console.ReadKey();
                    bool overwrite = false;
                    while (true)
                    {
                        if (userChoice.KeyChar == 'y' || userChoice.KeyChar == 'Y')
                        {
                            ConsoleClear(ref consoleLineNumber);
                            overwrite = true;
                            //DirectoryCopy(source, destination, true, overwrite);
                            break;
                        }
                        else if (userChoice.KeyChar == 'n' || userChoice.KeyChar == 'N')
                        {
                            ConsoleClear(ref consoleLineNumber);
                            overwrite = false;
                            //DirectoryCopy(source, destination, true, overwrite);
                            break;
                        }
                        else
                        {
                            ConsoleClear(ref consoleLineNumber);
                            ConsoleWrite("Please select a valid option for the overwrite parameter...", ref consoleLineNumber);
                            ConsoleWrite("\n[Y] Overwrite\n[N] Do Not Overwrite", ref consoleLineNumber);
                        }
                    }
                    ConsoleClear(ref consoleLineNumber);
                    ConsoleWrite("Please confirm the following parameters...\n\t>Source Directory: " + source + "\n\t>Destination Directory: " + destination + "\n\tOverwrite: " + overwrite.ToString().ToUpper(), ref consoleLineNumber);
                    ConsoleWrite("\n[Enter] Continue\n[Backspace] Reset\n[Escape] Exit", ref consoleLineNumber);
                    userChoice = Console.ReadKey();
                    ConsoleClear(ref consoleLineNumber);
                    while (true)
                    {
                        if (userChoice.Key == ConsoleKey.Enter || userChoice.Key == ConsoleKey.Escape || userChoice.Key == ConsoleKey.Backspace)
                        {
                            break;
                        }
                        else
                        {
                            for(int i = 0; i<3; ++i)
                            {
                                ConsoleClear(ref consoleLineNumber);
                                ConsoleWrite("Invalid User Choice", ref consoleLineNumber);
                                Thread.Sleep(250);
                                Console.Write(". ");
                                Thread.Sleep(250);
                                Console.Write(". ");
                                Thread.Sleep(250);
                                Console.Write(". ");
                            }

                            ConsoleClear(ref consoleLineNumber);
                            ConsoleWrite("Please confirm the following parameters...\n\t>Source Directory: " + source + "\n\t>Destination Directory: " + destination + "\n\tOverwrite: " + overwrite.ToString().ToUpper(), ref consoleLineNumber);
                            ConsoleWrite("\n[Enter] Continue\n[Backspace] Reset\n[Escape] Exit", ref consoleLineNumber);
                        }
                    }
                    if (userChoice.Key == ConsoleKey.Enter)
                    {
                        //Run Function to copy all necessary files based on parameters given
                        DirectoryCopy(source, destination, true, overwrite);
                        //Post-Run Logging
                        ConsoleWrite("\n\n", ref consoleLineNumber);
                        string endmsg = "";
                        for (int i = 0; i < Console.WindowWidth; ++i)
                        {
                            endmsg += "-";
                        };
                        ConsoleWrite(endmsg, ref consoleLineNumber);
                        int setTopReport = consoleLineNumber;
                        ConsoleWrite("\nThe app has finished migrating source files to the desired destination...\n------- SOURCE: " + source + "\n------- DESTINATION: " + destination + "\n------- OVERWRITE: " + overwrite.ToString().ToUpper() + "\n------------ Directories Effected: " + directoriesAffected.ToString() + "\n------------ Files Moved: " + filesMoved.ToString() + "\n", ref consoleLineNumber);
                        Console.WriteLine(endmsg);
                        Console.SetWindowPosition(0,setTopReport);
                        break;
                    }
                    else if (userChoice.Key == ConsoleKey.Backspace)
                    {
                        ConsoleClear(ref consoleLineNumber);
                    }
                    else if (
                        userChoice.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else
                    {

                    }

                }
                
            }
            else
            {
                string source = @"C:\Users\van02084\Desktop\source - Copy";
                string destination = @"C:\Users\van02084\Desktop\destination - Copy";
                Boolean overwrite = true;
                //DirectoryCopy(sourceDirectory<string>, destinationDirectory<string>, copySubDirectories<boolean>, overwrite<boolean>)
                DirectoryCopy(source, destination, true, overwrite);
                //Post-Run Logging
                ConsoleWrite("\n\n", ref consoleLineNumber);
                string endmsg = "";
                for (int i = 0; i < Console.WindowWidth; ++i)
                {
                    endmsg += "-";
                };
                ConsoleWrite(endmsg, ref consoleLineNumber);
                int setTopReport = consoleLineNumber;
                ConsoleWrite("\nThe app has finished migrating source files to the desired destination...\n\n\tSOURCE: " + source + "\n\tDESTINATION: " + destination + "\n\tOVERWRITE: " + overwrite.ToString().ToUpper() + "\n\nDirectories Effected: " + directoriesAffected.ToString() + "\nFiles Moved: " + filesMoved.ToString(), ref consoleLineNumber);
                Console.WriteLine(endmsg);
                Console.SetWindowPosition(0, setTopReport);

            }
            
            
            
        }
        public static void ConsoleClear(ref int consoleLineNumber)
        {
            Console.Clear();
            consoleLineNumber = 0;
        }
        public static void ConsoleWrite(string msg, ref int consoleLineNumber)
        {
            Console.WriteLine(msg);
            consoleLineNumber = msg.Split('\n').Length + 1;
        }
        private static bool CheckPerms(string dirPath)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool overwrite)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            List<FileInfo> files = new List<FileInfo>();
            files = dir.GetFiles().ToList<FileInfo>();
            files.RemoveAll(item => item.FullName.EndsWith(".xaml"));
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                try
                {
                    if (overwrite == true)
                    {
                        file.CopyTo(temppath, true);
                    }
                    else
                    {
                        file.CopyTo(temppath, false);
                    }
                    filesMoved += 1;
                }
                catch (Exception e)
                {
                    if ((!e.Message.Contains("Access to the path") && !e.Message.Contains("is denied")) && !e.Message.Contains("already exists"))
                    {
                        ConsoleWrite("Unable to copy file: " + file.Name + " to directory: " + destDirName + "\n\t" + e.Message.ToString() + "\n\t[OVERWRITE BOOLEAN: " + overwrite.ToString().ToUpper() + "]", ref consoleLineNumber);
                    }
                }
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    try
                    {
                        if (overwrite == true)
                        {
                            DirectoryCopy(subdir.FullName, temppath, copySubDirs, true);
                        }
                        else
                        {
                            DirectoryCopy(subdir.FullName, temppath, copySubDirs, false);
                        }
                        directoriesAffected += 1;
                    }
                    catch(Exception e)
                    {
                        if((!e.Message.Contains("Access") && !e.Message.Contains("denied")) && !e.Message.Contains("already exists"))
                        {
                            ConsoleWrite("Unable to copy subdirectory " + subdir.Name + " to directory: " + destDirName + "\n\t" + e.Message.ToString() + "\n\t[OVERWRITE BOOLEAN: " + overwrite.ToString().ToUpper() + "]", ref consoleLineNumber);
                        }
                    }
                }
            }
        }


        //PREVIOUSLY WORKING CODE >> ONLY MOVED FILES, NOT COPY
        //{
        //    Console.WriteLine("PLEASE TYPE THE PATH TO NUGET PACKAGES FOLDER YOU WOULD LIKE TO IMPORT AND HIT ENTER");
        //    string addPackages = Console.ReadLine();
        //    Console.Clear();
        //    if (!System.IO.Directory.Exists(addPackages))
        //    {
        //        Console.WriteLine("The directory path is inaccesible or does not exist.");
        //        Console.ReadKey();
        //        Console.WriteLine("The folder will default to \"\\\\vaprd-rp-01\\Depts\\Business Automation\\Robotics\\Nuget Packages Repository\\Packages To Add\" -- Press any key to continue");
        //        Console.ReadKey();
        //        Console.Clear();
        //        addPackages = @"\\vaprd-rp-01\Depts\Business Automation\Robotics\Nuget Packages Repository\Packages To Add";
        //    }
        //
        //    string corePackages = @"\\vaprd-rp-01\Depts\Business Automation\Robotics\Nuget Packages Repository\nuget";
        //    List<string> toAddFiles = new List<string>();
        //    List<string> nugetPackages = new List<string>();
        //    var tAddFiles = System.IO.Directory.GetDirectories(addPackages);
        //    toAddFiles.AddRange(tAddFiles);
        //    var currentFiles = System.IO.Directory.GetDirectories(corePackages);
        //    nugetPackages.AddRange(currentFiles);
        //
        //    foreach(string dir in toAddFiles)
        //    {
        //        Console.WriteLine("Checking if " + System.IO.Path.GetFileName(dir) + " exists in " + corePackages);
        //        bool dirExists = nugetPackages.Any(s => System.IO.Path.GetFileName(s) == System.IO.Path.GetFileName(dir));
        //        if (dirExists == false)
        //        {
        //            Console.WriteLine(dirExists.ToString());
        //        }
        //        if (dirExists == true)
        //        {
        //            AddOnlyNeededVersions(dir, nugetPackages);
        //        }
        //        else
        //        {
        //            Console.WriteLine("Trying to move " + dir + " to: " + corePackages);
        //            System.IO.Directory.Move(dir, corePackages+"\\"+System.IO.Path.GetFileName(dir));
        //        }
        //        try
        //        {
        //            if (System.IO.Directory.Exists(dir))
        //            {
        //                System.IO.Directory.Delete(dir, true);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Error deleting " + dir + " -- " + e.Message.ToString());
        //        }
        //    }
        //}
        //
        //public static void AddOnlyNeededVersions(string dir, List<string> nugetPackages)
        //{
        //    var coreNugetDir = nugetPackages.First(s => s.Contains(System.IO.Path.GetFileName(dir)));
        //    string[] cND = System.IO.Directory.GetDirectories(coreNugetDir);
        //    List<string> coreNugetSubDir = new List<string>();
        //    coreNugetSubDir.AddRange(cND);
        //
        //    string[] tAddVersions = System.IO.Directory.GetDirectories(dir);
        //    List<string> toAddVersions = new List<string>();
        //    toAddVersions.AddRange(tAddVersions);
        //    foreach(string toAddSub in toAddVersions)
        //    {
        //        bool dirExists = coreNugetSubDir.Exists(s => s.Contains(System.IO.Path.GetFileName(toAddSub)));
        //        if (dirExists == false)
        //        {
        //            ConsoleWrite("Trying to copy " + toAddSub + " to: " + coreNugetDir);
        //            System.IO.Directory.Move(toAddSub, coreNugetDir+"\\"+System.IO.Path.GetFileName(toAddSub));
        //        }
        //    }
        //}
    }
}
