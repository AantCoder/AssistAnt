using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AssistAnt.ActualCopy
{
    public class ACPoint
    {
        public bool Active { get; set; }

        /// <summary>
        /// Полный путь к папке источнику без завершающего символа \
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Полный путь к папке назначения без завершающего символа \
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Исключения.
        /// Если содержит \ в любом месте, то строка ищется в имени пути относительно Source (строка пути начинается с символа \),
        /// иначе поиск по имени файла с поддержкой *.
        /// Например для Source = "C:\Source":
        /// "\Hlam" - игнорирует папку C:\Source\Hlam
        /// "Hlam\" - игнорирует все папки Hlam на любой вложенности
        /// "*.zip" - игнорирует файлы *.zip на любой вложенности
        /// "Lol" - игнорирует файлы Lol без расширения на любой вложенности
        /// </summary>
        public List<string> Excludes { get; set; }

        [XmlIgnore]
        public string ExcludesText 
        { 
            get => string.Join(Environment.NewLine, Excludes);
            set => Excludes = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Статус после последней синхронизации, информационное значения для пользователя
        /// </summary>
        public string LastLogMessage { get; set; }
    }
}
