using RI_App.Models;
using System.Collections.Generic;

namespace RI_App.DataStructure
{
    // Node for BST
    public class ReportIssueNode
    {
        public ReportIssue Data { get; set; }
        public ReportIssueNode? Left { get; set; }
        public ReportIssueNode? Right { get; set; }

        public ReportIssueNode(ReportIssue data) => Data = data;
    }

    // Simple BST keyed by Id (int). You can adapt to other keys (DateReported, Priority).
    public class ReportIssueBST
    {
        private ReportIssueNode? _root;

        // Insert node by Id
        public void Insert(ReportIssue issue)
        {
            _root = InsertRec(_root, issue);
        }

        private ReportIssueNode InsertRec(ReportIssueNode? node, ReportIssue issue)
        {
            if (node == null) return new ReportIssueNode(issue);

            if (issue.Id < node.Data.Id)
                node.Left = InsertRec(node.Left, issue);
            else
                node.Right = InsertRec(node.Right, issue);

            return node;
        }

        // Search by id
        public ReportIssue? SearchById(int id)
        {
            var node = _root;
            while (node != null)
            {
                if (id == node.Data.Id) return node.Data;
                node = id < node.Data.Id ? node.Left : node.Right;
            }
            return null;
        }

        // In-order traversal (sorted by Id)
        public List<ReportIssue> InOrder()
        {
            var list = new List<ReportIssue>();
            TraverseInOrder(_root, list);
            return list;
        }

        private void TraverseInOrder(ReportIssueNode? node, List<ReportIssue> list)
        {
            if (node == null) return;
            TraverseInOrder(node.Left, list);
            list.Add(node.Data);
            TraverseInOrder(node.Right, list);
        }

        // Simple delete by id (returns true if removed)
        public bool Remove(int id)
        {
            bool removed = false;
            _root = RemoveRec(_root, id, ref removed);
            return removed;
        }

        private ReportIssueNode? RemoveRec(ReportIssueNode? node, int id, ref bool removed)
        {
            if (node == null) return null;

            if (id < node.Data.Id)
                node.Left = RemoveRec(node.Left, id, ref removed);
            else if (id > node.Data.Id)
                node.Right = RemoveRec(node.Right, id, ref removed);
            else
            {
                removed = true;
                // Node with only one child or no child
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                // Node with two children: get inorder successor (smallest in the right subtree)
                var successor = node.Right;
                while (successor.Left != null)
                    successor = successor.Left;

                node.Data = successor.Data;
                node.Right = RemoveRec(node.Right, successor.Data.Id, ref removed);
            }
            return node;
        }
    }
}
