using RI_App.Models;
using System.Collections.Generic;

namespace RI_App.DataStructure
{
    // Min-heap based on ReportIssue.Priority (lower number = lower priority; we treat higher number as more urgent, so we invert)
    public class ReportIssueMinHeap
    {
        private readonly List<ReportIssue> _heap = new();

        public int Count => _heap.Count;

        public void Insert(ReportIssue issue)
        {
            _heap.Add(issue);
            HeapifyUp(_heap.Count - 1);
        }

        public ReportIssue? Peek()
        {
            return _heap.Count > 0 ? _heap[0] : null;
        }

        public ReportIssue? ExtractMaxPriority()
        {
            if (_heap.Count == 0) return null;

            // We want highest Priority number first -> treat as max-heap by Priority
            var root = _heap[0];
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);
            HeapifyDown(0);
            return root;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_heap[index].Priority <= _heap[parent].Priority) break;
                Swap(index, parent);
                index = parent;
            }
        }

        private void HeapifyDown(int index)
        {
            int last = _heap.Count - 1;
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int largest = index;

                if (left <= last && _heap[left].Priority > _heap[largest].Priority) largest = left;
                if (right <= last && _heap[right].Priority > _heap[largest].Priority) largest = right;
                if (largest == index) break;
                Swap(index, largest);
                index = largest;
            }
        }

        private void Swap(int a, int b)
        {
            var tmp = _heap[a];
            _heap[a] = _heap[b];
            _heap[b] = tmp;
        }

        public List<ReportIssue> ToListOrderedByPriority()
        {
            // Create a shallow copy to pop items without mutating original heap
            var copy = new ReportIssueMinHeap();
            foreach (var i in _heap) copy.Insert(i);

            var result = new List<ReportIssue>();
            while (copy.Count > 0)
            {
                var item = copy.ExtractMaxPriority();
                if (item != null) result.Add(item);
            }
            return result;
        }
    }
}
