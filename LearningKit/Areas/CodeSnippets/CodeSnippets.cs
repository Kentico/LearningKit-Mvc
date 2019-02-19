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
using CMS.SiteProvider;
using CMS.WebAnalytics;

using Kentico.Membership;

using LearningKit.Models.Products;


namespace LearningKit.Areas.CodeSnippets
{
    /// <summary>
    /// This is a dummy class with code snippets used in the Kentico documentation.
    /// These code snippets do NOT take any part in the runnable LearningKit project.
    /// </summary>
    public class CodeSnippets : Controller
    {

        private readonly IShoppingService shoppingService = Service.Resolve<IShoppingService>();
        private readonly ICatalogPriceCalculatorFactory calculatorFactory = Service.Resolve<ICatalogPriceCalculatorFactory>();

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
            ProductCatalogPrices productPrice = Service.Resolve<ICatalogPriceCalculatorFactory>()
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
            List<ProductVariant> variants = VariantHelper.GetVariants(product.NodeSKUID).OnSite(SiteContext.CurrentSiteID).ToList()
                .Select(s => new ProductVariant(s.SKUID))
                .OrderBy(v => v.Variant.SKUPrice).ToList();

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

            //DocSection:RedirectForManualPayment
            return RedirectToAction("ThankYou", new { orderID = order.OrderID });
            //EndDocSection:RedirectForManualPayment
        }


        //DocSection:DisplayProduct
        /// <summary>
        /// Displays a product detail page of a product specified by the GUID of the product's page.
        /// </summary>
        /// <param name="guid">Node GUID of the product's page.</param>
        /// <param name="productAlias">Node alias of the product's page.</param>
        public ActionResult Detail(Guid guid, string productAlias)
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
        /// Retrieves the product specified by GUID of the product's page.
        /// </summary>
        /// <param name="nodeGuid">Node GUID of the product's page.</param>
        private SKUTreeNode GetProduct(Guid nodeGuid)
        {
            // Gets the page with the node GUID
            TreeNode node = DocumentHelper.GetDocuments()
                            .LatestVersion(false)
                            .Published(true)
                            .OnSite(SiteContext.CurrentSiteName)
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


        private object DummyEcommerceMethod2()
        {
            SKUInfo sku = null;

            //DocSection:DisplayCatalogDiscounts
            // Gets the current shopping cart
            ShoppingCartInfo shoppingCart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart();

            // Calculates prices for the specified product
            ProductCatalogPrices price = Service.Resolve<ICatalogPriceCalculatorFactory>()
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
            ShoppingCartInfo shoppingCart = Service.Resolve<IShoppingService>().GetCurrentShoppingCart();

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
            IEnumerable<string> searchIndexes = new List<string> { "ProductIndex", "ArticleIndex" };
            int pageNumber = 1;
            int pageSize = 10;
            UserInfo searchUser = MembershipContext.AuthenticatedUser;
            string cultureCode = "en-us";
            bool combineWithDefaultCulture = true;

            SearchParameters searchParameters = SearchParameters.PrepareForPages("search query", searchIndexes, pageNumber, pageSize, searchUser, cultureCode, combineWithDefaultCulture);

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
        private string userName = "PlaceHolder";

        private void DummyContactMergeMethod()
        {
            //DocSection:MergeContact
            ContactManagementContext.UpdateUserLoginContact(userName);
            //EndDocSection:MergeContact
        }

        private async void DummyRoleCheckMethod()
        {
            UserManager UserManager = HttpContext.GetOwinContext().Get<UserManager>();
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
        private readonly IPagesActivityLogger pageActivityLogger;

        public SampleSearchController()
        {
            pageActivityLogger = Service.Resolve<IPagesActivityLogger>();

            // ...
        }

        // ...
        //EndDocSection:InternalSearchActivityLoggerInit

        private string searchKeywords = "search query";

        private void DummyLogSearchMethod()
        {
            //DocSection:LogInternalSearchActivity
            pageActivityLogger.LogInternalSearch(searchKeywords);
            //EndDocSection:LogInternalSearchActivity
        }
    }    
}

namespace LearningKit.Areas.CodeSnippetDuplicates
{
    //DocSection:MembershipActivityLoggerInit
    public class MembershipController : Controller
    {
        private readonly IMembershipActivityLogger membershipActivityLogger;

        public MembershipController()
        {
            membershipActivityLogger = Service.Resolve<IMembershipActivityLogger>();

            // ...
        }

        // ...
        //EndDocSection:MembershipActivityLoggerInit

        private string userName = "PlaceHolder";

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