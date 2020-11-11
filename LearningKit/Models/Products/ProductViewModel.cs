using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;


namespace LearningKit.Models.Products
{
    public class ProductViewModel
    {
        //DocSection:BasicModel
        public readonly PriceDetailViewModel PriceDetail;
        public readonly string Name;
        public readonly string Description;
        public readonly string ShortDescription;
        public readonly int SKUID;
        public readonly string ImagePath;
        public readonly bool IsInStock;
        
        /// <summary>
        /// Creates a new product model.
        /// </summary>
        /// <param name="productPage">Product's page.</param>
        /// <param name="priceDetail">Price of the product.</param>
        public ProductViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail)
        {
            // Fills the page information   
            Name = productPage.DocumentName;
            Description = productPage.DocumentSKUDescription;
            ShortDescription = productPage.DocumentSKUShortDescription;
            
            // Fills the SKU information
            SKUInfo sku = productPage.SKU;
            SKUID = sku.SKUID;
            ImagePath = string.IsNullOrEmpty(sku.SKUImagePath) ? null : new FileUrl(sku.SKUImagePath, true)
                                                                            .WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(400))
                                                                            .RelativePath;

            IsInStock = sku.SKUTrackInventory == TrackInventoryTypeEnum.Disabled ||
                        sku.SKUAvailableItems > 0;

            PriceDetail = new PriceDetailViewModel()
            {
                Price = priceDetail.Price,
                ListPrice = priceDetail.ListPrice,
                CurrencyFormatString = priceDetail.Currency.CurrencyFormatString
            };
        }
        //EndDocSection:BasicModel


        //DocSection:VariantModel
        public bool HasProductVariants { get; set; }
        public SelectList VariantSelectList { get; set; }
        public int SelectedVariantID { get; set; }
        
        /// <summary>
        /// Creates a new product model with variants.
        /// </summary>
        /// <param name="productPage">Product's page.</param>
        /// <param name="priceDetail">Price of the selected variant.</param>
        /// <param name="variants">Collection of selectable variants.</param>
        /// <param name="selectedVariantID">ID of the selected variant.</param>
        public ProductViewModel(SKUTreeNode productPage, ProductCatalogPrices priceDetail, List<ProductVariant> variants, int selectedVariantID)
            : this(productPage, priceDetail)
        {
            // Fills the selectable variants
            HasProductVariants = variants.Any();
            
            // Continues if the product has any variants
            if (HasProductVariants)
            {
                // Selects a default variant
                var selectedVariant = variants.FirstOrDefault(v => v.Variant.SKUID == selectedVariantID);
                
                if (selectedVariant != null)
                {
                    IsInStock = (selectedVariant.Variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled) || (selectedVariant.Variant.SKUAvailableItems > 0);
                    SelectedVariantID = selectedVariantID;
                }
                
                // Creates a list of product variants
                VariantSelectList = new SelectList(variants.Select(v => new SelectListItem
                {
                    Text = string.Join(", ", v.ProductAttributes.Select(a => a.SKUName)),
                    Value = v.Variant.SKUID.ToString()
                }), "Value", "Text");
            }
        }
        //EndDocSection:VariantModel
    }
}