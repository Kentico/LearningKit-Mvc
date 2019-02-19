using System;

using CMS.ContactManagement;
using CMS.Personas;

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.CustomVisibilityConditions;


// Registers the visibility condition in the system
[assembly: RegisterFormVisibilityCondition("IsInPersonaVisibilityCondition", typeof(IsInPersonaVisibilityCondition), "User is in persona")]

namespace LearningKit.FormBuilder.CustomVisibilityConditions
{
    [Serializable]
    // A visibility condition that checks whether a user is in the specified persona
    public class IsInPersonaVisibilityCondition : VisibilityCondition
    {
        // Defines a configuration interface for the visibility condition
        // The 'EditingComponent' attribute specifies which form component is used as the property's value editor
        [EditingComponent(TextInputComponent.IDENTIFIER, Label = "Required persona")]
        public string RequiredPersona { get; set; }


        // Checks whether the current user belongs to the specified persona
        // Called when the visibility condition is evaluated by the server
        public override bool IsVisible()
        {
            ContactInfo currentContact = ContactManagementContext.GetCurrentContact();

            string currentPersonaName = currentContact?.GetPersona()?.PersonaName;

            return String.Equals(currentPersonaName, RequiredPersona, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}