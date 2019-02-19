using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

using CMS.DocumentEngine;

using Kentico.Forms.Web.Mvc;

using LearningKit.Models.FormComponents;
using LearningKit.Models.FormComponents.FormComponentProperties;


[assembly: RegisterFormComponent(CustomDropDownComponent.IDENTIFIER, typeof(CustomDropDownComponent), "Drop-down with custom data", IconClass = "icon-menu")]

namespace LearningKit.Models.FormComponents
{
    public class CustomDropDownComponent : SelectorFormComponent<CustomDropDownComponentProperties>
    {
        public const string IDENTIFIER = "CustomDropDownComponent";


        // Retrieves data to be displayed in the selector
        protected override IEnumerable<SelectListItem> GetItems()
        {
            // Perform data retrieval operations here
            // The following example retrieves all pages of the 'DancingGoatMvc.Article' page type 
            // located under the 'Articles' section of the Dancing Goat sample website
            DocumentQuery query = DocumentHelper.GetDocuments("DancingGoatMvc.Article")
                                .Path("/Articles/", PathTypeEnum.Children)
                                .Columns("DocumentName", "DocumentGUID")
                                .OnSite("DancingGoatMvc")
                                .Culture("en-us")
                                .LatestVersion();

            var sampleData = query.TypedResult.Select(x => new { Name = x.DocumentName,
                                                                 Guid = x.DocumentGUID.ToString() });

            // Iterates over retrieved data and transforms it into SelectListItems
            foreach (var item in sampleData)
            {
                var listItem = new SelectListItem()
                {
                    Value = item.Guid,
                    Text = item.Name
                };

                yield return listItem;
            }
        }
    }
}