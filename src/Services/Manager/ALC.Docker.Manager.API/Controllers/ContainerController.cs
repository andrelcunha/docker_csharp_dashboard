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
        private readonly IDockerService _dockerService;
        public ContainerController(ILogger<ContainerController> logger, IDockerService dockerService)
        {
            _logger = logger;
            _dockerService = dockerService;
        }

        [HttpGet("list")]
        public async Task<ICollection<Container>> List(int limit = 10)
        {
            var containers = (await _dockerService.GetAll(limit)).ToList()
                .ConvertAll(c => new Container(c, Models.Environment.Docker));
            return containers;
        }

        [HttpGet("start/{id}")]
        public async Task<IActionResult> Start(string id)
        {
            var containers = await _dockerService.GetAll(null);

            var container = containers.FirstOrDefault(c => c.ID.Contains(id, StringComparison.OrdinalIgnoreCase));
            if (container is null)
            {
                return NotFound();
            }
            await _dockerService.Start(container.ID);
            return NoContent();
        }

        [HttpGet("stop/{id}")]
        public async Task<IActionResult> Stop(string id)
        {
            var containers = await _dockerService.GetAll(null);

            var container = containers.FirstOrDefault(c => c.ID.Contains(id, StringComparison.OrdinalIgnoreCase));
            if (container is null)
            {
                return NotFound();
            }
            await _dockerService.Stop(container.ID);
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var containers = await _dockerService.GetAll(null);

            var container = containers.FirstOrDefault(c => c.ID.Contains(id, StringComparison.OrdinalIgnoreCase));
            if (container is null)
            {
                return NotFound();
            }
            await _dockerService.Delete(container.ID);
            return Ok();
        }

    }
}
