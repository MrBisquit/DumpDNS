using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.CLI
{
    public sealed class Version : SynchronousCommandLineAction
    {
        public override int Invoke(ParseResult parseResult)
        {
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var runtimeVersion = Environment.Version;

            Console.WriteLine($"Assembly Version:\t{appVersion}");
            Console.WriteLine($".NET Runtime Version:\t{runtimeVersion}");
            Console.Write($"DumpDNS Version:\t...");

            Functionality.Version.StartCheck().Wait();
            Console.WriteLine($"\rDumpDNS Version:\t{Functionality.Version.VersionString}");
            if (Functionality.Version.IsNewVersionAvailable)
            {
                Console.Write($"\nThere is a new version available ({Functionality.Version.VersionString}), " +
                    "see https://github.com/MrBisquit/DumpDNS/releases/latest/ to download it.");
            }
            else
            {
                Console.WriteLine($"\nUp to date! (Current: {Functionality.Version.CurrentVersion} Available: {Functionality.Version.VersionString})");
            }

            return 0;
        }
    }
}
