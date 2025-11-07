using RI_App.Models;
using System.Collections.Generic;

namespace RI_App.DataStructure
{
    public class ServiceRequestHeap
    {
        private readonly List<ServiceRequest> _heap = new();

        private int Compare(ServiceRequest a, ServiceRequest b)
        {
            // Higher priority requests come first
            // You can adjust logic: 1 = Low, 2 = Medium, 3 = High, etc.
            return b.Priority.CompareTo(a.Priority);
        }

        public void Insert(ServiceRequest request)
        {
            _heap.Add(request);
            HeapifyUp(_heap.Count - 1);
        }

        public ServiceRequest ExtractHighestPriority()
        {
            if (_heap.Count == 0) return null;

            var root = _heap[0];
            _heap[0] = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);
            HeapifyDown(0);
            return root;
        }

        public List<ServiceRequest> GetAll()
        {
            return new List<ServiceRequest>(_heap);
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (Compare(_heap[index], _heap[parentIndex]) <= 0)
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
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

                if (left <= lastIndex && Compare(_heap[left], _heap[largest]) > 0)
                    largest = left;
                if (right <= lastIndex && Compare(_heap[right], _heap[largest]) > 0)
                    largest = right;

                if (largest == index)
                    break;

                Swap(index, largest);
                index = largest;
            }
        }

        private void Swap(int i, int j)
        {
            var temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }

    }
}
