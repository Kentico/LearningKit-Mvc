using System.Collections.Generic;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Components.Web.Mvc.FormComponents;


namespace LearningKit.Models.Widgets.SelectorsWidget
{
    public class SelectorsWidgetProperties : IWidgetProperties
    {
        // Assigns a selector component to the 'Images' property
        [EditingComponent(MediaFilesSelector.IDENTIFIER)]
        // Limits the maximum number of files that can be selected at once.
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        // Returns a list of media files selector items (objects that contain the GUIDs of selected media files)
        public IList<MediaFilesSelectorItem> Images { get; set; }

        // Assigns a selector component to the Pages property
        [EditingComponent(PageSelector.IDENTIFIER)]
        // Returns a list of page selector items (node GUIDs)
        public IList<PageSelectorItem> Pages { get; set; }

        // Assigns a selector component to the PagePaths property
        [EditingComponent(PathSelector.IDENTIFIER)]
        // Returns a list of path selector items (page paths)
        public IList<PathSelectorItem> PagePaths { get; set; }
    }
}