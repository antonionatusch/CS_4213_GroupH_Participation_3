namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class SubmittedState : ProjectStateBase
{
    private SubmittedState() { }
    public static SubmittedState Instance { get; } = new();
    public override string Name => "SUBMITTED";

    public override IProjectState Approve(Project ctx)
    {
        ctx.Log("Management approved the project.");
        return ApprovedState.Instance;
    }

    public override IProjectState Reject(Project ctx)
    {
        ctx.Log("Rejected; returning to Draft for rework.");
        return DraftState.Instance;
    }
}
