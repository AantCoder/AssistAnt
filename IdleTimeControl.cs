using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AssistAnt
{
    public class IdleTimeControl
    {
        public Action EventIdle;
        private Thread Waiter;
        private DateTime LastActive;
        private int CountActive;
        private int EventAfterMaxMinute = 5;
        private int EventAfterMaxCount = 20;

        internal void Start()
        {
            if (Waiter != null) return;
            Waiter = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(5_000);
                    if (LastActive != DateTime.MinValue 
                        && (    (DateTime.UtcNow - LastActive).TotalSeconds > EventAfterMaxMinute * 60
                            ||  (DateTime.UtcNow - LastActive).TotalSeconds > 30 && CountActive >= EventAfterMaxCount)
                        && EventIdle != null)
                    {
                        EventIdle();
                    }
                }
            });
            Waiter.IsBackground = true;
            LastActive = DateTime.MinValue;
            Waiter.Start();
        }

        internal void Process()
        {
            LastActive = DateTime.UtcNow;
            CountActive++;
        }
    }
}
