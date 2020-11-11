//DocSection:Using
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.Base;
using CMS.MediaLibrary;

using Kentico.Content.Web.Mvc;
//EndDocSection:Using

using LearningKit.Models.MediaLibrary;


namespace LearningKit.Controllers
{
    public class MediaLibraryController : Controller
    {
        //DocSection:GetMediaFiles
        private readonly IMediaFileUrlRetriever mediaFileUrlRetriever;
        private readonly IMediaLibraryInfoProvider mediaLibraryInfoProvider;
        private readonly IMediaFileInfoProvider mediaFileInfoProvider;
        private readonly ISiteService siteService;

        // Initializes instances of required services using dependency injection
        public MediaLibraryController(IMediaFileUrlRetriever mediaFileUrlRetriever,
                                      IMediaLibraryInfoProvider mediaLibraryInfoProvider,
                                      IMediaFileInfoProvider mediaFileInfoProvider,
                                      ISiteService siteService)
        {
            this.mediaFileUrlRetriever = mediaFileUrlRetriever;
            this.mediaLibraryInfoProvider = mediaLibraryInfoProvider;
            this.mediaFileInfoProvider = mediaFileInfoProvider;
            this.siteService = siteService;
        }


        /// <summary>
        /// Retrieves media files with the .jpg extension from the 'SampleMediaLibrary'.
        /// </summary>
        public ActionResult ShowMediaFiles()
        {
            // Gets an instance of the 'SampleMediaLibrary' media library for the current site
            MediaLibraryInfo mediaLibrary = mediaLibraryInfoProvider.Get("SampleMediaLibrary", siteService.CurrentSite.SiteID);
            
            // Gets a collection of media files with the .jpg extension from the media library
            IEnumerable<MediaFileInfo> mediaLibraryFiles = mediaFileInfoProvider.Get()
                                        .WhereEquals("FileLibraryID", mediaLibrary.LibraryID)
                                        .WhereEquals("FileExtension", ".jpg");
                        
            // Prepares a collection of view models containing required data of the media files
            IEnumerable<MediaFileViewModel> model = mediaLibraryFiles.Select(
                    mediaFile => {
                        IMediaFileUrl fileUrl = mediaFileUrlRetriever.Retrieve(mediaFile);
                        return new MediaFileViewModel
                        {
                            FileTitle = mediaFile.FileTitle,
                            // Gets the relative path to the media file
                            RelativeUrl = fileUrl.RelativePath
                        };
                    }
            );

            // Passes the model to the view
            return View(model);
        }
        //EndDocSection:GetMediaFiles
    }
}
