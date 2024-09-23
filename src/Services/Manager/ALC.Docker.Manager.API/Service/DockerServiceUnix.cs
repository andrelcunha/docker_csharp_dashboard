using ALC.Docker.Manager.API.Extensions;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Options;

namespace ALC.Docker.Manager.API.Service;

public class DockerServiceUnix:IDockerService
{
    private readonly ILogger<IDockerService> _logger;
    private readonly DockerClient client;
    private readonly string _uri = string.Empty;//= "unix:///var/run/docker.sock";

    public DockerServiceUnix(ILogger<IDockerService>  logger, IOptions<AppSetting> appSettings)
    {
        _logger = logger;
#pragma warning disable CS8601 // Possible null reference assignment.
        _uri = appSettings.Value.DockerClientDockerUri;

#pragma warning restore CS8601 // Possible null reference assignment.
        if (string.IsNullOrEmpty(_uri)) 
        {
            logger.LogCritical("The DockerClientDockerUri setting is empty or not found");
            Environment.Exit(1);
        }
        
        client = new DockerClientConfiguration(new Uri(_uri)).CreateClient();
    }

    public async Task<IList<ContainerListResponse>> GetAll(int? limit)
    {
        var parameters = limit.HasValue
                ? new ContainersListParameters { Limit = limit.Value }
                : new ContainersListParameters { All = true };
            
        return await client.Containers.ListContainersAsync(parameters);
    }

    public async Task Start(string id)
    {
        await client.Containers.StartContainerAsync(id, new ContainerStartParameters());
    } 

    public async Task Stop(string id)
    {
        // var timeToWait = 30;
        var stopped = await client.Containers.StopContainerAsync(id,
        new ContainerStopParameters
        {
            WaitBeforeKillSeconds = null
        },
        CancellationToken.None);
    }

}
