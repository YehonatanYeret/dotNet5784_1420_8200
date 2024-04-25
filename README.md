# dotNet5784_1420_8200

## Project Management in C\# using WPF

## !By Maor Noy && Yehonatan Yeret!

**Project Summary:**
The project is a **C# .NET application** designed for **project management**. It serves two main user roles:

1. **Manager Side:**
   - **Task Assignment:** Managers can assign tasks to engineers and establish complex task dependencies.
   - **Schedule Management:** The system manages the project schedule, ensuring tasks are aligned with the project timeline.

2. **Engineer Side:**
   - **Task Execution:** Engineers can initiate, advance, and report on tasks assigned to them.
   - **Progress Tracking:** The system allows for monitoring the status and progression of tasks.

**Key Features:**
- **Gantt Chart Visualization:** All tasks are displayed in a Gantt chart, sorted in topological order for clear visualization of the project timeline.
- **Dependency Validation:** The application checks for circular dependencies between tasks using the **Depth-First Search (DFS) scanning method**.
- **Data Management:**
   - **Timekeeping:** The project operates on a clock maintained in an XML file.
   - **Data Storage:** Task data and other relevant information are saved in XML files.
   - **Task Restoration:** There is functionality to restore tasks and generate an automatic timetable.

This system streamlines the administrative and operational aspects of project management, enhancing efficiency and collaboration between managers and engineers.
