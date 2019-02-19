using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.SiteProvider;
using CMS.Core;

using LearningKit.Models.Products;

namespace LearningKit.Controllers
{
    public class ProductController : Controller
    {
        private readonly string siteName = SiteContext.CurrentSiteName;
        private readonly IShoppingService shoppingService;
        private readonly ICatalogPriceCalculatorFactory calculatorFactory;


        //DocSection:Constructor
        /// <summary>
        /// Constructor for the ProductController class.
        /// </summary>
        public ProductController()
        {
            // Initializes instances of services required to manage product price calculation and the shopping cart
            // For real-world projects, we recommend using a dependency injection container to initialize service instances
            shoppingService = Service.Resolve<IShoppingService>();
            calculatorFactory = Service.Resolve<ICatalogPriceCalculatorFactory>();
        }
        //EndDocSection:Constructor


        //DocSection:DisplayProduct
        /// <summary>
        /// Displays a product detail page of a product specified by the GUID of the product's page.
        /// </summary>
        /// <param name="guid">Node GUID of the product's page.</param>
        /// <param name="productAlias">Node alias of the product's page.</param>
        public ActionResult BasicDetail(Guid guid, string productAlias)
        {
            // Gets the product from the connected Kentico database
            SKUTreeNode product = GetProduct(guid);

            // If the product is not found or if it is not allowed for sale, redirects to error 404
            if ((product == null) || (product.SKU == null) || !product.SKU.SKUEnabled)
            {
                return HttpNotFound();
            }

            // Redirects if the specified page alias does not match
            if (!string.IsNullOrEmpty(productAlias) && !product.NodeAlias.Equals(productAlias, StringComparison.InvariantCultureIgnoreCase))
            {
                return RedirectToActionPermanent("Detail", new { guid = product.NodeGUID, productAlias = product.NodeAlias });
            }

            // Initializes the view model of the product with a calculated price
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            ProductCatalogPrices price = calculatorFactory
                .GetCalculator(cart.ShoppingCartSiteID)
                .GetPrices(product.SKU, Enumerable.Empty<SKUInfo>(), cart);

            // Fills the product model with retrieved data
            ProductViewModel viewModel = new ProductViewModel(product, price);

            // Displays the product details page
            return View(viewModel);
        }

        /// <summary>
        /// Retrieves the product specified by the GUID of the product's page.
        /// </summary>
        /// <param name="nodeGuid">Node GUID of the product's page.</param>
        private SKUTreeNode GetProduct(Guid nodeGuid)
        {
            // Gets the page with the node GUID
            TreeNode node = DocumentHelper.GetDocuments()
                            .LatestVersion(false)
                            .Published(true)
                            .OnSite(siteName)
                            .Culture("en-US")
                            .CombineWithDefaultCulture()
                            .WhereEquals("NodeGUID", nodeGuid)
                            .FirstOrDefault();

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
        /// Displays product detail page of a product or product variant specified by the GUID of the product's or variant's page.
        /// </summary>
        /// <param name="guid">Node GUID of the product's (variant's) page.</param>
        /// <param name="productAlias">Node alias of the product's (variant's) page.</param>
        public ActionResult Detail(Guid guid, string productAlias)
        {
            // Gets the product from the connected Kentico database
            SKUTreeNode product = GetProduct(guid);

            // If the product is not found or if it is not allowed for sale, redirects to error 404
            if ((product == null) || !product.SKU.SKUEnabled)
            {
                return HttpNotFound();
            }

            // Redirects if the specified page alias does not match
            if (!string.IsNullOrEmpty(productAlias) && !product.NodeAlias.Equals(productAlias, StringComparison.InvariantCultureIgnoreCase))
            {
                return RedirectToActionPermanent("Detail", new { guid = product.NodeGUID, productAlias = product.NodeAlias });
            }

            // Gets all product variants of the product
            List<ProductVariant> variants = VariantHelper.GetVariants(product.NodeSKUID).OnSite(SiteContext.CurrentSiteID).ToList()
                .Select(sku => new ProductVariant(sku.SKUID))
                .OrderBy(v => v.Variant.SKUPrice).ToList();

            // Selects the first product variant
            ProductVariant selectedVariant = variants.FirstOrDefault();

            // Calculates the price of the product or the variant
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            SKUInfo selectedProduct = selectedVariant != null ? selectedVariant.Variant : product.SKU;

            ProductCatalogPrices priceDetail = calculatorFactory
               .GetCalculator(cart.ShoppingCartSiteID)
               .GetPrices(selectedProduct, Enumerable.Empty<SKUInfo>(), cart);

            // Initializes the view model of the product or product variant
            ProductViewModel viewModel = new ProductViewModel(product, priceDetail, variants, selectedVariant?.Variant?.SKUID ?? 0);

            // Displays the product detail page
            return View(viewModel);
        }

        /// <summary>
        /// Loads information about the demanded variant to change the page content.
        /// </summary>
        /// <param name="variantID">ID of the selected variant.</param>
        [HttpPost]
        public JsonResult Variant(int variantID)
        {
            // Gets SKU information based on the variant's ID
            SKUInfo variant = SKUInfoProvider.GetSKUInfo(variantID);

            // If the variant is null, returns null
            if (variant == null)
            {
                return null;
            }

            var cart = shoppingService.GetCurrentShoppingCart();

            // Calculates the price of the variant
            ProductCatalogPrices variantPrice = calculatorFactory
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