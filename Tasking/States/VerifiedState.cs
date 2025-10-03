namespace CS_4213_GroupH_Participation_3.Tasking.States;

public sealed class VerifiedState : ProjectTaskStateBase
{
    private VerifiedState() { }
    public static VerifiedState Instance { get; } = new();
    public override string Name => "VERIFIED";
    // terminal in this demo
}
