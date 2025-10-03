using CS_4213_GroupH_Participation_3.Project.States;
using CS_4213_GroupH_Participation_3.Tasking;

namespace CS_4213_GroupH_Participation_3.Project;

public class Project
{
    public string Title { get; }
    public IProjectState State { get; private set; }
    public ProjectTask SubTask { get; }

    public Project(string title)
    {
        Title = title;
        State = DraftState.Instance;
        SubTask = new ProjectTask($"{title} - Initial Task");
        Log($"Created Project in state: {State.Name}");
    }

    public void Submit() => Transition(s => s.Submit(this));
    public void Approve() => Transition(s => s.Approve(this));
    public void Reject() => Transition(s => s.Reject(this));
    public void Kickoff() => Transition(s => s.Kickoff(this));
    public void KpiBreach() => Transition(s => s.KpiBreach(this));
    public void Pause() => Transition(s => s.Pause(this));
    public void Resume() => Transition(s => s.Resume(this));
    public void Finish() => Transition(s => s.Finish(this));
    public void Finalize() => Transition(s => s.Finalize(this));

    private void Transition(Func<IProjectState, IProjectState> f)
    {
        var next = f(State);
        if (!ReferenceEquals(next, State))
        {
            Log($"Transition: {State.Name} -> {next.Name}");
            State = next!;
        }
    }

    public void Log(string msg) => Console.WriteLine($"[Project:{Title}] {msg}");
}
