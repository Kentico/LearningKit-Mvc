using System.Collections.Generic;

using CMS.Search;

namespace LearningKit.Models.Search.SmartSearch
{
    //DocSection:SearchResultModel
    public class SearchResultModel
    {
        public string Query { get; set; }
        
        public IEnumerable<SearchResultItem> Items { get; set; }
    }
    //EndDocSection:SearchResultModel
}