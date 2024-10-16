using Microsoft.AspNetCore.Components;
using Xunit;
using Trains.Shared;
using Xunit.Abstractions;
using Route = Trains.Data.Route;

namespace Trains.Tests;

public class RouteTests : TestBase
{
    public RouteTests(Railway railway) : base(railway)
    {
        //
    }

    [InlineData("ABC", 9)]
    [InlineData("AD", 5)]
    [InlineData("ADC", 13)]
    [InlineData("AEBCD", 22)]
    [InlineData("AED", -1)]
    [Theory]
    public void Distance_ShouldReturnDistanceFromStartToFinish(string path, int expectedDistance)
    {
        // Act
        var route = new Route(path, _railway);

        // Assert
        Assert.Equal(expectedDistance, route.Distance); // A-B (5) + B-C (4) = 9
    }
    
}