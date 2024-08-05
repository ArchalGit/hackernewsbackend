using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Domain.DTO
{
    public class HackerNewsDTO
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>
        /// Title.
        /// </value>
        public int id { get; set; }
        /// <summary>
        /// Gets or sets the Title.
        /// </summary>
        /// <value>
        /// Title.
        /// </value>
        public string title { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// URL
        /// </value>
        public string url { get; set; }
    }
}
