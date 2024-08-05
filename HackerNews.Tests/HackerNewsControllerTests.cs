using HackerNews.Controllers;
using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class HackerNewsControllerTests
{
    [Fact]
    public async Task GetSomeData_ReturnsOkResult_WithExpectedData()
    {
        // Arrange
        var expectedData = new List<HackerNewsDTO>();
        var mockService = new Mock<IHackerNewsService>();
        mockService.Setup(service => service.GetNewStoriesAsync()).ReturnsAsync(expectedData);

        var controller = new HackerNewsController(mockService.Object);

        // Act
        var result = await controller.GetNewStories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedData, okResult.Value);
    }
}
