# Participation 3: Daily Operations Dashboard – State Pattern Implementation

## Purpose

This assignment demonstrates how to refactor a conventional **nested-switch** approach for a project lifecycle (and nested tasks) into a robust implementation using the **State Design Pattern**. The goal is to help students understand and apply the State Pattern, clarify how it reduces code complexity, and practice collaborative coding.

**Project lifecycle modeled:**  
`Draft → Submitted → Approved → Active → AtRisk → OnHold → Completed → Archived`

**Nested Task lifecycle:**  
`Pending → Assigned → InProgress → Completed → Verified`

---

## Why Use the State Pattern?

### 1. Reducing Complexity

**Traditional nested-switch code:**
- Multiple states × multiple events = large, unwieldy switch/case blocks.
- All transition logic grouped in one place, making the method hard to read and maintain.
- Each new state or event increases the number of cases exponentially.

**State Pattern approach:**
- Each state is a separate class, handling only its own valid transitions.
- Invalid transitions are handled simply and locally.
- No nested switches: each event is a method call, dispatched through the current state object.
- Adding states or transitions means creating/modifying small, focused classes.

### 2. Improving Maintainability & Scalability

- **Separation of Concerns:** Each state class is responsible for its own behavior; the context (`Project`, `ProjectTask`) delegates to the state.
- **Extensibility:** Adding new states or events is easy—just add a new class or method, and the compiler enforces completeness.
- **Testability:** Each state can be unit tested in isolation.
- **Readability:** State logic is highly organized; bugs are easier to detect and fix.
- **Open/Closed Principle:** System is open for extension (new states/events can be added) but closed for modification (existing code remains stable).

---

## Cyclomatic Complexity Analysis

Cyclomatic complexity (CC) is a measure of a program’s decision logic: the number of independent paths through the code.

| Approach         | Cyclomatic Complexity |
|------------------|----------------------|
| Nested Switch    | **26**               |
| State Pattern    | **12**               |

**Analysis:**
- **Nested-switch version:** High complexity (26) due to many states/events handled in a single large method with multiple nested switches and cases.
- **State Pattern version:** Much lower complexity (12), as each state handles only its own valid transitions in small, focused classes.
- **Result:** The State Pattern reduces complexity by more than half, making the code easier to test, understand, and maintain.

---

## How to Run

### Prerequisites

- .NET SDK (C#) installed  
  [Download .NET](https://dotnet.microsoft.com/download)
- Clone this repository:
  ```shell
  git clone https://github.com/antonionatusch/CS_4213_GroupH_Participation_3.git
  cd CS_4213_GroupH_Participation_3
  ```

### Run the Demo

1. Open a terminal in the repo directory.
2. Build and run:
   ```shell
   dotnet run
   ```
   This executes `Program.cs` and prints example project/task state transitions.

### Output

You will see two scenarios:
- A "happy path" showing all valid transitions, including a KPI breach and recovery.
- An "invalid event" demo showing how the State Pattern safely handles out-of-sequence operations.

---

## Files Overview

- `Project/Project.cs` - Project context class
- `Project/States/*.cs` - Project state classes
- `Tasking/ProjectTask.cs` - Task context class
- `Tasking/States/*.cs` - Task state classes
- `Program.cs` - Demo runner

---

## References

- Section 13.5, State Pattern, in the course textbook
- Assignment specification: [Assignment.md](Assignment.md), [ASSIGNMENT_NOTES.md](ASSIGNMENT_NOTES.md)

---

## Summary

This repo shows how the State Pattern:
- Dramatically reduces code complexity versus nested-switch logic
- Improves maintainability, readability, and scalability
- Provides a clear, extensible model for real-world workflow state management

Ready for group review and class presentation!