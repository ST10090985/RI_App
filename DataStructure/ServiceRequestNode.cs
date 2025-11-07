using RI_App.Models;
using System;
using System.Collections.Generic;

namespace RI_App.DataStructure
{
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

    public class ServiceRequestTree
    {
        private ServiceRequestNode _root;
        private bool _isSeeded = false; // prevent reseeding

        public ServiceRequestTree()
        {
            if (!_isSeeded)
            {
               // SeedDummyData();
                _isSeeded = true;
            }
        }

        // Insert node
        public void Insert(ServiceRequest request)
        {
            _root = InsertNode(_root, request);
        }

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

        // Find node by ID
        public ServiceRequest Find(int id)
        {
            return FindNode(_root, id)?.Data;
        }

        private ServiceRequestNode FindNode(ServiceRequestNode node, int id)
        {
            if (node == null)
                return null;

            if (id == node.Data.Id)
                return node;

            if (id < node.Data.Id)
                return FindNode(node.Left, id);
            else
                return FindNode(node.Right, id);
        }

        // Traverse in-order
        public List<ServiceRequest> InOrderTraversal()
        {
            List<ServiceRequest> list = new();
            TraverseInOrder(_root, list);
            return list;
        }

        private void TraverseInOrder(ServiceRequestNode node, List<ServiceRequest> list)
        {
            if (node == null) return;
            TraverseInOrder(node.Left, list);
            list.Add(node.Data);
            TraverseInOrder(node.Right, list);
        }

        // ===========================
        // Dummy Data for Demonstration
        // ===========================
        private void SeedDummyData()
        {
            var dummyRequests = new[]
            {
                new ServiceRequest
                {
                    Id = 1001,
                    Title = "Printer Not Working",
                    Description = "Printer in the admin office is offline.",
                    Status = "Pending",
                    Priority = 2,
                    Progress = 0,
                    CreatedDate = DateTime.Now.AddDays(-3)
                },
                new ServiceRequest
                {
                    Id = 1002,
                    Title = "Wi-Fi Connection Issue",
                    Description = "Network connection is dropping intermittently.",
                    Status = "In Progress",
                    Priority = 3,
                    Progress = 50,
                    CreatedDate = DateTime.Now.AddDays(-2)
                },
                new ServiceRequest
                {
                    Id = 1003,
                    Title = "Software Update Needed",
                    Description = "Requesting update for accounting software.",
                    Status = "Completed",
                    Priority = 1,
                    Progress = 100,
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new ServiceRequest
                {
                    Id = 1004,
                    Title = "Broken Projector",
                    Description = "Projector in classroom B2 needs repair.",
                    Status = "Pending",
                    Priority = 3,
                    Progress = 0,
                    CreatedDate = DateTime.Now.AddDays(-1)
                },
                new ServiceRequest
                {
                    Id = 1005,
                    Title = "Request for New Chairs",
                    Description = "Staff lounge needs new chairs.",
                    Status = "Pending",
                    Priority = 1,
                    Progress = 0,
                    CreatedDate = DateTime.Now.AddDays(-4)
                }
            };

            foreach (var req in dummyRequests)
            {
                Insert(req);
            }
        }
    }
}
