using System.Linq.Expressions;
using AngleSharp.Common;
using Microsoft.Extensions.Options;

namespace Trains.Shared;

public class Railway
{
    public Dictionary<string, Dictionary<string, int>> Routes { get; set; } = new Dictionary<string, Dictionary<string, int>>();
    
    public Railway()
    {
        AddRoute("AB5");
        AddRoute("BC4");
        AddRoute("CD8");
        AddRoute("DC8");
        AddRoute("DE6");
        AddRoute("AD5");
        AddRoute("CE2");
        AddRoute("EB3");
        AddRoute("AE7");
    }

    public void AddRoute(string shorthand)
    {
        // parse the shorthand
        var start = shorthand[0].ToString();
        var end = shorthand[1].ToString();
        var distance = int.Parse(shorthand[2].ToString());

        AddRoute(start, end, distance);
    }

    public void AddRoute(string start, string end, int distance)
    {
        if (!Routes.ContainsKey(start))
        {
            Routes[start] = new Dictionary<string, int>();
        }

        Routes[start][end] = distance;
    }

    public bool HasRoute(string start, string end, bool Direct = false)
    {
        return GetDistance(start, end, Direct: Direct) > 0;
    }

    public int GetDirectDistance(string start, string end)
    {
        return GetDistance(start, end, Direct: true);
    }

    public int GetDistance(string start, string end, string[]? visited = null, bool Direct = false)
    {
        var current = Routes[start];

        if (visited == null)
        {
            visited = [];
        }

        if (DirectPathExists(current, end))
        {
            return current[end];
        }

        // if we are looking for a direct path from start to end, then fail
        if (Direct)
        {
            return -1;
        }

        current = FilterVisited(current, visited);

        foreach (var neighbor in current.Keys)
        {
            var d = GetDistance(neighbor, end, visited.Append(start).ToArray(), Direct);
            // check if there is a route from the neighbor to the end node
            if (d > 0)
            {
                return d;
            }
        }

        return -1;
    }

    public int GetRouteDistance(Route route)
    {
        return route.Distance;
    }

    public int FindShortestRoute(string start, string end)
    {
        return GetRoutes(start, end)
            .OrderBy(x => x.Distance)
            .First()?
            .Distance ?? -1;
    }

    private Dictionary<string, int> FilterVisited(Dictionary<string, int> current, string[] visited)
    {
        return current
            .Where(x => !visited.Contains(x.Key))
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private bool DirectPathExists(Dictionary<string, int> current, string end)
    {
        return current.ContainsKey(end);
    }

    // Given a start and end node, return a list of all possible routes
    public List<Route> GetRoutes(string start, string end, int maxStops = 10, bool exact = false,
        int? maxDistance = null)
    {
        if (!Routes.ContainsKey(start))
        {
            throw new ArgumentException($"Start node {start} not found");
        }
        
        if(!Routes.ContainsKey(end))
        {
            throw new ArgumentException($"End node {end} not found");
        }
        
        
        List<Route> routes = new List<Route>();

        DepthFirstSearch(start, end, start, routes, maxStops);

        if (maxStops != null)
        {
            routes = exact
                ? routes.Where(x => x.StopCount == maxStops).ToList()
                : routes.Where(x => x.StopCount <= maxStops).ToList();
        }

        if (maxDistance != null)
        {
            routes = routes.Where(x => x.Distance < maxDistance).ToList();
        }

        return routes;
    }


    // recursive method for DFS traversal
    private void DepthFirstSearch(string current, string destination, string currentPath, List<Route> routes, int maxStops = 10)
    {
        // Base case: if current town (not start) is the destination, add to routes
        if (currentPath.Length > 1 && current == destination)
        {
            routes.Add(new Route(currentPath, this));
        }
        
        if (currentPath.Length > maxStops)
        {
            return;
        }

        if (Routes.ContainsKey(current))
        {
            foreach (var neighbor in Routes[current])
            {
                // Continue the DFS if the next town isn't already in the current path (avoiding revisits)
                DepthFirstSearch(neighbor.Key, destination, currentPath + neighbor.Key, routes);
            }
        }
    }
}