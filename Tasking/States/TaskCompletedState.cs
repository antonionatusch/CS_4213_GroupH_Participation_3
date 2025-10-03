namespace CS_4213_GroupH_Participation_3.Tasking.States;

public sealed class TaskCompletedState : ProjectTaskStateBase
{
    private TaskCompletedState() { }
    public static TaskCompletedState Instance { get; } = new();
    public override string Name => "COMPLETED";

    public override IProjectTaskState Verify(ProjectTask ctx)
    {
        ctx.Log("Verified by manager/QA.");
        return VerifiedState.Instance;
    }
}
