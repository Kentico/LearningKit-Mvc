using System.Linq;
using System.Collections.Generic;

using Kentico.Forms.Web.Mvc;
using Kentico.Forms.Web.Mvc.FormComponents;


namespace LearningKit.FormBuilder
{
    public class IndividualFormComponentsFilter : IFormComponentFilter
    {
        public IEnumerable<FormComponentDefinition> Filter(IEnumerable<FormComponentDefinition> formComponents, FormComponentFilterContext context)
        {
            // Filters specified form components
            return formComponents.Where(component => !GetComponentsToFilter().Contains(component.Identifier));
        }


        // A collection of form component identifiers to filter
        private IEnumerable<string> GetComponentsToFilter()
        {
            return new string[] { TextInputComponent.IDENTIFIER, TextAreaComponent.IDENTIFIER, USPhoneComponent.IDENTIFIER };
        }
    }
}