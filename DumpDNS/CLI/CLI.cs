using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DnsClient;
using DumpDNS.Functionality;
using DumpDNS.Functionality.Records;

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

        public enum Format
        {
            None,
            JSON,           // Dictionary
            JSONList,       // List
            ScriptFriendly, // Output is script-friendly
            Fancy           // Fancy output
        }

        public static Dictionary<Format, string> FormatOptions = new()
        {
            { Format.None,              "Normal console output" },
            { Format.JSON,              "Outputs information as a JSON dictionary" },
            { Format.JSONList,          "Outputs information as a JSON list" },
            { Format.ScriptFriendly,    "Outputs information that is easy to use in scripts" },
            { Format.Fancy,             "Fancy output with colours" }
        };

        public enum Depth
        {
            Minimal,
            Medium,
            Full
        }

        public static string FormatOptionsAsString()
        {
            string str = "";

            for (int i = 0; i < FormatOptions.Count; i++)
            {
                if (i != 0) str += "\n  ";
                else str += "  ";
                str += $"{((Format)i).ToString() + ":",-15} {FormatOptions[(Format)i]}";
            }

            return str;
        }

        public static int Run(string[] args)
        {
            Argument<string> domain = new("Domain")
            {
                Description = "The domain to query."
            };

            Argument<string> action = new("Action")
            {
                Description = "The action to take",
                Arity = ArgumentArity.ZeroOrOne,
                DefaultValueFactory = _ => { return "dump"; }
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

            Option<Format> format = new("--format", "-f", "/format", "/f")
            {
                Description = "Specifies the output format, options:\n" +
                $"{FormatOptionsAsString()}",
                DefaultValueFactory = _ => { return Format.None; }
            };

            Option<Depth> depth = new("--depth", "-dt", "/depth", "/dt")
            {
                Description = "The depth of the information returned, the deeper the information, " +
                "the longer it takes to fetch.",
                DefaultValueFactory = _ => { return Depth.Minimal; }
            };

            RootCommand rootCommand = new("DumpDNS")
            {
                domain,
                action,
                dump,
                dns,
                dnsPort,
                records,
                stats,
                colour,
                //format,
                depth
            };
            rootCommand.SetAction((result) =>
            {
                if (result.Errors.Count == 0 &&
                    result.GetValue(domain) is string parsedDomain
                )
                {
                    var parsedDump = result.GetValue(dump);
                    var parsedDNS = result.GetValue(dns);
                    var parsedDNSPort = result.GetValue(dnsPort);
                    var parsedRecords = result.GetValue(records);
                    var parsedStats = result.GetValue(stats);
                    var parsedColour = result.GetValue(colour);
                    var parsedFormat = result.GetValue(format);
                    var parsedDepth = result.GetValue(depth);

                    if (dump == null) Console.WriteLine($"DumpDNS Looking up \"{parsedDomain}\" on {(parsedDNS == null ? "default" : parsedDNS)}:{parsedDNSPort}");

                    return Dump.StartDump(parsedDomain, parsedDNS, parsedDNSPort, parsedRecords, parsedStats, parsedColour, parsedFormat, parsedDump, parsedDepth);
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
