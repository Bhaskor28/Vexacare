using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.Interfaces;
using Vexacare.Domain.Enums;

namespace Vexacare.Web.Controllers
{
    public class OrderManagementController : Controller
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderManagementController> _logger;

        #endregion

        #region Ctor
        public OrderManagementController(
            IOrderService orderService,
            ILogger<OrderManagementController> logger
            )
        {
            _orderService = orderService;
            _logger = logger;
        }

        #endregion



        public async Task<IActionResult> DisplayOrderList()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        //public async Task<IActionResult> UpdateStatus(int orderId, KitState kitState,  StateStatus stateStatus)
        //{
        //    var order = await _orderService.GetOrderByIdAsync(orderId);
        //    if (order != null)
        //    {
        //        try
        //        {

        //            await _orderService.UpdateOrderAsync(order);
        //            TempData["SuccessMessage"] = "Order status updated successfully!";
        //            _logger.LogInformation($"Order {orderId} status updated to {status}.");
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            // Log the error (uncomment ex variable name and write a log.)
        //            TempData["ErrorMessage"] = "Error updating order status. Please try again.";
        //            _logger.LogError(ex, $"Error updating order {orderId} status to {status}.");
        //        }

        //    }
        //    return RedirectToAction(nameof(Details), new { id = orderId });
        //}

        #region Delete
        
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                _logger.LogInformation(@"Successfully Delete the order no: {id}");
                return RedirectToAction("DisplayOrderList", "OrderManagement");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return RedirectToAction("DisplayOrderList", "OrderManagement");
            }
        }

        #endregion
    }
}

