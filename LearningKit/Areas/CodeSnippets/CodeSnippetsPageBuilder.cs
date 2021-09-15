using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.DocumentEngine;
using CMS.MediaLibrary;
using CMS.Base;

using Kentico.Content.Web.Mvc;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Kentico.Components.Web.Mvc.FormComponents;

namespace LearningKit.Areas.CodeSnippets
{
    public class PageBuilderInitializationExample : Controller
    {
        //DocSection:PageBuilderInitialize
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextInitializer pageDataContextInitializer;

        // Gets instances of required services using dependency injection
        public PageBuilderInitializationExample(IPageRetriever pageRetriever,
                                                IPageDataContextInitializer pageDataContextInitializer)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextInitializer = pageDataContextInitializer;
        }

        public ActionResult Home()
        {
            // Retrieves a page from the Xperience database with the '/Home' node alias path
            TreeNode page = pageRetriever.Retrieve<TreeNode>(query => query
                                .Path("/Home", PathTypeEnum.Single))
                                .FirstOrDefault();

            // Responds with the HTTP 404 error when the page is not found
            if (page == null)
            {
                return HttpNotFound();
            }

            // Initializes the page data context using the retrieved page
            pageDataContextInitializer.Initialize(page);

            return View();
        }
        //EndDocSection:PageBuilderInitialize
    }

    public class PageTemplateCustomRoutingInitialization : Controller
    {
        //DocSection:PageTemplateAction
        private readonly IPageRetriever pagesRetriever;

        // Gets instances of required services using dependency injection
        public PageTemplateCustomRoutingInitialization(IPageRetriever pagesRetriever)
        {
            this.pagesRetriever = pagesRetriever;
        }

        /// <summary>
        /// A GET action displaying a page where you wish to use page templates.
        /// </summary>
        /// <param name="pageAlias">Page alias of the displayed page.</param>
        public ActionResult Index(string pageAlias)
        {
            // Retrieves a page from the Xperience database
            TreeNode page = pagesRetriever.Retrieve<TreeNode>(query => query
                                .Path("/Landing-pages", PathTypeEnum.Children)
                                .WhereEquals("NodeAlias", pageAlias)
                                .TopN(1))
                                .FirstOrDefault();

            // Responds with the HTTP 404 error when the page is not found
            if (page == null)
            {
                return HttpNotFound();
            }

            // Returns a TemplateResult object, created with the retrieved page
            // Automatically initializes the page data context and the page builder feature
            // for all editable areas placed within templates
            return new TemplateResult(page);
        }
        //EndDocSection:PageTemplateAction
    }

    public class PageTemplateAdvancedRoutingInitialization : Controller
    {
        //DocSection:PageTemplatesAdvancedRouting
        public ActionResult Index()
        {
            // Custom processing logic

            // Leverages information provided by the router when serving the request
            // to retrieve the corresponding page. No need to specify the page to render.
            return new TemplateResult();
        }
        //EndDocSection:PageTemplatesAdvancedRouting
    }

    public class MediaFilesSelectorExample : WidgetController<CustomWidgetProperties>
    {
        //DocSection:MediaFilesSelector
        private readonly IMediaFileInfoProvider mediaFileInfo;
        private readonly IComponentPropertiesRetriever componentPropertiesRetriever;
        private readonly ISiteService siteService;

        public MediaFilesSelectorExample(IMediaFileInfoProvider mediaFileInfo, 
                                         IComponentPropertiesRetriever componentPropertiesRetriever,
                                         ISiteService siteService)
        {
            this.mediaFileInfo = mediaFileInfo;
            this.componentPropertiesRetriever = componentPropertiesRetriever;
            this.siteService = siteService;
        }

        public ActionResult Index()
        {
            // Retrieves the GUID of the first selected media file from the 'Images' property
            Guid guid = componentPropertiesRetriever.Retrieve<CustomWidgetProperties>().Images.FirstOrDefault()?.FileGuid ?? Guid.Empty;
            // Retrieves the MediaFileInfo object that corresponds to the selected media file GUID
            MediaFileInfo mediaFile = mediaFileInfo.Get(guid, siteService.CurrentSite.SiteID);

            string url = String.Empty;
            if (mediaFile != null)
            {
                // Retrieves an URL of the selected media file
                url = MediaLibraryHelper.GetDirectUrl(mediaFile);
            }

            // Custom logic...

            return View();
        }
        //EndDocSection:MediaFilesSelector
    }

    public class PathSelectorExample : WidgetController<CustomWidgetProperties>
    {
        //DocSection:PathSelectorController
        private readonly IPageRetriever pagesRetriever;
        private readonly IComponentPropertiesRetriever componentPropertiesRetriever;

        public PathSelectorExample(IPageRetriever pagesRetriever, IComponentPropertiesRetriever componentPropertiesRetriever)
        {
            this.pagesRetriever = pagesRetriever;
            this.componentPropertiesRetriever = componentPropertiesRetriever;
        }

        public ActionResult Index()
        {           
            // Retrieves the node alias paths of the selected pages from the 'PagePaths' property
            string[] selectedPagePaths = componentPropertiesRetriever.Retrieve<CustomWidgetProperties>().PagePaths
                                                                     .Select(i => i.NodeAliasPath)
                                                                     .ToArray();

            // Retrieves the pages that correspond to the selected alias paths
            List<TreeNode> pages = pagesRetriever.Retrieve<TreeNode>(query => query
                                                 .Path(selectedPagePaths))
                                                 .ToList();

            // Custom logic...

            return View();
        }
        //EndDocSection:PathSelectorController
    }

    public class PageSelectorExample : WidgetController<CustomWidgetProperties>
    {
        //DocSection:PageSelectorController
        private readonly IPageRetriever pagesRetriever;
        private readonly IComponentPropertiesRetriever componentPropertiesRetriever;

        public PageSelectorExample(IPageRetriever pagesRetriever, IComponentPropertiesRetriever componentPropertiesRetriever)
        {
            this.pagesRetriever = pagesRetriever;
            this.componentPropertiesRetriever = componentPropertiesRetriever;
        }

        public ActionResult Index()
        {
            // Retrieves the node GUIDs of the selected pages from the 'Pages' property
            List<Guid> selectedPageGuids = componentPropertiesRetriever.Retrieve<CustomWidgetProperties>().Pages
                                                                       .Select(i => i.NodeGuid)
                                                                       .ToList();

            // Retrieves the pages that correspond to the selected GUIDs
            List<TreeNode> pages = pagesRetriever.Retrieve<TreeNode>(query => query
                                                 .WhereIn("NodeGUID", selectedPageGuids))
                                                 .ToList();

            // Custom logic...

            return View();
        }
        //EndDocSection:PageSelectorController
    }

    public class AttachmentSelectorExample : WidgetController<CustomWidgetProperties>
    {
        //DocSection:AttachmentSelectorController
        private readonly IComponentPropertiesRetriever propertiesRetriever;
        private readonly ISiteService siteService;
        private readonly IPageAttachmentUrlRetriever attachmentUrlRetriever;

        public AttachmentSelectorExample(IComponentPropertiesRetriever propertiesRetriever,
                                         ISiteService siteService,
                                         IPageAttachmentUrlRetriever attachmentUrlRetriever)
        {
            this.propertiesRetriever = propertiesRetriever;
            this.siteService = siteService;
            this.attachmentUrlRetriever = attachmentUrlRetriever;
        }


        public ActionResult Index()
        {
            // Retrieves the GUID of the first selected attachment from the 'Attachments' property
            Guid guid = propertiesRetriever.Retrieve<CustomWidgetProperties>().Attachments.FirstOrDefault()?.FileGuid ?? Guid.Empty;
            // Retrieves the DocumentAttachment object that corresponds to the selected attachment GUID
            DocumentAttachment attachment = DocumentHelper.GetAttachment(guid, siteService.CurrentSite.SiteID);

            string url = String.Empty;
            if (attachment != null)
            {
                // Retrieves the relative URL of the selected attachment
                url = attachmentUrlRetriever.Retrieve(attachment).RelativePath;
            }

            // Custom logic...

            return View();
        }
        //EndDocSection:AttachmentSelectorController
    }


    public class CustomWidgetProperties : IWidgetProperties
    {
        //DocSection:MediaFilesSelectorConfig
        // Assigns a selector component to the 'Images' property
        [EditingComponent(MediaFilesSelector.IDENTIFIER)]
        // Configures the media library from which you can select files in the selector
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.LibraryName), "Graphics")]
        // Limits the maximum number of files that can be selected at once
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.MaxFilesLimit), 5)]
        // Configures the allowed file extensions for the selected files
        [EditingComponentProperty(nameof(MediaFilesSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        // Returns a list of media files selector items (objects that contain the GUIDs of selected media files)
        public IEnumerable<MediaFilesSelectorItem> Images { get; set; } = Enumerable.Empty<MediaFilesSelectorItem>();
        //EndDocSection:MediaFilesSelectorConfig

        //DocSection:PageSelectorConfig
        // Assigns a selector component to the Pages property
        [EditingComponent(PageSelector.IDENTIFIER)]
        // Limits the selection of pages to a subtree rooted at the 'Products' page
        [EditingComponentProperty(nameof(PageSelectorProperties.RootPath), "/Products")]
        // Sets an unlimited number of selectable pages
        [EditingComponentProperty(nameof(PageSelectorProperties.MaxPagesLimit), 0)]
        // Returns a list of page selector items (node GUIDs)
        public IEnumerable<PageSelectorItem> Pages { get; set; } = Enumerable.Empty<PageSelectorItem>();
        //EndDocSection:PageSelectorConfig

        //DocSection:PathSelectorConfig
        // Assigns a selector component to the 'PagePaths' property
        [EditingComponent(PathSelector.IDENTIFIER)]
        // Limits the selection of pages to a subtree rooted at the 'Products' page
        [EditingComponentProperty(nameof(PathSelectorProperties.RootPath), "/Products")]
        // Sets the maximum number of selected pages to 6
        [EditingComponentProperty(nameof(PathSelectorProperties.MaxPagesLimit), 6)]
        // Returns a list of path selector items (page paths)
        public IEnumerable<PathSelectorItem> PagePaths { get; set; } = Enumerable.Empty<PathSelectorItem>();
        //EndDocSection:PathSelectorConfig

        //DocSection:AttachmentSelectorConfig
        // Assigns a selector component to the 'Attachments' property
        [EditingComponent(AttachmentSelector.IDENTIFIER)]
        // Limits the maximum number of attachments that can be selected at once
        [EditingComponentProperty(nameof(AttachmentSelectorProperties.MaxFilesLimit), 3)]
        // Configures the allowed file extensions for the selected attachments
        [EditingComponentProperty(nameof(AttachmentSelectorProperties.AllowedExtensions), ".gif;.png;.jpg;.jpeg")]
        // Returns a list of attachment selector items (attachment objects)
        public IEnumerable<AttachmentSelectorItem> Attachments { get; set; } = Enumerable.Empty<AttachmentSelectorItem>();
        //EndDocSection:AttachmentSelectorConfig
    }
}