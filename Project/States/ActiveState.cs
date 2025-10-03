namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class ActiveState : ProjectStateBase
{
    private ActiveState() { }
    public static ActiveState Instance { get; } = new();
    public override string Name => "ACTIVE";

    public override IProjectState KpiBreach(Project ctx)
    {
        ctx.Log("KPIs breached; marking project At Risk.");
        return AtRiskState.Instance;
    }

    public override IProjectState Finish(Project ctx)
    {
        ctx.Log("All tasks done; moving to Completed.");
        return CompletedState.Instance;
    }
}
