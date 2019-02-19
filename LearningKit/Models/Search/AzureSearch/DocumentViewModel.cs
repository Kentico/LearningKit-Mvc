using System.Collections.Generic;

namespace LearningKit.Models.Search.AzureSearch
{
    // Encapsulates document search results
    public class DocumentViewModel
    {
        public string DocumentTitle { get; set; }

        public string DocumentShortDescription { get; set; }

        public IDictionary<string, IList<string>> Highlights { get; set; }
    }
}