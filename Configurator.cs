using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace FolderMakerUtility
{
    internal class Configurator
    {
        /**
         * Класс для чтения в него информации из json-конфига. Запускается из конструктора класса FolderMaker
         * Требует наличия файла конфигурации 'config.json' в корне приложения.
         */
        public string Path { get; set; }

        public bool DeleteEmpty { get; set; }
        public string FolderMask { get; set; }
        public bool DeleteOlder { get; set; }
        public double SavePeriod { get; set; }

        public static string GetOsType()
        {
            var result = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                result = "Windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                result = "Linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                result = "MacOS";
            }
            
            return result;
        }
    }
}