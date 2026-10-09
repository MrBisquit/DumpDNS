using System;

namespace DumpDNS.Internal;

public interface ITask
{
    private static readonly Queue<ITask> tasks = [];
    private static readonly List<OngoingTask> ongoing = [];
    private static readonly List<FinishedTask> finished = [];

    public static Queue<ITask> Tasks { get { return tasks; } }
    public static OngoingTask[] OnGoing { get { return [.. ongoing]; } }
    public static FinishedTask[] Finished { get { return [.. finished]; } }

    public static async Task StartQueue()
    {
        while (tasks.Count != 0 || ongoing.Count != 0)
        {
            while (ongoing.Count < Global.ConcurrentTasks)
            {
                ITask? next = null;

                foreach (var task in tasks)
                {
                    if (task.WaitingFor.Count == 0)
                    {
                        next = task;
                        break;
                    }
                    else
                    {
                        tasks.Enqueue(tasks.Dequeue());
                    }
                }

                if (next != null)
                {
                    tasks.Dequeue();
                    ongoing.Add(new(next));
                    continue;
                }

                break;
            }

            if (ongoing.Count >= Global.ConcurrentTasks) continue;

            if (ongoing.Count > 0)
            {
                await Task.WhenAny(ongoing.Select(x => x.Task));
            }
            else
            {
                // Tasks do exist, but none of them can be run
                //
                // - Try to requeue everything, then try until it has
                //   either made a full loop, or the issue is resolved
                // - Maybe try to determine if all the tasks depend on
                //   one another
            }
        }
    }

    public static void Enqueue(ITask task)
    {
        tasks.Enqueue(task);
    }

    public static void Finish(OngoingTask finishedTask)
    {
        finishedTask.Finished = DateTime.Now;
        ongoing.Remove(finishedTask);
        finished.Add(new(finishedTask));

        foreach (var task in tasks)
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
        Task = Task.Factory.StartNew(_ => { try { task.Action(this); } finally { ITask.Finish(this); } }, this, CancellationToken);
    }
}

public class FinishedTask
{
    public ITask ITask;

    public DateTime Started;

    public DateTime Finished;

    public FinishedTask(OngoingTask ongoing)
    {
        ITask = ongoing.ITask;
        Started = ongoing.Started;
        Finished = ongoing.Finished;
    }
}
