/* 
 * This is a sample form component used to demonstrate manipulation with custom form component properties.
 * For more information, visit the Kentico documentation.
 */

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.FormComponents;
using LearningKit.FormBuilder.FormComponentProperties;


[assembly: RegisterFormComponent(CustomPropertyComponent.IDENTIFIER, typeof(CustomPropertyComponent), "Custom text input", Description = "Custom single-line text input", IconClass = "icon-l-text")]

namespace LearningKit.FormBuilder.FormComponents
{
    public class CustomPropertyComponent : FormComponent<CharacterSizeProperties, string>
    {
        public const string IDENTIFIER = "CustomPropertyComponent";


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