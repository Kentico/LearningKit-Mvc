using Kentico.Web.Mvc;


namespace OutputCacheCustomization
{
    public static class OutputCacheKeyOptionsExtensions
    {
        // Varies the output cache based on the contact's gender
        public static IOutputCacheKeyOptions VaryByContactGender(this IOutputCacheKeyOptions options)
        {
            // Adds the ContactGenderOutputCacheKey to the options object
            options.AddCacheKey(new ContactGenderOutputCacheKey());

            return options;
        }
    }
}