# TestContainers.Xunit.Reusable
A test fixture for xUnit V3 supporting [reusable](https://dotnet.testcontainers.org/api/resource_reuse/) containers.

This is a drop-in replacement for [TestContainers.XunitV3](https://www.nuget.org/packages/Testcontainers.XunitV3).

## Installation
```Shell
dotnet add package TestContainers.XunitV3.Reusable
```

https://www.nuget.org/packages/TestContainers.XunitV3.Reusable/

## Getting Started
This library has the same functionality as [TestContainers.XunitV3](https://www.nuget.org/packages/Testcontainers.XunitV3) but also supports test runner parallelism when using reusable containers.
To enable the reuse feature, set the `TESTCONTAINERS_REUSE_ENABLE` environment variable to `true` or override the `Reuse` property of [`ContainerFixture`](src/TestContainers.Xunit/ContainerFixture.cs). The fixture will use the Docker API to guarantee idempotency when the reusable container is created, i.e. only one container with the current set of labels will be created no matter how many test runners start the fixture at the same time. It does so by using the reuse hash as the container name.

You can read more here: https://github.com/testcontainers/testcontainers-dotnet/issues/1482

### Reusable Container Fixture
```dotnet
public sealed class HelloWorldContainerFixture(IMessageSink sink)
    : ContainerFixture<HelloWorldContainerBuilder, HelloWorldContainer>(sink)
{
    protected override bool Reuse => true;    
}
```

# Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

# License
[MIT](LICENSE)