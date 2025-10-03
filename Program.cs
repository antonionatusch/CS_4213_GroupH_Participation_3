using CS_4213_GroupH_Participation_3.Project;
using CS_4213_GroupH_Participation_3.Tasking;

namespace CS_4213_GroupH_Participation_3;

public static class Program
{
    public static void Main()
    {
        var proj = new Project.Project("Website Revamp");

        proj.Submit();
        proj.Approve();
        proj.Kickoff();        // assigns nested task
        proj.SubTask.Start();  // work on nested task
        proj.KpiBreach();      // project risk appears
        proj.Pause();          // move to ONHOLD
        proj.Resume();         // back to ACTIVE
        proj.SubTask.Complete();
        proj.SubTask.Verify();
        proj.Finish();
        proj.Finalize();

        // invalid transition example (no-op in ARCHIVED)
        proj.Approve();
    }
}
