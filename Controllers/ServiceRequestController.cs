using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;
using System;
using System.Linq;

namespace RI_App.Controllers
{
    public class ServiceRequestController : Controller
    {
        private static ServiceRequestTree _tree = new();
        private static ServiceRequestHeap _heap = new();



        // Displays all requests
        [HttpGet]
        public IActionResult Index()
        {
            var requests = _tree.InOrderTraversal();
            return View(requests);
        }

        // Create a new request form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Handles form submission for new request
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
                TempData["SuccessMessage"] = "Service request added unsuccessfully!";
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
        // PRIORITY QUEUE VIEW
        // =============================
        [HttpGet] 
        public IActionResult PriorityQueue()
        {
            var heapList = _heap.GetAll();
            return View(heapList);
        }


    }
}
