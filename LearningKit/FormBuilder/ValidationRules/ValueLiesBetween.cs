using System;

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.CustomValidationRules;


// Registers the validation rule in the system
[assembly: RegisterFormValidationRule("ValueLiesBetweenValidationRule", typeof(ValueLiesBetween), "Closed interval validation", Description = "Checks whether the input lies in the specified closed interval.")]

namespace LearningKit.FormBuilder.CustomValidationRules
{
    [Serializable]
    public class ValueLiesBetween : ValidationRule<int>
    {
        // Defines a configuration interface for the rule
        // The EditingComponent attribute specifies which form component is used as an editing interface for the rule's properties
        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "Minimum value", Order = 0)]
        public int MinimumValue { get; set; }

        [EditingComponent(IntInputComponent.IDENTIFIER, Label = "Maximum value", Order = 1)]
        public int MaximumValue { get; set; }


        // Gets the title of the validation rule as displayed in the list of applied validation rules
        public override string GetTitle()
        {
            return $"Value lies between [{MinimumValue};{MaximumValue}].";
        }


        // Returns true if the component's value lies between the specified boundaries
        protected override bool Validate(int value)
        {
            return (MinimumValue <= value) && (value <= MaximumValue);
        }
    }
}
