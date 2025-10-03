using CS_4213_GroupH_Participation_3.Project;
using CS_4213_GroupH_Participation_3.Project.States;
using CS_4213_GroupH_Participation_3.Tasking;

namespace CS_4213_GroupH_Participation_3;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Demo 1: Happy path with detour ===");
        var proj = new Project.Project("Website Revamp");
        proj.Submit();
        proj.Approve();
        proj.Kickoff();        // assigns nested task if Pending
        proj.SubTask.Start();
        proj.KpiBreach();      // detour to AtRisk
        proj.Pause();          // OnHold
        proj.Resume();         // back to Active
        proj.SubTask.Complete();
        proj.SubTask.Verify();
        proj.Finish();
        proj.Finalize();

        Console.WriteLine();
        Console.WriteLine("=== Demo 2: Guardrails (invalid events are safe) ===");
        var p2 = new Project.Project("Mobile App");
        p2.Kickoff();          // invalid in DRAFT -> logged, no crash
        p2.Reject();           // invalid in DRAFT
        p2.Submit();           // DRAFT -> SUBMITTED
        p2.Finish();           // invalid in SUBMITTED
        p2.Approve();          // SUBMITTED -> APPROVED
        p2.Finalize();         // invalid in APPROVED
    }
}
