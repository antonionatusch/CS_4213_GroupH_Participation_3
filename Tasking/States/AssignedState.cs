namespace CS_4213_GroupH_Participation_3.Tasking.States;

public sealed class AssignedState : ProjectTaskStateBase
{
    private AssignedState() { }
    public static AssignedState Instance { get; } = new();
    public override string Name => "ASSIGNED";

    public override IProjectTaskState Start(ProjectTask ctx)
    {
        ctx.Log("Work started.");
        return InProgressState.Instance;
    }
}
