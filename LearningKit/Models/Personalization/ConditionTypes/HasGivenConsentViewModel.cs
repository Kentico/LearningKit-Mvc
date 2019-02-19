using System.ComponentModel.DataAnnotations;

namespace LearningKit.Personalization.ConditionTypes
{
    public class HasGivenConsentViewModel
    {
        [Required]
        [Display(Name = "Consent code name")]
        public string ConsentCodeName { get; set; }
    }
}