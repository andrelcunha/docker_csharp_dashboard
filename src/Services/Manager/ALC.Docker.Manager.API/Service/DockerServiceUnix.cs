using Docker.DotNet;
using Docker.DotNet.Models;

namespace ALC.Docker.Manager.API.Service;

public class DockerServiceUnix:IDockerService
{
    private readonly DockerClient client;
    private readonly string uri = "unix:///var/run/docker.sock";
    public DockerServiceUnix()
    {
        client = new DockerClientConfiguration(new Uri(uri)).CreateClient();
    }

    public async Task<IList<ContainerListResponse>> GetAll(int? limit)
    {
        ContainersListParameters parameters;

        if (limit is not null) 
        {
            parameters = new ContainersListParameters() {Limit = limit};
        }
        else
        {
            parameters = new ContainersListParameters() {All = true };
        }
            
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
