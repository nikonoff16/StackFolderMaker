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

        private static void CreateFolder(string path, string name)
        {
            path = Path.Join(path.AsSpan(), name.AsSpan());
            try
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Папка {name} была создана в {path}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Попытка создания папки {name} завершилась с ошибкой: {e.Message}");
            }
        }

        private static void DeleteFolder(string path)
        {
            try
            {
                Directory.Delete(path, true);
                Console.WriteLine($"Папка {path} удалена.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Попытка удаления папки {path} завершилась с ошибкой: {e.Message}");
            }
        }
 
        private static bool IsFolderEmpty(string path)
        {
            
            var files = Directory.GetFiles(path);
            var folders = Directory.GetDirectories(path);

            if (files.Length == 0 && folders.Length == 0)
            {
                Console.WriteLine($"В папке {path} нет ничего.");
                return true;
            }

            Console.WriteLine($"В папке {path} есть содержимое.");
            return false;
        }

        private bool IsFolderOld(string directoryPath, string subdirectoryPath, DateTime currentDate)
        {
            directoryPath += Configurator.GetOsType() == "Windows" ? @"\" : @"/";
            
            var folderDate = subdirectoryPath.Replace(directoryPath, "");

            try
            {
                var oldDate = DateTime.Parse(folderDate);
                var delta = (currentDate - oldDate).TotalDays;

                if (delta > _configuration.SavePeriod)
                {
                    Console.WriteLine($"Обнаружена старая папка, {subdirectoryPath}");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Папка {folderDate} не относится к сгенерированным автоматически ({e}.");
                return false;
            }
            return false;
        }

        public void Start()
        {
            var currentDateTime = DateTime.Now;
            var newFolderName = currentDateTime.ToString(_configuration.FolderMask);

            var folders = Directory.GetDirectories(_configuration.Path);
            var isTheDirectoryInPlace = false;

            foreach (var item in folders)
            {
                var alreadyExists = item.Contains(newFolderName);
                isTheDirectoryInPlace = (isTheDirectoryInPlace || alreadyExists);
                var isEmpty = IsFolderEmpty(item);
                var isOld = IsFolderOld(_configuration.Path, item, currentDateTime);

                var isPermittedToDelete = (isEmpty && _configuration.DeleteEmpty && !alreadyExists) | (isOld && _configuration.DeleteOlder);
                if (isPermittedToDelete)
                {
                    DeleteFolder(item);
                }
            }

            if (!isTheDirectoryInPlace)
            {
                CreateFolder(_configuration.Path, newFolderName);
            }
        }
    }
}
