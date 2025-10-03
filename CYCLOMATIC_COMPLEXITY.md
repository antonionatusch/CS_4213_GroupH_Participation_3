# Cyclomatic Complexity Analysis

This document explains and compares the cyclomatic complexity of two approaches to managing project state transitions for the Daily Operations Dashboard:

- **Original Nested-Switch Approach**
- **Refactored State Pattern Approach**

---

## 1. Cyclomatic Complexity of the Nested-Switch Code

Cyclomatic complexity (CC) is calculated as:

> **CC = Number of decision points + 1**

Decision points include each `case` in a `switch`, each `default`, and each outer `switch` branch.

**Counting the decision points:**

- Outer `switch (state)`: **7 branches** (excluding ARCHIVED, which is terminal and not handled)
- For each state, nested `switch (event)`:
    - DRAFT: 1 case + 1 default = 2
    - SUBMITTED: 2 cases + 1 default = 3
    - APPROVED: 1 case + 1 default = 2
    - ACTIVE: 2 cases + 1 default = 3
    - ATRISK: 3 cases + 1 default = 4
    - ONHOLD: 1 case + 1 default = 2
    - COMPLETED: 1 case + 1 default = 2

Total decision points:
- Outer switch: 7
- Inner switches: 2 + 3 + 2 + 3 + 4 + 2 + 2 = 18

**Sum: 7 (outer) + 18 (inner) = 25 decision points**

Cyclomatic complexity formula (branch count + 1):

> **CC = 25 + 1 = 26**

---

## 2. Cyclomatic Complexity of the State Pattern Code (Refactored)

In the State Pattern, each state is encapsulated in its own class. The code is distributed, and only valid transitions are implemented in each state.

**Counting transitions (decision points):**

- DRAFT: submit (1)
- SUBMITTED: approve, reject (2)
- APPROVED: kickoff (1)
- ACTIVE: kpiBreach, finish (2)
- ATRISK: pause, resume, finish (3)
- ONHOLD: resume (1)
- COMPLETED: finalize (1)
- ARCHIVED: terminal (0)

**Sum: 1 + 2 + 1 + 2 + 3 + 1 + 1 = 11 decision points**

Cyclomatic complexity formula:

> **CC = 11 + 1 = 12**

---

## Summary Table

| Approach            | Cyclomatic Complexity |
|---------------------|----------------------|
| Nested Switch       | **26**               |
| State Pattern       | **12**               |

---

## Conclusion

- The **State Pattern** reduces cyclomatic complexity by distributing logic and eliminating nested switches.
- This leads to code that is much easier to maintain, test, and extend.
- Cyclomatic complexity is **less than half** in the State Pattern compared to the original nested-switch code, making the implementation safer and more robust.

---