using JetBrains.Annotations;
using Xunit.Sdk;

namespace TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

[UsedImplicitly]
public sealed class NonReusableHelloWorldContainerFixture(IMessageSink sink)
    : ContainerFixture<HelloWorldContainerBuilder, HelloWorldContainer>(sink)
{
    protected override bool Reuse => false;

    private readonly HttpClient _httpClient = new();
    public Task<string> GetGuidAsync(CancellationToken cancellation = default)
    {
        var requestUri = new UriBuilder(Uri.UriSchemeHttp, Container.Hostname, Container.GetMappedPublicPort(HelloWorldContainerBuilder.Port), "uuid").Uri;
        return _httpClient.GetStringAsync(requestUri, cancellation);
    }

    protected override ValueTask DisposeAsyncCore()
    {
        _httpClient.Dispose();
        return base.DisposeAsyncCore();
    }
}