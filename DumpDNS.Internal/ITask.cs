using System;

namespace DumpDNS.Internal;

public interface ITask
{
    private static readonly Queue<ITask> Tasks = [];
    private static readonly List<OngoingTask> Ongoing = [];
    private static readonly List<FinishedTask> Finished = [];

    public static async Task StartQueue()
    {
        while (Tasks.Count != 0)
        {
            if (Ongoing.Count >= Global.ConcurrentTasks) continue;

            var task = Tasks.Dequeue();
            if (task.WaitingFor.Count > 0)
                Tasks.Enqueue(task);
            else
                Ongoing.Add(new(task));
        }
    }

    public static void Enqueue(ITask task)
    {
        Tasks.Enqueue(task);
    }

    public static void Finish(OngoingTask finishedTask)
    {
        finishedTask.Finished = DateTime.Now;
        Ongoing.Remove(finishedTask);
        Finished.Add(new(finishedTask));

        foreach (var task in Tasks)
        {
            task.WaitingFor.Remove(finishedTask.ITask.TaskID);
        }
    }

    string TaskName { get; }

    Guid TaskID { get; }

    // These tasks must be completed before this one can be executed
    HashSet<Guid> WaitingFor { get; set; }

    Action<OngoingTask> Action { get; }
}

public class OngoingTask
{
    public ITask ITask;
    public Task Task;
    public CancellationTokenSource CancellationTokenSource;
    public CancellationToken CancellationToken;
    public IProgress<double> Progress;

    public DateTime Started;
    public DateTime Finished;

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
        Started = DateTime.Now;
        Task = Task.Factory.StartNew(_ => task.Action(this), this, CancellationToken);
    }
}

public class FinishedTask
{
    ITask ITask;

    public DateTime Started;

    public DateTime Finished;

    public FinishedTask(OngoingTask ongoing)
    {
        ITask = ongoing.ITask;
        Started = ongoing.Started;
        Finished = ongoing.Finished;
    }
}
