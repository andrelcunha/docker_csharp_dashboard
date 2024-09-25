using ALC.Docker.Manager.API.Extensions;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Options;

namespace ALC.Docker.Manager.API.Service;

public class DockerServiceUnix:IDockerService
{
    private readonly ILogger<IDockerService> _logger;
    private readonly DockerClient _client;
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
        
        _client = new DockerClientConfiguration(new Uri(_uri)).CreateClient();
    }

    public async Task<IList<ContainerListResponse>> GetAll(int? limit)
    {
        var parameters = limit.HasValue
                ? new ContainersListParameters { Limit = limit.Value }
                : new ContainersListParameters { All = true };
            
        return await _client.Containers.ListContainersAsync(parameters);
    }

    public async Task Start(string id)
    {
        await _client.Containers.StartContainerAsync(id, new ContainerStartParameters());
    } 

    public async Task Stop(string id)
    {
        // var timeToWait = 30;
        var stopped = await _client.Containers.StopContainerAsync(id,
        new ContainerStopParameters
        {
            WaitBeforeKillSeconds = null
        },
        CancellationToken.None);
    }
    
    public async Task<string> GetStatus(string id)
    {
        var containerInspectResponse = await _client.Containers.InspectContainerAsync(id);
        var status = containerInspectResponse.State.Status;
        return status;
    }

    public async Task<ICollection<ImagesListResponse>> ListImages(string value)
    {
        // throw new NotImplementedException();
        var parameters = new ImagesListParameters();
        var images = await _client.Images.ListImagesAsync(parameters);

        return images;
    }

    public async Task<string> CreateContainerFromImage(string name, string image)
    {
        var hostConfig = new HostConfig();
        var parameters = new CreateContainerParameters
        {
            Image = image,
            Name = name,
            HostConfig = hostConfig,

        };

        var container = await _client.Containers.CreateContainerAsync(parameters);
        return container.ID;
    }

    public async Task Delete(string id)
    {
        var parameters = new ContainerRemoveParameters();
        await _client.Containers.RemoveContainerAsync(id,parameters);
    }
}
