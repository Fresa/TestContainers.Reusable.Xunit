using System.Net;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Docker.DotNet;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using TestContainers.Xunit.Observability.Logs;
using Xunit;
using Xunit.Sdk;

namespace TestContainers.Xunit;

/// <summary>
/// Reusable container fixture with multiprocess/threading support
/// </summary>
/// <typeparam name="TBuilderEntity">Container builder</typeparam>
/// <typeparam name="TContainerEntity">Container</typeparam>
public abstract class ContainerFixture
    <TBuilderEntity, TContainerEntity> : IAsyncLifetime
    where TBuilderEntity : IContainerBuilder<TBuilderEntity, TContainerEntity>, new()
    where TContainerEntity : IContainer
{
    /// <summary>
    /// Reuse the container?
    /// Defaults to the environment variable TESTCONTAINERS_REUSE_ENABLE or false
    /// </summary>
    protected virtual bool Reuse { get; } = 
        bool.Parse(Environment.GetEnvironmentVariable("TESTCONTAINERS_REUSE_ENABLE") ?? "false");
    
    private readonly Lazy<TContainerEntity> _container;

    private ExceptionDispatchInfo? _exception;

    /// <summary>
    /// Initializes a new instance of ContainerFixture
    /// </summary>
    /// <param name="sink">Message sink for diagnostics</param>
    protected ContainerFixture(IMessageSink sink)
    {
        _container = new Lazy<TContainerEntity>(() =>
            ConfigureName(
                    Configure(new TBuilderEntity()
                        .WithLogger(new MessageSinkLogger(sink))
                        .WithReuse(Reuse)))
                .Build());
    }

    /// <summary>
    /// Resolves the container
    /// </summary>
    public TContainerEntity Container
    {
        get
        {
            _exception?.Throw();
            return _container.Value;
        }
    }

    /// <inheritdoc />
    ValueTask IAsyncLifetime.InitializeAsync() => InitializeAsync();

    /// <inheritdoc />
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await DisposeAsyncCore()
            .ConfigureAwait(false);

        GC.SuppressFinalize(this);
    }
    
    /// <summary>
    /// Configure the container
    /// </summary>
    /// <param name="builder">The builder used to configure the container</param>
    /// <returns>The configured container builder</returns>
    protected virtual TBuilderEntity Configure(TBuilderEntity builder) => builder;

    private TBuilderEntity ConfigureName(TBuilderEntity builder)
    {
        if (!Reuse)
        {
            return builder;
        }

        var configuration = ResolveContainerConfiguration(builder);
        return builder.WithName(configuration.GetReuseHash()
            // Replace invalid base64 characters 
            .Replace('=', '_')
            .Replace('/', '_')
            .Replace('+', '_'));
    }

    private static IContainerConfiguration ResolveContainerConfiguration(TBuilderEntity builder)
    {
        const string dockerResourceConfigurationPropertyName = "DockerResourceConfiguration";
        var builderType = typeof(TBuilderEntity);
            
        // There is no way to get this information without reflection :( 
        var property = builderType.GetProperty(dockerResourceConfigurationPropertyName,
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        if (property == null)
        {
            throw new InvalidOperationException(
                $"Could not locate property {dockerResourceConfigurationPropertyName} on {builderType}");
        }

        var propertyValue = property.GetValue(builder);
        if (propertyValue == null)
        {
            throw new InvalidOperationException(
                $"The property {dockerResourceConfigurationPropertyName} has not been set");
        }

        if (propertyValue is not IContainerConfiguration configuration)
        {
            throw new InvalidOperationException(
                $"The property {dockerResourceConfigurationPropertyName} is not of type {typeof(IContainerConfiguration)}");
        }
        return configuration;
    }

    /// <inheritdoc cref="IAsyncLifetime" />
    protected virtual async ValueTask InitializeAsync()
    {
        try
        {
            // Trigger any container configuration validation issues
            _ = Container;
        }
        catch (Exception ex)
        {
            _exception = ExceptionDispatchInfo.Capture(ex);
            return;
        }

        var startAttempts = Reuse ? 5 : 1;
        do
        {
            try
            {
                await Container.StartAsync()
                    .ConfigureAwait(false);
                return;
            }
            // Handles optimistic concurrency when two test runners
            // try to start the same reusable container at the same time.
            catch (DockerApiException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
            {
                startAttempts--;
                if (startAttempts <= 0)
                {
                    throw;
                }
                Thread.SpinWait(1);
            }
        } while (Reuse);
    }

    /// <inheritdoc cref="IAsyncLifetime" />
    protected virtual ValueTask DisposeAsyncCore() => 
        _exception == null && !Reuse ? 
            Container.DisposeAsync() : 
            ValueTask.CompletedTask;
}