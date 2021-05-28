using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Скрипт_создания_папки_ежедневки
{
    class FolderMaker
    {
        /**
         * Класс выполняет работу с папками согласно записям в конфигурационном файле.
         * Входная точка для вызывающего класс кода - метод 'start'.
         */
        private Configurator configuration;
        public FolderMaker(string configPath)
        {
            /**
             * Считываем конфигурацию из файла 'config.json'
             */
            string jsonString = File.ReadAllText(configPath);
            configuration = JsonSerializer.Deserialize<Configurator>(jsonString);
        }

        private void CreateFolder(string path, string name)
        {
            /**
             * Создание подпапки с именем name в папке path.
             */
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
            /**
             * Безвозвратное рекурсивное удаление папки, переданной в параметре path
             */
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
            /**
             * Проверка наличия файлов и папок в указанной директории
             */
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
            /**
             * Проверка того, считается ли папка подлежащей удалению из-за срока давности, указанного в параметере SavePeriod
             * Исхожу из соображения, что имя папки создается с именем даты в неком формате.
             */
            directoryPath += @"\";
            string folderDate = subdirectoryPath.Replace(directoryPath, "");

            try
            {
                var oldDate = DateTime.Parse(folderDate);
                double delta = (currentDate - oldDate).TotalDays;

                if (delta > configuration.SavePeriod)
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

        public void start()
        {
            /**
             * Входная точка в приложение, единственный публичный метод класса.
             */
            var currentDateTime = DateTime.Now;
            var newFolderName = currentDateTime.ToString(configuration.FolderMask);

            var folders = Directory.GetDirectories(configuration.Path);

            foreach (var item in folders)
            {
                bool isEmpty = IsFolderEmpty(item);
                bool isOld = IsFolderOld(configuration.Path, item, currentDateTime);

                bool isPermittedToDelete = (isEmpty && configuration.DeleteEmpty) | (isOld && configuration.DeleteOlder);
                if (isPermittedToDelete)
                {
                    DeleteFolder(item);
                }
            }
            CreateFolder(configuration.Path, newFolderName);
        }
    }
}
