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

                // Record category searches for recommendations
                if (!string.IsNullOrEmpty(category))
                {
                    _recentSearches.Push(category);
                    if (_recentSearches.Count > 5)
                        _recentSearches.Pop(); // keep recent 5
                }
            }
            else
            {
                events = _localEventManager.GetAllEvents();
            }

            // Generate recommendations based on previous search patterns
            var recommended = new List<LocalEvent>();
            if (_recentSearches.Any())
            {
                var recentCategory = _recentSearches.Peek();
                recommended = _localEventManager
                    .GetEventsByCategory(recentCategory)
                    .Where(e => e.Date >= DateTime.Today)
                    .Take(3)
                    .ToList();
            }

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
