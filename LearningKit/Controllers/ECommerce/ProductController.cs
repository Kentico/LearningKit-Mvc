using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.LearningKit;
using CMS.Base;
using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using LearningKit.Models.Products;
using LearningKit.Controllers;


//DocSection:ProductRoute
[assembly: RegisterPageRoute(LearningProductType.CLASS_NAME, typeof(ProductController), ActionName = nameof(ProductController.Detail))]
//EndDocSection:ProductRoute

namespace LearningKit.Controllers
{
    public class ProductController : Controller
    {
        private readonly IShoppingService shoppingService;
        private readonly ICatalogPriceCalculatorFactory priceCalculatorFactory;
        private readonly IPageDataContextRetriever pageRetriever;
        private readonly ISiteService siteService;
        private readonly ISKUInfoProvider skuInfoProvider;


        //DocSection:Constructor
        /// <summary>
        /// Constructor for the ProductController class.
        /// </summary>
        public ProductController(IShoppingService shoppingService,
                                 ICatalogPriceCalculatorFactory priceCalculatorFactory,
                                 IPageDataContextRetriever pageRetriever,
                                 ISiteService siteService,
                                 ISKUInfoProvider skuInfoProvider)
        {
            // Initializes instances of services required to manage product price calculation and the shopping cart
            this.shoppingService = shoppingService;
            this.priceCalculatorFactory = priceCalculatorFactory;
            this.pageRetriever = pageRetriever;
            this.siteService = siteService;
            this.skuInfoProvider = skuInfoProvider;
        }
        //EndDocSection:Constructor


        //DocSection:DisplayProduct
        /// <summary>
        /// Displays a product detail page of a product.
        /// </summary>
        public ActionResult BasicDetail()
        {
            // Gets the product from the connected Xperience database
            SKUTreeNode product = GetProduct();

            // If the product is not found or if it is not allowed for sale, redirects to error 404
            if ((product == null) || (product.SKU == null) || !product.SKU.SKUEnabled)
            {
                return HttpNotFound();
            }

            // Initializes the view model of the product with a calculated price
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            ProductCatalogPrices price = priceCalculatorFactory
                .GetCalculator(cart.ShoppingCartSiteID)
                .GetPrices(product.SKU, Enumerable.Empty<SKUInfo>(), cart);

            // Fills the product model with retrieved data
            ProductViewModel viewModel = new ProductViewModel(product, price);

            // Displays the product details page
            return View("Detail", viewModel);
        }


        /// <summary>
        /// Retrieves the coupled product from the data context of the current page.
        /// </summary>
        private SKUTreeNode GetProduct()
        {
            // Gets the page from the data context using IPageDataContextRetriever
            TreeNode node = pageRetriever.Retrieve<TreeNode>().Page;

            // If the found page is not a product, returns null
            if (node == null || !node.IsProduct())
            {
                return null;
            }

            // Loads specific fields of the product's product page type from the database
            node.MakeComplete(true);

            // Returns the found page as a product page
            return node as SKUTreeNode;
        }
        //EndDocSection:DisplayProduct


        //DocSection:DisplayVariant
        /// <summary>
        /// Displays product detail page of a product or product variant.
        /// </summary>
        public ActionResult Detail()
        {
            // Gets the product from the connected Xperience database
            SKUTreeNode product = GetProduct();

            // If the product is not found or if it is not allowed for sale, redirects to error 404
            if ((product == null) || !product.SKU.SKUEnabled)
            {
                return HttpNotFound();
            }

            // Gets all product variants of the product
            List<ProductVariant> variants = VariantHelper
                .GetVariants(product.NodeSKUID)
                .OnSite(siteService.CurrentSite.SiteID).ToList()
                    .Select(sku => new ProductVariant(sku.SKUID))
                .OrderBy(v => v.Variant.SKUPrice).ToList();

            // Selects the first product variant
            ProductVariant selectedVariant = variants.FirstOrDefault();

            // Calculates the price of the product or the variant
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            SKUInfo selectedProduct = selectedVariant != null ? selectedVariant.Variant : product.SKU;

            ProductCatalogPrices priceDetail = priceCalculatorFactory
               .GetCalculator(cart.ShoppingCartSiteID)
               .GetPrices(selectedProduct, Enumerable.Empty<SKUInfo>(), cart);

            // Initializes the view model of the product or product variant
            ProductViewModel viewModel = new ProductViewModel(product, priceDetail, variants, selectedVariant?.Variant?.SKUID ?? 0);

            // Displays the product detail page
            return View(viewModel);
        }


        /// <summary>
        /// Loads information about the requested variant to change the page content.
        /// </summary>
        /// <param name="variantID">ID of the selected variant.</param>
        [HttpPost]
        public JsonResult Variant(int variantID)
        {
            // Gets SKU information based on the variant's ID
            SKUInfo variant = skuInfoProvider.Get(variantID);

            // If the variant is null, returns null
            if (variant == null)
            {
                return null;
            }

            var cart = shoppingService.GetCurrentShoppingCart();

            // Calculates the price of the variant
            ProductCatalogPrices variantPrice = priceCalculatorFactory
                .GetCalculator(cart.ShoppingCartSiteID)
                .GetPrices(variant, Enumerable.Empty<SKUInfo>(), cart);

            // Finds out whether the variant is in stock
            bool isInStock = variant.SKUTrackInventory == TrackInventoryTypeEnum.Disabled || variant.SKUAvailableItems > 0;

            // Creates a JSON response for the JavaScript that switches the variants
            var response = new
            {
                totalPrice = String.Format(cart.Currency.CurrencyFormatString, variantPrice.Price),
                inStock = isInStock,
                stockMessage = isInStock ? "Yes" : "No"
            };

            // Returns the response
            return Json(response);
        }
        //EndDocSection:DisplayVariant
    }
}