using LearningKit.Models.Widgets.ColorWidget;
using LearningKit.Models.Widgets.NumberWidget;
using LearningKit.Models.Widgets.SelectorsWidget;

using LearningKit.Models.Sections.CustomSection;

using LearningKit.Models.PageTemplates.LandingPage;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;


// Registers the 'Selected number' widget
[assembly: RegisterWidget("LearningKit.Widgets.NumberWidget", "Selected number", typeof(NumberWidgetProperties), customViewName: "Widgets/_NumberWidget", IconClass = "icon-octothorpe")]
// Registers the 'Colored widget'
[assembly: RegisterWidget("LearningKit.Widgets.ColorWidget", "Colored widget", typeof(ColorWidgetProperties), customViewName: "Widgets/_ColorWidget", IconClass = "icon-palette")]
// Registers the 'Selectors widget'
[assembly: RegisterWidget("LearningKit.Widgets.SelectorsWidget", "Selectors widget", typeof(SelectorsWidgetProperties), customViewName: "Widgets/_SelectorsWidget", IconClass = "icon-form")]

// Registers the default page builder section used by the LearningKit project
[assembly: RegisterSection("LearningKit.Sections.DefaultSection", "Default section", customViewName: "Sections/_DefaultSection", IconClass = "icon-square")]
// Registers a sample section with a background color customizable by a section property
[assembly: RegisterSection("LearningKit.Sections.CustomSection", "Custom section", typeof(CustomSectionProperties), customViewName: "Sections/_CustomSection", IconClass = "icon-square")]

// Registers the landing page template
[assembly: RegisterPageTemplate("LearningKit.LandingPageTemplate", "Default Landing page template", typeof(LandingPageProperties), customViewName: "PageTemplates/_LandingPageTemplate", IconClass = "icon-l-rows-2")]