using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusNet.Models
{
    // Clase que representa un grafo dirigido para la red social.
    public class SocialGraph
    {
        private readonly Dictionary<string, User> _vertices;
        private readonly Dictionary<string, List<string>> _adjacencyList;

        public SocialGraph()
        {
            _vertices = new Dictionary<string, User>();
            _adjacencyList = new Dictionary<string, List<string>>();
        }

        public bool AddUser(User user)
        {
            if (user == null || _vertices.ContainsKey(user.Id))
                return false;

            _vertices[user.Id] = user;
            _adjacencyList[user.Id] = new List<string>();
            return true;
        }

        public bool RemoveUser(string userId)
        {
            if (!_vertices.ContainsKey(userId))
                return false;

            _vertices.Remove(userId);
            _adjacencyList.Remove(userId);

            foreach (var key in _adjacencyList.Keys)
            {
                _adjacencyList[key].Remove(userId);
            }

            return true;
        }

        public bool UpdateUser(string userId, string newName, UserRole newRole)
        {
            if (!_vertices.ContainsKey(userId))
                return false;

            _vertices[userId].Name = newName;
            _vertices[userId].Role = newRole;
            return true;
        }

        public User? GetUser(string userId)
        {
            return _vertices.TryGetValue(userId, out var user) ? user : null;
        }

        public IEnumerable<User> GetAllUsers() => _vertices.Values;

        public bool AddFollowRelation(string followerId, string followedId)
        {
            if (!_vertices.ContainsKey(followerId) || !_vertices.ContainsKey(followedId))
                return false;

            if (followerId == followedId)
                return false;

            if (_adjacencyList[followerId].Contains(followedId))
                return false;

            _adjacencyList[followerId].Add(followedId);
            return true;
        }

        public bool RemoveFollowRelation(string followerId, string followedId)
        {
            if (!_adjacencyList.ContainsKey(followerId))
                return false;

            return _adjacencyList[followerId].Remove(followedId);
        }

        public Dictionary<string, List<string>> GetAdjacencyList()
        {
            return _adjacencyList.ToDictionary(kvp => kvp.Key, kvp => new List<string>(kvp.Value));
        }

        public int GetOutDegree(string userId)
        {
            return _adjacencyList.ContainsKey(userId) ? _adjacencyList[userId].Count : 0;
        }

        public int GetInDegree(string userId)
        {
            return _adjacencyList.Values.Count(list => list.Contains(userId));
        }

        public List<User> GetUsersWithZeroInDegree()
        {
            return _vertices.Values
                .Where(u => GetInDegree(u.Id) == 0)
                .ToList();
        }

        public List<User> GetMostInfluentialUsers()
        {
            if (!_vertices.Any()) return new List<User>();
            int maxInDegree = _vertices.Keys.Max(GetInDegree);
            return _vertices.Values.Where(u => GetInDegree(u.Id) == maxInDegree).ToList();
        }

        public List<User> GetMostActiveUsers()
        {
            if (!_vertices.Any()) return new List<User>();
            int maxOutDegree = _vertices.Keys.Max(GetOutDegree);
            return _vertices.Values.Where(u => GetOutDegree(u.Id) == maxOutDegree).ToList();
        }

        public (List<string> Order, int Count) BFS(string startId)
        {
            var visitedOrder = new List<string>();
            if (!_vertices.ContainsKey(startId))
                return (visitedOrder, 0);

            var visited = new HashSet<string>();
            var queue = new Queue<string>();

            visited.Add(startId);
            queue.Enqueue(startId);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                visitedOrder.Add(current);

                foreach (string neighbor in _adjacencyList[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return (visitedOrder, visitedOrder.Count);
        }

        public bool IsReachableBFS(string startId, string targetId)
        {
            var (visitedOrder, _) = BFS(startId);
            return visitedOrder.Contains(targetId);
        }

        public (List<string> DiscoveryOrder, bool HasCycles, List<string> CyclePath) DFSFull()
        {
            var discoveryOrder = new List<string>();
            var visitedState = new Dictionary<string, int>();
            bool hasCycle = false;
            var cyclePath = new List<string>();

            foreach (var userId in _vertices.Keys)
            {
                visitedState[userId] = 0;
            }

            foreach (var userId in _vertices.Keys)
            {
                if (visitedState[userId] == 0)
                {
                    if (DFSVisit(userId, visitedState, discoveryOrder, cyclePath))
                    {
                        hasCycle = true;
                    }
                }
            }

            return (discoveryOrder, hasCycle, cyclePath);
        }

        private bool DFSVisit(string u, Dictionary<string, int> visitedState, List<string> discoveryOrder, List<string> cyclePath)
        {
            visitedState[u] = 1;
            discoveryOrder.Add(u);
            bool cycleDetected = false;

            foreach (string v in _adjacencyList[u])
            {
                if (visitedState[v] == 1)
                {
                    cyclePath.Add($"{u} -> {v}");
                    cycleDetected = true;
                }
                else if (visitedState[v] == 0)
                {
                    if (DFSVisit(v, visitedState, discoveryOrder, cyclePath))
                        cycleDetected = true;
                }
            }

            visitedState[u] = 2;
            return cycleDetected;
        }
    }
}