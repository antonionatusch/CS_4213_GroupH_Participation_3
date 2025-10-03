using CS_4213_GroupH_Participation_3.Tasking.States;

namespace CS_4213_GroupH_Participation_3.Tasking;

public class ProjectTask
{
    public string Title { get; }
    public IProjectTaskState State { get; private set; }

    public ProjectTask(string title)
    {
        Title = title;
        State = PendingState.Instance;
        Log($"Created Task in state: {State.Name}");
    }

    public void Assign() => Transition(s => s.Assign(this));
    public void Start() => Transition(s => s.Start(this));
    public void Complete() => Transition(s => s.Complete(this));
    public void Verify() => Transition(s => s.Verify(this));

    private void Transition(Func<IProjectTaskState, IProjectTaskState> f)
    {
        var next = f(State);
        if (!ReferenceEquals(next, State))
        {
            Log($"Transition: {State.Name} -> {next.Name}");
            State = next!;
        }
    }

    public void Log(string msg) => Console.WriteLine($"  [Task:{Title}] {msg}");
}
