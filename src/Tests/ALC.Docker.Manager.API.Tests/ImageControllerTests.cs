using ALC.Docker.Manager.API.Controllers;
using ALC.Docker.Manager.API.Service;
using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ALC.Docker.Manager.API.Tests;

public class ImageControllerTests
{
    private readonly Mock<IDockerService> _mockDockerService;
    private readonly Mock<ILogger<ImageController>> _mockLogger;
    private readonly ImageController _controller;

    public ImageControllerTests()
    {
        _mockDockerService = new Mock<IDockerService>();
        _mockLogger = new Mock<ILogger<ImageController>>();
        _controller = new ImageController(_mockLogger.Object, _mockDockerService.Object);
    }

    [Fact]
    public async Task List_ReturImages()
    {
        //Arrange
        var mockImages = new List<ImagesListResponse> 
        {
            new ImagesListResponse {},
            new ImagesListResponse {},
        };
        _mockDockerService.Setup(service => service.ListImages(It.IsAny<string>()))
            .ReturnsAsync(mockImages);

        //Act
        var result = await _controller.List();

        //Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task Create_ValidParameters_ReturnCreated()
    {
        //Arrange
        var mockParameter = new ContainerCreateParameters
        {
            Name = "name",
            Image = "image"
        };
        var id = "1";
        _mockDockerService.Setup(service => 
            service.CreateContainerFromImage(
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync(id);
        //Act
        var result = await _controller.Create(mockParameter);

        //Assert
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task Create_InvalidParameters_Return500()
    {
        //Arrange
        var mockParameter = new ContainerCreateParameters();
        _mockDockerService.Setup(service => service
            .CreateContainerFromImage(It.IsAny<string>(), It.IsAny<string>()));

        //Act
        var result = await _controller.Create(mockParameter);

        //Assert
        Assert.Equal(Results.Problem(
                    detail: "Failed creating container",
                    statusCode:500),
                    result);
    }
}
