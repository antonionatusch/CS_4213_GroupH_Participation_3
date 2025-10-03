namespace CS_4213_GroupH_Participation_3.Project.States;

public abstract class ProjectStateBase : IProjectState
{
    public abstract string Name { get; }

    protected IProjectState Invalid(Project ctx, string evt)
    {
        ctx.Log($"Event '{evt}' not allowed in state {Name}");
        return this;
    }

    public virtual IProjectState Submit(Project ctx) => Invalid(ctx, nameof(Submit));
    public virtual IProjectState Approve(Project ctx) => Invalid(ctx, nameof(Approve));
    public virtual IProjectState Reject(Project ctx) => Invalid(ctx, nameof(Reject));
    public virtual IProjectState Kickoff(Project ctx) => Invalid(ctx, nameof(Kickoff));
    public virtual IProjectState KpiBreach(Project ctx) => Invalid(ctx, nameof(KpiBreach));
    public virtual IProjectState Pause(Project ctx) => Invalid(ctx, nameof(Pause));
    public virtual IProjectState Resume(Project ctx) => Invalid(ctx, nameof(Resume));
    public virtual IProjectState Finish(Project ctx) => Invalid(ctx, nameof(Finish));
    public virtual IProjectState Finalize(Project ctx) => Invalid(ctx, nameof(Finalize));
}
