namespace CS_4213_GroupH_Participation_3.Tasking.States;

public abstract class ProjectTaskStateBase : IProjectTaskState
{
    public abstract string Name { get; }

    protected IProjectTaskState Invalid(ProjectTask ctx, string evt)
    {
        ctx.Log($"Event '{evt}' not allowed in state {Name}");
        return this;
    }

    public virtual IProjectTaskState Assign(ProjectTask ctx) => Invalid(ctx, nameof(Assign));
    public virtual IProjectTaskState Start(ProjectTask ctx) => Invalid(ctx, nameof(Start));
    public virtual IProjectTaskState Complete(ProjectTask ctx) => Invalid(ctx, nameof(Complete));
    public virtual IProjectTaskState Verify(ProjectTask ctx) => Invalid(ctx, nameof(Verify));
}
