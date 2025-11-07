using RI_App.Models;
using System.Collections.Generic;

namespace RI_App.DataStructure
{
    public class ServiceRequestHeap
    {
        private List<ServiceRequest> _heap = new();

        // Insert request into heap
        public void Insert(ServiceRequest request)
        {
            _heap.Add(request);
            HeapifyUp(_heap.Count - 1);
        }

        // Get highest priority request (e.g., earliest submitted)
        public ServiceRequest ExtractMin()
        {
            if (_heap.Count == 0) return null;

            var min = _heap[0];
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);
            HeapifyDown(0);

            return min;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (_heap[index].CreatedDate < _heap[parent].CreatedDate)
                {
                    Swap(index, parent);
                    index = parent;
                }
                else break;
            }
        }

        private void HeapifyDown(int index)
        {
            int lastIndex = _heap.Count - 1;
            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int smallest = index;

                if (left <= lastIndex && _heap[left].CreatedDate < _heap[smallest].CreatedDate)
                    smallest = left;
                if (right <= lastIndex && _heap[right].CreatedDate < _heap[smallest].CreatedDate)
                    smallest = right;

                if (smallest != index)
                {
                    Swap(index, smallest);
                    index = smallest;
                }
                else break;
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