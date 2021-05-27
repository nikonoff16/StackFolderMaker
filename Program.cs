using System;
using System.IO;

namespace Скрипт_создания_папки_ежедневки
{
    class Program {
        static void Main(string[] args) {
            string[] paths = {@"C:\Users\osipov\Downloads\Filestack"};

            FolderMaker test = new FolderMaker(@"config.json");
            test.test();

            //foreach (string path in paths) {
            //    if (Directory.Exists(path)) {
            //        // This path is a directory
            //        Console.WriteLine("This path is a directory");
            //        ProcessDirectory(path);
            //    }
            //    else
            //    {
            //        Console.WriteLine($"{path} is not a valid file or directory.");
            //    }
            //}
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        private static void ProcessDirectory(string targetDirectory) {
            // Get current date and prepare it to be a folder name
            var currentDateTime = DateTime.Now;
            var newFolderName = currentDateTime.ToString("yyyy-MM-dd");

            // Set flag to check all the array
            var isSuchFolderHere = false;

            // Search directory
            var subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (var subdirectory in subdirectoryEntries) {
                Console.WriteLine(subdirectory);
                if (!subdirectory.Contains(newFolderName)) continue;
                Console.WriteLine($"Folder {newFolderName} already exists.");
                isSuchFolderHere = true;
                break;
            }

            // Create new one if it does not exists
            if (isSuchFolderHere) return;
            var path = String.Join('\\', targetDirectory, newFolderName);
            System.IO.Directory.CreateDirectory(path);
        }

        // class FolderChanger {}
        // {
        //     
        // }
    }
}
