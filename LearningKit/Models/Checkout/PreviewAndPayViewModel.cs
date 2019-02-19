using CMS.Ecommerce;

namespace LearningKit.Models.Checkout
{
    //DocSection:PreviewAndPayViewModel
    public class PreviewAndPayViewModel
    {
        public DeliveryDetailsViewModel DeliveryDetails { get; set; }

        public ShoppingCartViewModel Cart { get; set; }

        public PaymentMethodViewModel PaymentMethod { get; set; }

    }
    //EndDocSection:PreviewAndPayViewModel
}
