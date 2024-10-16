using Microsoft.AspNetCore.Components;
using Trains.Shared;

namespace Trains.Data;

public class Route
{
    public int Id { get; set; }
    public string Path { get; set; }
    private Railway Railway { get; set; }

    public Route(string path)
    {
        Path = path;
    }
    
    public Route(string path, Railway railway)
    {
        Railway = railway;
        Path = path;

        // remove non letters
        Path = new string(Path.Where(char.IsLetter).ToArray());
    }

    public Route(Dictionary<int, string> path, Railway railway)
    {
        Railway = railway;
        Path = string.Join("", path.Values);
    }
    
    public int Length
    {
        get { return Path.Length; }
    }
    
    public int StopCount
    {
        get { return Path.Length - 1; }
    }
    
    public int Distance
    {
        get
        {
            var towns = ToDictionary();
            var totalDistance = 0;

            foreach (var (i, currentTown) in towns)
            {
                var nextIndex = i + 1;
                if (!towns.ContainsKey(nextIndex))
                {
                    break; // we're finished
                }

                var nextTown = towns[nextIndex];
                var dist = Railway.GetDistance(currentTown, nextTown, Direct: true);

                if (dist < 0)
                {
                    return -1;
                }

                totalDistance += dist;
            }


            return totalDistance;
        }
    }

    public Dictionary<int, string> ToDictionary()
    {
        return Path.Select(
            (x, i) => new { i, x }
        ).ToDictionary(
            x => x.i,
            x => x.x.ToString()
        );
    }

    public List<string> ToList()
    {
        return Path.Select(
            x => x.ToString()
        ).ToList();
    }
}