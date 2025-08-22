using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

namespace TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

public sealed class HelloWorldContainer(IContainerConfiguration configuration) : DockerContainer(configuration);