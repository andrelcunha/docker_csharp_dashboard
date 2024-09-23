using System;
using Docker.DotNet.Models;

namespace ALC.Docker.Manager.API.Service;

public interface IDockerService
{
        public Task<IList<ContainerListResponse>> GetAll(int limit);

}
