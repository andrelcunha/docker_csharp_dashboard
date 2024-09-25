using ALC.Docker.Manager.API.Service;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Mvc;

namespace ALC.Docker.Manager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IDockerService _dockerService;
        
        public ImageController(ILogger<ImageController> logger, IDockerService dockerService)
        {
            _logger = logger;
            _dockerService = dockerService;
        }

        [HttpGet("List")]
        public async Task<ICollection<ImagesListResponse>> List(){
            var images = await _dockerService.ListImages(string.Empty);
            return images;
        }

        [HttpPost("Create")]
        public async Task<IResult> Create(ContainerCreateParameters parameters)
        {
            var id =  await _dockerService.CreateContainerFromImage(
                parameters.Name,
                parameters.Image
            );
            if (string.IsNullOrEmpty(id))
            {
                return Results.Problem(
                    detail: "Failed creating container",
                    statusCode: 500);
            }
            return Results.Created();
        } 
    }

    public class ContainerCreateParameters
    {
        public string Image {get; set;} = string.Empty;
        public string Name {get;set;} = string.Empty;
    }
}
