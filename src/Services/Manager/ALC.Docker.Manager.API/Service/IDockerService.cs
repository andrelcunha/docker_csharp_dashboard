using System;
using Docker.DotNet.Models;

namespace ALC.Docker.Manager.API.Service;

public interface IDockerService
{
    public Task<IList<ContainerListResponse>> GetAll(int? limit);
    public Task Start(string id);
    public Task Stop(string id);
    public Task<string> GetStatus(string id);
    public Task<ICollection<ImagesListResponse>> ListImages(string value);
    public  Task<string> CreateContainerFromImage(string name, string image);

}
