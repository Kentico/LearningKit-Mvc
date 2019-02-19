namespace LearningKit.Models.Checkout
{
    //DocSection:DeliveryDetailsViewModel
    public class DeliveryDetailsViewModel
    {

        public CustomerViewModel Customer { get; set; }

        public BillingAddressViewModel BillingAddress { get; set; }

        public ShippingOptionViewModel ShippingOption { get; set; }
    }
    //EndDocSection:DeliveryDetailsViewModel
}