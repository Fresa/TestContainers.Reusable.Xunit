using AwesomeAssertions;
using TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

namespace TestContainers.Xunit.IntegrationTests;

public class NonReusableContainerFixtureTests(
    NonReusableHelloWorldContainerFixture helloWorldContainerFixture)
    : IClassFixture<NonReusableHelloWorldContainerFixture>
{
    [Fact]
    public void WhenUsingNonReusability_AReusableContainerShouldStartSuccessfully()
    {
        _ = helloWorldContainerFixture.Container;
    }
    
    [Fact]
    public async Task WhenUsingNonReusability_TheContainerShouldRespond()
    {
        var response = await helloWorldContainerFixture.GetGuidAsync(TestContext.Current.CancellationToken)
            .ConfigureAwait(true);
        Guid.TryParse(response, out _).Should().BeTrue();
    }
    
    [Fact]
    public void WhenUsingNonReusability_TheContainerShouldHaveAStartTime()
    {
        var container = helloWorldContainerFixture.Container;
        container.StartedTime.Should().BeBefore(DateTime.UtcNow);
    }
    
    [Fact]
    public void WhenUsingNonReusability_TheContainerShouldNotBeStopped()
    {
        var container = helloWorldContainerFixture.Container;
        container.StoppedTime.Should().Be(DateTime.MinValue);
    }
}