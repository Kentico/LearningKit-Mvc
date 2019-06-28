using System.Linq;
using System.Collections.Generic;

using Kentico.Forms.Web.Mvc;
using Kentico.Forms.Web.Mvc.FormComponents;


namespace LearningKit.FormBuilder
{
    public class FormComponentsFilter : IFormComponentFilter
    {
        public IEnumerable<FormComponentDefinition> Filter(IEnumerable<FormComponentDefinition> formComponents, FormComponentFilterContext context)
        {
            // Filters out all Kentico form components from the form builder UI
            return formComponents.Where(component => !component.Identifier.StartsWith("Kentico"));
        }
    }
}