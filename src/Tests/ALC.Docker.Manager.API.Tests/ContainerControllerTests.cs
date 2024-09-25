using Moq;
using ALC.Docker.Manager.API.Service;
using Microsoft.Extensions.Logging;
using ALC.Docker.Manager.API.Controllers;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

    [Fact]
    public async Task Start_ValidId_ReturnNoContent()
    {
        //Arrange
        var mockContainers = new List<ContainerListResponse> 
        {
            new ContainerListResponse { ID = "1" }
        };

        _mockDockerService.Setup(service => service.GetAll(null))
            .ReturnsAsync(mockContainers);
        
        _mockDockerService.Setup(service => service.Start("1"))
            .Returns(Task.CompletedTask);
        
        //Act
        var result = await _controller.Start("1");

        //Assert
        Assert.IsType<NoContentResult>(result);

    }

    [Fact]
    public async Task Start_InvalidId_ReturnsNotFound()
    {
        //Arrange
        var mockContainers = new List<ContainerListResponse> ();
        _mockDockerService.Setup(service => service.GetAll(null))
            .ReturnsAsync(mockContainers);

        //Act
        var result = await _controller.Start("invalid");

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Stop_ValidId_NoContent()
    {
        //Arrange
        var mockContainers = new List<ContainerListResponse> ()
        {
            new ContainerListResponse { ID = "1"}
        };

        _mockDockerService.Setup(service => service.GetAll(null))
            .ReturnsAsync(mockContainers);

        //Act
        var result = await _controller.Stop("1");

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Stop_Invalid_NotFound()
    {
        //Arrange
        var mockContainers = new List<ContainerListResponse> ();
        _mockDockerService.Setup(service => service.GetAll(null))
            .ReturnsAsync(mockContainers);

        //Act
        var result = await _controller.Stop("invalid");

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

}
