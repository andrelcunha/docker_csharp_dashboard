using ALC.Docker.Manager.API.Service;
using ALC.Docker.Manager.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ALC.Docker.Manager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly ILogger<ContainerController> _logger;
        private readonly IDockerService dockerService;
        public ContainerController(ILogger<ContainerController> logger)
        {
            _logger = logger;
            dockerService = new DockerServiceUnix();
        }

        [HttpGet]
        public async Task<ICollection<Container>> GetContainers(int limit = 10){
            var containers =  (await dockerService.GetAll(limit)).ToList()
                .ConvertAll(c => new Container(c, Models.Environment.Docker));
            return containers;
        }


    }
}
