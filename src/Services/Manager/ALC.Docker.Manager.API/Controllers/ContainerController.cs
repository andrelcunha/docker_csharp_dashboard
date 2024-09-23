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

        [HttpGet("list")]
        public async Task<ICollection<Container>> List(int limit = 10)
        {
            var containers = (await dockerService.GetAll(limit)).ToList()
                .ConvertAll(c => new Container(c, Models.Environment.Docker));
            return containers;
        }

        [HttpGet("start/{id}")]
        public async Task<IActionResult> Start(string id)
        {
            var containers = await dockerService.GetAll(null);

            var container = containers.FirstOrDefault(c => c.ID.Contains(id, StringComparison.OrdinalIgnoreCase));
            if (container is null)
            {
                return NotFound();
            }
            await dockerService.Start(container.ID);
            return NoContent();
        }

        [HttpGet("stop/{id}")]
        public async Task<IActionResult> Stop(string id)
        {
            var containers = await dockerService.GetAll(null);

            var container = containers.FirstOrDefault(c => c.ID.Contains(id, StringComparison.OrdinalIgnoreCase));
            if (container is null)
            {
                return NotFound();
            }
            await dockerService.Stop(container.ID);
            return NoContent();
        }

    }
}
