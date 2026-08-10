using System;

namespace DumpDNS.Components;

public static class StatusBar
{
    public static void Render(Types.SizeAndPos dimensions)
    {
        (int, int) pos = Console.GetCursorPosition();
        Console.SetCursorPosition(0, dimensions.Height - (BottomBar.Visible ? 2 : 1));
        if (Internal.ITask.OnGoing.Length == 0)
        {
            // Draw an empty bar
            Console.BackgroundColor = ConsoleColor.Blue;
            string text = $"No tasks to be completed ({Internal.ITask.OnGoing.Length} ongoing, and {Internal.ITask.Finished.Length} finished)";
            Console.Write($"{text}{new string(' ', dimensions.Width - text.Length)}");
            Console.ResetColor();
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Green;
            string text = "Tasks running";
            Console.Write($"{text}{new string(' ', dimensions.Width - text.Length)}");
            Console.ResetColor();
        }
        Console.SetCursorPosition(pos.Item1, pos.Item2);
    }

    static DateTime last = DateTime.Now;

    public static void CheckRender()
    {
        if ((DateTime.Now - last).TotalMilliseconds >= 100)
            RenderList.Add(Render);

        if (Internal.ITask.OnGoing.Length > 0)
        {
            if ((DateTime.Now - last).TotalMilliseconds >= 100)
                RenderList.Add(Render);
            last = DateTime.Now;
        }
    }
}
