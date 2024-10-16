using Trains.Shared;

namespace Trains.Tests;

public class TestBase
{
    protected readonly Railway _railway;
    
    public TestBase(Railway railway)
    {
        _railway = railway;
        _railway.AddRoute("AB5");
        _railway.AddRoute("BC4");
        _railway.AddRoute("CD8");
        _railway.AddRoute("DC8");
        _railway.AddRoute("DE6");
        _railway.AddRoute("AD5");
        _railway.AddRoute("CE2");
        _railway.AddRoute("EB3");
        _railway.AddRoute("AE7");
    }
}