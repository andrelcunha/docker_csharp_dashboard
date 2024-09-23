using Moq;
using ALC.Docker.Manager.API.Service;
using Microsoft.Extensions.Logging;
using ALC.Docker.Manager.API.Controllers;
using Docker.DotNet.Models;

namespace ALC.Docker.Manager.API.Tests;

public class ContainerControllerTests
{
    private readonly Mock<IDockerService> _mockDockerService;
    private readonly Mock<ILogger<ContainerController>> _mockLogger;
    private readonly ContainerController _controller;

    public ContainerControllerTests()
    {
        _mockDockerService = new Mock<IDockerService>();
        _mockLogger = new Mock<ILogger<ContainerController>>();
        _controller = new ContainerController(_mockLogger.Object, _mockDockerService.Object);
    }

    [Fact]
    public async Task List_ReturnContainers()
    {
        var mockContainers = new List<ContainerListResponse> 
        {
            new ContainerListResponse { ID = "1" },
            new ContainerListResponse { ID = "2" },
        };

        _mockDockerService.Setup(service => service.GetAll(It.IsAny<int?>()))
            .ReturnsAsync(mockContainers);

        //Act
        var result = await _controller.List(2);

        //Assert
        Assert.Equal(2, result.Count);
    }

}
