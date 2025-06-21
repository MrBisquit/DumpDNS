using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpDNS.Functionality
{
    public class DumpFile
    {
        public static void CreateDump(string path, string domain)
        {
            FileStream fs = File.Open(path, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(fs);

            writer.WriteLine($"DNS DUMP File for \"{domain}\"");
            writer.WriteLine($"* Created at {DateTime.Now}");
            writer.WriteLine($"* Created with DNSDump/{Version.CurrentVersion} (https://github.com/MrBisquit/DumpDNS)");
            writer.WriteLine($"* Record types included in this dump:");
            for (int i = 0; i < Types.RecordTypes.Length; i++)
            {
                writer.WriteLine($"\t* {Types.DNSRecordTypeDictionary[Types.RecordTypes[i]]}");
            }

            writer.WriteLine($"\n\n{new string('-', 16)} DUMP CONTENT BEGIN {new string('-', 16)}\n\n");

            // Contents
            for (int i = 0; i < Types.RecordTypes.Length; i++)
            {
                writer.WriteLine($"RECORD TYPE: {Types.DNSRecordTypeDictionary[Types.RecordTypes[i]]}");
                writer.Write(Types.IRecords[Types.RecordTypes[i]].Dump()); // Dump it's contents
                writer.WriteLine($"\n{new string('-', 52)}\n");
            }

            // Extra 2 on the end so it matches up,
            // only the first 16 count as the marker so it doesn't really matter
            writer.WriteLine($"\n\n{new string('-', 16)} DUMP CONTENT END {new string('-', 16)}--");

            writer.Flush();
            writer.Close();
            fs.Close();
        }

        public static char[] AllowedChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz.1234567890 \/-_()[]:".ToCharArray();

        public string Domain;
        public StringBuilder Path;
        public int Cursor;
        public (int, int) Dimensions;

        public DumpFile(string domain, (int, int) dimensions)
        {
            Domain = domain;
            Dimensions = dimensions;
            Path = new StringBuilder();
        }

        public bool StartCycle()
        {
            Console.Clear();
            Program.Render += Render;
            Program.Render(this, Dimensions);
            while(true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    // Ctrl+C should already be handled by the shell, so there is no point in creating it here
                    if (key.Key == ConsoleKey.R && key.Modifiers == ConsoleModifiers.Control)
                    {
                        Path.Clear();
                        Cursor = 0;
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        Program.Render -= Render;
                        return false;
                    }
                    else if (key.Key == ConsoleKey.LeftArrow)
                    {
                        if (Cursor == 0) continue;
                        Cursor--;
                    }
                    else if (key.Key == ConsoleKey.RightArrow)
                    {
                        if (Cursor == Path.Length) continue;
                        Cursor++;
                    }
                    else if (key.Key == ConsoleKey.Backspace)
                    {
                        if (Path.Length == 0) continue;
                        Path.Remove(Cursor - 1, 1);
                        Cursor--;
                    }
                    else if (key.Key == ConsoleKey.Delete)
                    {
                        if (Cursor == Path.Length) continue;
                        Path.Remove(Cursor, 1);
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Program.Render -= Render;
                        CreateDump(Path.ToString(), Domain);
                        return true;
                    }
                    else
                    {
                        if (Path.Length == 256 || Path.Length == Dimensions.Item1) continue;
                        if (AllowedChars.Contains(key.KeyChar))
                        {
                            Path.Insert(Cursor, key.KeyChar);
                            Cursor++;
                        }
                    }

                    Render(this, Dimensions);
                }
                Thread.Sleep(10);
            }
        }

        public void Render(object? sender, (int, int) dimensions)
        {
            Dimensions = dimensions;

            Console.CursorTop = 2;
            Console.CursorLeft = 0;

            Console.ResetColor();
            Console.WriteLine("Type the path of where you would like to save the DUMP file");

            if (Path.Length > dimensions.Item1)
            {
                Path.Clear();
                Cursor = 0;
            }

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(new string(' ', dimensions.Item1));
            Console.CursorTop--;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(Path.ToString());
            Console.ResetColor();
            Console.CursorTop--;
            Console.CursorLeft = Cursor;
        }
    }
}