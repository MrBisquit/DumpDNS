using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.CLI
{
    public static class Results
    {
        public static void DisplayResultTotals(List<Types.DnsRecordType> records, bool colour, CLI.Depth depth)
        {
            Console.WriteLine("Results returned:");
            foreach (var record in records)
            {
                Console.Write($"\t{record}:\t");
                if (colour) Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{Types.IRecords[record].Rows.Count}");
                if (colour) Console.ResetColor();
            }
        }

        public static int ResultsPadding = 15;

        public static void DisplayResults(List<Types.DnsRecordType> records, bool colour, CLI.Depth depth)
        {
            foreach (var item in records)
            {
                var record = Types.IRecords[item];

                for(int i = 0; i < record.Rows.Count; i++)
                {
                    var row = record.Rows[i];
                    Console.WriteLine($"{item} Record\t({i + 1} of {record.Rows.Count})");

                    if(depth == CLI.Depth.Minimal)
                    {
                        for (int j = 0; j < row.Count; j++)
                        {
                            Console.Write($"\t{(j + 1).ToString().PadLeft(2, '0')} ");
                            string header = (record.Headers.Count >= j ? record.Headers[j] : "?") + ":";
                            Console.Write(header.PadRight(ResultsPadding));
                            if (colour) Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"{row[j]}");
                            if (colour) Console.ResetColor();
                        }
                    }
                    else
                        record.Results(depth, colour, i);
                }
            }
        }
    }
}