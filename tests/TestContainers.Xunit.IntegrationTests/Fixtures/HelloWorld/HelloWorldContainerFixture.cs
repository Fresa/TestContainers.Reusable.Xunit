using JetBrains.Annotations;
using Xunit.Sdk;

namespace TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

[UsedImplicitly]
public sealed class HelloWorldContainerFixture(IMessageSink sink)
    : ContainerFixture<HelloWorldContainerBuilder, HelloWorldContainer>(sink);