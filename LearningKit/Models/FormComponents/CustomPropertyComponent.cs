/* 
 * This is a sample form component used to demonstrate manipulation with custom form component properties.
 * For more information, visit the Kentico documentation.
 */

using Kentico.Forms.Web.Mvc;

using LearningKit.Models.FormBuilder.CustomFormComponents;


[assembly: RegisterFormComponent("CustomPropertyComponent", typeof(CustomPropertyComponent), "Custom text input", Description = "Custom single-line text input", IconClass = "icon-l-text")]

namespace LearningKit.Models.FormBuilder.CustomFormComponents
{
    public class CustomPropertyComponent : FormComponent<CharacterSizeProperties, string>
    {
        [BindableProperty]
        public string Value { get; set; }


        public override string GetValue()
        {
            return Value;
        }


        public override void SetValue(string value)
        {
            Value = value;
        }
    }
}