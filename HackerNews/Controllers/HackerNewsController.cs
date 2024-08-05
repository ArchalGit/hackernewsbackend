using HackerNews.Domain.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        /// <summary>
        /// API to get latest new stories with caching of the data for 5 minutes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("NewStories")]
        public async Task<IActionResult> GetNewStories()
        {
            var newStories = await _hackerNewsService.GetNewStoriesAsync();
            return Ok(newStories);
        }

    }
}
