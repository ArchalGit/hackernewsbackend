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
        /// to get the details of any item be that a story, comment or a post.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("Item")]
        public async Task<dynamic> GetItem(string Id)
        {
            return await _hackerNewsService.GetItem(Id);
        }

        /// <summary>
        /// To get the stories based on type of stories needed.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("Stories")]
        public async Task<ActionResult> GetStories(string Id)
        {
            List<string> allowedValues = new List<string> { "jobstories", "showstories", "askstories", "beststories", "newstories", "topstories" };
            if (allowedValues.Contains(Id))
                return Ok(await _hackerNewsService.GetStories(Id.ToLower()));
            else
            {
                return BadRequest("Value not allowed");
            }
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
