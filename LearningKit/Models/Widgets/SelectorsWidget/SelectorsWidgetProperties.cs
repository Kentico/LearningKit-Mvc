using System.Collections.Generic;
using System.Linq;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Components.Web.Mvc.FormComponents;


namespace LearningKit.Models.Widgets.SelectorsWidget
{
    public class SelectorsWidgetProperties : IWidgetProperties
    {
        // Assigns a selector component to the Images property
        [EditingComponent(MediaFilesSelector.IDENTIFIER)]
        // Limits the maximum number of files that can be selected at once.
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 1)]
        // Returns a list of media files selector items (objects that contain the GUIDs of selected media files)
        public IEnumerable<MediaFilesSelectorItem> Images { get; set; } = Enumerable.Empty<MediaFilesSelectorItem>();

        // Assigns a selector component to the Pages property
        [EditingComponent(PageSelector.IDENTIFIER)]
        // Returns a list of page selector items (node GUIDs)
        public IEnumerable<PageSelectorItem> Pages { get; set; } = Enumerable.Empty<PageSelectorItem>();

        // Assigns a selector component to the Attachments property
        [EditingComponent(AttachmentSelector.IDENTIFIER)]
        // Returns a list of attachment selector items
        public IEnumerable<AttachmentSelectorItem> Attachments { get; set; } = Enumerable.Empty<AttachmentSelectorItem>();

        // Assigns a selector component to the PagePaths property
        [EditingComponent(PathSelector.IDENTIFIER)]
        // Returns a list of path selector items (page paths)
        public IEnumerable<PathSelectorItem> PagePaths { get; set; } = Enumerable.Empty<PathSelectorItem>();
    }
}