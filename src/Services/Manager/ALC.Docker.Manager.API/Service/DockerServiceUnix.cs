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

    public async Task<IList<ContainerListResponse>> GetAll(int limit = 10)
    {
        return await client.Containers.ListContainersAsync(
            new ContainersListParameters() {Limit = limit}
        );
    }

}
