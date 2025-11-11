using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ITDCA_B44_Project_Tyger_Valley_Campus_EDUV4848998
{
    // ========================================
    // QUESTION 1 & 2: AVL TREE CLASSES
    // ========================================

    //QUESTION 1 CLASS INITIALIZATIONS (Book + Binary Search Tree + Node CLASSES)
    public class q1_Book
    {
        public string Title;
        public string Author;
        public int Year;

        public q1_Book(string title, string author, int year) //book constructor
        {
            Title = title;
            Author = author;
            Year = year;
        }
    }

    public class Node
    {
        public q1_Book Data; //every book has data,
                             //and in the case of a tree, every node has to have a right and left reference.
        public Node Left;
        public Node Right;

        //ADDITIONS / CHANGES TO THIS CLASS TO SUIT AN AVL TREE:
        public int Height; //required for an AVL, so that the tree can rebalance itself.
    }

    public class q1_BinarySearchTree
    {
        /*
         *References used:
         *Medium https://medium.com/better-programming/introduction-to-binary-search-trees-dde166368210 - last accessed 29/10/2025. Last modified 24/06/2019.
         *GeeksForGeekss https://www.geeksforgeeks.org/dsa/binary-search-tree-data-structure/ last accessed 29/10/2025. Last modified 24/09/2025.
         */
        public Node Root; //Root variable initialized. Root is the most important refernce to build from.

        public q1_BinarySearchTree()
        {
            Root = null;
        } //new search tree, no root. constructor

        public Node Search(int key) //search using a key identifier, which in this case would be the book YEAR.
        {
            Node current = Root;//the node that is currently being worked with, at the very beginning, is the root. naturally.
            while (current != null)
            {
                if (key == current.Data.Year)
                    return current; // found one match -> return it

                current = (key < current.Data.Year) ? current.Left : current.Right; //traverse to the left or right of the tree depending if the year is greater or less than the key.
            }
            return null; // not found
        }

        public virtual void Insert(q1_Book book)
        {
            if (Root == null) { Root = new Node { Data = book }; return; } //A new binary search tree is created if there is no root.

            Node current = Root; //current is different to the child of the parent. The current represents the node that has been traversed to.
            Node parent = null;

            while (current != null)
            {
                parent = current; //the current node is now assumed to be a parent, which implies it has OR WILL HAVE children.
                if (book.Year < current.Data.Year) current = current.Left; //if the key is smaller than the curernt's key, then move to the left child of the current node.
                else current = current.Right; //vice versa to the original if.
            }

            if (book.Year < parent.Data.Year) parent.Left = new Node { Data = book };//if the book's year is smaller than the parent's book year, then make that book go to the left of its parent, and make it a new parent.
            else parent.Right = new Node { Data = book };
        }

        public void q2_InOrder(Node node, Action<Node> visit) //in order traversal through the tree. produces keys in ascending order
        {
            if (node == null) return;
            q2_InOrder(node.Left, visit); //recursive, it compares every node that it passes through to first go to the left of the tree/subtree (if it is smaller), and if it isn't, traverse to the right of the tree (or subtree)
            visit(node);
            q2_InOrder(node.Right, visit);
        }

        public void LevelOrder(Action<Node> visit)
        {
            if (Root == null) return;
            var q = new Queue<Node>();
            q.Enqueue(Root); //2 reasons, so that the while loop can have a true condition to begin with, and to visit each node level by level, which is always lower than the root.
            while (q.Count > 0)
            {
                var n = q.Dequeue(); //dequeue returns that value.
                visit(n);
                if (n.Left != null) q.Enqueue(n.Left);//populate the parent with a child to the left.
                if (n.Right != null) q.Enqueue(n.Right);
            }
        }

        public List<Node> q2_GetAllByYear(int year)
        {
            var results = new List<Node>();
            // use in-order traversal to collect matches (in-order will find them sorted by title/author for the same year)
            q2_InOrder(Root, node =>
            {
                if (node.Data.Year == year) results.Add(node);
            });
            return results;
        }
    }

    public class q1_AVLTree : q1_BinarySearchTree //Avl tree inherits from the BST
    {
        private int GetHeight(Node node) => node == null ? 0 : node.Height;

        private int GetBalance(Node node) => node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);

        private Node RotateRight(Node z) //Node Z is a Parent node, at least one of its children has a subtree. And the children's heights are unbalanced, and in this case, it is unbalanced to the RIGHT RIGHT of the Parent. 
        {
            Node y = z.Left; //y is Z's left child.
            Node T3 = y.Right; // T3 is Y's right child or subtree or leaf. - meaning Y = left of Z, T3 = left of Z, right of Y.

            //rotation
            y.Right = z; // set the RIGHT subtree as the new parent
            z.Left = T3; //set the (newly set) parent's LEFT child as the unbalanced subtree.

            //updating height
            z.Height = Math.Max(GetHeight(z.Left), GetHeight(z.Right)) + 1; // in case that Z has children and subchildren.
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            //new root
            return y;
        }

        private Node RotateLeft(Node z) //same as RotateRight, but the subtree is unbalanced to the left. Left-Left of Parent is unbalanced.
        {
            Node y = z.Right;
            Node T2 = y.Left;

            y.Left = z;
            z.Right = T2;

            z.Height = Math.Max(GetHeight(z.Left), GetHeight(z.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        public override void Insert(q1_Book book)
        {
            Root = InsertNode(Root, book);
        }

        private Node InsertNode(Node node, q1_Book book)
        {
            if (node == null) return new Node { Data = book, Height = 1 };

            int cmp = CompareBooks(node, book);
            if (cmp < 0) node.Left = InsertNode(node.Left, book);
            else if (cmp > 0) node.Right = InsertNode(node.Right, book);
            else
            {
                Console.WriteLine("Error: Duplicate book entry");
                return node;
            }

            //updating the height of the node
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            //bf = BALANCE FACTOR - the difference in height of a parent's left and right child with all of their subtrees. IF the balance difference (balance factor) is LESS than -1 or MORE than 1, then a rotation is prompted.
            int bf = GetBalance(node);

            /*References used for this code: 
             * GeeksForGeeks - https://www.geeksforgeeks.org/dsa/insertion-in-an-avl-tree/ accessed 29/10/2025. Last modified: 23/7/2025
             * W3Schools - https://www.w3schools.com/dsa/dsa_data_avltrees.php accessed 29/10/2025. Last modified N/A
             * DZone - https://dzone.com/articles/understanding-avl-trees-in-c-a-guide-to-self-balan accessed 29/10/2025. Last modified 03/04/2023
             * An AVL tree is essentially a self balancing binary search tree that automatically updates (or maintains) its height - which is what gets used to determine if the tree is balanced or not - after every single insertion or deletion. In a BST (binary search tree), nodes can very easily become unbalanced after a couple of deletions and insertions, which cause much slower operations. 
             * AVL trees make rotations to make sure the balance factor (height difference between subtrees) is always at most 1. These rotations restructure the tree, it comes in 4 different forms: Right (Left left imbalance) Rotation, Left (Right Right imbalance) Rotation, Left Right Rotation (left right imbalance) and a Right Left Rotation (Right left imbalance).
             * Each rotation repositions nodes white retaining the needed BST property, which ensures that all operations (search, insert, delete) run in O(log N) time.
             */
            if (bf > 1 && CompareBooks(node.Left, book) < 0) return RotateRight(node);
            if (bf < -1 && CompareBooks(node.Right, book) > 0) return RotateLeft(node);
            if (bf > 1 && CompareBooks(node.Left, book) > 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }
            if (bf < -1 && CompareBooks(node.Right, book) < 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        private int CompareBooks(Node node, q1_Book book)
        {
            int cmp = node.Data.Year.CompareTo(book.Year);
            if (cmp != 0) return cmp;

            cmp = string.Compare(node.Data.Title, book.Title, StringComparison.OrdinalIgnoreCase); //this is a tiebreaker if the books have the same year.
            if (cmp != 0) return cmp;

            return string.Compare(node.Data.Author, book.Author, StringComparison.OrdinalIgnoreCase);
        }

        // ---------- AVL DELETE IMPLEMENTATION ----------
        // The delete algorithm removes the provided book (matching year/title/author)
        // and then rebalances the AVL if necessary.
        public void Delete(q1_Book book)
        {
            Root = DeleteNode(Root, book);
        }

        private Node GetMinValueNode(Node node)
        {
            Node current = node;
            while (current.Left != null)
                current = current.Left;
            return current;
        }

        private Node DeleteNode(Node node, q1_Book book)
        {
            if (node == null) return null;

            int cmp = CompareBooks(node, book);
            if (cmp < 0)
            {
                node.Left = DeleteNode(node.Left, book);
            }
            else if (cmp > 0)
            {
                node.Right = DeleteNode(node.Right, book);
            }
            else
            {
                // node to be deleted found
                // Case: node with only one child or no child
                if (node.Left == null || node.Right == null)
                {
                    Node temp = node.Left ?? node.Right;
                    if (temp == null)
                    {
                        // No child case
                        node = null;
                    }
                    else
                    {
                        // One child case
                        node = temp;
                    }
                }
                else
                {
                    // node with two children: get inorder successor (smallest in the right subtree)
                    Node temp = GetMinValueNode(node.Right);
                    // Copy the inorder successor's data to this node
                    node.Data = temp.Data;
                    // Delete the inorder successor
                    node.Right = DeleteNode(node.Right, temp.Data);
                }
            }

            // If the tree had only one node then return
            if (node == null) return null;

            // UPDATE HEIGHT
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            // GET BALANCE FACTOR
            int bf = GetBalance(node);

            // LEFT LEFT CASE
            if (bf > 1 && GetBalance(node.Left) >= 0)
                return RotateRight(node);

            // LEFT RIGHT CASE
            if (bf > 1 && GetBalance(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // RIGHT RIGHT CASE
            if (bf < -1 && GetBalance(node.Right) <= 0)
                return RotateLeft(node);

            // RIGHT LEFT CASE
            if (bf < -1 && GetBalance(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }
        // ---------- END AVL DELETE ----------
    }

    // ========================================
    // QUESTION 1 & 2: MENU MODULES
    // ========================================

    internal class Q1Q2Module
    {
        public static void RunQ1Menu(q1_AVLTree avl, List<q1_Book> books, ref int CountBooks)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== QUESTION 1: AVL Tree Menu ===");
                Console.WriteLine("1. Insert a new book");
                Console.WriteLine("2. Delete a book (by year; if multiple matches you'll be prompted for title)");
                Console.WriteLine("3. Display Level Order (tree structure)");
                Console.WriteLine("4. Return to Main Menu");
                Console.Write("Enter choice: ");
                var input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid choice.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        {
                            Console.Write("Enter Title: ");
                            string title = Console.ReadLine() ?? "";
                            Console.Write("Enter Author: ");
                            string author = Console.ReadLine() ?? "";
                            Console.Write("Enter Year: ");
                            if (!int.TryParse(Console.ReadLine(), out int year))
                            {
                                Console.WriteLine("Invalid year input.");
                                break;
                            }
                            var newBook = new q1_Book(title, author, year);
                            avl.Insert(newBook);
                            books.Add(newBook);
                            CountBooks++;
                            Console.WriteLine("Book inserted into AVL tree.");
                            break;
                        }
                    case 2:
                        {
                            // Delete by year; if multiple matches ask for title as per user request
                            Console.Write("Enter the year of the book to delete: ");
                            if (!int.TryParse(Console.ReadLine(), out int yearToDelete))
                            {
                                Console.WriteLine("Invalid year input.");
                                break;
                            }
                            var matches = avl.q2_GetAllByYear(yearToDelete);
                            if (matches.Count == 0)
                            {
                                Console.WriteLine($"No books found for year {yearToDelete}.");
                                break;
                            }
                            else if (matches.Count == 1)
                            {
                                var toDelete = matches[0].Data;
                                avl.Delete(toDelete);
                                // also remove from books list (first matching reference)
                                var foundIndex = books.FindIndex(b => b.Year == toDelete.Year && string.Equals(b.Title, toDelete.Title, StringComparison.OrdinalIgnoreCase) && string.Equals(b.Author, toDelete.Author, StringComparison.OrdinalIgnoreCase));
                                if (foundIndex >= 0) books.RemoveAt(foundIndex);
                                CountBooks = Math.Max(0, CountBooks - 1);
                                Console.WriteLine($"Deleted: {toDelete.Year} - {toDelete.Title} by {toDelete.Author}");
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Multiple books found for year {yearToDelete}:");
                                for (int i = 0; i < matches.Count; i++)
                                {
                                    var n = matches[i];
                                    Console.WriteLine($"{i + 1}. {n.Data.Year} - {n.Data.Title} by {n.Data.Author}");
                                }
                                Console.Write("Enter the title of the book to delete (case-insensitive): ");
                                string titleChoice = Console.ReadLine() ?? "";
                                var chosen = matches.Find(n => string.Equals(n.Data.Title, titleChoice, StringComparison.OrdinalIgnoreCase));
                                if (chosen == null)
                                {
                                    Console.WriteLine("No matching title found among those entries. Aborting delete.");
                                    break;
                                }
                                var bookToDelete = chosen.Data;
                                avl.Delete(bookToDelete);
                                // remove from books list
                                var idx = books.FindIndex(b => b.Year == bookToDelete.Year && string.Equals(b.Title, bookToDelete.Title, StringComparison.OrdinalIgnoreCase) && string.Equals(b.Author, bookToDelete.Author, StringComparison.OrdinalIgnoreCase));
                                if (idx >= 0) books.RemoveAt(idx);
                                CountBooks = Math.Max(0, CountBooks - 1);
                                Console.WriteLine($"Deleted: {bookToDelete.Year} - {bookToDelete.Title} by {bookToDelete.Author}");
                                break;
                            }
                        }
                    case 3:
                        {
                            Console.WriteLine("Level-order traversal (shows tree structure by levels):\n");
                            avl.LevelOrder(node =>
                            {
                                Console.WriteLine($"{node.Data.Year} - {node.Data.Title} by {node.Data.Author}");
                            });
                            break;
                        }
                    case 4:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        public static void RunQ2Menu(q1_AVLTree avl, List<q1_Book> books, ref int CountBooks)
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("\n=== QUESTION 2: Traversal / Search / Display ===");
                Console.WriteLine("1. Display all books (in-order traversal)");
                Console.WriteLine("2. Search for books by year");
                Console.WriteLine("3. Display most recent book");
                Console.WriteLine("4. Display total number of books");
                Console.WriteLine("5. Return to Main Menu");
                Console.Write("Enter choice: ");
                var input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid choice.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Books in-order (sorted by Year, then Title, then Author):\n");
                        avl.q2_InOrder(avl.Root, node =>
                        {
                            Console.WriteLine($"{node.Data.Year} - {node.Data.Title} by {node.Data.Author}");
                        });
                        break;
                    case 2:
                        Console.Write("Enter year to search: ");
                        if (!int.TryParse(Console.ReadLine(), out int year))
                        {
                            Console.WriteLine("Invalid year input.");
                            break;
                        }
                        var matches = avl.q2_GetAllByYear(year);
                        if (matches.Count == 0)
                        {
                            Console.WriteLine($"No books found for year {year}.");
                        }
                        else
                        {
                            Console.WriteLine($"Books from {year}:");
                            foreach (var n in matches)
                                Console.WriteLine($"{n.Data.Year} - {n.Data.Title} by {n.Data.Author}");
                        }
                        break;
                    case 3:
                        {
                            int highest = int.MinValue;
                            string title = "";
                            avl.q2_InOrder(avl.Root, node =>
                            {
                                if (node.Data.Year > highest) { highest = node.Data.Year; title = node.Data.Title; }
                            });
                            if (highest == int.MinValue)
                                Console.WriteLine("No books in the tree.");
                            else
                                Console.WriteLine($"The most recent book is from the year: {highest},{title}");
                            break;
                        }
                    case 4:
                        Console.WriteLine($"The total number of Books found in the tree is: {CountBooks}");
                        break;
                    case 5:
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }

    // ========================================
    // QUESTION 3: PATIENT QUEUE
    // ========================================

    public class q3_Patient //A simple patient class was initialized and instantiated with relatively arbitrary attributes, which were not essential to the task.
    {
        string Name;
        int Age;

        public q3_Patient(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public string Display()
        {
            return $"{Name}, Age: {Age}";
        }
    }

    internal class Q3Module
    {
        static PriorityQueue<q3_Patient, int> q3_patient = new PriorityQueue<q3_Patient, int>(); //PriorityQueue utilized using the Patient type in order to make a usable queuing system
                                                                                                 //which prioritizes patients accordingly.
        static int priority = 5; //The lowest priority that a patient can have is 4. 5 is out of bounds, so if a patient has a priority level of 5, it is clearly bad data then.

        // Exposed runner for combined program
        public static void RunQ3()
        {
            bool running = true;
            int option = 0;

            while (running) //Menu options that keep executing until user wishes to exit.
            {
                Console.WriteLine("Do you want to Enqueue or Dequeue a Patient?\n1. Enqueue\n2. Dequeue \n3. Exit\n");
                int.TryParse(Console.ReadLine(), out option);
                if (option > 0 && option < 4)
                {
                    switch (option)
                    {
                        case 1:
                            q3_patient.Enqueue(getPatient(), priority);
                            break;
                        case 2:
                            if (q3_patient.Count > 0)// make sure that there is a patient TO dequeue, otherwise it will throw an error.
                            {
                                Console.WriteLine($"{q3_patient.Dequeue().Display()} has been removed from the queue."); //Patient has been dequeued.
                                if (q3_patient.Count > 0)
                                {
                                    Console.WriteLine($"Next patient in the queue: {q3_patient.Peek().Display()}\n");
                                } //This runs if there is a patient that follows after the first patient gets removed from the queue.
                                else
                                {
                                    Console.WriteLine("No more patients in the queue!\n"); //if the last patient was removed from the queue, let the user know about it.
                                }
                            }
                            else Console.WriteLine("There are no patients in the queue.\n");
                            break; //this happens if there are no patients in the queue if attempting to dequeue for the first time.
                        case 3:
                            running = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid input. Enter a number ranging from 1 to 3.");
                }
            }
        }

        private static q3_Patient getPatient()
        {
            string name = "";
            int age = -1;
            priority = 5;
            bool valid = false;
            //start off with a patient of bad credentials

            Console.Write("Enter the patient name: ");
            name = Console.ReadLine();

            while (!valid)
            {
                Console.Write("Enter the patient age: ");
                if (int.TryParse(Console.ReadLine(), out age) && age >= 0 && age < 130) //until the user enters a valid number for an age, keep going.
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid age. Please enter a value between 0 and 129.");
                }
            }

            valid = false;

            while (!valid)
            {
                Console.Write("Enter the patient's priority level (0 = highest, 4 = lowest): ");
                if (int.TryParse(Console.ReadLine(), out priority) && priority >= 0 && priority <= 4)
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Invalid priority. Please enter a value between 0 and 4.");
                }
            }
            q3_Patient sample_Patient = new q3_Patient(name, age);
            Console.WriteLine("Patient successfully added!\n");
            return sample_Patient;
        }
    }

    // ========================================
    // QUESTION 4: HOTEL SORTING
    // ========================================

    public class q4_Hotel //Hotel class solely used to store hotel objects and to be able to be used in the booking class.
    {
        [JsonPropertyName("id")] //JSON uses snake_case and C# uses PascalCase. This is only done to keep naming conventions and coding in C# consistent,
                                 //while still being able to correctly work with the JSON file and the hotel's corresponding attributes.
                                 //Reference StackOverflow https://stackoverflow.com/questions/8796618/how-can-i-change-property-names-when-serializing-with-json-net, accessed 29/10/2025. Last modified 1 year, 5 months ago.
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("nightly_rate")]
        public double NightlyRate { get; set; }

        [JsonPropertyName("stars")]
        public int Stars { get; set; }

        [JsonPropertyName("distance_from_airport")]
        public double DistanceFromAirport { get; set; }

        public override string ToString()
        {
            return $"{Name} \n{Stars} stars \nR{NightlyRate} per night \n{DistanceFromAirport}km from airport\n";
        }
    }

    class q4_Booking //This approach was made that there would be a BOOKING and a HOTEL class.
                     //This booking class would handle all processes (if the program were to be expanded and include events and flights as well).
                     //The hotel class is a simple class that makes a hotel object
                     //The processes that would logically follow is -
                     //Loading Hotels from a JSON file.
                     //A reusable method that takes the private string and defines it as the way to compare two hotels. Method would be called Compare.
                     //QuickSort method that uses the compare method and gets called recursively (subtrees) until all elements are sorted.
                     //The Booking class would also have a simple sort method that calls Quicksort and any other helper methods to make sorting successful.
    {
        private string sortBy = "distance_from_airport";  // the metric that is being sorted by, and will be changed by the marker.

        private bool sorted = false; //This boolean is used to make sure that the hotels are successfully sorted, and
                                     //allows the display function to display the hotels as output.

        private List<q4_Hotel> q4_hotels = new List<q4_Hotel>(); // a list of hotel objects that will be usable throughout the entire Booking class.

        /* if the data wants to be programmatically added, hotels should become a VAR type and then jsonString 
         * should be Serialized, and then File.WriteAllText should be called and executed.
         * this is how it would look, if the data were hardcoded.
         * 
         * string filePath = "q4_data (1).json";

        if (File.Exists(filePath))
         {
         var hotels = new List<Hotel>
            {
     new Hotel { Id = 0, Name = "Hotel Cape Town", NightlyRate = 1500, Stars = 4, DistanceFromAirport = 15 },
     new Hotel { Id = 1, Name = "Mountain View Lodge", NightlyRate = 950, Stars = 3, DistanceFromAirport = 30 },
     new Hotel { Id = 2, Name = "Seaside Retreat", NightlyRate = 1800, Stars = 5, DistanceFromAirport = 10 }
            };

         string json = JsonSerializer.Serialize(hotels, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
         }
        */

        public void LoadHotels(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            q4_hotels = JsonSerializer.Deserialize<List<q4_Hotel>>(jsonString);

            //Reference Microsoft (https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to) last accessed: 29/10/2025. Last Modified 10/04/2025.
        }

        public void SortHotels()
        {
            QuickSort(q4_hotels, 0, q4_hotels.Count - 1);
            sorted = true;
        }

        /* 
             * QuickSort has lots of resources and documentation on how it has been implemented, as well as the concepts explaining what QuickSort is and does.
             * References: GeeksForGeeks -GFG for intext referencing- (https://www.geeksforgeeks.org/dsa/quick-sort-algorithm/) accessed on 29/10/2025. Last Updated 03/10/2025.
             *             W3Schools -W3S for intext referencing- (https://www.w3schools.com/dsa/dsa_algo_quicksort.php) accessed on 29/10/2025. Last Updated N/A
             *             W3resource -W3R for intext referncing- (https://www.w3resource.com/csharp-exercises/searching-and-sorting-algorithm/searching-and-sorting-algorithm-exercise-9.php) accessed on 29/10/2025. Last Updated 25/08/2025
             *             Microsoft C# Sorting algorithms implementation -MS for intext referencing- (https://learn.microsoft.com/en-us/answers/questions/1259438/c-sorting-algorithms-implementation) accessed on 29/10/2025. Last Updated 26/04/2023
             *    QuickSort is one of the fastest sorting algorithms (W3S) and picks an element as a pivot point to sort items (or objects) in an array to the left and right of the pivot. Items less than the pivot go to the left, and items greater go to the right. This conceptually (and functionally) splits the list in 2 parts. (MS,W3S,W3R)
             *    After each split is done (left to right distribution of a pivot), a new pivot is picked which creates a subdivision. This is called recursion, and it is done until the amount of elements in the division is down to 2 (or the Pivot does not have any more elements to compare to).
             *   The pivot is crucial and how it gets chosen is important. A new pivot is usually chosen to be swapped with the first element of the higher values so that the pivot element lands between the higher and lower values, but it can also be chosen by selecting the median, first or last element, etc. (GFG, W3S)
             *   The next step after selecting the pivot is called partitioning (GFG). The array gets rearranged around the pivot, and all elements are sorted to be either on the left (smaller than pivot) or right (greater than pivot), and then we obtain the INDEX of the pivot in that array.
             *   QuickSort then gets called recursively to apply the same process to the two partitioned sub-arrays (left and right of the pivot), and only stops until there is only one element left in the sub-array.
             */

        private void QuickSort(List<q4_Hotel> list, int low, int high) //The list is the array which was described, low indicates the lowest index of the array, and high = the highest index of the array.
        {
            if (low < high) //IF the array's length is still greater than 1, then execute the sort.
            {
                int pivot = Partition(list, low, high);
                QuickSort(list, low, pivot - 1);
                QuickSort(list, pivot + 1, high);
            }
        }

        private int Partition(List<q4_Hotel> list, int low, int high)
        {
            q4_Hotel pivotValue = list[high]; // choose pivot. This selects the VALUE in the list which will be used to compare the rest of the elements with.
            q4_Hotel temp; //will be needed in order to swap the pivot.
            int i = low - 1; // track index of smaller element. This is almost like a pointer element which keeps track of the index that is being worked with. Starts with low-1 because there are no smaller elements found yet.

            for (int j = low; j < high; j++)
            {
                if (Compare(list[j], pivotValue)) //if the comparison returns true, which is only done when the item in the list is SMALLER than the pivot. This is for sorting in ascending order
                {
                    i++; //pointer gets put 1 forward, because a smaller element has been found, and can therefore make the "smaller" section one element longer. A new smaller element gets added one step to the right.

                    temp = list[i]; //When this condition executes, it swaps the current smaller element (list[j]) into its correct position in the left partition (at index i).
                    list[i] = list[j];
                    list[j] = temp; //this swaps the value in the list so that it is done ascendingly (to the LEFT side of the array) and that the swap is fully complete.
                }
            }

            temp = list[i + 1];
            list[i + 1] = list[high];
            list[high] = temp; // move pivot to correct position, and set the pivot to new highest value of the subdivision that will follow.
            return i + 1; // return pivot's final index
        }

        private bool Compare(q4_Hotel a, q4_Hotel b) //this is to assign what sortBy means in terms of comparing the attributes of the hotel, and returns either true or false if it is smaller or greater (than the pivot)
        {
            if (sortBy == "nightly_rate")
                return a.NightlyRate < b.NightlyRate;
            else if (sortBy == "stars")
                return a.Stars < b.Stars;
            else if (sortBy == "distance_from_airport")
                return a.DistanceFromAirport < b.DistanceFromAirport;
            else if (sortBy == "name")
                return string.Compare(a.Name, b.Name) < 0;
            else return false;
        }

        public void DisplayHotels() //Display Hotels only IF it has been marked as sorted.
        {
            if (sorted == true)
            {
                Console.WriteLine($"Hotels sorted by {sortBy}:\n");
                foreach (q4_Hotel hotel in q4_hotels)
                {
                    Console.WriteLine(hotel.ToString());
                }
            }
        }
    }

    internal class Q4Module
    {
        // Runner exposed for the combined program
        public static void RunQ4()
        {
            q4_Booking booking = new q4_Booking();
            if (!File.Exists("q4_data (1).json"))
            {
                Console.WriteLine("JSON file not found!");
                return; // stop program
            }

            booking.LoadHotels("q4_data (1).json");
            booking.SortHotels();
            booking.DisplayHotels();
        }
    }

    // ========================================
    // MAIN PROGRAM
    // ========================================

    class Program
    {
        static void Main(string[] args)
        {

            var books = new List<q1_Book>
            {
                new q1_Book("The Great Gatsby", "F. Scott Fitzgerald", 1925),
                new q1_Book("To Kill a Mockingbird", "Harper Lee", 1960),
                new q1_Book("1984", "George Orwell", 1949),
                new q1_Book("The Silent Patient", "Alex Michaelides", 2019),
                new q1_Book("The Catcher in the Rye", "J.D. Salinger", 1951),
                new q1_Book("Pride and Prejudice", "Jane Austen", 1813),
                new q1_Book("Moby-Dick", "Herman Melville", 1851),
                new q1_Book("The Hobbit", "J.R.R. Tolkien", 1937),
                new q1_Book("Brave New World", "Aldous Huxley", 1932),
                new q1_Book("The Book Thief", "Markus Zusak", 2005),
                new q1_Book("The Road", "Cormac McCarthy", 2006),
                new q1_Book("Harry Potter and the Sorcerer's Stone", "J.K. Rowling", 1997),
                new q1_Book("The Girl with the Dragon Tattoo", "Stieg Larsson", 2005),
                new q1_Book("The Alchemist", "Paulo Coelho", 1988),
                new q1_Book("The Shining", "Stephen King", 1977),
                new q1_Book("Wuthering Heights", "Emily BrontÃ«", 1847),
                new q1_Book("Catch-22", "Joseph Heller", 1961),
                new q1_Book("The Hunger Games", "Suzanne Collins", 2008),
                new q1_Book("The Da Vinci Code", "Dan Brown", 2003),
                new q1_Book("The Outsiders", "S.E. Hinton", 1967)
            };

            var avl = new q1_AVLTree();
            int CountBooks = 0;
            foreach (var b in books)
            {
                avl.Insert(b);
                ++CountBooks;
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n=== MAIN MENU ===");
                Console.WriteLine("1. Question 1 (AVL Tree / Insert / Delete)");
                Console.WriteLine("2. Question 2 (AVL Tree / Traversal / Search / Display)");
                Console.WriteLine("3. Question 3 (Patient Queue)");
                Console.WriteLine("4. Question 4 (Hotel Sorting)");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                var input = Console.ReadLine();
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input, enter a number 1-5.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        Q1Q2Module.RunQ1Menu(avl, books, ref CountBooks);
                        break;
                    case 2:
                        Q1Q2Module.RunQ2Menu(avl, books, ref CountBooks);
                        break;
                    case 3:
                        Q3Module.RunQ3();
                        break;
                    case 4:
                        Q4Module.RunQ4();
                        break;
                    case 5:
                        exit = true;
                        Console.WriteLine("Exiting. Good luck with your submission!");
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}