namespace CS_4213_GroupH_Participation_3.Tasking.States;

public sealed class PendingState : ProjectTaskStateBase
{
    private PendingState() { }
    public static PendingState Instance { get; } = new();
    public override string Name => "PENDING";

    public override IProjectTaskState Assign(ProjectTask ctx)
    {
        ctx.Log("Assigned to department staff.");
        return AssignedState.Instance;
    }
}
