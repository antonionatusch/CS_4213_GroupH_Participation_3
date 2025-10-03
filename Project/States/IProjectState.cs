namespace CS_4213_GroupH_Participation_3.Project.States;

public interface IProjectState
{
    string Name { get; }

    IProjectState Submit(Project ctx);
    IProjectState Approve(Project ctx);
    IProjectState Reject(Project ctx);
    IProjectState Kickoff(Project ctx);
    IProjectState KpiBreach(Project ctx);
    IProjectState Pause(Project ctx);
    IProjectState Resume(Project ctx);
    IProjectState Finish(Project ctx);
    IProjectState Finalize(Project ctx);
}
