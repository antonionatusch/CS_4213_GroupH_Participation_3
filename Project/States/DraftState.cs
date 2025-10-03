namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class DraftState : ProjectStateBase
{
    private DraftState() { }
    public static DraftState Instance { get; } = new();
    public override string Name => "DRAFT";

    public override IProjectState Submit(Project ctx)
    {
        ctx.Log("Project submitted for approval.");
        return SubmittedState.Instance;
    }
}
