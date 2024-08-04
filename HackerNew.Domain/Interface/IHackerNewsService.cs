using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.Interface
{
    public interface IHackerNewsService
    {
        Task<dynamic> GetItem(string Id);
        Task<dynamic> GetStories(string Id);
        Task<IEnumerable<int>> GetNewStoriesAsync();
    }
}
