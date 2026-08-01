using System;

namespace DumpDNS.Components;

public static class BottomBar
{
    private static bool visible = true;
    public static bool Visible { get { return visible; } }

    public static List<(ConsoleKey, ConsoleModifiers[], string)> Values { get; } = [];

    public static void Render(Types.SizeAndPos dimensions)
    {
        (int, int) pos = Console.GetCursorPosition();
        Console.SetCursorPosition(dimensions.X, dimensions.Y);
        if(Internal.ITask.OnGoing.Length == 0)
        {
            // Draw an empty bar
            
        } else
        {
            
        }
        Console.SetCursorPosition(pos.Item1, pos.Item2);
    }
}