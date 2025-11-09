using System;
using System.Collections.Generic;
using System.Linq;
using RI_App.Models;

namespace RI_App.DataStructure
{
    public class ReportIssueQueue
    {
        private readonly List<ReportIssue> _issues;   // Base storage
        private readonly IssueBST _bst;               // For sorting by date
        private readonly IssueHeap _heap;             // For priority management
        private readonly IssueGraph _graph;           // For category-based recommendations

        public ReportIssueQueue()
        {
            _issues = new List<ReportIssue>();
            _bst = new IssueBST();
            _heap = new IssueHeap();
            _graph = new IssueGraph();
        }

        // ✅ Add a new issue
        public void AddIssue(ReportIssue issue)
        {
            _issues.Add(issue);
            _bst.Insert(issue);
            _heap.Insert(issue);
            _graph.AddIssue(issue);
        }

        // ✅ Get all issues
        public List<ReportIssue> GetAllIssues()
        {
            return _issues.ToList();
        }

        // ✅ Remove the issue with the highest priority (Heap-based)
        public ReportIssue RemoveHighestPriorityIssue()
        {
            var removed = _heap.RemoveMax();
            if (removed != null)
            {
                _issues.Remove(removed);
                _bst.Remove(removed);
                _graph.RemoveIssue(removed);
            }
            return removed;
        }

        // ✅ Sort issues by date (BST-based)
        public List<ReportIssue> GetIssuesSortedByDate()
        {
            return _bst.InOrderTraversal();
        }

        // ✅ Update the priority of an issue
        public bool UpdatePriority(int id, int newPriority)
        {
            var issue = _issues.FirstOrDefault(i => i.Id == id);
            if (issue == null) return false;

            issue.Priority = newPriority;
            _heap.RebuildHeap(_issues);
            return true;
        }

        // ✅ Recommend issues based on category (Graph-based)
        public List<ReportIssue> RecommendedRelated(string category)
        {
            return _graph.GetRelatedIssues(category);
        }
    }

    // ======================================================
    // =============== Supporting Data Structures ============
    // ======================================================

    // -------- Binary Search Tree for sorting by date --------
    public class IssueBST
    {
        private class Node
        {
            public ReportIssue Issue;
            public Node Left, Right;

            public Node(ReportIssue issue)
            {
                Issue = issue;
            }
        }

        private Node _root;

        public void Insert(ReportIssue issue)
        {
            _root = InsertRec(_root, issue);
        }

        private Node InsertRec(Node root, ReportIssue issue)
        {
            if (root == null)
                return new Node(issue);

            if (issue.DateReported < root.Issue.DateReported)
                root.Left = InsertRec(root.Left, issue);
            else
                root.Right = InsertRec(root.Right, issue);

            return root;
        }

        public void Remove(ReportIssue issue)
        {
            _root = RemoveRec(_root, issue);
        }

        private Node RemoveRec(Node root, ReportIssue issue)
        {
            if (root == null)
                return root;

            if (issue.Id < root.Issue.Id)
                root.Left = RemoveRec(root.Left, issue);
            else if (issue.Id > root.Issue.Id)
                root.Right = RemoveRec(root.Right, issue);
            else
            {
                if (root.Left == null) return root.Right;
                if (root.Right == null) return root.Left;

                root.Issue = MinValue(root.Right);
                root.Right = RemoveRec(root.Right, root.Issue);
            }

            return root;
        }

        private ReportIssue MinValue(Node node)
        {
            ReportIssue minv = node.Issue;
            while (node.Left != null)
            {
                minv = node.Left.Issue;
                node = node.Left;
            }
            return minv;
        }

        public List<ReportIssue> InOrderTraversal()
        {
            var list = new List<ReportIssue>();
            InOrderRec(_root, list);
            return list;
        }

        private void InOrderRec(Node root, List<ReportIssue> list)
        {
            if (root != null)
            {
                InOrderRec(root.Left, list);
                list.Add(root.Issue);
                InOrderRec(root.Right, list);
            }
        }
    }

    // -------- Max Heap for issue priority --------
    public class IssueHeap
    {
        private List<ReportIssue> _heap = new List<ReportIssue>();

        public void Insert(ReportIssue issue)
        {
            _heap.Add(issue);
            HeapifyUp(_heap.Count - 1);
        }

        public ReportIssue RemoveMax()
        {
            if (_heap.Count == 0) return null;

            var max = _heap[0];
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);
            HeapifyDown(0);
            return max;
        }

        public void RebuildHeap(IEnumerable<ReportIssue> issues)
        {
            _heap = issues.ToList();
            for (int i = _heap.Count / 2 - 1; i >= 0; i--)
                HeapifyDown(i);
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_heap[index].Priority <= _heap[parent].Priority)
                    break;

                (_heap[index], _heap[parent]) = (_heap[parent], _heap[index]);
                index = parent;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = _heap.Count - 1;
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int largest = index;

                if (left <= lastIndex && _heap[left].Priority > _heap[largest].Priority)
                    largest = left;
                if (right <= lastIndex && _heap[right].Priority > _heap[largest].Priority)
                    largest = right;

                if (largest == index) break;

                (_heap[index], _heap[largest]) = (_heap[largest], _heap[index]);
                index = largest;
            }
        }
    }

    // -------- Graph for related categories --------
    public class IssueGraph
    {
        private readonly Dictionary<string, List<ReportIssue>> _graph = new();

        public void AddIssue(ReportIssue issue)
        {
            if (!_graph.ContainsKey(issue.Category))
                _graph[issue.Category] = new List<ReportIssue>();

            _graph[issue.Category].Add(issue);
        }

        public void RemoveIssue(ReportIssue issue)
        {
            if (_graph.ContainsKey(issue.Category))
                _graph[issue.Category].Remove(issue);
        }

        public List<ReportIssue> GetRelatedIssues(string category)
        {
            return _graph.ContainsKey(category)
                ? _graph[category]
                : new List<ReportIssue>();
        }
    }
}
