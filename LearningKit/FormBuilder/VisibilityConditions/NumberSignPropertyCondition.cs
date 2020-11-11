using Kentico.Forms.Web.Mvc;

namespace LearningKit.FormBuilder.VisibilityConditions
{
    // Visibility condition that evaluates whether the value of a related integer property is positive, negative or zero
    public class NumberSignPropertyCondition : AnotherPropertyVisibilityCondition<int>
    {        
        // Parameter indicating whether the visibility condition is fulfilled for positive/negative numbers or zero
        public string RequiredSign { get; set; }

        // Determines whether the property is visible
        public override bool IsVisible()
        {
            string requiredSign = RequiredSign.ToLower();

            switch (requiredSign)
            {
                case "zero":
                    return DependeePropertyValue == 0;
                case "positive":
                    return DependeePropertyValue > 0;
                case "negative":
                    return DependeePropertyValue < 0;
                default:
                    return false;
            }
        }
    }
}