namespace CS_4213_GroupH_Participation_3.Project.States;

public sealed class ArchivedState : ProjectStateBase
{
    private ArchivedState() { }
    public static ArchivedState Instance { get; } = new();
    public override string Name => "ARCHIVED";
    // terminal state in this demo
}
