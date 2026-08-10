using System;

namespace DumpDNS.CLI;

public static class Display
{
    public static void DisplayError(string error, bool fatal = false)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(fatal ? "FATAL" : "ERROR");
        Console.ForegroundColor = previous;
        Console.WriteLine($": {error}");
    }
}