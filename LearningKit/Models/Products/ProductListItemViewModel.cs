using System;

using CMS.Ecommerce;

namespace LearningKit.Models.Products
{
    //DocSection:ProductListingModel
    public class ProductListItemViewModel
    {
        public readonly PriceDetailViewModel PriceModel;
        public string Name;
        public string ImagePath;
        public string PublicStatusName;
        public bool Available;
        public Guid ProductPageGuid;
        public string ProductPageAlias;
        
        /// <summary>
        /// Constructor for the ProductListItemViewModel class.
        /// </summary>
        /// <param name="productPage">Product's page.</param>
        /// <param name="priceDetail">Price of the product.</param>
        /// <param name="publicStatusName">Display name of the product's public status.</param>
        public ProductListItemViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail, string publicStatusName)
        {
            // Sets the page information
            Name = productPage.DocumentName;
            ProductPageGuid = productPage.NodeGUID;
            ProductPageAlias = productPage.NodeAlias;
            
            // Sets the SKU information
            ImagePath = productPage.SKU.SKUImagePath;
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