using TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

namespace TestContainers.Xunit.IntegrationTests;

public class ContainerFixtureTests(
    HelloWorldContainerFixture helloWorldContainerFixture)
    : IClassFixture<HelloWorldContainerFixture>
{
    [Fact]
    public void Test1()
    {
        var container = helloWorldContainerFixture.Container;
    }
}