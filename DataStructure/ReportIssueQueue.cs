using RI_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RI_App.DataStructure
{
    public class ReportIssueQueue
    {
        // ===============================
        // === Internal Data Structures ===
        // ===============================

        private Queue<ReportIssue> _queue = new Queue<ReportIssue>();
        private BinarySearchTree _bst = new BinarySearchTree();
        private MinHeap _heap = new MinHeap();
        private Graph _graph = new Graph();

        private int _nextId = 1; // auto-increment integer ID

        // ===============================
        // === Wrapper Methods (Public) ===
        // ===============================

        // Add new issue
        public void AddIssue(ReportIssue issue)
        {
            issue.Id = _nextId++;
            issue.DateReported = DateTime.Now;
            issue.Status = "Pending";

            // Store in multiple structures
            _queue.Enqueue(issue);
            _bst.Insert(issue);
            _heap.Insert(issue);
            _graph.AddIssue(issue);
        }

        // Retrieve all issues (by insertion order)
        public IEnumerable<ReportIssue> GetAllIssues()
        {
            return _queue.ToList();
        }

        // Retrieve issue by ID
        public ReportIssue GetById(int id)
        {
            return _queue.FirstOrDefault(i => i.Id == id);
        }

        // Update issue status
        public void UpdateStatus(int id, string newStatus)
        {
            var issue = _queue.FirstOrDefault(i => i.Id == id);
            if (issue != null)
            {
                issue.Status = newStatus;
            }
        }

        // Recommended issues (Graph-based)
        public IEnumerable<ReportIssue> RecommendRelated(string category)
        {
            return _graph.GetRelatedIssues(category);
        }

        // Get highest priority issue (Heap-based)
        public ReportIssue GetNextPriorityIssue()
        {
            return _heap.ExtractMin();
        }

        // Retrieve issues sorted by Date using BST
        public List<ReportIssue> GetIssuesSortedByDate()
        {
            return _bst.InOrderTraversal();
        }
    }

    // ======================================
    // === Binary Search Tree (by Date) ===
    // ======================================
    public class BinarySearchTree
    {
        private class Node
        {
            public ReportIssue Data;
            public Node Left;
            public Node Right;

            public Node(ReportIssue data)
            {
                Data = data;
            }
        }

        /// <summary>
<<<<<<<<< Temporary merge branch 1
        /// Returns all currently stored issues.
=========
        /// Returns all stored issues as a list.
>>>>>>>>> Temporary merge branch 2
        /// </summary>
        public List<ReportIssue> GetAll() => _issues.ToList();

            return root;
        }

        public List<ReportIssue> InOrderTraversal()
        {
            var list = new List<ReportIssue>();
            InOrder(root, list);
            return list;
        }

        private void InOrder(Node node, List<ReportIssue> list)
        {
            if (node == null) return;
            InOrder(node.Left, list);
            list.Add(node.Data);
            InOrder(node.Right, list);
        }
    }

    // ======================================
    // === Min-Heap (earliest issue first) ===
    // ======================================
    public class MinHeap
    {
        private List<ReportIssue> _heap = new List<ReportIssue>();

        private int Parent(int i) => (i - 1) / 2;
        private int Left(int i) => 2 * i + 1;
        private int Right(int i) => 2 * i + 2;

        public void Insert(ReportIssue issue)
        {
            _heap.Add(issue);
            int i = _heap.Count - 1;
            while (i > 0 && _heap[Parent(i)].DateReported > _heap[i].DateReported)
            {
                Swap(i, Parent(i));
                i = Parent(i);
            }
        }

        public ReportIssue ExtractMin()
        {
            if (_heap.Count == 0) return null;
            ReportIssue root = _heap[0];
            _heap[0] = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);
            Heapify(0);
            return root;
        }

        private void Heapify(int i)
        {
            int smallest = i;
            int l = Left(i);
            int r = Right(i);

            if (l < _heap.Count && _heap[l].DateReported < _heap[smallest].DateReported)
                smallest = l;
            if (r < _heap.Count && _heap[r].DateReported < _heap[smallest].DateReported)
                smallest = r;

            if (smallest != i)
            {
                Swap(i, smallest);
                Heapify(smallest);
            }
        }

        private void Swap(int i, int j)
        {
            var temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }
    }

    // ======================================
    // === Graph (category-based links) ===
    // ======================================
    public class Graph
    {
        private Dictionary<string, List<ReportIssue>> _adjacencyList = new();

        public void AddIssue(ReportIssue issue)
        {
            if (!_adjacencyList.ContainsKey(issue.Category))
                _adjacencyList[issue.Category] = new List<ReportIssue>();

            _adjacencyList[issue.Category].Add(issue);
        }

        public IEnumerable<ReportIssue> GetRelatedIssues(string category)
        {
            if (_adjacencyList.ContainsKey(category))
                return _adjacencyList[category];
            return new List<ReportIssue>();
>>>>>>>>> Temporary merge branch 2
>>>>>>>>> Temporary merge branch 2
        }
    }
}
