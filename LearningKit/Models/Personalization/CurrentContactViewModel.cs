using CMS.ContactManagement;
using CMS.Personas;

namespace LearningKit.Models.Personalization
{
    public class CurrentContactViewModel
    {
        public string Contact { get; }
        public string ContactPersonaName { get; }
        public string ContactPersonaDisplayName { get; }

        public CurrentContactViewModel(ContactInfo currentContact)
        {
            PersonaInfo contactPersona = currentContact.GetPersona();
            
            if (contactPersona != null)
            {
                ContactPersonaName = contactPersona.PersonaName;
                ContactPersonaDisplayName = contactPersona.PersonaDisplayName;
            }
        }
    }
}