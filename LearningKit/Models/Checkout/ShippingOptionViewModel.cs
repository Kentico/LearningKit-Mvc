﻿using System.ComponentModel;
using System.Web.Mvc;

using CMS.Ecommerce;

namespace LearningKit.Models.Checkout
{
    //DocSection:ShippingOptionModel
    public class ShippingOptionViewModel
    {
        public string ShippingOptionDisplayName { get; set; }

        [DisplayName("Shipping option")]
        public int ShippingOptionID { get; set; }

        public SelectList ShippingOptions { get; set; }

        
        /// <summary>
        /// Creates a shipping option model.
        /// </summary>
        /// <param name="shippingOption">Shipping option.</param>
        /// <param name="shippingOptions">List of shipping options.</param>
        public ShippingOptionViewModel(ShippingOptionInfo shippingOption, SelectList shippingOptions)
        {
            ShippingOptions = shippingOptions;
            
            if (shippingOption != null)
            {
                ShippingOptionID = shippingOption.ShippingOptionID;
                ShippingOptionDisplayName = shippingOption.ShippingOptionDisplayName;
            }
        }

        
        /// <summary>
        /// Creates an empty shipping option model.
        /// </summary>
        public ShippingOptionViewModel()
        {
        }
    }
    //EndDocSection:ShippingOptionModel
}