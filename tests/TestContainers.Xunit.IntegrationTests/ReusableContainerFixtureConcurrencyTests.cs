using TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

namespace TestContainers.Xunit.IntegrationTests;

/// <summary>
/// Tries to simulate starting multiple reusable containers concurrently when using a test runner that runs tests concurrently 
/// </summary>
/// <param name="helloWorldContainerFixture"></param>
public class ReusableContainerFixtureConcurrencyTests1(
    ReusableHelloWorldContainerFixture helloWorldContainerFixture)
    : IClassFixture<ReusableHelloWorldContainerFixture>
{
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted2()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted3()
    {
        _ = helloWorldContainerFixture.Container;
    }
}

public class ReusableContainerFixtureConcurrencyTests2(
    ReusableHelloWorldContainerFixture helloWorldContainerFixture)
    : IClassFixture<ReusableHelloWorldContainerFixture>
{
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted2()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted3()
    {
        _ = helloWorldContainerFixture.Container;
    }
}

public class ReusableContainerFixtureConcurrencyTests3(
    ReusableHelloWorldContainerFixture helloWorldContainerFixture)
    : IClassFixture<ReusableHelloWorldContainerFixture>
{
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted2()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldHaveStarted3()
    {
        _ = helloWorldContainerFixture.Container;
    }
}