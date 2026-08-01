using System;

namespace DumpDNS;

public static class RenderList
{
    private static readonly Queue<Action<Types.SizeAndPos>> queue = [];

    public static void Add(Action<Types.SizeAndPos> item)
        => queue.Enqueue(item);

    public static void Render(Types.SizeAndPos pos)
    {
        while(queue.Count > 0)
            queue.Dequeue()(pos);
    }
}