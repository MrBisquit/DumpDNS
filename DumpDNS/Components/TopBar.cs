using System;
using DumpDNS.Internal;

namespace DumpDNS.Components;

public static class TopBar
{
    public static void Render(Types.SizeAndPos dimensions)
    {
        (int, int) pos = Console.GetCursorPosition();
        Console.SetCursorPosition(0, 0);
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Write($"{new string(' ', dimensions.Width)}");
        string text = "DumpDNS";
        Console.SetCursorPosition((dimensions.Width / 2) - (text.Length / 2), 0);
        Console.Write(text);

        if(Global.VersionString.Length > 0)
        {
            Console.SetCursorPosition(dimensions.Width - Global.VersionString.Length, 0);
            if(Global.VersionUnreleased) Console.BackgroundColor = ConsoleColor.Magenta;
            if(Global.VersionAvailable) Console.BackgroundColor = ConsoleColor.Green;
            else Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write(Global.VersionString);
        }

        Console.ResetColor();
        Console.SetCursorPosition(pos.Item1, pos.Item2);
    }
}