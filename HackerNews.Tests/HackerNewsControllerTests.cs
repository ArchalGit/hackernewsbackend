using HackerNews.Controllers;
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
        var expectedData = "{\r\n  \"by\": \"dhouston\",\r\n  \"descendants\": 71,\r\n  \"id\": 8863,\r\n  \"kids\": [9224, 8917, 8884, 8887, 8952, 8869, 8873, 8958, 8940, 8908, 9005, 9671, 9067, 9055, 8865, 8881, 8872, 8955, 10403, 8903, 8928, 9125, 8998, 8901, 8902, 8907, 8894, 8870, 8878, 8980, 8934, 8943, 8876],\r\n  \"score\": 104,\r\n  \"time\": 1175714200,\r\n  \"title\": \"My YC app: Dropbox - Throw away your USB drive\",\r\n  \"type\": \"story\",\r\n  \"url\": \"http://www.getdropbox.com/u/2/screencast.html\"\r\n}";
        var mockService = new Mock<IHackerNewsService>();
        mockService.Setup(service => service.GetItem(It.IsAny<string>())).ReturnsAsync(expectedData);

        var controller = new HackerNewsController(mockService.Object);

        // Act
        var result = await controller.GetItem("8863");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedData, okResult.Value);
    }
}
