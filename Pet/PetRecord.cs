using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistAnt
{
    public class PetRecord
    {
        public string Name { get; set; }
        
        public string Command { get; set; }

        public string Pattern { get; set; }

        public bool Autostart { get; set; }

        public bool CloseWhenFormHidden { get; set; }
    }
}
