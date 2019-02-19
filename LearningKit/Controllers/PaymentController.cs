using System;
using System.Web.Mvc;

using CMS.Ecommerce;
using CMS.SiteProvider;

using LearningKit.Models.Checkout;

namespace LearningKit.Controllers
{
    /// <remarks>
    /// This class tries only to demonstrate behavior of a payment controller.
    /// However, the class is not suitable for any real usage and cannot work on its own.
    /// </remarks>
    public class PaymentController : Controller
    {
        /// <summary>
        /// Fictitious method for creating a payment response information and paying the order.
        /// </summary>
        /// <param name="orderID">ID of the paid order.</param>
        public ActionResult Index(int orderID)
        {
            // Gets the order
            OrderInfo order = OrderInfoProvider.GetOrderInfo(orderID);

            // Validates the retrieved order
            if (order?.OrderSiteID != SiteContext.CurrentSiteID)
            {
                // Redirects back to the order review step if validation fails
                return RedirectToAction("PreviewAndPay");
            }

            // Creates a fictitious response
            ResponseViewModel response = new ResponseViewModel()
            {
                InvoiceNo = order.OrderID,
                Message = "Successfully paid",
                Completed = true,
                TransactionID = new Random().Next(100000, 200000).ToString(),
                Amount = order.OrderTotalPrice,
                ResponseCode = "",
                Approved = true
            };

            // Validates the response and pays the order
            Validate(response);

            // Redirects to the thank-you page
            return RedirectToAction("ThankYou", "Checkout", new { OrderID = orderID});
        }


        /// <summary>
        /// Pays the specified order with a fictitious response.
        /// </summary>
        /// <param name="response">Fictitious response about payment.</param>
        private void Validate(ResponseViewModel response)
        {
            //DocSection:PaymentValidation
            if (response != null)
            {
                // Gets the order based on the invoice number from the response
                OrderInfo order = OrderInfoProvider.GetOrderInfo(response.InvoiceNo);
                if (order?.OrderSiteID != SiteContext.CurrentSiteID)
                {
                    order = null;
                }

                // Checks whether the paid amount of money matches the order price
                // and whether the payment was approved
                if (order != null && response.Amount == order.OrderTotalPrice && response.Approved)
                {
                    // Creates a payment result object that will be viewable in Kentico
                    PaymentResultInfo result = new PaymentResultInfo
                    {
                        PaymentDate = DateTime.Now,
                        PaymentDescription = response.Message,
                        PaymentIsCompleted = response.Completed,
                        PaymentTransactionID = response.TransactionID,
                        PaymentStatusValue = response.ResponseCode,
                        PaymentMethodName = "PaymentName"
                    };
                    
                    // Saves the payment result to the database
                    order.UpdateOrderStatus(result);
                }
            }
            //EndDocSection:PaymentValidation
        }
    }
}