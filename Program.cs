using System;
using System.IO;

namespace Скрипт_создания_папки_ежедневки
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths = { @"C:\Users\osipov\Downloads\Filestack" };

            foreach (string path in paths)
            {
                if (Directory.Exists(path))
                {
                    // This path is a directory
                    Console.WriteLine("This path is a directory");
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }

        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        public static void ProcessDirectory(string targetDirectory)
        {
            // Get current date and prepare it to be a foldername
            DateTime currentDateTime = DateTime.Now;
            string newFolderName = currentDateTime.ToString("yyyy-MM-dd");

            // Set flag to check all the array
            bool isSuchFolderHere = false;

            // Search directory
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries) 
            {
                Console.WriteLine(subdirectory);
                if (subdirectory.Contains(newFolderName))
                {
                    Console.WriteLine($"Folder {newFolderName} already exists.");
                    isSuchFolderHere = true;
                    break;
                }
            }

            // Create new one if it does not exists
            if (!isSuchFolderHere)
            {
                string path = String.Join('\\', targetDirectory, newFolderName);
                System.IO.Directory.CreateDirectory(path);
            }
        }
    }
}
