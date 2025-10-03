# Participation 3: State Pattern Implementation
## CS 4213 - Software Design Principles

---

## Step 1: Individual Assignment - Review Specification ✅

**Requirement:** Read Section 13.5 on the State Pattern in textbook and understand:
- The Intent of the State Pattern
- The Structure (Context, State Interface, Concrete States)
- The Collaboration between classes

**How This Implementation Addresses It:**

### Intent of State Pattern
> "Allow an object to alter its behavior when its internal state changes. The object will appear to change its class."

✅ **Demonstrated in our code:**
- `Project` object behaves differently based on its current state
- Calling `project.Submit()` does different things depending on if state is Draft vs. Active
- From outside, it looks like the Project "changes its class" as it transitions

### Structure
The key components are all present:

1. **Context** (`Project.cs`, `ProjectTask.cs`)
   - Maintains reference to current state: `public IProjectState State { get; private set; }`
   - Delegates operations via helper: `public void Submit() => Transition(s => s.Submit(this));`
   - Transition helper validates and updates state

2. **State Interface** (`IProjectState.cs`, `IProjectTaskState.cs`)
   - Defines operations all states must implement
   - Example: `IProjectState Submit(Project ctx);` - returns next state

3. **Base State Classes** (`ProjectStateBase.cs`, `ProjectTaskStateBase.cs`)
   - Provide default implementation for all interface methods
   - Invalid operations return `this` (current state) with logging

4. **Concrete States** (Individual files per state)
   - Extend base class: `public sealed class DraftState : ProjectStateBase`
   - Use Singleton pattern: `public static DraftState Instance { get; } = new();`
   - Handle valid transitions by returning next state: `return SubmittedState.Instance;`

### Collaboration Between Classes
```
Project ──delegates via Transition()──> IProjectState <──extends── ProjectStateBase <──sealed── DraftState
   │                                                                                   <──sealed── ActiveState
   │                                                                                   <──sealed── CompletedState
   │                                                                                          │
   └──────receives next state and updates State property <─────────────────────────returns──┘
```

- Context delegates to current state via `Transition()` helper
- States decide on transitions and **return** the next state (or `this` if invalid)
- Context validates the returned state and updates if different
- This creates a clean separation of concerns with type-safe transitions

---

## Step 2: Group Work ✅

**Requirement:** Focus on:
- Project lifecycle (Draft → Submitted → Approved → Active → AtRisk → OnHold → Completed → Archived)
- Nested Task lifecycle (Pending → Assigned → InProgress → Completed → Verified)

**Implementation Status:**

### ✅ Project Lifecycle - All 8 States Implemented
Located in `ProjectStates.cs`:
1. `DraftState` - Initial state, can submit
2. `SubmittedState` - Awaiting approval
3. `ApprovedState` - Approved, can kickoff
4. `ActiveState` - Work in progress
5. `AtRiskState` - Needs attention
6. `OnHoldState` - Temporarily paused
7. `CompletedState` - Work finished
8. `ArchivedState` - Final state

**Valid Transitions:**
```
Draft → Submit → Submitted → Approve → Approved → Kickoff → Active
         ↑           ↓                                         ↓
         └─────── Reject                             KpiBreach/Finish
                                                          ↓       ↓
                                                      AtRisk  Completed
                                                        ↓           ↓
                                              Pause/Resume/Finish  Finalize
                                                        ↓           ↓
                                                     OnHold     Archived
                                                        ↓
                                                      Resume
                                                        ↓
                                                      Active
```

### ✅ Task Lifecycle - All 5 States Implemented
Located in `TaskStates.cs`:
1. `PendingState` - Not yet assigned
2. `AssignedState` - Has an assignee
3. `InProgressState` - Being worked on
4. `TaskCompletedState` - Done, awaiting verification
5. `VerifiedState` - Fully complete

**Valid Transitions:**
```
Pending → Assign → Assigned → Start → InProgress → Complete → Completed → Verify → Verified
```

### Nested Relationship
- Projects contain a SubTask: `public ProjectTask SubTask { get; }`
- Both use State Pattern independently with their own state machines
- Demonstrates pattern can be nested/composed
- Project can interact with SubTask state (e.g., ApprovedState assigns the SubTask during Kickoff)

**Ready for group discussion.**

---

## Step 3: Individual Work - Design Code Skeleton ✅

**Requirement:** Create lightweight skeleton showing:
- ✅ A Context class that delegates state-specific behavior
- ✅ A State interface defining event-handling methods
- ✅ Several Concrete State classes implementing the interface
- ✅ Example transitions
- ✅ Keep it lightweight

### What Was Created

#### 1. Context Classes (Delegates Behavior)
**Project.cs** (41 lines)
```csharp
public class Project
{
    public string Title { get; }
    public IProjectState State { get; private set; }
    public ProjectTask SubTask { get; }
    
    public Project(string title)
    {
        Title = title;
        State = DraftState.Instance;
        SubTask = new ProjectTask($"{title} - Initial Task");
        Log($"Created Project in state: {State.Name}");
    }
    
    // Delegates all operations via Transition helper
    public void Submit() => Transition(s => s.Submit(this));
    public void Approve() => Transition(s => s.Approve(this));
    public void Finish() => Transition(s => s.Finish(this));
    // ... 9 total operations
    
    private void Transition(Func<IProjectState, IProjectState> f)
    {
        var next = f(State);
        if (!ReferenceEquals(next, State))
        {
            Log($"Transition: {State.Name} -> {next.Name}");
            State = next!;
        }
    }
}
```

**ProjectTask.cs** (33 lines)
```csharp
public class ProjectTask
{
    public string Title { get; }
    public IProjectTaskState State { get; private set; }
    
    public ProjectTask(string title)
    {
        Title = title;
        State = PendingState.Instance;
        Log($"Created Task in state: {State.Name}");
    }
    
    // Delegates all operations via Transition helper
    public void Assign() => Transition(s => s.Assign(this));
    public void Start() => Transition(s => s.Start(this));
    // ... 4 total operations
    
    private void Transition(Func<IProjectTaskState, IProjectTaskState> f)
    {
        var next = f(State);
        if (!ReferenceEquals(next, State))
        {
            Log($"Transition: {State.Name} -> {next.Name}");
            State = next!;
        }
    }
}
```

#### 2. State Interfaces (Event-Handling Methods)
**IProjectState.cs** (16 lines)
```csharp
public interface IProjectState
{
    string Name { get; }  // Property to get state name
    
    // All methods return next state (or self if invalid)
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
```

**IProjectTaskState.cs** (11 lines)
```csharp
public interface IProjectTaskState
{
    string Name { get; }  // Property to get state name
    
    // All methods return next state (or self if invalid)
    IProjectTaskState Assign(ProjectTask ctx);
    IProjectTaskState Start(ProjectTask ctx);
    IProjectTaskState Complete(ProjectTask ctx);
    IProjectTaskState Verify(ProjectTask ctx);
}
```

#### 3. Base Classes and Concrete State Classes

**ProjectStateBase.cs** (22 lines)
```csharp
public abstract class ProjectStateBase : IProjectState
{
    public abstract string Name { get; }
    
    // Default implementation returns self and logs invalid operation
    protected IProjectState Invalid(Project ctx, string evt)
    {
        ctx.Log($"Event '{evt}' not allowed in state {Name}");
        return this;
    }
    
    // All interface methods default to Invalid
    public virtual IProjectState Submit(Project ctx) => Invalid(ctx, nameof(Submit));
    public virtual IProjectState Approve(Project ctx) => Invalid(ctx, nameof(Approve));
    // ... etc (9 total methods)
}
```

**Individual Project State Files** (8 state classes, each in own file):
- **DraftState.cs** - Implements Submit() → SubmittedState
- **SubmittedState.cs** - Implements Approve() → ApprovedState, Reject() → DraftState
- **ApprovedState.cs** - Implements Kickoff() → ActiveState (also assigns SubTask)
- **ActiveState.cs** - Implements KpiBreach() → AtRiskState, Finish() → CompletedState
- **AtRiskState.cs** - Implements Pause() → OnHoldState, Resume() → ActiveState, Finish() → CompletedState
- **OnHoldState.cs** - Implements Resume() → ActiveState
- **CompletedState.cs** - Implements Finalize() → ArchivedState
- **ArchivedState.cs** - Terminal state (no transitions)

All use **Singleton pattern**:
```csharp
public sealed class DraftState : ProjectStateBase
{
    private DraftState() { }
    public static DraftState Instance { get; } = new();
    public override string Name => "DRAFT";
    
    public override IProjectState Submit(Project ctx)
    {
        ctx.Log("Project submitted for approval.");
        return SubmittedState.Instance;  // Returns next state
    }
}
```

**ProjectTaskStateBase.cs** (17 lines)
```csharp
public abstract class ProjectTaskStateBase : IProjectTaskState
{
    public abstract string Name { get; }
    
    protected IProjectTaskState Invalid(ProjectTask ctx, string evt)
    {
        ctx.Log($"Event '{evt}' not allowed in state {Name}");
        return this;
    }
    
    // All interface methods default to Invalid
    public virtual IProjectTaskState Assign(ProjectTask ctx) => Invalid(ctx, nameof(Assign));
    // ... 4 total methods
}
```

**Individual Task State Files** (5 state classes, each in own file):
- **PendingState.cs** - Implements Assign() → AssignedState
- **AssignedState.cs** - Implements Start() → InProgressState
- **InProgressState.cs** - Implements Complete() → TaskCompletedState
- **TaskCompletedState.cs** - Implements Verify() → VerifiedState
- **VerifiedState.cs** - Terminal state (no transitions)

Example:
```csharp
public sealed class PendingState : ProjectTaskStateBase
{
    private PendingState() { }
    public static PendingState Instance { get; } = new();
    public override string Name => "PENDING";
    
    public override IProjectTaskState Assign(ProjectTask ctx)
    {
        ctx.Log("Assigned to department staff.");
        return AssignedState.Instance;  // Returns next state
    }
}
```

#### 4. Example Transitions
**Program.cs** (35 lines) - Two demonstration scenarios:
```csharp
// Demo 1: Happy path with detour
var proj = new Project("Website Revamp");
proj.Submit();         // DRAFT → SUBMITTED
proj.Approve();        // SUBMITTED → APPROVED  
proj.Kickoff();        // APPROVED → ACTIVE (also assigns SubTask)
proj.SubTask.Start();  // Task: PENDING → ASSIGNED → INPROGRESS
proj.KpiBreach();      // ACTIVE → ATRISK
proj.Pause();          // ATRISK → ONHOLD
proj.Resume();         // ONHOLD → ACTIVE
proj.SubTask.Complete();  // Task: INPROGRESS → COMPLETED
proj.SubTask.Verify();    // Task: COMPLETED → VERIFIED
proj.Finish();         // ACTIVE → COMPLETED
proj.Finalize();       // COMPLETED → ARCHIVED

// Demo 2: Guardrails (invalid events are logged, not crashed)
var p2 = new Project("Mobile App");
p2.Kickoff();   // Invalid in DRAFT → logged, stays in DRAFT
p2.Submit();    // Valid: DRAFT → SUBMITTED
p2.Finish();    // Invalid in SUBMITTED → logged
```

### Key Implementation Features ✅
- **Base classes with default behavior** - Invalid operations are handled gracefully via base class
- **Singleton pattern for states** - Each state has a single `Instance` property (memory efficient)
- **Type-safe transitions** - States return next state; context validates and updates
- **Clear separation** - Each state in its own file for maintainability
- **Minimal logging** - Just shows state transitions and key events
- **Nested lifecycle demo** - Project contains SubTask demonstrating pattern composition

---

## Step 4: Group Consolidation (Before Next Class)

**Requirement:** 
- Share skeleton with group members
- Consolidate into one group version
- Be ready to explain how it reduces complexity and improves maintainability/scalability

### Talking Points for Group Discussion

#### 1. How Design Reduces Complexity vs. Nested Switch

**Without State Pattern:**
```csharp
public void HandleOperation(string operation) {
    switch(currentState) {
        case "Draft":
            switch(operation) {
                case "Submit": currentState = "Submitted"; break;
                case "Approve": throw new Exception(); break;
                case "Kickoff": throw new Exception(); break;
                // ... 6 more operations
            }
            break;
        case "Submitted":
            switch(operation) {
                case "Submit": throw new Exception(); break;
                case "Approve": currentState = "Approved"; break;
                // ... 6 more operations
            }
            break;
        // ... 6 MORE states with nested switches
    }
}
```
**Problems:**
- 8 states × 9 operations = 72 cases in ONE method
- Hard to read and understand
- Easy to miss a case
- All logic in one giant file

**With State Pattern:**
```csharp
// Base class provides default behavior (inherited by all states)
public abstract class ProjectStateBase : IProjectState
{
    protected IProjectState Invalid(Project ctx, string evt)
    {
        ctx.Log($"Event '{evt}' not allowed in state {Name}");
        return this;
    }
    public virtual IProjectState Approve(Project ctx) => Invalid(ctx, nameof(Approve));
}

// Each state overrides only valid transitions
public sealed class DraftState : ProjectStateBase
{
    public static DraftState Instance { get; } = new();
    public override string Name => "DRAFT";
    
    public override IProjectState Submit(Project ctx)
    {
        ctx.Log("Project submitted for approval.");
        return SubmittedState.Instance;  // Valid transition
    }
    // Approve, Kickoff, etc. handled by base class (Invalid)
}
```
**Benefits:**
- Each state is ~10-20 lines in its own file
- Clear which operations are valid per state
- Easy to find and modify specific state behavior
- Compiler enforces completeness
- Base class eliminates boilerplate for invalid transitions
- Singleton pattern ensures one instance per state (memory efficient)

#### 2. How Design Improves Maintainability

**Separation of Concerns:**
- Each state class handles ONE state's behavior in its own file
- Changes to DraftState don't affect ActiveState
- Easy to locate bugs (check the specific state class file)

**Single Responsibility Principle:**
- `Project.cs` - Manages context and delegates operations
- `ProjectStateBase.cs` - Provides default behavior for all states
- `DraftState.cs` - Handles Draft-specific behavior
- `ActiveState.cs` - Handles Active-specific behavior
- Each class has one clear job

**Real Example:**
Need to add email notification when project is submitted?
```csharp
// Just modify DraftState.cs - that's it!
public sealed class DraftState : ProjectStateBase
{
    public override IProjectState Submit(Project ctx)
    {
        ctx.NotifyStakeholders();  // NEW - only change one file
        ctx.Log("Project submitted for approval.");
        return SubmittedState.Instance;
    }
}
```

#### 3. How Design Improves Scalability

**Adding a New State:**
1. Create new state file: `CancelledState.cs`
2. Extend base class: `public sealed class CancelledState : ProjectStateBase`
3. Override only valid transitions (base class handles invalid ones)
4. Add Singleton: `public static CancelledState Instance { get; } = new();`
5. Add transitions in relevant states to return new state

**Adding a New Operation:**
1. Add to interface: `IProjectState Postpone(Project ctx);`
2. Add default implementation to base: `public virtual IProjectState Postpone(Project ctx) => Invalid(ctx, nameof(Postpone));`
3. Add to context: `public void Postpone() => Transition(s => s.Postpone(this));`
4. Override in specific states where valid
5. Compiler enforces completeness - type safety!

**Adding Business Logic:**
- Add methods to `Project.cs` context
- States can call them: `ctx.NewMethod()`
- State machine structure remains unchanged

**Extensibility Example:**
```csharp
// Want different workflows? Create alternate state classes
public sealed class AgileActiveState : ProjectStateBase
{
    public static AgileActiveState Instance { get; } = new();
    public override string Name => "AGILE_ACTIVE";
    // Implement agile-specific transitions
}

// States naturally compose with inheritance and interfaces
```

### Files to Share with Group

**Project State Machine:**
1. `Project/States/IProjectState.cs` - Interface definition
2. `Project/States/ProjectStateBase.cs` - Base class with default behavior
3. `Project/States/DraftState.cs` - Draft state implementation
4. `Project/States/SubmittedState.cs` - Submitted state
5. `Project/States/ApprovedState.cs` - Approved state
6. `Project/States/ActiveState.cs` - Active state
7. `Project/States/AtRiskState.cs` - At Risk state
8. `Project/States/OnHoldState.cs` - On Hold state
9. `Project/States/CompletedState.cs` - Completed state
10. `Project/States/ArchivedState.cs` - Archived state
11. `Project/Project.cs` - Context implementation

**Task State Machine:**
12. `Tasking/IProjectTaskState.cs` - Interface definition
13. `Tasking/ProjectTaskStateBase.cs` - Base class with default behavior
14. `Tasking/States/PendingState.cs` - Pending state
15. `Tasking/States/AssignedState.cs` - Assigned state
16. `Tasking/States/InProgressState.cs` - In Progress state
17. `Tasking/States/TaskCompletedState.cs` - Task Completed state
18. `Tasking/States/VerifiedState.cs` - Verified state
19. `Tasking/ProjectTask.cs` - Task context

**Demo and Documentation:**
20. `Program.cs` - Demonstration scenarios
21. `README.md` - Overview
22. This file (`ASSIGNMENT_NOTES.md`) - Detailed explanation

---

## Step 5: Presentations

**Requirement:** 4 minutes max, explain design

### Suggested Presentation Structure (4 min)

**1. Introduction (30 sec)**
- "We implemented the State Pattern for project/task lifecycles"
- Show state diagram on screen

**2. Code Walkthrough (2 min)**
- Show `IProjectState` interface (returns next state)
- Show `ProjectStateBase` (default behavior for invalid operations)
- Show one concrete state (`DraftState` with Singleton pattern)
- Show `Project` context with Transition helper
- Run demo showing valid transitions and graceful handling of invalid operations

**3. Benefits Explanation (1 min)**
- Complexity: Each state is isolated in own file vs. giant nested switch
- Maintainability: Change one state file, not entire method; base class eliminates boilerplate
- Scalability: Add states by creating new classes; add operations by extending interface
- Type Safety: Compiler enforces all states implement all operations
- Memory Efficiency: Singleton pattern ensures one instance per state

**4. Q&A Prep (30 sec)**
Common questions:
- Q: "Why return state instead of void?"
  A: "Type-safe transitions - context validates returned state before updating"
- Q: "Why use Singleton pattern for states?"
  A: "States are stateless - only one instance needed (memory efficient)"
- Q: "Why base classes?"
  A: "Eliminates boilerplate - only override valid transitions, base handles invalid"
- Q: "Isn't this more files?"
  A: "Yes, but each file is focused, testable, and easy to find"
- Q: "When would you use this?"
  A: "Any time you have complex state transitions - workflows, game states, UI flows, etc."

---

## Summary of Implementation

| Component | Files | Purpose |
|-----------|-------|---------|
| State Interfaces | 2 | Define operations (IProjectState, IProjectTaskState) |
| Base State Classes | 2 | Provide default behavior for all states |
| Context Classes | 2 | Delegate behavior and manage state transitions |
| Project States | 8 | Implement Project lifecycle (Draft→Archived) |
| Task States | 5 | Implement Task lifecycle (Pending→Verified) |
| Demo | 1 | Show it working with two scenarios |
| **Total** | **20** | **Complete implementation with nested lifecycles** |

### Key Achievements ✅
- Demonstrates State Pattern structure clearly with modern C# features
- Implements all required lifecycles (Project: 8 states + Task: 5 states)
- Shows valid transitions with type-safe state returns
- Base classes eliminate boilerplate while maintaining type safety
- Singleton pattern for memory efficiency
- Each state in own file for maintainability
- Nested lifecycle demonstrates pattern composition
- Graceful handling of invalid operations (logged, not crashed)
- Ready for group consolidation and presentation

### Design Principles Applied
1. **Single Responsibility** - Each state class has one job; each in its own file
2. **Open/Closed** - Open for extension (new states via inheritance), closed for modification
3. **Dependency Inversion** - Context depends on interface, not concrete states
4. **Separation of Concerns** - State logic separated from business logic
5. **DRY (Don't Repeat Yourself)** - Base classes provide default behavior, eliminating boilerplate
6. **Type Safety** - Compiler enforces all states implement all operations; return types validate transitions

---

**Ready for Steps 4 & 5!**
