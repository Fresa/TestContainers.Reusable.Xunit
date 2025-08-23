using AwesomeAssertions;
using TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

namespace TestContainers.Xunit.IntegrationTests;

public class ReusableContainerFixtureTests(
    ReusableHelloWorldContainerFixture helloWorldContainerFixture)
    : IClassFixture<ReusableHelloWorldContainerFixture>
{
    [Fact]
    public void WhenUsingReusability_AReusableContainerShouldStartSuccessfully()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public async Task WhenUsingReusability_TheContainerShouldRespond()
    {
        var response = await helloWorldContainerFixture.GetGuidAsync(TestContext.Current.CancellationToken)
            .ConfigureAwait(true);
        Guid.TryParse(response, out _).Should().BeTrue();
    }
    
    [Fact]
    public void WhenUsingReusability_TheContainerShouldHaveAStartTime()
    {
        var container = helloWorldContainerFixture.Container;
        container.StartedTime.Should().BeBefore(DateTime.UtcNow);
    }
    
    [Fact]
    public void WhenUsingReusability_TheContainerShouldNotBeStopped()
    {
        var container = helloWorldContainerFixture.Container;
        container.StoppedTime.Should().Be(DateTime.MinValue);
    }
}