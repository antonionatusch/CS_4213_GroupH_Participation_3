namespace CS_4213_GroupH_Participation_3.Tasking.States;

public sealed class InProgressState : ProjectTaskStateBase
{
    private InProgressState() { }
    public static InProgressState Instance { get; } = new();
    public override string Name => "INPROGRESS";

    public override IProjectTaskState Complete(ProjectTask ctx)
    {
        ctx.Log("Work completed; pending verification.");
        return TaskCompletedState.Instance;
    }
}
