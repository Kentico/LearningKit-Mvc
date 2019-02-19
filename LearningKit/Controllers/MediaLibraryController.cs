//DocSection:Using
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.MediaLibrary;
using CMS.SiteProvider;
//EndDocSection:Using

using LearningKit.Models.MediaLibrary;

namespace LearningKit.Controllers
{
    public class MediaLibraryController : Controller
    {
        //DocSection:GetMediaFiles
        /// <summary>
        /// Retrieves media files with the .jpg extension from the 'SampleMediaLibrary'.
        /// </summary>
        public ActionResult ShowMediaFiles()
        {
            // Gets an instance of the 'SampleMediaLibrary' media library for the current site
            MediaLibraryInfo mediaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo("SampleMediaLibrary", SiteContext.CurrentSiteName);
            
            // Gets a collection of media files with the .jpg extension from the media library
            IEnumerable<MediaFileInfo> mediaLibraryFiles = MediaFileInfoProvider.GetMediaFiles()
                .WhereEquals("FileLibraryID", mediaLibrary.LibraryID)
                .WhereEquals("FileExtension", ".jpg");
                        
            // Prepares a collection of view models containing required data of the media files
            IEnumerable<MediaFileViewModel> model = mediaLibraryFiles.Select(
                    mediaFile => new MediaFileViewModel
                    {
                        FileTitle = mediaFile.FileTitle,
                        DirectUrl = MediaLibraryHelper.GetDirectUrl(mediaFile),
                        PermanentUrl = MediaLibraryHelper.GetPermanentUrl(mediaFile)
                    }
            );

            // Passes the model to the view
            return View(model);
        }
        //EndDocSection:GetMediaFiles
    }
}
