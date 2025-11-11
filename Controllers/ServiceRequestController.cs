using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;
using System;
using System.Linq;

namespace RI_App.Controllers
{
    public class ServiceRequestController : Controller
    {
        // Both data structures — one for order (BST), one for priority (Heap)
        private static ServiceRequestTree _tree = new();
        private static ServiceRequestHeap _heap = new();

        private static bool _dataInitialized = false; // ensures we only seed once

        // Constructor: seeds dummy data into both structures
        public ServiceRequestController()
        {
            if (!_dataInitialized)
            {
                SeedDummyData();
                _dataInitialized = true;
            }
        }

        /// <summary>
        /// Inserts dummy requests into both the BST and Heap.
        /// </summary>
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
                _tree.Insert(req); // stored by ID order
                _heap.Insert(req); // stored by priority
            }
        }

        // =============================
        // INDEX (BST View with Search)
        // =============================
        [HttpGet]
        public IActionResult Index(string? searchTerm)
        {
            var requests = _tree.InOrderTraversal();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                requests = requests
                    .Where(r => (r.Title != null && r.Title.ToLower().Contains(searchTerm)) ||
                                (r.Description != null && r.Description.ToLower().Contains(searchTerm)))
                    .ToList();
            }

            ViewBag.SearchTerm = searchTerm;
            return View(requests);
        }

        // =============================
        // CREATE NEW REQUEST
        // =============================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ServiceRequest request)
        {
            if (ModelState.IsValid)
            {
                request.Id = new Random().Next(1000, 9999);
                request.Status = "Pending";
                request.Progress = 0;
                _tree.Insert(request);
                _heap.Insert(request);

                TempData["SuccessMessage"] = "Service request added successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Service request creation failed!";
            }

            return View(request);
        }

        // =============================
        // UPDATE STATUS / PROGRESS PAGE
        // =============================
        [HttpGet]
        public IActionResult UpdateStatus(int id)
        {
            var request = _tree.Find(id);
            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpPost]
        public IActionResult UpdateStatus(ServiceRequest updatedRequest)
        {
            var request = _tree.Find(updatedRequest.Id);
            if (request == null)
                return NotFound();

            // Update status and progress fields
            request.Status = updatedRequest.Status;
            request.Progress = updatedRequest.Progress;

            TempData["SuccessMessage"] = "Request status updated successfully!";
            return RedirectToAction("Index");
        }
        // =============================
        // PRIORITY QUEUE VIEW (HEAP)
        // =============================
        [HttpGet]
        [HttpGet]
        public IActionResult PriorityQueue(string? searchTerm, string sortOrder = "desc")
        {
            var heapList = _heap.GetAll();

            // 🔍 Optional search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                heapList = heapList
                    .Where(r => (r.Title != null && r.Title.ToLower().Contains(searchTerm)) ||
                                (r.Description != null && r.Description.ToLower().Contains(searchTerm)))
                    .ToList();
            }

            // 🔽 Sorting logic
            if (sortOrder == "asc")
                heapList = heapList.OrderBy(r => r.Priority).ToList();
            else
                heapList = heapList.OrderByDescending(r => r.Priority).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortOrder = sortOrder; // store for toggle button

            return View(heapList);
        }

    }
}
