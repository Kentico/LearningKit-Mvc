using System;

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.FormComponents;
using LearningKit.FormBuilder.FormComponentProperties;

//DocSection:ComponentRegistration
[assembly: RegisterFormComponent(RgbInputComponent.IDENTIFIER, typeof(RgbInputComponent), "RGB color input", Description = "Allows users to specify a color in the RGB hexadecimal format either manually, or by using a color selector", IconClass = "icon-palette")]
//EndDocSection:ComponentRegistration

namespace LearningKit.FormBuilder.FormComponents
{
    //DocSection:RgbInputComponentImplementation
    public class RgbInputComponent : FormComponent<RgbInputComponentProperties, string>
    {
        public const string IDENTIFIER = "RgbInputComponent";


        // Specifies that the property carries data for binding by the form builder
        [BindableProperty]
        // Used to store the value of the input field of the component
        public string RedComponent { get; set; } = "ff";


        [BindableProperty]
        public string GreenComponent { get; set; } = "00";


        [BindableProperty]
        public string BlueComponent { get; set; } = "00";


        // Disables automatic server-side evaluation for the component
        public override bool CustomAutopostHandling => true;


        // Gets the submitted values of the three properties and normalizes them to form a hexadecimal string of length 7,
        // e.g., in case a color was submitted in the #363 shorthand (representing #336633)
        // The returned value is subsequently saved to the corresponding column in the form's database table
        public override string GetValue()
        {
            return $"#{NormalizeReceivedValue(RedComponent)}{NormalizeReceivedValue(GreenComponent)}{NormalizeReceivedValue(BlueComponent)}";
        }


        // Normalizes individual submitted color components to 2 characters, e.g., F -> FF, 5 -> 55
        private string NormalizeReceivedValue(string value)
        {
            return value.Length == 1 ? value + value : value;
        }


        // Sets values of the properties (represented by individual 'input' elements)
        public override void SetValue(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                RedComponent = value.Substring(1, 2);
                GreenComponent = value.Substring(3, 2);
                BlueComponent = value.Substring(5, 2);
            }
            else
            {
                SetValue("#ff0000");
            }
        }
    }
    //EndDocSection:RgbInputComponentImplementation
}