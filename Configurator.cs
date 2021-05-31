using System;
using System.Collections.Generic;
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
    }
}