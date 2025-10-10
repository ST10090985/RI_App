using RI_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RI_App.DataStructure
{
    public class LocalEventManager
    {
        // Dictionary where key = date, value = list of events
        private readonly SortedDictionary<DateTime, List<LocalEvent>> _events = new();

        // Set to store unique event categories
        private readonly HashSet<string> _categories = new();

        // Queue for upcoming events
        private readonly Queue<LocalEvent> _upcomingEvents = new();

        // Stack to track recent searches for recommendations
        private readonly Stack<string> _recentSearches = new();

        private int _nextId = 1;

        public LocalEventManager()
        {
            SeedDummyData(); // Load initial dummy data
        }

        // Add a new event
        public void AddEvent(LocalEvent e)
        {
            e.Id = _nextId++;
            _categories.Add(e.Category);

            if (!_events.ContainsKey(e.Date.Date))
                _events[e.Date.Date] = new List<LocalEvent>();

            _events[e.Date.Date].Add(e);
            _upcomingEvents.Enqueue(e);
        }

        // Get all events sorted by date
        public IEnumerable<LocalEvent> GetAllEvents()
        {
            return _events.Values.SelectMany(e => e).OrderBy(e => e.Date);
        }

        // Track recent searches for recommendations
        public IEnumerable<LocalEvent> GetRecommendedEvents()
        {
            if (_recentSearches.Count == 0)
                return Enumerable.Empty<LocalEvent>();

            string lastCategory = _recentSearches.Peek();
            return GetEventsByCategory(lastCategory).Take(3);
        }

        // Get events filtered by category
        public IEnumerable<LocalEvent> GetEventsByCategory(string category)
        {
            return _events.Values.SelectMany(e => e)
                .Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        // Get all unique categories
        public IEnumerable<string> GetCategories() => _categories.OrderBy(c => c);

        // Get events occurring soon (next 7 days)
        public IEnumerable<LocalEvent> GetUpcomingEvents()
        {
            return _upcomingEvents
                .Where(e => e.Date >= DateTime.Today && e.Date <= DateTime.Today.AddDays(7))
                .OrderBy(e => e.Date)
                .ToList();
        }

        //  Search by category and/or date
        public IEnumerable<LocalEvent> Search(string? category, DateTime? date)
        {
            if (!string.IsNullOrEmpty(category))
                _recentSearches.Push(category); // Track user searches

            var results = GetAllEvents();

            if (!string.IsNullOrEmpty(category))
                results = results.Where(e => e.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

            if (date.HasValue)
                results = results.Where(e => e.Date.Date == date.Value.Date);

            return results;
        }

        // Seed dummy data
        private void SeedDummyData()
        {
            var dummyEvents = new List<LocalEvent>
            {
                new LocalEvent
                {
                    Title = "Community Clean-Up Drive",
                    Category = "Sanitation",
                    Description = "Join us to clean and beautify the local park area.",
                    Date = DateTime.Now.AddDays(1)
                },
                new LocalEvent
                {
                    Title = "Garbage Collection Awareness",
                    Category = "Sanitation",
                    Description = "A talk on improving local waste management practices.",
                    Date = DateTime.Now.AddDays(4)
                },
                new LocalEvent
                {
                    Title = "Road Resurfacing - Main Street",
                    Category = "Roads",
                    Description = "Road resurfacing between 5th and 10th Avenue.",
                    Date = DateTime.Now.AddDays(3)
                },
                new LocalEvent
                {
                    Title = "Pothole Repairs in Residential Area",
                    Category = "Roads",
                    Description = "Minor pothole repairs scheduled for the east district.",
                    Date = DateTime.Now.AddDays(6)
                },
                new LocalEvent
                {
                    Title = "Water Pipe Maintenance",
                    Category = "Utilities",
                    Description = "Scheduled water supply maintenance in the central area.",
                    Date = DateTime.Now.AddDays(2)
                },
                new LocalEvent
                {
                    Title = "Electricity Line Upgrades",
                    Category = "Utilities",
                    Description = "Upgrading power lines in the industrial zone.",
                    Date = DateTime.Now.AddDays(8)
                }
            };

            foreach (var ev in dummyEvents)
            {
                AddEvent(ev);
            }
        }
    }
}
