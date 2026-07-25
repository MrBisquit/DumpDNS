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
                Description = "The domain to query"
            };

            Option<string> dump = new("--dump", "-d", "/dump", "/d")
            {
                HelpName = "path",
                Description = "If provided, DumpDNS will dump the results to " +
                "the file path specified instead of the console."
            };

            Option<IPAddress?> dns = new("--dns", "/dns")
            {
                HelpName = "ip",
                Description = "Specifies the IP address (and optionally the port) " +
                "of the DNS server to use instead of the default DNS server on the " +
                "current computer.",
                DefaultValueFactory = _ => default
            };

            Option<List<Types.DnsRecordType>> records = new("--records", "-r", "/records", "/r")
            {
                HelpName = "record type",
                Description = $"Specify one or more of the following:\n{string.Join(", ", Types.RecordTypes)}\n" +
                $"Specifying none defaults to all",
                DefaultValueFactory = _ => [.. Types.RecordTypes],
                AllowMultipleArgumentsPerToken = true
            };

            RootCommand rootCommand = new("DumpDNS")
            {
                domain,
                dump,
                dns,
                records
            };
            rootCommand.SetAction((result) =>
            {
                if(result.Errors.Count == 0 &&
                    result.GetValue(domain) is string parsedDomain
                ) {
                    var parsedDump = result.GetValue(dump);
                    var parsedDNS = result.GetValue(dns);
                    var parsedRecords = result.GetValue(records);

                    Console.WriteLine($"Running DumpDNS on domain \"{parsedDomain}\" (With {(parsedDNS == null ? "default" : parsedDNS)} DNS server)");

                    return 0;
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