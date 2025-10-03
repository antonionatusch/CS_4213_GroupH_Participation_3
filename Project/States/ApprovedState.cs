using CS_4213_GroupH_Participation_3.Tasking.States;

namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class ApprovedState : ProjectStateBase
{
    private ApprovedState() { }
    public static ApprovedState Instance { get; } = new();
    public override string Name => "APPROVED";

    public override IProjectState Kickoff(Project ctx)
    {
        ctx.Log("Kickoff held; moving to Active. Distributing initial tasks.");
        if (ctx.SubTask.State is PendingState)
            ctx.SubTask.Assign();
        return ActiveState.Instance;
    }
}
