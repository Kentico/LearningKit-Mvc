using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.DocumentEngine.Types.LearningKit;

using Kentico.Content.Web.Mvc;

using LearningKit.Models.ProductFilter;
using LearningKit.Models.Products;

namespace LearningKit.Controllers
{
    public class ProductFilterController : Controller
    {
        private readonly IShoppingService shoppingService;
        private readonly ICatalogPriceCalculatorFactory catalogPriceCalculator;
        private readonly IPageUrlRetriever pageUrlRetriever;
        private readonly IPageRetriever pageRetriever;


        //DocSection:Controller
        /// <summary>
        /// Initializes instances of services required to facilitate product filtering.
        /// </summary>
        public ProductFilterController(IShoppingService shoppingService,
                                       ICatalogPriceCalculatorFactory catalogPriceCalculator,
                                       IPageUrlRetriever pageUrlRetriever,
                                       IPageRetriever pageRetriever)
        { 
            this.shoppingService = shoppingService;
            this.catalogPriceCalculator = catalogPriceCalculator;
            this.pageUrlRetriever = pageUrlRetriever;
            this.pageRetriever = pageRetriever;
        }
        //EndDocSection:Controller



        /// <summary>
        /// Displays a product listing page of the class's product page type with a possibility
        /// to filter products based on page type information.
        /// </summary>
        public ActionResult FilterPageProperty()
        {
            ProductFilterViewModel model = new ProductFilterViewModel
            {
                LPTWithFeature = false,
                FilteredProducts = LoadProducts()
            };

            return View(model);
        }


        //DocSection:PagePropertyModel
        /// <summary>
        /// Displays a product listing page of the class's product page type
        /// filtered based on the specified model.
        /// </summary>
        /// <param name="model">Model specifying all filtered products.</param>
        [HttpPost]
        public ActionResult FilterPageProperty(ProductFilterViewModel model)
        {                        
            // Creates a new ProductFilterViewModel which holds a list of products
            // that have the "LPTWithFeature" property checked.
            ProductFilterViewModel filteredModel = new ProductFilterViewModel
            {
                LPTWithFeature = model.LPTWithFeature,
                FilteredProducts = LoadProducts(GetWithFeatureWhereCondition(model.LPTWithFeature))
            };            

            return View(filteredModel);
        }
        //EndDocSection:PagePropertyModel


        //DocSection:PagePropertyWhere   
        /// <summary>
        /// Returns a where condition to correctly retrieve which products are selected in the filter.
        /// </summary>
        /// <param name="model">Model specifying all filtered products.</param>
        /// <returns>Where condition specifying which products are selected.</returns>
        private WhereCondition GetWithFeatureWhereCondition(bool withFeature)
        {   
                  
            // Initializes a new where condition
            WhereCondition withFeatureWhere = new WhereCondition();

            // If the feature is selected, sets the where condition
            if (withFeature)
            {
                withFeatureWhere.WhereTrue("LPTWithFeature");
            }
            
            // Returns the where condition
            return withFeatureWhere;
        }
        //EndDocSection:PagePropertyWhere


        /// <summary>
        /// Displays a product listing page of the class's product page type 
        /// with a possibility to filter products based on SKU information.
        /// </summary>
        public ActionResult FilterSKUProperty()
        {
            ProductFilterViewModel model = new ProductFilterViewModel
            {
                FilteredProducts = LoadProducts()
            };

            return View(model);
        }


        //DocSection:SKUPropertyModel
        /// <summary>
        /// Displays a product listing page of the class's product page type
        /// filtered based on the specified model.
        /// </summary>
        /// <param name="model">Model specifying all filtered products.</param>
        [HttpPost]
        public ActionResult FilterSKUProperty(ProductFilterViewModel model)
        {            
            // Creates a view model that consists of the entered price range
            // and a list of products
            ProductFilterViewModel filteredModel = new ProductFilterViewModel
            {
                PriceFrom = model.PriceFrom,
                PriceTo = model.PriceTo,
                FilteredProducts = LoadProducts(GetPriceWhereCondition(model))
            };

            return View(filteredModel);            
        }
        //EndDocSection:SKUPropertyModel


        //DocSection:SKUPropertyWhere
        /// <summary>
        /// Returns a where condition to correctly retrieve which products are selected in the filter.
        /// </summary>
        /// <param name="model">Model specifying all filtered products.</param>
        /// <returns>Where condition specifying which products are selected.</returns>
        private WhereCondition GetPriceWhereCondition(ProductFilterViewModel model)
        {
            
            // Initializes a new where condition
            WhereCondition priceWhere = new WhereCondition();

            // Sets the price where condition based on the model's values and limited by the price from-to range
            if (Constraint(model.PriceFrom, model.PriceTo))
            {
                priceWhere.WhereGreaterOrEquals("SKUPrice", model.PriceFrom)
                    .And().WhereLessOrEquals("SKUPrice", model.PriceTo);
            }            

            // Returns the where condition
            return priceWhere;
        }
        //EndDocSection:SKUPropertyWhere


        /// <summary>
        /// Dummy check that the from and to are not zero to provide a working example.
        /// </summary>
        /// <param name="from">Price from.</param>
        /// <param name="to">Price true.</param>
        /// <returns>True if at least one of the parameters is not 0.</returns>
        private bool Constraint(decimal from, decimal to)
        {
            return (from != 0) || to != 0;
        }


        //DocSection:ForeignPropertyGetModel
        /// <summary>
        /// Displays a product listing page of the class's product page type with a possibility
        /// to filter products based on a foreign entity.
        /// </summary>
        public ActionResult FilterForeignProperty()
        {
            
            // Creates a view model that consists of all foreign objects (manufacturers) related to the products
            // and a list of products that will be filtered
            ProductFilterViewModel model = new ProductFilterViewModel
            {
                Manufacturers = GetManufacturers(),
                FilteredProducts = LoadProducts()
            };

            return View(model);            
        }
        //EndDocSection:ForeignPropertyGetModel


        //DocSection:ForeignPropertyPostModel
        /// <summary>
        /// Displays a product listing page of the class's product page type
        /// filtered based on the specified model.
        /// </summary>
        /// <param name="model">Model specifying all foreign objects and all filtered products.</param>
        [HttpPost]
        public ActionResult FilterForeignProperty(ProductFilterViewModel model)
        {            
            // Creates a view model that consists of all foreign objects (manufacturers) related to the products
            // and a list of products that will be filtered with their selected state
            ProductFilterViewModel filteredModel = new ProductFilterViewModel
            {
                Manufacturers = model.Manufacturers,
                FilteredProducts = LoadProducts(GetManufacturersWhereCondition(model))
            };

            return View(filteredModel);           
        }
        //EndDocSection:ForeignPropertyPostModel


        //DocSection:GetManufacturers
        /// <summary>
        /// Loads all available manufacturers assigned to products of the LearningProductType product page type.
        /// </summary>
        /// <returns>List of manufacturers' models and their unselected state.</returns>
        private List<ProductFilterCheckboxViewModel> GetManufacturers()
        {            
            // Gets all manufacturers assigned to products of the LearningProductType 
            var manufacturers = pageRetriever.Retrieve<LearningProductType>()
                .ToList()
                .Where(skuPage => skuPage.Product.Manufacturer != null)
                .Select(skuPage =>
                    new
                    {
                        skuPage.Product.Manufacturer?.ManufacturerID,
                        skuPage.Product.Manufacturer?.ManufacturerDisplayName
                    })
                .Distinct();

            // Returns a list of models that contain the manufacturers' display name, ID and false select state
            return manufacturers.Select(manufacturer => new ProductFilterCheckboxViewModel
            {
                DisplayName = manufacturer.ManufacturerDisplayName,
                Id = manufacturer.ManufacturerID.ToString(),
                IsChecked = false
            }).ToList();            
        }
        //EndDocSection:GetManufacturers


        //DocSection:ForeignPropertyWhere
        /// <summary>
        /// Returns a where condition to correctly retrieve which manufacturers are selected in the filter.
        /// </summary>
        /// <param name="model">Model specifying all foreign objects and all filtered products.</param>
        private WhereCondition GetManufacturersWhereCondition(ProductFilterViewModel model)
        {            
            // Initializes a new where condition
            WhereCondition manufacturersWhere = new WhereCondition();

            // Gets a list of manufacturers that were selected on the live site
            List<string> selectedManufacturersIds = model.Manufacturers
                                                        .Where(manufacturer => manufacturer.IsChecked)
                                                        .Select(manufacturer => manufacturer.Id)
                                                        .ToList();

            // If any manufacturer is selected, sets the where condition
            if (selectedManufacturersIds.Any())
            {
                manufacturersWhere.WhereIn("SKUManufacturerID", selectedManufacturersIds);
            }            

            // Returns the where condition
            return manufacturersWhere;
        }
        //EndDocSection:ForeignPropertyWhere


        /// <summary>
        /// Loads pages from the current site of the LearningProductType product page type.
        /// </summary>
        /// <returns>List of view models representing a products, its prices and public status display name.</returns>
        private List<ProductListItemViewModel> LoadProducts()
        {
            return LoadProducts(null);
        }


        //DocSection:LoadProducts
        /// <summary>
        /// Loads pages of the LearningProductType product page type based on the specified where condition.
        /// </summary>
        /// <param name="where">Where condition that restricts the returned products.</param>
        /// <returns>List of view models representing products, its prices and public status display name.</returns>
        private List<ProductListItemViewModel> LoadProducts(WhereCondition where)
        {            
            // Gets products of the LearningProductType page type from the current site
            List<LearningProductType> products = pageRetriever.Retrieve<LearningProductType>(query => query
               .CombineWithDefaultCulture()
               .WhereTrue("SKUEnabled")
               .Where(where)
               .OrderByDescending("SKUInStoreFrom"))
               .ToList();

            // Gets the current shopping cart (necessary to contextualize product price calculations)
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Returns a list of products filtered by the where condition
            return products.Select(
                product => new ProductListItemViewModel(
                    product,
                    GetPrice(product.SKU, cart),
                    pageUrlRetriever,
                    product.Product.PublicStatus?.PublicStatusDisplayName)
                ).ToList();
        }


        // Retrieves a ProductCatalogPrices instance that contains calculated price information for the given product
        private ProductCatalogPrices GetPrice(SKUInfo product, ShoppingCartInfo cart)
        {
            return catalogPriceCalculator
                .GetCalculator(cart.ShoppingCartSiteID)
                .GetPrices(product, Enumerable.Empty<SKUInfo>(), cart);
        }
        //EndDocSection:LoadProducts
    }
}