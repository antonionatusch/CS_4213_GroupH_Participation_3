namespace CS_4213_GroupH_Participation_3.Tasking.States;

public interface IProjectTaskState
{
    string Name { get; }

    IProjectTaskState Assign(ProjectTask ctx);   // Pending -> Assigned
    IProjectTaskState Start(ProjectTask ctx);    // Assigned -> InProgress
    IProjectTaskState Complete(ProjectTask ctx); // InProgress -> Completed
    IProjectTaskState Verify(ProjectTask ctx);   // Completed -> Verified
}
