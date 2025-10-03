namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class CompletedState : ProjectStateBase
{
    private CompletedState() { }
    public static CompletedState Instance { get; } = new();
    public override string Name => "COMPLETED";

    public override IProjectState Finalize(Project ctx)
    {
        ctx.Log("Final reports prepared; archiving project.");
        return ArchivedState.Instance;
    }
}
