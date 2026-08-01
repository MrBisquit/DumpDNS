using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Octokit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DumpDNS
{
    public static class Ranges
    {
        public enum Family
        {
            IPv4 = 4,
            IPv6 = 6
        }

        public class Source
        {
            public string Label;
            public ConsoleColor Colour;
            public List<Range> Ranges = new();
            public List<(Family, string)> Sources = new();
        }

        public class Range
        {
            public string CIDR;
            public Family Family;
        }

        public static string Dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DumpDNS");
        public static string SourceList = Path.Combine(Dir, ".sources");
        public static string SourcesPath = Path.Combine(Dir, ".data");
        public static string UpdatePath = Path.Combine(Dir, ".source_update");
        public static string SourcePath = "https://github.com/MrBisquit/DumpDNS/raw/refs/heads/master/.sources";

        public static Dictionary<string, Source> Sources = [];
        public static DateTime NeedsUpdating = new();
        public static void LoadSources()
        {
            string sourceText;

            if (File.Exists(SourceList))
            {
                sourceText = File.ReadAllText(SourceList);
            }
            else
            {
                using HttpClient client = new();

                client.Timeout = TimeSpan.FromSeconds(15);
                Task<string> d = client.GetStringAsync(SourcePath);
                d.Wait();
                File.WriteAllText(SourceList, d.Result);
                sourceText = d.Result;
            }

            string[] sourceLines = sourceText.Split('\n');
            string area = "Ungrouped";
            for (int i = 0; i < sourceLines.Length; i++)
            {
                if (sourceLines[i].Length == 0 || sourceLines[i][0] == ';') continue;

                string[] split = sourceLines[i].Split(",");
                if (split.Length == 2)
                {
                    area = split[0];
                    Sources.TryAdd(area, new Source { Label = split[0], Colour = (ConsoleColor)int.Parse(split[1]) });
                }
                else if (split.Length == 3)
                {
                    Sources[area].Sources.Add(new((Family)int.Parse(split[0]), split[1]));
                }
            }
        }
        public static void UpdateRanges()
        {
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);

            Sources.Clear();
            Sources.Add("Ungrouped", new Source { Label = "Ungrouped", Colour = ConsoleColor.Gray });

            // Read (or download) the source list
            // Then parse the source list

            LoadSources();

            // Download the data from the source list
            // Then parse, and save the data for later

            foreach (var source in Sources)
            {
                foreach (var item in source.Value.Sources)
                {
                    using HttpClient client = new();

                    Task<byte[]> d = client.GetByteArrayAsync(item.Item2);
                    d.Wait();
                    string[] lines = Encoding.Default.GetString(d.Result).Split('\n');
                    foreach (var line in lines)
                    {
                        if (line == "") continue;

                        Sources[source.Key].Ranges.Add(new Range
                        {
                            CIDR = line,
                            Family = item.Item1
                        });
                    }
                }
            }

            /*foreach (var source in DefaultSources)
            {
                var data = source;

                foreach (var item in data.Sources)
                {
                    using (HttpClient client = new())
                    {
                        client.Timeout = TimeSpan.FromSeconds(15);
                        Task<byte[]> d = client.GetByteArrayAsync(item.Item2);
                        d.Wait();
                        string[] lines = Encoding.Default.GetString(d.Result).Split('\n');
                        foreach (var line in lines)
                        {
                            data.Ranges.Add(new Range
                            {
                                CIDR = line,
                                Family = item.Item1
                            });
                        }
                    }
                }

                Sources.Add(data);
            }*/

            NeedsUpdating = DateTime.Now.AddDays(1);

            List<string> rangesLines = [];
            foreach (var key in Sources)
            {
                rangesLines.Add(key.Key);
                foreach (var range in key.Value.Ranges)
                {
                    rangesLines.Add($"{(int)range.Family},{range.CIDR}");
                }
            }
            File.WriteAllLines(SourcesPath, rangesLines);
            //File.WriteAllText(SourcesPath, JsonSerializer.Serialize(Sources));
            File.WriteAllText(UpdatePath, JsonSerializer.Serialize(NeedsUpdating));
            List<string> newSourceLines = [];
            foreach (var key in Sources.Keys)
            {
                newSourceLines.Add($"{key},{(int)Sources[key].Colour}");
                foreach (var source in Sources[key].Sources)
                {
                    newSourceLines.Add($"{(int)source.Item1},{source.Item2},source");
                }
            }
            File.WriteAllLines(SourceList, newSourceLines);
        }
        public static void LoadRanges()
        {
            if (!Directory.Exists(Dir) ||
                !File.Exists(SourcesPath) ||
                !File.Exists(UpdatePath) ||
                !File.Exists(SourceList))
            {
                UpdateRanges();
                return;
            }

            LoadSources();

            var sources = File.ReadAllLines(SourcesPath);
            string area = "Ungrouped";
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].Length == 0 || sources[i][0] == ';') continue;

                string[] split = sources[i].Split(",");
                if (split.Length == 1)
                {
                    area = split[0];
                }
                else if (split.Length == 2)
                {
                    Sources[area].Ranges.Add(new Range { Family = (Family)int.Parse(split[0]), CIDR = split[1] });
                }
            }

            //var sources = JsonSerializer.Deserialize<List<Source>>(File.ReadAllText(SourcesPath));
            var update = JsonSerializer.Deserialize<DateTime>(File.ReadAllText(UpdatePath));
            if (sources.Length == 0 || update <= DateTime.Now)
            {
                UpdateRanges();
                return;
            }

            //Sources = sources;
            //NeedsUpdating = update;
        }
        public static (List<Source>, List<string>) Find(IPAddress ip, Family family)
        {
            HashSet<Source> sources = [];
            List<string> CIDRs = [];
            foreach (var source in Sources)
            {
                foreach (var range in source.Value.Ranges)
                {
                    if (range.Family == family)
                    {
                        if (Internal.Utils.IsIPInCIDR(ip, range.CIDR))
                        {
                            sources.Add(source.Value);
                            CIDRs.Add(range.CIDR);
                        }
                    }
                }
            }

            return (sources.ToList(), CIDRs);
        }
    }
}
