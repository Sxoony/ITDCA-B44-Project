# ITDCA Data Structures Project
A comprehensive C# console application demonstrating advanced data structures including AVL trees, priority queues, and sorting algorithms.

## Description
This project is a multi-module data structures implementation for an academic assignment. It showcases an AVL tree-based book management system with self-balancing capabilities, a priority queue for hospital patient management, and a QuickSort implementation for hotel booking systems. The application provides interactive menu-driven interfaces for each module, allowing users to insert, delete, search, and display data using various traversal and sorting techniques.

## Features
- **AVL Tree Book Management** - Self-balancing binary search tree with insert, delete, and search operations
- **Multiple Traversal Methods** - In-order and level-order tree traversals for comprehensive data display
- **Priority Queue System** - Hospital patient management with priority-based queuing (0-4 priority levels)
- **QuickSort Implementation** - Efficient hotel sorting by multiple criteria (distance, price, stars, name)
- **JSON Data Integration** - Hotel data loaded from external JSON files
- **Interactive Menus** - User-friendly console interface for all operations
- **Duplicate Handling** - Intelligent handling of multiple books with the same year
- **Real-time Rebalancing** - Automatic AVL tree rebalancing after insertions and deletions

## Technologies
- **Language/Framework:** C# / .NET (compatible with .NET 6+)
- **Libraries:** 
  - System.Text.Json (JSON serialization/deserialization)
  - System.Collections.Generic (data structures)
  - System.ComponentModel.DataAnnotations (validation attributes)

## How to Run (Local)
1. **Clone:** `git clone https://github.com/Sxoony/ITDCA-Data-Structures-Project.git`
2. **Navigate:** `cd ITDCA-Data-Structures-Project`
3. **Setup:**
   - Open the solution in Visual Studio 2022 or later
   - Ensure the `q4_data (1).json` file is in the project directory (for Question 4 functionality)
   - Restore NuGet packages if prompted
4. **Run:**
   - Press F5 or click "Start" in Visual Studio
   - Follow the on-screen menu to navigate between questions
   - Select options 1-4 to test different modules, or 5 to exit

## Project Structure
```
├── Q1Q2Module        # AVL Tree implementation and menus
├── Q3Module          # Priority Queue patient management
├── Q4Module          # QuickSort hotel booking system
├── Program (Main)    # Main menu and program entry point
└── q4_data (1).json  # Hotel data file
```

## Data Structures Implemented
- **AVL Tree** - Self-balancing binary search tree with O(log n) operations
- **Priority Queue** - Min-heap based queue for patient prioritization
- **Binary Search Tree** - Base implementation for AVL inheritance

## Sample Dataset
The project includes 20 pre-loaded books spanning from 1813 to 2019, including classics like "1984", "The Great Gatsby", and modern titles like "The Silent Patient".

## Author
**Zamir Kruger**  
ITDCA B44 Project - Tyger Valley Campus