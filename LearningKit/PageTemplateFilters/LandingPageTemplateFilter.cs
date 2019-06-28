using System;
using System.Collections.Generic;
using System.Linq;

using Kentico.PageBuilder.Web.Mvc.PageTemplates;

namespace LearningKit.PageTemplateFilters
{
    public class LandingPageTemplateFilter : IPageTemplateFilter
    {
        public IEnumerable<PageTemplateDefinition> Filter(IEnumerable<PageTemplateDefinition> pageTemplates, PageTemplateFilterContext context)
        {
            // Applies filtering to a collection of page templates based on the page type of the currently edited page
            if (context.PageType.Equals("LearningKit.LandingPage", StringComparison.InvariantCultureIgnoreCase))
            {
                // Filters the collection to only contain filters allowed for landing pages
                return pageTemplates.Where(t => GetLandingPageTemplates().Contains(t.Identifier));
            }

            // Excludes all landing page templates from the collection if the context does not match this filter
            // Assumes that the categories of page templates are mutually exclusive
            return pageTemplates.Where(t => !GetLandingPageTemplates().Contains(t.Identifier));
        }

        // Gets all page templates that are allowed for landing pages
        public IEnumerable<string> GetLandingPageTemplates() => new string[] { "LearningKit.LandingPageTemplate" };
    }
}