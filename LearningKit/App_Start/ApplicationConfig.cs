using Kentico.Web.Mvc;
using Kentico.Activities.Web.Mvc;
using Kentico.CampaignLogging.Web.Mvc;
using Kentico.Content.Web.Mvc;
using Kentico.Newsletters.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.OnlineMarketing.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using LearningKit.PageTemplateFilters;

namespace LearningKit
{
    public class ApplicationConfig
    {
        public static void RegisterFeatures(ApplicationBuilder builder)
        {
            builder.UsePreview();

            builder.UsePageBuilder(new PageBuilderOptions() {
                DefaultSectionIdentifier = "LearningKit.Sections.DefaultSection",
                RegisterDefaultSection = false
            });

            builder.UseDataAnnotationsLocalization();
            builder.UseCampaignLogger();
            builder.UseResourceSharingWithAdministration();
            builder.UseActivityTracking();
            builder.UseEmailTracking(new EmailTrackingOptions());
            builder.UseABTesting();
            builder.UsePageRouting(new PageRoutingOptions
            {
                EnableAlternativeUrls = true
            });

            PageBuilderFilters.PageTemplates.Add(new LandingPageTemplateFilter());
        }
    }
}