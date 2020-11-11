using System.Collections.Generic;
using System.Linq;

using CMS.Ecommerce;

using Kentico.Content.Web.Mvc;

namespace LearningKit.Models.Checkout
{
    //DocSection:ShoppingCartViewModel
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCartItemViewModel> CartItems { get; set; }

        public string CurrencyFormatString { get; set; }

        public IEnumerable<string> CouponCodes { get; set; }

        public decimal TotalTax { get; set;}

        public decimal TotalShipping { get; set; }

        public decimal GrandTotal { get; set; }

        public decimal RemainingAmountForFreeShipping { get; set; }

        public bool IsEmpty { get; set; }

        /// <summary>
        /// Constructor for the ShoppingCartViewModel. 
        /// </summary>
        /// <param name="cart">A shopping cart object.</param>
        public ShoppingCartViewModel(ShoppingCartInfo cart)
        {
            // Creates a collection containing all lines from the given shopping cart
            CartItems = cart.CartProducts.Select((cartItemInfo) =>
            {
                return new ShoppingCartItemViewModel()
                {
                    CartItemUnits = cartItemInfo.CartItemUnits,
                    SKUName = cartItemInfo.SKU.SKUName,
                    TotalPrice = cartItemInfo.TotalPrice,
                    CartItemID = cartItemInfo.CartItemID,
                    SKUID = cartItemInfo.SKUID,
                    SKUImageUrl = string.IsNullOrEmpty(cartItemInfo.SKU.SKUImagePath) ? null : new FileUrl(cartItemInfo.SKU.SKUImagePath, true)
                                                                                                .WithSizeConstraint(SizeConstraint.MaxWidthOrHeight(100))
                                                                                                .RelativePath
                };
            });
            CurrencyFormatString = cart.Currency.CurrencyFormatString;
            CouponCodes = cart.CouponCodes.AllAppliedCodes.Select(x => x.Code);
            TotalTax = cart.TotalTax;
            TotalShipping = cart.TotalShipping;
            GrandTotal = cart.GrandTotal;
            RemainingAmountForFreeShipping = cart.CalculateRemainingAmountForFreeShipping();
            IsEmpty = cart.IsEmpty;
        }
       
    }
    //EndDocSection:ShoppingCartViewModel
}