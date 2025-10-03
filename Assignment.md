# Participation 3: Implementing the Daily Operations Dashboard with the State Pattern 

In this activity, you will work individually and in groups to Move beyond the conventional nested-switch approach and apply the State Pattern to implement part of the Daily Operations Dashboard.
Objective:

- Gain hands-on familiarity with State design pattern.

- Learn how to implement the pattern effectively.

- Practice working as a group to build a collective understanding and share insights.

## Instructions:
### Step 1 - Individual Assignment

Review the Specification (Preparation).
- Read Section 13.5 on the State Pattern in your textbook
- Pay attention to:  
    - The Intent of the State Pattern.
    - The Structure (Context, State Interface, Concrete States).
    - The Collaboration between classes.

### Step 2 - Group Work

Each group will focus on

- Project lifecycle (Draft → Submitted → Approved → Active → AtRisk → OnHold → Completed → Archived)
- include a nested Task lifecycle (Pending → Assigned → InProgress → Completed → Verified) 

### Step 3 - Individual Work

Design Your Code Skeleton

Use any programming language you are comfortable with (Java, Python, C#, etc.).  should show:
- A Context class (e.g., Project) that delegates state-specific behavior.
- A State interface (e.g., ProjectState) defining event-handling methods.
- Several Concrete State classes (e.g., DraftState, SubmittedState, ActiveState) implementing the interface.
- Example transitions (e.g., approve(), kickoff(), finish()).
Note: Keep it lightweight: no need to fully implement all methods, just enough to demonstrate the structure.

### Step 4 - Group Consolidation (before next class)

Share your skeleton with your group members.
Consolidate into one group version to present in the next session.
- Be ready to explain:
- How your design reduces complexity compared to the nested-switch version.
- How it improves maintainability and scalability.

### Step 5 - Presentations

Presentations will be split across class sessions.

Each group will have 4 minutes max.
