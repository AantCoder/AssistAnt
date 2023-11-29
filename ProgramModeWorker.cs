using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssistAnt
{
    internal class ProgramModeWorker
    {
        public void Main(string[] args)
        {
            if (args.Length < 2
                || args[0].ToLower() != "worker"
                || !int.TryParse(args[1], out var code)
                ) return;

            var server = new ExecutionMap(code);

            while (true)
            {
                server.Server().Wait();
            }
        }

    }
}
