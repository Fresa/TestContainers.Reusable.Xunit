using Docker.DotNet.Models;
using DotNet.Testcontainers.Configurations;

namespace TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

public sealed class HelloWorldConfiguration : ContainerConfiguration
{
    public HelloWorldConfiguration()
    {
    }

    public HelloWorldConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public HelloWorldConfiguration(IContainerConfiguration resourceConfiguration)
        : base(resourceConfiguration)
    {
    }

    public HelloWorldConfiguration(HelloWorldConfiguration resourceConfiguration)
        : this(new HelloWorldConfiguration(), resourceConfiguration)
    {
    }

    public HelloWorldConfiguration(HelloWorldConfiguration oldValue, HelloWorldConfiguration newValue)
        : base(oldValue, newValue)
    {
    }
}