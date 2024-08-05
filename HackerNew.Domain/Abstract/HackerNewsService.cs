using HackerNews.Domain.DTO;
using HackerNews.Domain.Interface;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
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

        public async Task<List<HackerNewsDTO>> GetNewStoriesAsync()
        {
            if (!_memoryCache.TryGetValue(NewStoriesCacheKey, out List<HackerNewsDTO> hackernewslist))
            {
                hackernewslist = new List<HackerNewsDTO>();
                var client = _httpClientFactoryService.CreateClient();

                // Fetch the top stories
                string hackernewslisturl = "https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty";
                var response = await client.GetAsync(hackernewslisturl);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the list of story IDs
                var storyIds = JsonSerializer.Deserialize<List<int>>(responseContent);

                // Fetch details for each story in parallel
                var tasks = storyIds.Select(async id =>
                {
                    var storyResponse = await client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json?print=pretty");
                    storyResponse.EnsureSuccessStatusCode();
                    var storyContent = await storyResponse.Content.ReadAsStringAsync();
                    var story = JsonSerializer.Deserialize<HackerNewsDTO>(storyContent);
                    return story;
                });

                var stories = await Task.WhenAll(tasks);
                hackernewslist.AddRange(stories);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(_cacheDuration)
                    .SetAbsoluteExpiration(_cacheDuration);

                _memoryCache.Set(NewStoriesCacheKey, hackernewslist, cacheEntryOptions);
            }

            return hackernewslist;
        }

    }


}
