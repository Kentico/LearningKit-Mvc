using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using CMS.Ecommerce;


namespace LearningKit.Models.Checkout
{
    //DocSection:BillingAddressModel
    public class BillingAddressViewModel
    {
        [Required]
        [DisplayName("Address line 1")]
        [MaxLength(100, ErrorMessage = "The maximum length allowed for the field has been exceeded.")]
        public string Line1 { get; set; }

        [DisplayName("Address line 2")]
        [MaxLength(100, ErrorMessage = "The maximum length allowed for the field has been exceeded.")]
        public string Line2 { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The maximum length allowed for the field has been exceeded.")]
        public string City { get; set; }

        [Required]
        [DisplayName("Postal code")]
        [MaxLength(20, ErrorMessage = "The maximum length allowed for the field has been exceeded.")]
        [DataType(DataType.PostalCode, ErrorMessage = "Please enter a valid postal code.")]
        public string PostalCode { get; set; }

        [DisplayName("Country")]
        public int CountryID { get; set; }

        [DisplayName("State")]
        public int StateID { get; set; }

        public int AddressID { get; set; }

        public SelectList Countries { get; set; }

        public SelectList Addresses { get; set; }
        
        /// <summary>
        /// Creates a billing address model.
        /// </summary>
        /// <param name="address">Billing address.</param>
        /// <param name="countryList">List of countries.</param>
        public BillingAddressViewModel(AddressInfo address, SelectList countries, SelectList addresses)
        {
            if (address != null)
            {
                Line1 = address.AddressLine1;
                Line2 = address.AddressLine2;
                City = address.AddressCity;
                PostalCode = address.AddressZip;
                CountryID = address.AddressCountryID;
                StateID = address.AddressStateID;
                AddressID = address.AddressID;
            }
            
            Countries = countries;
            Addresses = addresses;
        }


        /// <summary>
        /// Creates an empty BillingAddressModel object. 
        /// Required by the MVC framework for model binding during form submission.
        /// </summary>
        public BillingAddressViewModel()
        {
                
        }


        /// <summary>
        /// Applies the model to an AddressInfo object.
        /// </summary>
        /// <param name="address">Billing address to which the model is applied.</param>
        public void ApplyTo(AddressInfo address)
        {
            address.AddressLine1 = Line1;
            address.AddressLine2 = Line2;
            address.AddressCity = City;
            address.AddressZip = PostalCode;
            address.AddressCountryID = CountryID;
            address.AddressStateID = StateID;
        }
    }
    //EndDocSection:BillingAddressModel
}