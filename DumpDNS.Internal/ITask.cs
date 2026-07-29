using System;

namespace DumpDNS.Internal;

public interface ITask
{
    private static Queue<ITask> Tasks = [];
    private static List<OngoingTask> Ongoing = [];

    public static async Task StartQueue()
    {
        while (Tasks.Count != 0)
        {
            if (Ongoing.Count >= Global.ConcurrentTasks) continue;

            var task = Tasks.Dequeue();
            Ongoing.Add(new(task));
        }
    }

    public static void Enqueue(ITask task)
    {
        Tasks.Enqueue(task);
    }

    string TaskName { get; }

    Action<OngoingTask> Action { get; }
}

public class OngoingTask
{
    public ITask ITask;
    public Task Task;
    public CancellationTokenSource CancellationTokenSource;
    public CancellationToken CancellationToken;
    public IProgress<double> Progress;

    public OngoingTask(
        ITask ITask,
        Task Task,
        CancellationTokenSource CancellationTokenSource,
        IProgress<double> Progress
    )
    {
        this.ITask = ITask;
        this.Task = Task;
        this.CancellationTokenSource = CancellationTokenSource;
        CancellationToken = CancellationTokenSource.Token;
        this.Progress = Progress;
    }

    public OngoingTask(ITask task)
    {
        ITask = task;
        CancellationTokenSource = new();
        CancellationToken = CancellationTokenSource.Token;
        Progress = new Progress<double>();
        Task = Task.Factory.StartNew(_ => task.Action(this), this, CancellationToken);
    }
}
