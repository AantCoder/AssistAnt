using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AssistAnt.ActualCopy
{
    public class ACSettingStorage
    {
        public string LastRun { get; set; }

        [XmlIgnore]
        public DateTime LastRunDate
        {
            get
            {
                if (string.IsNullOrEmpty(LastRun)
                    || !DateTime.TryParseExact(LastRun, "u", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                    ) return DateTime.MinValue;
                return date;
            }
            set
            {
                if (value == DateTime.MinValue) LastRun = null;
                else LastRun = value.ToString("u", CultureInfo.InvariantCulture);
            }
        }

        public List<ACPoint> Points { get; set; }
    }
}
