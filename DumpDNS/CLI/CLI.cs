using DnsClient;
using DumpDNS.Functionality;
using DumpDNS.Functionality.Records;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.CLI
{
    public static class CLI
    {
        // Command line version of DumpDNS introduced in version 3.0.0
        // Usage: DumpDNS "web.site" (options)
        // DumpDNS --help/-h
        // DumpDNS --version/--ver/-v
        //
        // Replace dashes with forward slashes for weird windows

        public static int Run(string[] args)
        {
            Argument<string> domain = new("Domain")
            {
                Description = "The domain to query."
            };

            Option<string> dump = new("--dump", "-d", "/dump", "/d")
            {
                HelpName = "path",
                Description = "If provided, DumpDNS will dump the results to " +
                "the file path specified instead of the console."
            };

            Option<IPAddress> dns = new("--dns", "/dns")
            {
                HelpName = "ip",
                Description = "Specifies the IP address " +
                "of the DNS server to use instead of the default DNS server on the " +
                "current computer.",
                CustomParser = result => IPAddress.Parse(result.Tokens.Last().Value)
            };

            Option<int> dnsPort = new("--dns-port", "-dp", "/dns-port", "/dp", "--port", "/port")
            {
                HelpName = "port",
                Description = "Specifies the port of the DNS server, this is only " +
                "useful when used when specifying a custom DNS server.\n" +
                "Use \"DumpDNS -?\" for usage information.",
                DefaultValueFactory = _ => { return 53; }
            };

            Option<List<Types.DnsRecordType>> records = new("--records", "-r", "/records", "/r")
            {
                HelpName = "record type",
                Description = $"Specify one or more of the following:\n{string.Join(", ", Types.RecordTypes)}\n" +
                $"Specifying none defaults to all.",
                DefaultValueFactory = _ => [.. Types.RecordTypes],
                AllowMultipleArgumentsPerToken = true
            };

            Option<bool> stats = new("--statistics", "--stats", "-s", "/statistics", "/stats", "/s")
            {
                Description = "Displays statistics.",
                DefaultValueFactory = _ => { return false; }
            };

            Option<bool> colour = new("--colour", "--color", "-c", "/colour", "/color", "/c")
            {
                Description = "Uses colour to highlight useful information.",
                DefaultValueFactory = _ => { return false; }
            };

            RootCommand rootCommand = new("DumpDNS")
            {
                domain,
                dump,
                dns,
                dnsPort,
                records,
                stats,
                colour
            };
            rootCommand.SetAction((result) =>
            {
                if(result.Errors.Count == 0 &&
                    result.GetValue(domain) is string parsedDomain
                ) {
                    var parsedDump = result.GetValue(dump);
                    var parsedDNS = result.GetValue(dns);
                    var parsedDNSPort = result.GetValue(dnsPort);
                    var parsedRecords = result.GetValue(records);
                    var parsedStats = result.GetValue(stats);
                    var parsedColour = result.GetValue(colour);

                    Console.WriteLine($"DumpDNS Looking up \"{parsedDomain}\" on {(parsedDNS == null ? "default" : parsedDNS)}:{parsedDNSPort}");

                    return Dump.StartDump(parsedDomain, parsedDNS, parsedDNSPort, parsedRecords, parsedStats, parsedColour);
                }

                foreach (var error in result.Errors)
                    Console.Error.WriteLine(error.Message);

                return 1;
            });

            for (int i = 0; i < rootCommand.Options.Count; i++)
                if (rootCommand.Options[i] is VersionOption)
                    rootCommand.Options[i].Action = new Version();

            return rootCommand.Parse(args).Invoke();

            /*ParseResult parseResult = rootCommand.Parse(args);
            if(parseResult.Errors.Count == 0)
            {
                return 0;
            }
            if(version.va)
            foreach (ParseError parseError in parseResult.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }
            return 1;*/
        }
    }
}