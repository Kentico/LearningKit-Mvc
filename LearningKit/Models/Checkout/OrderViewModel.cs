using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Ecommerce;

namespace LearningKit.Models.Checkout
{
    //DocSection:OrderViewModel
    public class OrderViewModel
    {
        public int OrderID { get; set; }

        public int OrderStatusID { get; set; }

        public string CurrencyFormatString { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal OrderTotalPrice { get; set; }

        public bool OrderIsPaid { get; set; }

        public OrderPaymentResultViewModel OrderPaymentResult { get; set; }

        public string OrderStatusDisplayName { get; set; }

        public OrderViewModel(OrderInfo order)
        {
            OrderID = order.OrderID;
            OrderStatusID = order.OrderStatusID;
            CurrencyFormatString = CurrencyInfoProvider.GetCurrencyInfo(order.OrderCurrencyID).CurrencyFormatString;
            OrderDate = order.OrderDate;
            OrderTotalPrice = order.OrderTotalPrice;
            OrderIsPaid = order.OrderIsPaid;
            OrderStatusDisplayName = OrderStatusInfoProvider.GetOrderStatusInfo(order.OrderStatusID)?.StatusDisplayName;
            if (order.OrderPaymentResult != null)
            {
                OrderPaymentResult = new OrderPaymentResultViewModel()
                {
                    PaymentMethodName = order.OrderPaymentResult.PaymentMethodName,
                    PaymentIsCompleted = order.OrderPaymentResult.PaymentIsCompleted
                };
            }
        }
    }
    //EndDocSection:OrderViewModel
}