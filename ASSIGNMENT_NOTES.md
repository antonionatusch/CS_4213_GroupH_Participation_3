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
The three key components are all present:

1. **Context** (`Project.cs`, `Task.cs`)
   - Maintains reference to current state: `private IProjectState _currentState;`
   - Delegates operations: `public void Submit() => _currentState.Submit(this);`

2. **State Interface** (`IProjectState.cs`, `ITaskState.cs`)
   - Defines operations all states must implement
   - Example: `void Submit(Project context);`

3. **Concrete States** (`ProjectStates.cs`, `TaskStates.cs`)
   - Implement the interface: `public class DraftState : IProjectState`
   - Handle transitions: `public void Submit(Project context) => context.SetState(new SubmittedState());`

### Collaboration Between Classes
```
Project ──delegates──> IProjectState <──implements── DraftState
   │                                   <──implements── ActiveState
   │                                   <──implements── CompletedState
   │                                           │
   └──────calls SetState()──────────────────────┘
```

- Context delegates to current state
- States decide on transitions and call `context.SetState()`
- This creates a clean separation of concerns

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
                                          ↓                    ↓
                                       OnHold              AtRisk/OnHold
                                          ↓                    ↓
                                       Resume              Complete
                                          ↓                    ↓
                                       Active            Completed → Archive → Archived
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
Pending → Assign → Assigned → StartWork → InProgress → Complete → Completed → Verify → Verified
```

### Nested Relationship
- Projects contain Tasks: `public List<Task> Tasks { get; private set; }`
- Both use State Pattern independently
- Demonstrates pattern can be nested/composed

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
**Project.cs** (42 lines)
```csharp
public class Project
{
    private IProjectState _currentState;  // Holds current state
    
    // Delegates all operations
    public void Submit() => _currentState.Submit(this);
    public void Approve() => _currentState.Approve(this);
    // ... etc
}
```

**Task.cs** (39 lines)
```csharp
public class Task
{
    private ITaskState _currentState;  // Holds current state
    
    // Delegates all operations
    public void Assign(string assignee) { 
        AssignedTo = assignee;
        _currentState.Assign(this);
    }
    // ... etc
}
```

#### 2. State Interfaces (Event-Handling Methods)
**IProjectState.cs** (18 lines)
```csharp
public interface IProjectState
{
    void Submit(Project context);
    void Approve(Project context);
    void Kickoff(Project context);
    void MarkAtRisk(Project context);
    void PutOnHold(Project context);
    void Resume(Project context);
    void Complete(Project context);
    void Archive(Project context);
    string GetStateName();
}
```

**ITaskState.cs** (14 lines)
```csharp
public interface ITaskState
{
    void Assign(Task context);
    void StartWork(Task context);
    void Complete(Task context);
    void Verify(Task context);
    string GetStateName();
}
```

#### 3. Concrete State Classes (Implementing Interface)
**ProjectStates.cs** (118 lines - 8 states)
```csharp
public class DraftState : IProjectState
{
    public void Submit(Project context) => context.SetState(new SubmittedState());
    public void Approve(Project context) { /* Invalid */ }
    // ... implements all 9 interface methods
}

public class SubmittedState : IProjectState { /* ... */ }
public class ApprovedState : IProjectState { /* ... */ }
// ... 5 more states
```

**TaskStates.cs** (56 lines - 5 states)
```csharp
public class PendingState : ITaskState
{
    public void Assign(Task context) => context.SetState(new AssignedState());
    public void StartWork(Task context) { /* Invalid */ }
    // ... implements all 5 interface methods
}
// ... 4 more states
```

#### 4. Example Transitions
**Program.cs** (40 lines)
```csharp
Project project = new Project("New Website");
project.Submit();    // Draft → Submitted
project.Approve();   // Submitted → Approved  
project.Kickoff();   // Approved → Active
project.Complete();  // Active → Completed
project.Archive();   // Completed → Archived

Task task = new Task("Build Feature");
task.Assign("Alice");   // Pending → Assigned
task.StartWork();       // Assigned → InProgress
task.Complete();        // InProgress → Completed
task.Verify();          // Completed → Verified
```

### Kept Lightweight ✅
- **No detailed error messages** - Invalid transitions are empty methods with comments
- **No extensive logging** - Just shows state transitions
- **No business logic** - Focus is on demonstrating pattern structure
- **Minimal demo** - Two simple scenarios showing valid transitions
- **~250 total lines** - Enough to show structure, not overengineered

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
                // ... 8 more operations
            }
            break;
        case "Submitted":
            switch(operation) {
                case "Submit": throw new Exception(); break;
                case "Approve": currentState = "Approved"; break;
                // ... 8 more operations
            }
            break;
        // ... 6 MORE states with nested switches
    }
}
```
**Problems:**
- 8 states × 8 operations = 64 cases in ONE method
- Hard to read and understand
- Easy to miss a case
- All logic in one giant file

**With State Pattern:**
```csharp
// Each state is its own class with clear responsibilities
public class DraftState : IProjectState
{
    public void Submit(Project c) => c.SetState(new SubmittedState()); // Valid
    public void Approve(Project c) { /* Invalid */ }  // Clear
    // Each method is simple and focused
}
```
**Benefits:**
- Each state is ~15 lines in its own class
- Clear which operations are valid per state
- Easy to find and modify specific state behavior
- Compiler enforces completeness

#### 2. How Design Improves Maintainability

**Separation of Concerns:**
- Each state class handles ONE state's behavior
- Changes to Draft state don't affect Active state
- Easy to locate bugs (check the specific state class)

**Single Responsibility Principle:**
- `Project.cs` - Delegates operations
- `DraftState.cs` - Handles Draft behavior
- `ActiveState.cs` - Handles Active behavior
- Each class has one clear job

**Real Example:**
Need to add email notification when project is submitted?
```csharp
// Just modify DraftState.cs - that's it!
public class DraftState : IProjectState
{
    public void Submit(Project context)
    {
        context.SetState(new SubmittedState());
        context.NotifyStakeholders();  // NEW - only change one file
    }
}
```

#### 3. How Design Improves Scalability

**Adding a New State:**
1. Create new state class: `CancelledState.cs`
2. Implement interface (compiler enforces all methods)
3. Add transitions in relevant states

**Adding a New Operation:**
1. Add to interface: `void Postpone(Project context);`
2. Compiler forces implementation in ALL states
3. Can't forget any state - type safety!

**Adding Business Logic:**
- Add methods to `Project.cs`
- States can call them: `context.NewMethod()`
- State machine remains unchanged

**Extensibility Example:**
```csharp
// Want different workflow for different project types?
public interface IProjectState { /* ... */ }

// Use different state classes
var agileProject = new Project(new AgileActiveState());
var waterfallProject = new Project(new WaterfallActiveState());
```

### Files to Share with Group
1. `IProjectState.cs` - Interface definition
2. `Project.cs` - Context implementation
3. `ProjectStates.cs` - All concrete states
4. `ITaskState.cs` - Task interface
5. `Task.cs` - Task context
6. `TaskStates.cs` - Task states
7. `Program.cs` - Demo
8. `README.md` - Overview
9. This file - Detailed explanation

---

## Step 5: Presentations

**Requirement:** 4 minutes max, explain design

### Suggested Presentation Structure (4 min)

**1. Introduction (30 sec)**
- "We implemented the State Pattern for project/task lifecycles"
- Show state diagram on screen

**2. Code Walkthrough (2 min)**
- Show `IProjectState` interface
- Show one concrete state (`DraftState`)
- Show `Project` context delegating
- Run demo showing transitions

**3. Benefits Explanation (1 min)**
- Complexity: Each state is isolated vs. giant switch
- Maintainability: Change one state class, not entire method
- Scalability: Add states by creating new classes

**4. Q&A Prep (30 sec)**
Common questions:
- Q: "Why implement all methods in each state?"
  A: "Compiler enforces completeness - can't forget operations"
- Q: "Isn't this more files?"
  A: "Yes, but each file is focused and testable"
- Q: "When would you use this?"
  A: "Any time you have complex state transitions - workflows, game states, etc."

---

## Summary of Implementation

| Component | Files | Lines | Purpose |
|-----------|-------|-------|---------|
| State Interfaces | 2 | 32 | Define operations |
| Context Classes | 2 | 81 | Delegate behavior |
| Concrete States | 2 | 174 | Implement transitions |
| Demo | 1 | 40 | Show it working |
| **Total** | **7** | **~327** | **Complete skeleton** |

### Key Achievements ✅
- Demonstrates State Pattern structure clearly
- Implements all required lifecycles (Project + Task)
- Shows valid transitions
- Keeps code lightweight and focused
- Ready for group consolidation
- Prepared for presentation

### Design Principles Applied
1. **Single Responsibility** - Each state class has one job
2. **Open/Closed** - Open for extension (new states), closed for modification  
3. **Dependency Inversion** - Context depends on interface, not concrete states
4. **Separation of Concerns** - State logic separated from business logic

---

**Ready for Steps 4 & 5!**
