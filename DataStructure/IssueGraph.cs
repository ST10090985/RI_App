using RI_App.Models;
using System.Collections.Generic;

namespace RI_App.DataStructure
{
    // Simple adjacency list graph where each issue node is keyed by Id.
    public class IssueGraph
    {
        // adjacency list keyed by issue id
        private readonly Dictionary<int, List<int>> _adj = new();

        // Add node (if not present)
        public void AddNode(ReportIssue issue)
        {
            if (!_adj.ContainsKey(issue.Id))
                _adj[issue.Id] = new List<int>();
        }

        // Add unweighted edge between two issues (bidirectional)
        public void AddEdge(int id1, int id2)
        {
            if (!_adj.ContainsKey(id1)) _adj[id1] = new List<int>();
            if (!_adj.ContainsKey(id2)) _adj[id2] = new List<int>();

            if (!_adj[id1].Contains(id2)) _adj[id1].Add(id2);
            if (!_adj[id2].Contains(id1)) _adj[id2].Add(id1);
        }

        // BFS traversal starting from a node id -> returns visited ids in order
        public List<int> BFS(int startId)
        {
            var visited = new List<int>();
            if (!_adj.ContainsKey(startId)) return visited;

            var q = new Queue<int>();
            var seen = new HashSet<int>();
            q.Enqueue(startId);
            seen.Add(startId);

            while (q.Count > 0)
            {
                var cur = q.Dequeue();
                visited.Add(cur);
                foreach (var neigh in _adj[cur])
                {
                    if (!seen.Contains(neigh))
                    {
                        seen.Add(neigh);
                        q.Enqueue(neigh);
                    }
                }
            }
            return visited;
        }

        // DFS (recursive) - returns visited order
        public List<int> DFS(int startId)
        {
            var visited = new List<int>();
            if (!_adj.ContainsKey(startId)) return visited;
            var seen = new HashSet<int>();
            DFSRec(startId, seen, visited);
            return visited;
        }

        private void DFSRec(int id, HashSet<int> seen, List<int> visited)
        {
            seen.Add(id);
            visited.Add(id);
            foreach (var neigh in _adj[id])
            {
                if (!seen.Contains(neigh)) DFSRec(neigh, seen, visited);
            }
        }

        // Get neighbors
        public List<int> GetNeighbors(int id) => _adj.TryGetValue(id, out var list) ? list : new List<int>();
    }
}
