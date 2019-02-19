using System.Collections.Generic;

namespace LearningKit.Models.Search.AzureSearch
{
    // Encapsulates search request data and search results
    public class AzureSearchViewModel
    {        
        public string SearchString { get; set; }

        public IList<DocumentViewModel> SearchResults { get; set; }

        public IList<FacetViewModel> FilterFarm { get; set; }

        public IList<FacetViewModel> FilterCountry { get; set; }

        public AzureSearchViewModel()
        {
            FilterCountry = new List<FacetViewModel>();
            FilterFarm = new List<FacetViewModel>();
            SearchResults = new List<DocumentViewModel>();
        }
    }
}