using System;
using System.IO;
using System.Text.Json;

namespace FolderMakerUtility
{
    internal class FolderMaker
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

        private static void CreateFolder(string path, string name)
        {
            /*
             * Создание подпапки с именем name в папке path.
             */
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

        private static bool IsFolderEmpty(string path)
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
            
            directoryPath += Configurator.GetOsType() == "Windows" ? @"\" : @"/";
            
            var folderDate = subdirectoryPath.Replace(directoryPath, "");

            try
            {
                var oldDate = DateTime.Parse(folderDate);
                var delta = (currentDate - oldDate).TotalDays;

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

        public void Start()
        {
            /*
             * Входная точка в приложение, единственный публичный метод класса.
             */
            var currentDateTime = DateTime.Now;
            var newFolderName = currentDateTime.ToString(configuration.FolderMask);

            var folders = Directory.GetDirectories(configuration.Path);
            var isTheDirectoryInPlace = false;

            foreach (var item in folders)
            {
                var alreadyExists = item.Contains(newFolderName);
                isTheDirectoryInPlace = (isTheDirectoryInPlace || alreadyExists);
                var isEmpty = IsFolderEmpty(item);
                var isOld = IsFolderOld(configuration.Path, item, currentDateTime);

                var isPermittedToDelete = (isEmpty && configuration.DeleteEmpty && !alreadyExists) | (isOld && configuration.DeleteOlder);
                if (isPermittedToDelete)
                {
                    DeleteFolder(item);
                }
            }

            if (!isTheDirectoryInPlace)
            {
                CreateFolder(configuration.Path, newFolderName);
            }
        }
    }
}
