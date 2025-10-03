namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class OnHoldState : ProjectStateBase
{
    private OnHoldState() { }
    public static OnHoldState Instance { get; } = new();
    public override string Name => "ONHOLD";

    public override IProjectState Resume(Project ctx)
    {
        ctx.Log("Resuming work from hold.");
        return ActiveState.Instance;
    }
}
