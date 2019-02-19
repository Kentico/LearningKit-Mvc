using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningKit.Models.Checkout
{
    public class OrderPaymentResultViewModel
    {
        public string PaymentMethodName { get; set; }

        public bool PaymentIsCompleted { get; set; }

    }
}