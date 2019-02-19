namespace LearningKit.Models.Search.AzureSearch
{
    // Encapsulates facet data
    public class FacetViewModel
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool Selected { get; set; }
    }
}