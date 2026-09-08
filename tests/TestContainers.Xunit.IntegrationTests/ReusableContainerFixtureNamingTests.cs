using AwesomeAssertions;
using TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;
using Xunit.Runner.Common;

namespace TestContainers.Xunit.IntegrationTests;

public sealed class ReusableContainerFixtureNamingTests
{
    [Fact]
    public async Task WhenTheReuseHashStartsWithABase64SpecialCharacter_TheContainerShouldStart()
    {
        var fixture = new ReusableHelloWorldContainerFixture(new AggregateMessageSink())
        {
            // Produces the reuse hash "+Uq/uyzhbemGhyVTL88k8Gcz7kI=" which is an invalid name
            CustomLabel = new KeyValuePair<string, string>("SALTED_KEY", "41")
        };

        await using (fixture.ConfigureAwait(false))
        {
            await fixture.InitializeAsync()
                .ConfigureAwait(true);

            fixture.Container.Name.TrimStart('/')
                .Should().Be("Uq_uyzhbemGhyVTL88k8Gcz7kI_");
        }
    }
}