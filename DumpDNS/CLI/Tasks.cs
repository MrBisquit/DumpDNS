using System;
using DumpDNS.Internal;

namespace DumpDNS.CLI;

public static class Tasks
{
    public static void HandleTasks()
    {
        ITask.StartQueue().Wait();
    }
}