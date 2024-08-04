using HackerNews.Domain.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

public class HackerNewsServiceTests
{
    [Fact]
    public async Task GetNewStoriesAsync_CachesData() //Can't be tested as the stories will keep on changing and no specified test cases can be provided
    {
        // Arrange
        var expectedStories = new List<int> { 1, 2, 3 };
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("[1, 2, 3]"),
            });

        var httpClient = new HttpClient(handlerMock.Object);

        var factoryServiceMock = new Mock<IHttpClientFactoryService>();
        factoryServiceMock.Setup(factory => factory.CreateClient()).Returns(httpClient);

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var service = new HackerNewsService(factoryServiceMock.Object, memoryCache);

        // Act
        var result = await service.GetNewStoriesAsync();
        var cachedResult = await service.GetNewStoriesAsync();

        // Assert
        Assert.Equal(expectedStories, result);
        Assert.Equal(expectedStories, cachedResult);
        factoryServiceMock.Verify(factory => factory.CreateClient(), Times.Once);
    }
}
