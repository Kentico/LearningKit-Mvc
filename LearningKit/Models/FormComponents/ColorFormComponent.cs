using Kentico.Forms.Web.Mvc;
using LearningKit.FormBuilder.FormComponents;

[assembly: RegisterFormComponent(ColorFormComponent.IDENTIFIER, typeof(ColorFormComponent), "Color component", IconClass = "icon-newspaper")]

namespace LearningKit.FormBuilder.FormComponents
{
    public class ColorFormComponent : FormComponent<ColorFormComponentProperties, string>
    {
        public const string IDENTIFIER = "ColorFormComponent";

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