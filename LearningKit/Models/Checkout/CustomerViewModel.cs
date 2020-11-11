using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using CMS.Ecommerce;

namespace LearningKit.Models.Checkout
{
    //DocSection:CustomerModel
    public class CustomerViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [MaxLength(100, ErrorMessage = "The maximum length allowed for the field has been exceeded.")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [MaxLength(100, ErrorMessage = "The maximum length allowed for the field has been exceeded.")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [DisplayName("Phone Number")]
        [MaxLength(20, ErrorMessage = "The maximum length allowed for the field has been exceeded.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }

        public string Company { get; set; }

        public string OrganizationID { get; set; }

        public string TaxRegistrationID { get; set; }

        [DisplayName("Are you ordering from a business account?")]
        public bool IsCompanyAccount { get; set; }
        
        /// <summary>
        /// Creates a customer model.
        /// </summary>
        /// <param name="customer">Customer details.</param>
        public CustomerViewModel(CustomerInfo customer)
        {
            if (customer == null)
            {
                return;
            }
            
            FirstName = customer.CustomerFirstName;
            LastName = customer.CustomerLastName;
            Email = customer.CustomerEmail;
            PhoneNumber = customer.CustomerPhone;
            Company = customer.CustomerCompany;
            OrganizationID = customer.CustomerOrganizationID;
            TaxRegistrationID = customer.CustomerTaxRegistrationID;
            IsCompanyAccount = customer.CustomerHasCompanyInfo;
        }

        /// <summary>
        /// Creates an empty customer model.
        /// Required by the MVC framework for model binding during form submission.
        /// </summary>
        public CustomerViewModel()
        {
        }
        
        /// <summary>
        /// Applies the model to a customer object.
        /// </summary>
        /// <param name="customer">Customer details to which the model is applied.</param>
        public void ApplyToCustomer(CustomerInfo customer)
        {
            customer.CustomerFirstName = FirstName;
            customer.CustomerLastName = LastName;
            customer.CustomerEmail = Email;
            customer.CustomerPhone = PhoneNumber;
            customer.CustomerCompany = Company;
            customer.CustomerOrganizationID = OrganizationID;
            customer.CustomerTaxRegistrationID = TaxRegistrationID;
        }
    }
    //EndDocSection:CustomerModel
}