using System.Web;

using CMS.ContactManagement;

using Kentico.Web.Mvc;


namespace OutputCacheCustomization
{
    public class ContactGenderOutputCacheKey : IOutputCacheKey
    {
        // Used as a prefix for this cache key part
        public string Name => "KenticoContactGender";

        // Invoked when constructing a cache key from the configured 'IOutputCacheKeyOptions' options object
        public string GetVaryByCustomString(HttpContextBase context, string custom)
        {
            // Gets the current contact, without creating a new anonymous contact for new visitors
            ContactInfo existingContact = ContactManagementContext.GetCurrentContact(createAnonymous: false);
            // Gets the contact's gender
            int? contactGender = existingContact?.ContactGender;
            return $"{Name}={contactGender}";
        }
    }
}