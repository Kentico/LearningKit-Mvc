using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.Ecommerce;
using CMS.DocumentEngine.Types.LearningKit;

using Kentico.Content.Web.Mvc;

using LearningKit.Models.Products;

namespace LearningKit.Controllers
{
    //DocSection:ListingController
    public class ProductListingController : Controller
    {
        private readonly IShoppingService shoppingService;
        private readonly ICatalogPriceCalculatorFactory priceCalculatorFactory;
        private readonly IPageRetriever pageRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;


        /// <summary>
        /// Initializes instances of services required to manage product price calculation and the shopping cart.
        /// </summary>
        public ProductListingController(IShoppingService shoppingService,
                                     ICatalogPriceCalculatorFactory priceCalculatorFactory,
                                     IPageRetriever pageRetriever,
                                     IPageUrlRetriever pageUrlRetriever)
        {
            this.shoppingService = shoppingService;
            this.priceCalculatorFactory = priceCalculatorFactory;
            this.pageRetriever = pageRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }


        /// <summary>
        /// Displays a product listing page of the class's product page type.
        /// </summary>
        public ActionResult Listing()
        {
            // Gets products of the product page type (via the generated page type code)           
            List<LearningProductType> products = pageRetriever.Retrieve<LearningProductType>(query => query
                            .CombineWithDefaultCulture()
                            .WhereTrue("SKUEnabled")
                            .OrderByDescending("SKUInStoreFrom"))
                            .ToList();

            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Prepares a collection of products of the LearningProductType page type to be sent to a view
            IEnumerable<ProductListItemViewModel> productListing = products.Select(
                            product => new ProductListItemViewModel(
                                product,
                                GetPrice(product.SKU, cart),
                                pageUrlRetriever,
                                product.Product.PublicStatus?.PublicStatusDisplayName));

            // Displays the action's view with an initialized view model
            return View(productListing);
        }


        // Retrieves a ProductCatalogPrices instance that contains calculated price information for the given product
        private ProductCatalogPrices GetPrice(SKUInfo product, ShoppingCartInfo cart)
        {
            return priceCalculatorFactory
                        .GetCalculator(cart.ShoppingCartSiteID)
                        .GetPrices(product, Enumerable.Empty<SKUInfo>(), cart);
        }
    }
    //EndDocSection:ListingController
}