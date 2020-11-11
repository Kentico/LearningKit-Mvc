using CMS.Membership;

using Kentico.Membership;


namespace MembershipCustomization
{
    // Extends the default Kentico.Membership.User object
    public class ExtendedUser : User
    {
        // Exposes the existing 'MiddleName' property of the 'UserInfo' object
        public string MiddleName
        {
            get;
            set;
        }


        // Property that corresponds to a custom field specified in the administration interface
        public string CustomField
        {
            get;
            set;
        }
        

        // Ensures field mapping between Kentico's user objects and the Kentico.Membership ASP.NET Identity implementation
        // Called when retrieving users from Kentico via Kentico.Membership.KenticoUserManager<TUser>
        public override void MapFromUserInfo(UserInfo source)
        {
            // Calls the base class implementation of the MapFromUserInfo method
            base.MapFromUserInfo(source);

            // Maps the 'MiddleName' property to the extended user object
            MiddleName = source.MiddleName;

            // Sets the value of the 'CustomField' property
            CustomField = source.GetValue<string>("CustomField", null);
        }


        // Ensures field mapping between Kentico's user objects and the Kentico.Membership ASP.NET Identity implementation
        // Called when creating or updating users using Kentico.Membership.KenticoUserManager<TUser>
        public override void MapToUserInfo(UserInfo target)
        {
            // Calls the base class implementation of the MapToUserInfo method
            base.MapToUserInfo(target);

            // Maps the 'MiddleName' property to the target 'UserInfo' object
            target.MiddleName = MiddleName;

            // Sets the value of the 'CustomField' custom user field
            target.SetValue("CustomField", CustomField);
        }
    }
}