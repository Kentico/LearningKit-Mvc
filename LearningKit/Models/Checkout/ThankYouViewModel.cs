using CMS.Ecommerce;

public class ThankYouViewModel
{
    public readonly string GtmPurchaseJsonString;

    // Constructor of the view model class for the "thank you" page of the checkout process
    public ThankYouViewModel(OrderInfo order)
    {
        // Creates the Google Tag Manager JSON data for the purchased order
        GtmPurchaseJsonString = GtmDataHelper.SerializeToJson(GtmOrderHelper.MapPurchase(order));
    }
}