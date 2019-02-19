using System;

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.CustomValidationRules;


// Registers the validation rule in the system
[assembly: RegisterFormValidationRule("IsHexadecimalNumberValidationRule", typeof(IsHexadecimalNumber), "Is hexadecimal number", Description = "Checks whether the submitted input is a hexadecimal string (including the leading # character).")]

namespace LearningKit.FormBuilder.CustomValidationRules
{
    [Serializable]
    public class IsHexadecimalNumber : ValidationRule<string>
    {
        // Gets the title of the validation rule as displayed in the list of applied validation rules
        public override string GetTitle()
        {
            return "Input is a hexadecimal number.";
        }


        // Returns true if the field value is in the hexadecimal format
        protected override bool Validate(string value)
        {
            // Fails if the submitted string does not contain a leading '#' character
            if (value[0] != '#')
            {
                return false;
            }

            // Strips the leading '#' character
            value = value.Substring(1);

            // Tries to convert the submitted value
            bool success = int.TryParse(value, System.Globalization.NumberStyles.AllowHexSpecifier, null, out int variable);

            return success;
        }
    }
}