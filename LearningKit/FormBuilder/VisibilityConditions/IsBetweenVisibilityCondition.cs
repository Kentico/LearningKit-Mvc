using System;

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.VisibilityConditions;


// Registers the visibility condition in the system
[assembly: RegisterFormVisibilityCondition("IsBetweenVisibilityCondition", typeof(IsBetweenVisibilityCondition), "Value of another field lies between")]

namespace LearningKit.FormBuilder.VisibilityConditions
{
    [Serializable]
    public class IsBetweenVisibilityCondition : AnotherFieldVisibilityCondition<int?>
    {
        // Defines a configuration interface for the visibility condition
        // The 'EditingComponent' attribute specifies which form component is used as the property's value editor
        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "Minimum", Order = 0)]
        public int Min { get; set; } = 0;

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "Maximum", Order = 1)]
        public int Max { get; set; } = 0;

        // Shows or hides the field based on the state of the dependee field
        public override bool IsVisible()
        {
            return (DependeeFieldValue >= Min) && (DependeeFieldValue <= Max);
        }
    }
}