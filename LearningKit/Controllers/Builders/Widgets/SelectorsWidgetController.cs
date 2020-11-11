using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CMS.Base;
using CMS.DocumentEngine;
using CMS.MediaLibrary;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Content.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using LearningKit.Controllers.Builders.Widgets;
using LearningKit.Models.Widgets.SelectorsWidget;


// Assembly attribute to register the widget for the connected Xperience instance.
[assembly: RegisterWidget("LearningKit.Widgets.SelectorsWidget", typeof(SelectorsWidgetController), "Selectors demo", IconClass = "icon-form")]

namespace LearningKit.Controllers.Builders.Widgets
{
    public class SelectorsWidgetController : WidgetController<SelectorsWidgetProperties>
    {
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;
        private readonly IComponentPropertiesRetriever propertiesRetriever;
        private readonly IMediaFileInfoProvider mediaFileInfoProvider;
        private readonly IMediaFileUrlRetriever mediaFileUrlRetriever;
        private readonly ISiteService siteService;

        public SelectorsWidgetController(IPageAttachmentUrlRetriever attachmentUrlRetriever,
                                         IComponentPropertiesRetriever propertiesRetriever,
                                         IMediaFileInfoProvider mediaFileInfoProvider,
                                         IMediaFileUrlRetriever mediaFileUrlRetriever,
                                         ISiteService siteService)
        {
            this.attachmentUrlRetriever = attachmentUrlRetriever;
            this.propertiesRetriever = propertiesRetriever;
            this.mediaFileInfoProvider = mediaFileInfoProvider;
            this.mediaFileUrlRetriever = mediaFileUrlRetriever;
            this.siteService = siteService;
        }

        public ActionResult Index()
        {
            var properties = propertiesRetriever.Retrieve<SelectorsWidgetProperties>();

            string mediaFileUrl = GetMediaFileUrl(properties.Images);            

            // Retrieves the Path and Guid values of the selected page
            string documentPath = properties.PagePaths.FirstOrDefault()?.NodeAliasPath;
            Guid? documentGuid = properties.Pages.FirstOrDefault()?.NodeGuid;

            string attachmentUrl = GetAttachmentUrl(properties.Attachments);      

            return PartialView("Widgets/_SelectorsWidget.cshtml", new SelectorsWidgetViewModel
            {
                MediaFileUrl = mediaFileUrl,
                DocumentGuid = documentGuid,
                DocumentPath = documentPath,
                AttachmentUrl = attachmentUrl
        });
        }

        // Returns the relative path to the first attachment selected via the page attachment selector component
        private string GetAttachmentUrl(IEnumerable<AttachmentSelectorItem> attachments)
        {
            Guid attachmentGuid = attachments.FirstOrDefault()?.FileGuid ?? Guid.Empty;

            DocumentAttachment attachment = DocumentHelper.GetAttachment(attachmentGuid, siteService.CurrentSite.SiteName);

            return attachmentUrlRetriever.Retrieve(attachment).RelativePath;
        }


        // Returns the relative path to the first image selected via the image selector component
        private string GetMediaFileUrl(IEnumerable<Kentico.Components.Web.Mvc.FormComponents.MediaFilesSelectorItem> images)
        {
            // Retrieves GUID of the first selected media file from the 'Images' property
            Guid guid = images.FirstOrDefault()?.FileGuid ?? Guid.Empty;

            // Retrieves the MediaFileInfo object that corresponds to the selected media file GUID
            MediaFileInfo mediaFile = mediaFileInfoProvider.Get(guid, siteService.CurrentSite.SiteID);

            return mediaFileUrlRetriever.Retrieve(mediaFile).RelativePath;
        }
    }
}