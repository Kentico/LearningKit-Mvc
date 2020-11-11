using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CMS.Base;
using CMS.Ecommerce;

namespace LearningKit.Controllers
{
    /// <summary>
    /// Controller providing EC utilities to ease the setup and use of Learning Kit EC implementations.
    /// </summary>
    public class ECUtilitiesController : Controller
    {
        private readonly ISiteService siteService;
        private readonly IShoppingService shoppingService;
        private readonly ISKUInfoProvider sKUInfoProvider;

        public ECUtilitiesController(ISiteService siteService,
                                     IShoppingService shoppingService,
                                     ISKUInfoProvider sKUInfoProvider)
        {
            this.siteService = siteService;
            this.shoppingService = shoppingService;
            this.sKUInfoProvider = sKUInfoProvider;
        }

        /// <summary>
        /// Randomly fills a cart with fake products from COM_SKU.
        /// If there are none, creates them.
        /// </summary>
        /// <remarks>
        /// To modify which products are relevant, edit <see cref="GetRelevantSKUIDs"/>.
        /// </remarks>
        public ActionResult FillShoppingCart()
        {
            var SKUIDs = GetRelevantSKUIDs();

            if (SKUIDs.Count >= 3)
            {
                FillCart(SKUIDs);
            }
            else
            {
                CreateSampleSKUs();
                FillCart(GetRelevantSKUIDs());
            }

            return RedirectToAction("ShoppingCart", "Checkout");
        }


        /// <summary>
        /// Fills the shopping cart with generated products of random quantities.
        /// </summary>
        /// <param name="SKUIDs">See <see cref="GetRelevantSKUIDs"/>></param>
        private void FillCart(List<int> SKUIDs)
        {
            for (int i = 0; i < 3; i++)
            {
                int chosenSKUID = new Random().Next(0, SKUIDs.Count - 1);
                int units = new Random().Next(1, 6);

                var info = shoppingService.AddItemToCart(SKUIDs[chosenSKUID], units);
                Console.WriteLine(info.ToString());
            }
        }


        /// <summary>
        /// If COM_SKU is empty or has less than 3 records (SKUs), creates up to 3 sample SKUs.
        /// </summary>
        public ActionResult CreateSampleSKUs()
        {
            var SKUIDs = GetRelevantSKUIDs();

            if (SKUIDs.Count < 3)
            {
                for (int i = 0; i < (3 - SKUIDs.Count); i++)
                {
                    SKUInfo.Provider.Set(new SKUInfo()
                    {
                        SKUName = "SampleProduct No. " + (i + 1),
                        SKUDescription = "This is a sample product for MVC Learning Kit.",
                        SKUShortDescription = "LearningKit_SampleData",
                        SKUPrice = 15.99m + new Random().Next(1, 25),
                        SKUSiteID = siteService.CurrentSite.SiteID,
                        SKUEnabled = true,
                        SKUTrackInventory = TrackInventoryTypeEnum.ByProduct,
                        SKUAvailableItems = 100
                    });
                }
            }

            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Deletes all sample SKUs created by <see cref="CreateSampleSKUs"/>.
        /// </summary>
        public ActionResult DeleteSampleSKUs()
        {
            var sampleSKUs = SKUInfoProvider.GetSKUs(siteService.CurrentSite.SiteID).WhereEquals("SKUShortDescription", "LearningKit_SampleData");

            foreach (var SKU in sampleSKUs)
            {
                sKUInfoProvider.Delete(SKU);
            }

            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Removes all items from the current shopping cart.
        /// </summary>
        public ActionResult RemoveAllItemsFromShoppingCart()
        {
            shoppingService.RemoveAllItemsFromCart();

            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Gets relevant SKUs from the database based on the query.
        /// </summary>
        /// <returns>List of IDs of relevant SKUs.</returns>
        private List<int> GetRelevantSKUIDs()
        {
            return SKUInfoProvider.GetSKUs(siteService.CurrentSite.SiteID)
                .WhereTrue("SKUEnabled")
                .WhereNull("SKUOptionCategoryID")
                .WhereEquals("SKUProductType", "PRODUCT")
                .WhereEquals("SKUTrackInventory", "ByProduct")
                .WhereEquals(nameof(SKUInfo.SKUShortDescription), "LearningKit_SampleData")
                .OrderBy("SKUID")
                .Select(sku => sku.SKUID)
                .ToList();
        }
    }
}