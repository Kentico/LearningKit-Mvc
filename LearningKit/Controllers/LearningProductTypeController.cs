using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.SiteProvider;
using CMS.Ecommerce;
using CMS.Core;
using CMS.DocumentEngine.Types.LearningKit;

using LearningKit.Models.Products;

namespace LearningKit.Controllers
{
    //DocSection:ListingController
    public class LearningProductTypeController : Controller
    {
        private readonly string siteName = SiteContext.CurrentSiteName;
        private readonly IShoppingService shoppingService;
        private readonly ICatalogPriceCalculatorFactory calculatorFactory;


        /// <summary>
        /// Constructor for the LearningProductTypeController class.
        /// </summary>
        public LearningProductTypeController()
        {
            // Initializes instances of services required to manage product price calculation and the shopping cart
            // For real-world projects, we recommend using a dependency injection container to initialize service instances
            shoppingService = Service.Resolve<IShoppingService>();
            calculatorFactory = Service.Resolve<ICatalogPriceCalculatorFactory>();
        }


        /// <summary>
        /// Displays a product listing page of the class's product page type.
        /// </summary>
        public ActionResult Listing()
        {
            // Gets products of the product page type (via the generated page type code)
            List<LearningProductType> products = LearningProductTypeProvider.GetLearningProductTypes()
                .LatestVersion(false)
                .Published(true)
                .OnSite(siteName)
                .Culture("en-US")
                .CombineWithDefaultCulture()
                .WhereTrue("SKUEnabled")
                .OrderByDescending("SKUInStoreFrom")
                .ToList();

            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Prepares a collection of products of the LearningProductType page type to be sent to a view
            IEnumerable<ProductListItemViewModel> productListing = products.Select(
                            product => new ProductListItemViewModel(
                                product,
                                GetPrice(product.SKU, cart),
                                product.Product.PublicStatus?.PublicStatusDisplayName));

            // Displays the action's view with an initialized view model
            return View(productListing);
        }


        // Retrieves a ProductCatalogPrices instance that contains calculated price information for the given product
        private ProductCatalogPrices GetPrice(SKUInfo product, ShoppingCartInfo cart)
        {
            return calculatorFactory
                        .GetCalculator(cart.ShoppingCartSiteID)
                        .GetPrices(product, Enumerable.Empty<SKUInfo>(), cart);
        }
    }
    //EndDocSection:ListingController
}