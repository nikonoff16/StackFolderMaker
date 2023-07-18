using System;
using System.IO;
using System.Text.Json;

namespace FolderMakerUtility
{
    internal class FolderMaker
    {
        private readonly Configurator _configuration;
        public FolderMaker(string configPath)
        {
            var jsonString = File.ReadAllText(configPath);
            _configuration = JsonSerializer.Deserialize<Configurator>(jsonString);
        }
        
 
        private static bool IsFolderEmpty(DirectoryInfo path)
        {
            
            var files = path.GetFiles();
            var folders = path.GetDirectories();

            if (files.Length == 0 && folders.Length == 0)
            {
                Console.WriteLine($"В папке {path} нет ничего.");
                return true;
            }

            Console.WriteLine($"В папке {path} есть содержимое.");
            return false;
        }

        private bool IsFolderOld(DirectoryInfo subdirectoryPath, DateTime currentDate)
        {
            try
            {
                var oldDate = DateTime.Parse(subdirectoryPath.Name);
                var delta = (currentDate - oldDate).TotalDays;

                if (delta > _configuration.SavePeriod)
                {
                    Console.WriteLine($"Обнаружена старая папка, {subdirectoryPath}");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Папка {subdirectoryPath.Name} не относится к сгенерированным автоматически ({e}.");
                return false;
            }
            return false;
        }

        public void Start()
        {
            var currentDateTime = DateTime.Now;
            var newFolderName = currentDateTime.ToString(_configuration.FolderMask);

            var rootDirectoryInfo = new DirectoryInfo(_configuration.Path);

            var folders = rootDirectoryInfo.GetDirectories();
            var isTheDirectoryInPlace = false;

            foreach (var item in folders)
            {
                var alreadyExists = item.Name == newFolderName;
                isTheDirectoryInPlace = (isTheDirectoryInPlace || alreadyExists);
                var isEmpty = IsFolderEmpty(item);
                var isOld = IsFolderOld(item, currentDateTime);

                var isPermittedToDelete = (isEmpty && _configuration.DeleteEmpty && !alreadyExists) | (isOld && _configuration.DeleteOlder);
                if (isPermittedToDelete)
                {
                    item.Delete();
                }
            }

            if (!isTheDirectoryInPlace)
            {
                rootDirectoryInfo.CreateSubdirectory(newFolderName);
            }
        }
    }
}
