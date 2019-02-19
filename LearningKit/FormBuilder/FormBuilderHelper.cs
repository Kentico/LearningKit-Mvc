using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearningKit.FormBuilder
{
    public static class FormBuilderHelper
    {
        //DocSection:CustomInputExtensionMethod
        // Renders an 'input' element of the specified type and with the collection of provided attributes
        public static MvcHtmlString CustomInput(this HtmlHelper helper, string inputType, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("input");

            // Specifies the input type, name, and value attributes
            tagBuilder.MergeAttribute("type", inputType);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("value", value.ToString());
            
            // Merges additional attributes into the element
            tagBuilder.MergeAttributes(htmlAttributes);            

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.StartTag)); 
        }
        //EndDocSection:CustomInputExtensionMethod
    }
}