using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;

namespace TestContainers.Xunit.IntegrationTests.Fixtures.HelloWorld;

public sealed class HelloWorldContainerBuilder : ContainerBuilder<HelloWorldContainerBuilder, HelloWorldContainer, HelloWorldConfiguration>
{
    private const string Image = "testcontainers/helloworld:1.2.0";
    internal const ushort Port = 8080;
    
    public HelloWorldContainerBuilder()
        : this(new HelloWorldConfiguration())
    {
        DockerResourceConfiguration = Init().DockerResourceConfiguration;
    }
    
    private HelloWorldContainerBuilder(HelloWorldConfiguration dockerResourceConfiguration) : base(dockerResourceConfiguration)
    {
        DockerResourceConfiguration = dockerResourceConfiguration;
    }

    public override HelloWorldContainer Build()
    {
        Validate();
        return new HelloWorldContainer(DockerResourceConfiguration);
    }

    protected override HelloWorldContainerBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new HelloWorldConfiguration(resourceConfiguration));
    }

    protected override HelloWorldContainerBuilder Merge(HelloWorldConfiguration oldValue, HelloWorldConfiguration newValue)
    {
        return new HelloWorldContainerBuilder(new HelloWorldConfiguration(oldValue, newValue));
    }

    protected override HelloWorldConfiguration DockerResourceConfiguration { get; }
    protected override HelloWorldContainerBuilder Clone(IContainerConfiguration resourceConfiguration)
    {
        return Merge(DockerResourceConfiguration, new HelloWorldConfiguration(resourceConfiguration));
    }

    protected override HelloWorldContainerBuilder Init()
    {
        return base.Init()
            .WithImage(Image)
            .WithPortBinding(Port, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)));
    }
}