using System;

namespace DumpDNS.Components;

public static class StatusBar
{
    public static void Render(Types.SizeAndPos dimensions)
    {
        (int, int) pos = Console.GetCursorPosition();
        Console.SetCursorPosition(0, dimensions.Height - (BottomBar.Visible ? 2 : 1));
        if(Internal.ITask.OnGoing.Length == 0)
        {
            // Draw an empty bar
            Console.BackgroundColor = ConsoleColor.Blue;
            string text = "No tasks to be completed";
            Console.Write($"{text}{new string(' ', dimensions.Width - text.Length)}");
            Console.ResetColor();
        } else
        {
            
        }
        Console.SetCursorPosition(pos.Item1, pos.Item2);
    }
}