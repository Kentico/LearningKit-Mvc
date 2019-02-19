using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

using CMS.Core;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;

using LearningKit.Models.Checkout;
using CMS.Globalization;

namespace LearningKit.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly string siteName = SiteContext.CurrentSiteName;
        private readonly IShoppingService shoppingService;


        //DocSection:Constructor
        /// <summary>
        /// Initializes an instance of the IShoppingService used to facilitate shopping cart interactions.
        /// </summary>
        public CheckoutController()
        {
            // Initializes an instance of a service required to manage the shopping cart
            // For real-world projects, we recommend using a dependency injection
            // container to initialize service instances
            shoppingService = Service.Resolve<IShoppingService>();
        }
        //EndDocSection:Constructor


        //DocSection:DisplayCart
        /// <summary>
        /// Displays the customer's current shopping cart.
        /// </summary>
        public ActionResult ShoppingCart()
        {
            // Gets the current user's shopping cart
            ShoppingCartInfo currentCart = shoppingService.GetCurrentShoppingCart();

            // Initializes the shopping cart model
            ShoppingCartViewModel model = new ShoppingCartViewModel(currentCart);

            // Displays the shopping cart
            return View(model);
        }
        //EndDocSection:DisplayCart


        //DocSection:AddItem
        /// <summary>
        /// Adds products to the customer's current shopping cart.
        /// </summary>
        /// <param name="itemSkuId">ID of the added item (its SKU object).</param>
        /// <param name="itemUnits">Quantity of the item to be added.</param>
        [HttpPost]
        public ActionResult AddItem(int itemSkuId, int itemUnits)
        {
            // Adds the specified number of units of a given product to the current shopping cart
            shoppingService.AddItemToCart(itemSkuId, itemUnits);

            // Displays the shopping cart
            return RedirectToAction("ShoppingCart");
        }
        //EndDocSection:AddItem


        //DocSection:UpdateItem
        /// <summary>
        /// Updates the quantity of an item in the customer's current shopping cart.
        /// </summary>
        /// <param name="itemID">ID of the shopping cart item to update.</param>
        /// <param name="itemUnits">Desired quantity of the shopping cart item being updated.</param>
        [HttpPost]
        public ActionResult UpdateItem(int itemID, int itemUnits)
        {
            // Updates the quantity of a given product with the specified number of units
            // If the quantity is set to zero, removes the product from the shopping cart
            shoppingService.UpdateItemQuantity(itemID, itemUnits);

            // Displays the shopping cart
            return RedirectToAction("ShoppingCart");
        }
        //EndDocSection:UpdateItem


        //DocSection:RemoveItem
        /// <summary>
        /// Removes a shopping cart item from the customer's current shopping cart.
        /// </summary>
        /// <param name="itemID">ID of the item to be removed.</param>
        [HttpPost]
        public ActionResult RemoveItem(int itemID)
        {
            // Removes a specified product from the shopping cart
            shoppingService.RemoveItemFromCart(itemID);

            // Displays the shopping cart
            return RedirectToAction("ShoppingCart");
        }
        //EndDocSection:RemoveItem


        //DocSection:RemoveAllItems
        /// <summary>
        /// Removes all products from the customer's current shopping cart.
        /// </summary>
        public ActionResult RemoveAllItems()
        {
            // Removes all products from the current shopping cart
            shoppingService.RemoveAllItemsFromCart();

            // Displays the shopping cart
            return RedirectToAction(nameof(CheckoutController.ShoppingCart));
        }
        //EndDocSection:RemoveAllItems


        //DocSection:DetailUrl
        /// <summary>
        /// Redirects to a product detail page based on the ID of a product's SKU object.
        /// </summary>
        /// <param name="skuID">ID of the product's SKU object.</param>
        public ActionResult ItemDetail(int skuID)
        {
            // Gets the SKU object
            SKUInfo sku = SKUInfoProvider.GetSKUInfo(skuID);

            // If the SKU does not exist or it is a product option, returns error 404
            if (sku == null || sku.IsProductOption)
            {
                return HttpNotFound();
            }

            // If the SKU is a product variant, uses its parent product's ID
            if (sku.IsProductVariant)
            {
                skuID = sku.SKUParentSKUID;
            }

            // Gets the product's page
            TreeNode node = DocumentHelper.GetDocuments()
                .LatestVersion(false)
                .Published(true)
                .OnSite(siteName)
                .Culture("en-us")
                .CombineWithDefaultCulture()
                .WhereEquals("NodeSKUID", skuID)
                .FirstOrDefault();

            // Returns 404 if no page for the specified product exists
            if (node == null)
            {
                return HttpNotFound();
            }

            // Redirects to the product details page action method with the product information
            return RedirectToAction("Detail", "Product", new
            {
                guid = node.NodeGUID,
                productAlias = node.NodeAlias
            });
        }
        //EndDocSection:DetailUrl


        //DocSection:Checkout
        /// <summary>
        /// Validates the shopping cart and proceeds to the next checkout step.
        /// </summary>
        [HttpPost]
        [ActionName("ShoppingCart")]
        public ActionResult ShoppingCartCheckout()
        {
            // Gets the current user's shopping cart
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Validates the shopping cart
            var cartValidator = ShoppingCartInfoProvider.ValidateShoppingCart(cart);

            // If the validation is successful, redirects to the next step of the checkout process
            if (!cartValidator.Any())
            {
                // Saves the validated shopping cart before redirecting the checkout step
                // This prevents loss of data between requests.
                shoppingService.SaveCart();

                return RedirectToAction("DeliveryDetails");
            }

            // If the validation fails, redirects back to the shopping cart
            return RedirectToAction("ShoppingCart");
        }
        //EndDocSection:Checkout


        //DocSection:CouponCodeAdd
        /// <summary>
        /// Adds the specified coupon code to the shopping cart.
        /// </summary>
        [HttpPost]
        public ActionResult AddCouponCode(string couponCode)
        {
            // Adds the coupon code to the shopping cart
            if ((couponCode == "") || !shoppingService.AddCouponCode(couponCode))
            {
                // Adds an error message to the model state if the entered coupon code is not valid
                ModelState.AddModelError("CouponCodeError", "The entered coupon code is not valid.");
            }

            // Initializes the shopping cart model
            ShoppingCartViewModel model = new ShoppingCartViewModel(shoppingService.GetCurrentShoppingCart());

            // Displays the shopping cart
            return View("ShoppingCart", model);
        }
        //EndDocSection:CouponCodeAdd


        //DocSection:CouponCodeRemove
        /// <summary>
        /// Removes the specified coupon code from the shopping cart.
        /// </summary>
        [HttpPost]
        public ActionResult RemoveCouponCode(string couponCode)
        {
            // Removes the specified coupon code
            shoppingService.RemoveCouponCode(couponCode);

            // Displays the shopping cart
            return RedirectToAction("ShoppingCart");
        }
        //EndDocSection:CouponCodeRemove


        //DocSection:DisplayDelivery
        /// <summary>
        /// Displays the customer details checkout process step.
        /// </summary>
        public ActionResult DeliveryDetails()
        {
            // Gets the current user's shopping cart
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // If the shopping cart is empty, displays the shopping cart
            if (cart.IsEmpty)
            {
                return RedirectToAction(nameof(CheckoutController.ShoppingCart));
            }

            // Gets all countries for the country selector
            SelectList countries = new SelectList(CountryInfoProvider.GetCountries(), "CountryID", "CountryDisplayName");

            // Creates a collection of shipping options enabled for the current site
            SelectList shippingOptions = CreateShippingOptionList(cart);

            // Loads the customer details
            DeliveryDetailsViewModel model = new DeliveryDetailsViewModel
            {
                Customer = new CustomerViewModel(shoppingService.GetCurrentCustomer()),
                BillingAddress = new BillingAddressViewModel(shoppingService.GetBillingAddress(), countries, null),
                ShippingOption = new ShippingOptionViewModel(ShippingOptionInfoProvider.GetShippingOptionInfo(shoppingService.GetShippingOption()), shippingOptions)
            };

            // Displays the customer details step
            return View(model);
        }


        // Prepares a shipping option select list together with calculated shipping prices
        private SelectList CreateShippingOptionList(ShoppingCartInfo cart)
        {
            // Gets the shipping options configured and enabled for the current site
            IEnumerable<ShippingOptionInfo> shippingOptions = ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID, true);

            // Creates a collection of SelectListItems
            IEnumerable<SelectListItem> selectList = shippingOptions.Select(shippingOption =>
            {
                // Calculates the shipping price for a given shipping option based on the contents of the current
                // shopping cart and currently running store promotions (e.g., free shipping offers)
                decimal shippingPrice = shoppingService.CalculateShippingOptionPrice(shippingOption);

                // Gets the currency information from the shopping cart
                CurrencyInfo currency = cart.Currency;

                // Creates a select list item with shipping option name and a calculate shipping price
                return new SelectListItem
                {
                    Value = shippingOption.ShippingOptionID.ToString(),
                    Text = $"{shippingOption.ShippingOptionDisplayName} ({String.Format(currency.CurrencyFormatString, shippingPrice)})"
                };
            });

            // Returns a new SelectList instance
            return new SelectList(selectList, "Value", "Text");
        }
        //EndDocSection:DisplayDelivery


        //DocSection:DisplayDeliveryAddressSelector
        /// <summary>
        /// Displays the customer details checkout process step with an address selector for known customers.
        /// </summary>
        public ActionResult DeliveryDetailsAddressSelector()
        {
            // Gets the current user's shopping cart
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // If the shopping cart is empty, redirects to the shopping cart view
            if (cart.IsEmpty)
            {
                return RedirectToAction("ShoppingCart");
            }

            // Gets all countries for the country selector
            SelectList countries = new SelectList(CountryInfoProvider.GetCountries(), "CountryID", "CountryDisplayName");

            // Gets the current customer
            CustomerInfo customer = shoppingService.GetCurrentCustomer();

            // Gets all customer billing addresses for the address selector
            IEnumerable<AddressInfo> customerAddresses = Enumerable.Empty<AddressInfo>();
            if (customer != null)
            {
                customerAddresses = AddressInfoProvider.GetAddresses(customer.CustomerID).ToList();
            }

            // Prepares address selector options
            SelectList addresses = new SelectList(customerAddresses, "AddressID", "AddressName");

            // Gets all enabled shipping options for the shipping option selector
            SelectList shippingOptions = new SelectList(ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID, true).ToList(), "ShippingOptionID", "ShippingOptionDisplayName");

            // Loads the customer details
            DeliveryDetailsViewModel model = new DeliveryDetailsViewModel
            {
                Customer = new CustomerViewModel(shoppingService.GetCurrentCustomer()),
                BillingAddress = new BillingAddressViewModel(shoppingService.GetBillingAddress(), countries, addresses),
                ShippingOption = new ShippingOptionViewModel(ShippingOptionInfoProvider.GetShippingOptionInfo(shoppingService.GetShippingOption()), shippingOptions)
            };


            // Displays the customer details step
            return View(model);
        }
        //EndDocSection:DisplayDeliveryAddressSelector


        //DocSection:PostDelivery
        /// <summary>
        /// Validates the entered customer details and proceeds to the order review step.
        /// </summary>
        /// <param name="model">View model with the customer details.</param>
        [HttpPost]
        public ActionResult DeliveryDetails(DeliveryDetailsViewModel model)
        {
            // Gets the user's current shopping cart
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Gets all enabled shipping options for the shipping option selector
            SelectList shippingOptions = new SelectList(ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID, true).ToList(),
                                                                                                                              "ShippingOptionID",
                                                                                                                              "ShippingOptionDisplayName");

            // If the ModelState is not valid, assembles the country list and the shipping option list and displays the step again
            if (!ModelState.IsValid)
            {
                model.BillingAddress.Countries = new SelectList(CountryInfoProvider.GetCountries(), "CountryID", "CountryDisplayName");
                model.ShippingOption.ShippingOptions = new ShippingOptionViewModel(ShippingOptionInfoProvider.GetShippingOptionInfo(shoppingService.GetShippingOption()), shippingOptions).ShippingOptions;
                return View(model);
            }

            // Gets the shopping cart's customer and applies the customer details from the checkout process step
            var customer = shoppingService.GetCurrentCustomer();

            if (customer == null)
            {
                UserInfo userInfo = cart.User;
                if (userInfo != null)
                {
                    customer = CustomerHelper.MapToCustomer(cart.User);
                }
                else
                {
                    customer = new CustomerInfo();
                }
            }
            model.Customer.ApplyToCustomer(customer);

            // Sets the updated customer object to the current shopping cart
            shoppingService.SetCustomer(customer);

            // Gets the shopping cart's billing address and applies the billing address from the checkout process step
            var address = AddressInfoProvider.GetAddressInfo(model.BillingAddress.AddressID) ?? new AddressInfo();
            model.BillingAddress.ApplyTo(address);

            // Sets the address personal name
            address.AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}";

            // Saves the billing address
            shoppingService.SetBillingAddress(address);

            // Sets the selected shipping option and evaluates the cart
            shoppingService.SetShippingOption(model.ShippingOption.ShippingOptionID);

            // Redirects to the next step of the checkout process
            return RedirectToAction("PreviewAndPay");
        }
        //EndDocSection:PostDelivery


        //DocSection:LoadingStates
        /// <summary>
        /// Loads states of the specified country.
        /// </summary>
        /// <param name="countryId">ID of the selected country.</param>
        /// <returns>Serialized display names of the loaded states.</returns>
        [HttpPost]
        public JsonResult CountryStates(int countryId)
        {
            // Gets the display names of the country's states
            var responseModel = StateInfoProvider.GetStates().WhereEquals("CountryID", countryId)
                .Select(s => new
                {
                    id = s.StateID,
                    name = HTMLHelper.HTMLEncode(s.StateDisplayName)
                });

            // Returns serialized display names of the states
            return Json(responseModel);
        }
        //EndDocSection:LoadingStates


        //DocSection:LoadingAddress
        /// <summary>
        /// Loads information of an address specified by its ID.
        /// </summary>
        /// <param name="addressID">ID of the address.</param>
        /// <returns>Serialized information of the loaded address.</returns>
        [HttpPost]
        public JsonResult CustomerAddress(int addressID)
        {
            // Gets the address with its ID
            AddressInfo address = AddressInfoProvider.GetAddressInfo(addressID);

            // Checks whether the address was retrieved
            if (address == null)
            {
                return null;
            }

            // Creates a response with all address information
            var responseModel = new
            {
                Line1 = address.AddressLine1,
                Line2 = address.AddressLine2,
                City = address.AddressCity,
                PostalCode = address.AddressZip,
                CountryID = address.AddressCountryID,
                StateID = address.AddressStateID,
                PersonalName = address.AddressPersonalName
            };

            // Returns serialized information of the address
            return Json(responseModel);
        }
        //EndDocSection:LoadingAddress


        //DocSection:PreparePayment
        /// <summary>
        /// Gets all applicable payment methods assigned to the current site.
        /// </summary>
        /// <param name="cart">Shopping cart of the site</param>
        /// <returns>Collection of applicable payment methods</returns>
        private IEnumerable<PaymentOptionInfo> GetApplicablePaymentMethods(ShoppingCartInfo cart)
        {
            // Gets all enabled payment methods from Kentico
            IEnumerable<PaymentOptionInfo> enabledPaymentMethods = PaymentOptionInfoProvider.GetPaymentOptions(SiteContext.CurrentSiteID, true).ToList();

            // Returns all applicable payment methods
            return enabledPaymentMethods.Where(paymentMethod => PaymentOptionInfoProvider.IsPaymentOptionApplicable(cart, paymentMethod));
        }
        //EndDocSection:PreparePayment


        //DocSection:PreparePreview
        /// <summary>
        /// Display the preview checkout process step.
        /// </summary>
        public ActionResult PreviewAndPay()
        {
            // If the current shopping cart is empty, returns to the shopping cart action
            if (shoppingService.GetCurrentShoppingCart().IsEmpty)
            {
                return RedirectToAction("ShoppingCart");
            }

            // Prepares a model from the preview step
            PreviewAndPayViewModel model = PreparePreviewViewModel();

            // Displays the preview step
            return View(model);
        }


        /// <summary>
        /// Prepares a view model of the preview checkout process step including the shopping cart,
        /// the customer details, and the payment method.
        /// </summary>
        /// <returns>View model with information about the future order.</returns>
        private PreviewAndPayViewModel PreparePreviewViewModel()
        {
            // Gets the current user's shopping cart
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Prepares the customer details
            DeliveryDetailsViewModel deliveryDetailsModel = new DeliveryDetailsViewModel
            {
                Customer = new CustomerViewModel(shoppingService.GetCurrentCustomer()),
                BillingAddress = new BillingAddressViewModel(shoppingService.GetBillingAddress(), null, null),
                ShippingOption = new ShippingOptionViewModel()
                {
                    ShippingOptionID = cart.ShippingOption.ShippingOptionID,
                    ShippingOptionDisplayName = ShippingOptionInfoProvider.GetShippingOptionInfo(cart.ShippingOption.ShippingOptionID).ShippingOptionDisplayName
                }
            };

            // Prepares the payment method
            PaymentMethodViewModel paymentViewModel = new PaymentMethodViewModel
            {
                PaymentMethods = new SelectList(GetApplicablePaymentMethods(cart), "PaymentOptionID", "PaymentOptionDisplayName")
            };

            // Gets the selected payment method
            PaymentOptionInfo paymentMethod = cart.PaymentOption;
            if (paymentMethod != null)
            {
                paymentViewModel.PaymentMethodID = paymentMethod.PaymentOptionID;
            }

            // Prepares a model from the preview step
            PreviewAndPayViewModel model = new PreviewAndPayViewModel
            {
                DeliveryDetails = deliveryDetailsModel,
                Cart = new ShoppingCartViewModel(cart),
                PaymentMethod = paymentViewModel
            };

            return model;
        }


        /// <summary>
        /// Decides whether the specified payment method is valid on the current site.
        /// </summary>
        /// <param name="paymentMethodID">ID of the applied payment method.</param>
        /// <returns>True if the payment method is valid.</returns>
        private bool IsPaymentMethodValid(int paymentMethodID)
        {
            // Gets the current shopping cart
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Gets a list of all applicable payment methods to the current user's shopping cart
            List<PaymentOptionInfo> paymentMethods = GetApplicablePaymentMethods(cart).ToList();

            // Returns whether an applicable payment method exists with the entered payment method's ID
            return paymentMethods.Exists(p => p.PaymentOptionID == paymentMethodID);
        }
        //EndDocSection:PreparePreview


        //DocSection:PostPreview
        /// <summary>
        /// Validates all collected information, creates an order,
        /// and redirects the customer to payment.
        /// </summary>
        /// <param name="model">View model with information about the future order.</param>
        [HttpPost]
        public ActionResult PreviewAndPay(PreviewAndPayViewModel model)
        {
            // Gets the current shopping cart
            ShoppingCartInfo cart = shoppingService.GetCurrentShoppingCart();

            // Validates the shopping cart
            var validationErrors = ShoppingCartInfoProvider.ValidateShoppingCart(cart);

            // Gets the selected payment method, assigns it to the shopping cart, and evaluates the cart
            shoppingService.SetPaymentOption(model.PaymentMethod.PaymentMethodID);

            // If the validation was not successful, displays the preview step again
            if (validationErrors.Any() || !IsPaymentMethodValid(model.PaymentMethod.PaymentMethodID))
            {
                // Prepares a view model from the order review step
                PreviewAndPayViewModel viewModel = PreparePreviewViewModel();

                // Displays the order review step again
                return View("PreviewAndPay", viewModel);
            }

            // Creates an order from the current shopping cart
            // If the order was created successfully, empties the cart
            OrderInfo order = shoppingService.CreateOrder();

            // Redirects to the payment gateway
            return RedirectToAction("Index", "Payment", new { orderID = order.OrderID });
        }
        //EndDocSection:PostPreview


        //DocSection:ThankYou
        /// <summary>
        /// Displays a thank-you page where user is redirected after creating an order.
        /// </summary>
        /// <param name="orderID">ID of the created order.</param>
        public ActionResult ThankYou(int orderID = 0)
        {
            ViewBag.OrderID = orderID;

            return View();
        }
        //EndDocSection:ThankYou
    }
}
