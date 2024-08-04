using HackerNews.Domain.Interface;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace HackerNews.Domain.Abstract
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);
        private const string NewStoriesCacheKey = "NewStories";
        public HackerNewsService(IHttpClientFactoryService httpClientFactoryService, IMemoryCache memoryCache)
        {
            _httpClientFactoryService = httpClientFactoryService;
            _memoryCache = memoryCache;
        }
        public async Task<dynamic> GetItem(string Id)
        {
            var client = _httpClientFactoryService.CreateClient();
            var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/item/" + Id + ".json?print=pretty");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<dynamic> GetStories(string Id)
        {
            var client = _httpClientFactoryService.CreateClient();
            var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/" + Id + ".json?print=pretty");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<int>> GetNewStoriesAsync()
        {
            if (!_memoryCache.TryGetValue(NewStoriesCacheKey, out IEnumerable<int> newStories))
            {
                var client = _httpClientFactoryService.CreateClient();
                var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty");
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                newStories = JsonSerializer.Deserialize<IEnumerable<int>>(responseContent);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration)
                    .SetAbsoluteExpiration(_cacheDuration);

                _memoryCache.Set(NewStoriesCacheKey, newStories, cacheEntryOptions);
            }

            return newStories;
        }
    }
}
