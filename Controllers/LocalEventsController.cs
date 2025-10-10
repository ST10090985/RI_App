using Microsoft.AspNetCore.Mvc;
using RI_App.DataStructure;
using RI_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RI_App.Controllers
{
    public class LocalEventsController : Controller
    {
        private static readonly LocalEventManager _localEventManager = new LocalEventManager();

        // Keep track of recent searches to generate recommendations
        private static readonly Stack<string> _recentSearches = new Stack<string>();

        // Display list of events + search + recommendations
        [HttpGet]
        public IActionResult ListEvents(string? category, DateTime? date)
        {
            IEnumerable<LocalEvent> events;

            // Search logic
            if (!string.IsNullOrEmpty(category) || date.HasValue)
            {
                events = _localEventManager.Search(category, date);
            }
            else
            {
                events = _localEventManager.GetAllEvents();
            }

            // Use data structure's built-in recommendation logic
            var recommended = _localEventManager.GetRecommendedEvents();

            // Pass categories and recommendations to view
            ViewBag.Categories = _localEventManager.GetCategories();
            ViewBag.Recommendations = recommended;

            return View(events);
        }


        // Add Event Page
        [HttpGet]
        public IActionResult AddEvent()
        {
            ViewBag.Categories = _localEventManager.GetCategories();
            return View();
        }

        // Add Event POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEvent(LocalEvent newEvent)
        {
            if (ModelState.IsValid)
            {
                _localEventManager.AddEvent(newEvent);
                return RedirectToAction("ListEvents");
            }

            ViewBag.Categories = _localEventManager.GetCategories();
            return View(newEvent);
        }
    }
}
