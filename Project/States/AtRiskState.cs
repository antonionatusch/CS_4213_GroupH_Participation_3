namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class AtRiskState : ProjectStateBase
{
    private AtRiskState() { }
    public static AtRiskState Instance { get; } = new();
    public override string Name => "ATRISK";

    public override IProjectState Pause(Project ctx)
    {
        ctx.Log("Paused due to issues; waiting on remediation.");
        return OnHoldState.Instance;
    }

    public override IProjectState Resume(Project ctx)
    {
        ctx.Log("Issues mitigated; resuming Active work.");
        return ActiveState.Instance;
    }

    public override IProjectState Finish(Project ctx)
    {
        ctx.Log("Despite risks, project finished; moving to Completed.");
        return CompletedState.Instance;
    }
}
