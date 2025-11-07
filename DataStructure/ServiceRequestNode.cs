using RI_App.Models;
using System.Collections.Generic;

namespace RI_App.DataStructure
{
    // Node class representing each service request in the tree
    //Binary Search Tree
    public class ServiceRequestNode
    {
        public ServiceRequest Data { get; set; }
        public ServiceRequestNode Left { get; set; }
        public ServiceRequestNode Right { get; set; }

        public ServiceRequestNode(ServiceRequest data)
        {
            Data = data;
        }
    }

    // Binary Search Tree to manage service requests
    public class ServiceRequestTree
    {
        private ServiceRequestNode _root;

        // Insert a new service request into the tree
        public void Insert(ServiceRequest request)
        {
            _root = InsertNode(_root, request);
        }

        // Recursive insertion logic
        private ServiceRequestNode InsertNode(ServiceRequestNode node, ServiceRequest request)
        {
            if (node == null)
                return new ServiceRequestNode(request);

            if (request.Id < node.Data.Id)
                node.Left = InsertNode(node.Left, request);
            else
                node.Right = InsertNode(node.Right, request);

            return node;
        }

        // Return a sorted list of requests using in-order traversal
        public List<ServiceRequest> InOrderTraversal()
        {
            List<ServiceRequest> list = new();
            TraverseInOrder(_root, list);
            return list;
        }

        // Recursive traversal helper
        private void TraverseInOrder(ServiceRequestNode node, List<ServiceRequest> list)
        {
            if (node == null) return;

            TraverseInOrder(node.Left, list);
            list.Add(node.Data);
            TraverseInOrder(node.Right, list);
        }

        // ===============================================
        // Find a specific service request by its ID
        // ===============================================
        public ServiceRequest Find(int id)
        {
            return FindRecursive(_root, id);
        }

        // Recursive helper for searching by ID
        private ServiceRequest FindRecursive(ServiceRequestNode node, int id)
        {
            if (node == null)
                return null;

            if (id == node.Data.Id)
                return node.Data;
            else if (id < node.Data.Id)
                return FindRecursive(node.Left, id);
            else
                return FindRecursive(node.Right, id);
        }
    }
}
