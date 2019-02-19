using System;

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.CustomValidationRules;


//DocSection:ValidationRuleRegistration
// Registers the validation rule in the system
[assembly: RegisterFormValidationRule("CustomValidationRule", typeof(CustomValidationRule), "Custom validation rule", Description = "Contains custom validation logic.")]
//EndDocSection:ValidationRuleRegistration

namespace LearningKit.FormBuilder.CustomValidationRules
{
    [Serializable]
    public class CustomValidationRule : ValidationRule<string>
    {
        //DocSection:Configuration
        // Defines a configuration interface for the rule
        // Uses the EditingComponent attribute to specify which form component is used to provide an editing interface for the property
        [EditingComponent(TextInputComponent.IDENTIFIER)]
        public string ConfigurableProperty { get; set; }
        //EndDocSection:Configuration


        //DocSection:Contract
        // Gets the title of the validation rule as displayed in the list of applied validation rules
        public override string GetTitle()
        {
            return "This title appears in the list of applied validation rules on the 'Validation' tab of individual form fields.";
        }


        // Contains custom validation logic
        // Invokes when validation occurs
        protected override bool Validate(string value)
        {
            return true;
        }
        //EndDocSection:Contract
    }
}