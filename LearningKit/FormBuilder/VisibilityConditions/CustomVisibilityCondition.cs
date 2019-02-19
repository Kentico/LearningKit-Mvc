using System;

using Kentico.Forms.Web.Mvc;

using LearningKit.FormBuilder.CustomVisibilityConditions;


//DocSection:VisibilityConditionRegistration
[assembly: RegisterFormVisibilityCondition("CustomVisibilityCondition", typeof(CustomVisibilityCondition), "Custom visibility condition")]
//EndDocSection:VisibilityConditionRegistration

namespace LearningKit.FormBuilder.CustomVisibilityConditions
{
    [Serializable]
    public class CustomVisibilityCondition : VisibilityCondition
    {
        //DocSection:Configuration
        // Defines a configuration interface for the condition
        // The 'EditingComponent' attribute specifies which form component is used as the property's value editor
        [EditingComponent(TextInputComponent.IDENTIFIER)]
        public string ConfigurableProperty { get; set; }
        //EndDocSection:Configuration


        //DocSection:Contract
        // Contains custom visibility logic evaluated by the server
        // True indicates the field is displayed, false indicates the field is hidden
        public override bool IsVisible()
        {
            return true;
        }
        //EndDocSection:Contract
    }
}