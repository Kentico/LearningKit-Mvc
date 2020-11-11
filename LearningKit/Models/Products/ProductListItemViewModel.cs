using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;

namespace LearningKit.Models.Products
{
    //DocSection:ProductListingModel
    public class ProductListItemViewModel
    {
        public readonly PriceDetailViewModel PriceModel;
        public string Name;
        public string ImagePath;
        public string PublicStatusName;
        public PageUrl ProductUrl;
        public bool Available;
        
        /// <summary>
        /// Constructor for the ProductListItemViewModel class.
        /// </summary>
        /// <param name="productPage">Product's page.</param>
        /// <param name="priceDetail">Price of the product.</param>
        /// <param name="publicStatusName">Display name of the product's public status.</param>
        public ProductListItemViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail, IPageUrlRetriever urlRetriever, string publicStatusName)
        {
            // Sets the page information
            Name = productPage.DocumentName;
            ProductUrl = urlRetriever.Retrieve(productPage);
            
            // Sets the SKU information
            ImagePath = string.IsNullOrEmpty(productPage.SKU.SKUImagePath) ?
                            null : new FileUrl(productPage.SKU.SKUImagePath, true)
                                        .WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(400))
                                        .RelativePath;

            Available = !productPage.SKU.SKUSellOnlyAvailable || productPage.SKU.SKUAvailableItems > 0;
            PublicStatusName = publicStatusName;

            // Sets the price format information
            PriceModel = new PriceDetailViewModel
            {
                Price = priceDetail.Price,
                ListPrice = priceDetail.ListPrice,
                CurrencyFormatString = priceDetail.Currency.CurrencyFormatString
            };
        }
    }
    //EndDocSection:ProductListingModel
}