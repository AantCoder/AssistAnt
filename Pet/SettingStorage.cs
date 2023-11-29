using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistAnt.Pet
{
    public class SettingStorage
    {
        public int WinPosX { get; set; }
        public int WinPosY { get; set; }
        public bool WinTopMost { get; set; }
        public bool WinHideOnStart { get; set; }

        public List<PetRecord> Consoles { get; set; } = new List<PetRecord>();

        public string Instructions { get; set; } = @"

Укажите в Pattern шаблон вывода информации из последней строки консоли:
ping - для команд пинг
percent - для вывода процентов
text - для вывода всей последней строки


Если указать свойство CloseWhenFormHidden, то консоль будет работать, только пока форма открыта.

Если указать свойство Autostart, то консоль будет автоматически запускаться при старте приложения или при открытии формы.

";

    }
}
