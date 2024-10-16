using Microsoft.EntityFrameworkCore;
using Trains.Data;
using Xunit;
using Trains.Shared;
using Xunit.Abstractions;
using Route = Trains.Data.Route;

namespace Trains.Tests;

public class RailwayTests : TestBase
{
    public RailwayTests(Railway railway) : base(railway)
    {
        //
    }

    [InlineData("A", "B")]
    [InlineData("B", "B")]
    [InlineData("C", "B")]
    [InlineData("D", "B")]
    [InlineData("E", "B")]
    [Theory]
    public void HasRoute_ShouldReturnTrueForExistingRoute(string start, string end)
    {
        // Act
        var result = _railway.HasRoute(start, end);

        // Assert
        Assert.True(result);
    }

    // There are no routes to A
    [InlineData("A", "A")]
    [InlineData("B", "A")]
    [InlineData("C", "A")]
    [InlineData("D", "A")]
    [InlineData("E", "A")]
    [Theory]
    public void HasRoute_ShouldReturnFalseForNonExistentRoute(string start, string end)
    {
        // Act
        var result = _railway.HasRoute(start, end);

        // Assert
        Assert.False(result);
    }

    
    /**
     * The number of trips starting at C and ending at C with a maximum of 3
       stops. In the test input, there are two such trips: C-D-C (2 stops). and C-E-B-C
       (3 stops). 
     */
    [Fact]
    public void GetRoutes_ShouldFilterRoutesByMaxStops()
    {
        // Act
        var result = _railway.GetRoutes("C", "C", maxStops: 3);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Path == "CDC");
        Assert.Contains(result, x => x.Path == "CEBC");
        
    }

    /**
     * The number of trips starting at A and ending at C with exactly 4 stops. In the
       test input, there are three such trips: A to C (via B,C,D); A to C (via D,C,D); and
       A to C (via D,E,B).
     */
    [Fact]
    public void GetRoutes_ShouldFilterByExactStops()
    {
        // Act
        var result = _railway.GetRoutes("A", "C", maxStops: 4, exact: true);
        
        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, x => x.Path == "ABCDC");
        Assert.Contains(result, x => x.Path == "ADCDC");
        Assert.Contains(result, x => x.Path == "ADEBC");
    }

    /**
     * The length of the shortest route (in terms of distance to travel) from A to C.
       The length of the shortest route (in terms of distance to travel) from B to B.
     */
    [InlineData("A", "C", 9)]
    [InlineData("B", "B", 9)]
    [Theory]
    public void FindShortestShortestRoute_ShouldReturnShortestDistanceForRoute(string start, string end, int expected)
    {
        // Act
        var result = _railway.FindShortestRoute(start, end);

        // Assert
        Assert.Equal(expected, result); 
    }
    
    [Fact]
    public void GetDistance_ShouldReturnNegativeOneResponseForNonExistingRoute()
    {
        // Act & Assert
        var dist = _railway.GetDirectDistance("A", "C"); 
        
        Assert.Equal(-1, dist);
    }
}