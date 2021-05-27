using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Скрипт_создания_папки_ежедневки
{
    class FolderMaker
    {
        private Configurator configuration;
        public FolderMaker(string configPath)
        {
            string jsonString = File.ReadAllText(configPath);
            configuration = JsonSerializer.Deserialize<Configurator>(jsonString);
        }

        private void CreateFolder(string path, string name)
        {
            path = String.Join('\\', path, name);
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

        private void DeleteFolder(string path)
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

        private bool IsFolderEmpty(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] folders = Directory.GetDirectories(path);

            if (files.Length == 0 && folders.Length == 0)
            {
                Console.WriteLine($"В папке {path} нет ничего.");
                return true;
            }
            else
            {
                Console.WriteLine($"В папке {path} есть содержимое.");
                return false;
            }
        }

        private bool IsFolderOld(string directoryPath, string subdirectoryPath, DateTime currentDate)
        {
            // Adding symbol to correctly retrieve date from subdirectory folder
            directoryPath += @"\";
            string folderDate = subdirectoryPath.Replace(directoryPath, "");
            //Console.WriteLine($"Извлечена дата из адреса - {folderDate}");

            // TODO: сделать проверку на неправильные значения (или проверить, как себя программа с ними будет вести)
            // TODO: написать непосредственную проверку разницы во времени в днях.
            return false;
        }

        public void test()
        {
            var currentDateTime = DateTime.Now;
            var newFolderName = currentDateTime.ToString(configuration.FolderMask);

            var folders = Directory.GetDirectories(configuration.Path);

            foreach (var item in folders)
            {
                bool isEmpty = IsFolderEmpty(item);
                bool isOld = IsFolderOld(configuration.Path, item, currentDateTime);
                if (isEmpty && configuration.DeleteEmpty)
                {
                    DeleteFolder(item);
                }
            }
            

            CreateFolder(configuration.Path, newFolderName);

        }
    }
}
