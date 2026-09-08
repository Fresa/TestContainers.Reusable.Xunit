using JetBrains.Annotations;
using Xunit.Sdk;

namespace TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

[UsedImplicitly]
public sealed class ReusableHelloWorldContainerFixture(IMessageSink sink)
    : ContainerFixture<HelloWorldContainerBuilder, HelloWorldContainer>(sink)
{
    protected override bool Reuse => true;

    public KeyValuePair<string, string>? CustomLabel { get; init; }

    protected override HelloWorldContainerBuilder Configure(HelloWorldContainerBuilder builder) =>
        CustomLabel.HasValue
            ? builder.WithLabel(CustomLabel.Value.Key, CustomLabel.Value.Value)
            : builder;

    private readonly HttpClient _httpClient = new();
    public Task<string> GetGuidAsync(CancellationToken cancellation = default)
    {
        var requestUri = new UriBuilder(Uri.UriSchemeHttp, Container.Hostname, Container.GetMappedPublicPort(HelloWorldContainerBuilder.Port), "uuid").Uri;
        return _httpClient.GetStringAsync(requestUri, cancellation);
    }

    public new ValueTask InitializeAsync() => base.InitializeAsync();

    protected override ValueTask DisposeAsyncCore()
    {
        _httpClient.Dispose();
        return base.DisposeAsyncCore();
    }
}