using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using CMS.Activities.Loggers;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Membership;
using CMS.Personas;
using CMS.Search;
using CMS.WebAnalytics;
using CMS.Base;

using Kentico.Membership;
using Kentico.Content.Web.Mvc;

using LearningKit.Models.Products;

namespace LearningKit.Areas.CodeSnippets
{
    public class PageDataContextInitialization : Controller
    {
        //DocSection:PageDataContextInitialize
        private readonly IPageRetriever pageRetriever;
        private readonly IPageDataContextInitializer pageDataContextInitializer;

        // Gets instances of required services using dependency injection
        public PageDataContextInitialization(IPageRetriever pageRetriever,
                                             IPageDataContextInitializer pageDataContextInitializer)
        {
            this.pageRetriever = pageRetriever;
            this.pageDataContextInitializer = pageDataContextInitializer;
        }

        public ActionResult Home()
        {
            // Retrieves a page from the Xperience database with the '/Home' node alias path
            TreeNode page = pageRetriever.Retrieve<TreeNode>(query => query
                                .Path("/Home", PathTypeEnum.Single))
                                .FirstOrDefault();

            // Responds with the HTTP 404 error when the page is not found
            if (page == null)
            {
                return HttpNotFound();
            }

            // Initializes the page data context using the retrieved page
            pageDataContextInitializer.Initialize(page);

            return View();
        }
        //EndDocSection:PageDataContextInitialize
    }

    /// <summary>
    /// This is a dummy class with code snippets used in the Xperience documentation.
    /// These code snippets do NOT take any part in the runnable LearningKit project.
    /// </summary>
    public class CodeSnippets : Controller
    {
        private readonly IShoppingService shoppingService;
        private readonly ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory;
        private readonly ISiteService siteService;
        private readonly IPageDataContextRetriever pageRetriever;

        public CodeSnippets(IShoppingService shoppingService, 
                            ICatalogPriceCalculatorFactory catalogPriceCalculatorFactory,
                            ISiteService siteService,
                            IPageDataContextRetriever pageRetriever)
        {
            this.shoppingService = shoppingService;
            this.catalogPriceCalculatorFactory = catalogPriceCalculatorFactory;
            this.siteService = siteService;
            this.pageRetriever = pageRetriever;
        }

        //DocSection:DifferentShippingAddress
        public bool ShippingAddressIsDifferent { get; set; }
        //EndDocSection:DifferentShippingAddress

        private object DummyEcommerceMethod()
        {
            ShoppingCartInfo shoppingCart = null;
            SKUInfo productSku = null;
            ProductVariant variant = null;
            SKUTreeNode product = null;
            SKUInfo sku = null;
            DummyViewModel model = null;
            OrderInfo order = null;
            PaymentResultInfo result = null;

            //DocSection:CalculatePriceOptions
            ProductCatalogPrices productPrice = catalogPriceCalculatorFactory
               .GetCalculator(shoppingCart.ShoppingCartSiteID)
               .GetPrices(productSku, Enumerable.Empty<SKUInfo>(), shoppingCart);
            //EndDocSection:CalculatePriceOptions

            //DocSection:FormatPriceOptions
            decimal price = 5.50M;
            string formattedPrice = String.Format(shoppingCart.Currency.CurrencyFormatString, price);
            //EndDocSection:FormatPriceOptions

            //DocSection:VariantDisplayImg
            var response = new
            {
                // ...

                imagePath = Url.Content(variant.Variant.SKUImagePath)
            };
            //EndDocSection:VariantDisplayImg

            //DocSection:DisplayAttributeSelection
            // Gets the cheapest variant of the product
            List<ProductVariant> variants = VariantHelper.GetVariants(product.NodeSKUID)
                .OnSite(siteService.CurrentSite.SiteID).ToList()
                    .Select(s => new ProductVariant(s.SKUID))
                .OrderBy(v => v.Variant.SKUPrice)
                .ToList();

            ProductVariant cheapestVariant = variants.FirstOrDefault();

            // Gets the product's option categories
            IEnumerable<OptionCategoryInfo> categories = VariantHelper.GetProductVariantsCategories(sku.SKUID).ToList();

            // Gets the cheapest variant's selected attributes
            IEnumerable<ProductOptionCategoryViewModel> variantCategories = cheapestVariant?.ProductAttributes.Select(
                option =>
                    new ProductOptionCategoryViewModel(sku.SKUID, option.SKUID,
                            categories.FirstOrDefault(c => c.CategoryID == option.SKUOptionCategoryID)));
            //EndDocSection:DisplayAttributeSelection

            //DocSection:ShippingIsDifferent
            if (model.IsShippingAddressDifferent)
            {
                // ...
            }
            //EndDocSection:ShippingIsDifferent

            //DocSection:DifferentPaymentMethods
            if (shoppingCart.PaymentOption.PaymentOptionName.Equals("PaymentMethodCodeName"))
            {
                return RedirectToAction("ActionForPayment", "MyPaymentGateway");
            }
            //EndDocSection:DifferentPaymentMethods

            //DocSection:SetPaymentResult
            order.UpdateOrderStatus(result);
            //EndDocSection:SetPaymentResult

            //DocSection:ImageUrl
            // Returns the relative path to the image of the provided product
            string relativeUrl = new FileUrl(sku.SKUImagePath, true).RelativePath;
            //EndDocSection:ImageUrl

            //DocSection:RedirectForManualPayment
            return RedirectToAction("ThankYou", new { orderID = order.OrderID });
            //EndDocSection:RedirectForManualPayment
        }


        //DocSection:DisplayProduct
        /// <summary>
        /// Displays a product detail page of a product.
        /// </summary>
        public ActionResult Detail()
        {
            // Gets the product from the data context
            SKUTreeNode product = GetProduct();

            // If the product is not found or if it is not allowed for sale, redirects to error 404
            if ((product == null) || (product.SKU == null) || !product.SKU.SKUEnabled)
            {
                return HttpNotFound();
            }

            // Initializes the view model of the product with a calculated price
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            ProductCatalogPrices price = catalogPriceCalculatorFactory
                .GetCalculator(cart.ShoppingCartSiteID)
                .GetPrices(product.SKU, Enumerable.Empty<SKUInfo>(), cart);

            // Fills the product model with retrieved data
            ProductViewModel viewModel = new ProductViewModel(product, price);

            // Displays the product details page
            return View("Detail", viewModel);
        }


        /// <summary>
        /// Retrieves the product.
        /// </summary>
        private SKUTreeNode GetProduct()
        {
            // Gets the current page from the router data context
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


        private object DummyEcommerceMethod2()
        {
            SKUInfo sku = null;

            //DocSection:DisplayCatalogDiscounts
            // Gets the current shopping cart using IShoppingService
            ShoppingCartInfo shoppingCart = shoppingService.GetCurrentShoppingCart();

            // Calculates prices for the specified product using ICatalogPriceCalculatorFactory
            ProductCatalogPrices price = catalogPriceCalculatorFactory
                                            .GetCalculator(shoppingCart.ShoppingCartSiteID)
                                            .GetPrices(sku, Enumerable.Empty<SKUInfo>(), shoppingCart);

            // Gets the catalog discount
            decimal catalogDiscount = price.StandardPrice - price.Price;
            //EndDocSection:DisplayCatalogDiscounts
            return null;
        }

        private void DummyEcommerceMethod3()
        {
            //DocSection:DisplayFreeShippingOffers
            // Gets the current shopping cart
            ShoppingCartInfo shoppingCart = shoppingService.GetCurrentShoppingCart();

            // Gets the remaining amount for free shipping
            decimal remainingFreeShipping = shoppingCart.CalculateRemainingAmountForFreeShipping();
            //EndDocSection:DisplayFreeShippingOffers
        }

        // Displaying product variants on MVC sites
        private object DummyEcommerceMethod4()
        {
            int productId = 0;
            IEnumerable<int> optionIds = new List<int>() { 1, 2, 3};

            //DocSection:VariantFromOptionIds
            // Retrieves a product variant from a given product ID and a collection of option IDs
            var variantSKU = VariantHelper.GetProductVariant(productId, new ProductAttributeSet(optionIds));

            if (variantSKU == null)
            {
                return null;
            }

            return variantSKU;
            //EndDocSection:VariantFromOptionIds
        }

        private void DummyPersonalizationMethod()
        {
            //DocSection:PersonaPersonalization
            // Gets the current contact
            ContactInfo currentContact = ContactManagementContext.GetCurrentContact();

            // Gets the code name of the current contact's persona
            string currentPersonaName = currentContact?.GetPersona()?.PersonaName;

            // Checks whether the current contact is assigned to the "EarlyAdopter" persona
            if (String.Equals(currentPersonaName, "EarlyAdopter", StringComparison.InvariantCultureIgnoreCase))
            {
                // Serve personalized content for the "EarlyAdopter" persona
            }
            //EndDocSection:PersonaPersonalization
        }

        private void DummySearchMethod()
        {
            //DocSection:InitializeSearch
            // Prepares variables required to perform the search operation
            IEnumerable<string> searchIndexes = new List<string> { "ProductIndex", "ArticleIndex" };
            int pageNumber = 1;
            int pageSize = 10;
            UserInfo searchUser = MembershipContext.AuthenticatedUser;
            string cultureCode = "en-us";
            /* Indicates whether the search service uses site default language version of pages as a replacement
            for pages that are not translated into the language specified by 'cultureCode' */
            bool combineWithDefaultCulture = true;

            // Prepares a 'SearchParameters' object to search through indexes of the 'Pages' type
            SearchParameters searchParameters = SearchParameters.PrepareForPages("search query", searchIndexes, pageNumber, pageSize, searchUser, cultureCode, combineWithDefaultCulture);

            // Searches the specified indexes
            SearchResult searchResult = SearchHelper.Search(searchParameters);
            //EndDocSection:InitializeSearch
        }

        private void DummyContactMethod()
        {
            //DocSection:GetCurrentContact
            // Gets the current contact
            ContactInfo currentContact = ContactManagementContext.GetCurrentContact();
            //EndDocSection:GetCurrentContact
        }
    }
                
    internal class ProductOptionCategoryViewModel
    {
        public ProductOptionCategoryViewModel(int skuID, int selectedOptionID, OptionCategoryInfo category)
        {
        }
    }
                
    internal class OrdersViewModel
    {
        public IEnumerable<OrderInfo> Orders { get; set; }
    }

    internal class DummyViewModel
    {
        public bool IsShippingAddressDifferent { get; set; }
    }

    public class MembershipController : Controller
    {
        private readonly string userName = "PlaceHolder";

        private void DummyContactMergeMethod()
        {
            //DocSection:MergeContact
            ContactManagementContext.UpdateUserLoginContact(userName);
            //EndDocSection:MergeContact
        }

        private async void DummyRoleCheckMethod()
        {
            KenticoUserManager UserManager = HttpContext.GetOwinContext().Get<KenticoUserManager>();
            User CurrentUser = UserManager.FindByName(User.Identity.Name);

            //DocSection:CheckRole
            // Checks whether the current user is assigned to the "KenticoRole" role
            if (await UserManager.IsInRoleAsync(CurrentUser.Id, "KenticoRole"))
            {
                // ...
            }
            //EndDocSection:CheckRole
        }
    }

    //DocSection:InternalSearchActivityLoggerInit
    public class SampleSearchController : Controller
    {
        private readonly IPagesActivityLogger pagesActivityLogger;

        public SampleSearchController(IPagesActivityLogger pagesActivityLogger)
        {
            this.pagesActivityLogger = pagesActivityLogger;

            // ...
        }

        // ...
        //EndDocSection:InternalSearchActivityLoggerInit

        private readonly string searchKeywords = "search query";

        private void DummyLogSearchMethod()
        {
            //DocSection:LogInternalSearchActivity
            pagesActivityLogger.LogInternalSearch(searchKeywords);
            //EndDocSection:LogInternalSearchActivity
        }
    }

    //DocSection:WishlistActivityLoggerInit
    public class SampleWishlistController : Controller
    {
        private readonly IEcommerceActivityLogger ecommerceActivityLogger;

        public SampleWishlistController(IEcommerceActivityLogger ecommerceActivityLogger)
        {
            this.ecommerceActivityLogger = ecommerceActivityLogger;

            // ...
        }

        // ...
        //EndDocSection:WishlistActivityLoggerInit

        private readonly SKUInfo wishlistProduct = new SKUInfo();

        private void DummyLogWishlistMethod()
        {
            //DocSection:LogWishlistActivity
            ecommerceActivityLogger.LogProductAddedToWishlistActivity(wishlistProduct);
            //EndDocSection:LogWishlistActivity
        }
    }
}

namespace LearningKit.Areas.CodeSnippetDuplicates
{
    //DocSection:MembershipActivityLoggerInit
    public class MembershipController : Controller
    {
        private readonly IMembershipActivityLogger membershipActivityLogger;

        public MembershipController(IMembershipActivityLogger membershipActivityLogger)
        {
            this.membershipActivityLogger = membershipActivityLogger;

            // ...
        }

        // ...
        //EndDocSection:MembershipActivityLoggerInit

        private readonly string userName = "PlaceHolder";

        private void DummyLogActivityMethod()
        {
            //DocSection:LogRegisterActivity
            membershipActivityLogger.LogRegistration(userName);
            //EndDocSection:LogRegisterActivity

            //DocSection:LogSignInActivity
            membershipActivityLogger.LogLogin(userName);
            //EndDocSection:LogSignInActivity
        }        
    }
}