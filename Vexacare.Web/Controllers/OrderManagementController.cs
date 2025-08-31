using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vexacare.Application.DoctorProfiles;
using Vexacare.Application.Interfaces;
using Vexacare.Application.Orders.OrderViewModel;
using Vexacare.Domain.Entities.Order;
using Vexacare.Domain.Enums;

namespace Vexacare.Web.Controllers
{
    public class OrderManagementController : Controller
    {
        #region Fields
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderManagementController> _logger;
        private readonly IMapper _mapper;

        #endregion

        #region Ctor
        public OrderManagementController(
            IOrderService orderService,
            ILogger<OrderManagementController> logger,
            IMapper mapper
            )
        {
            _orderService = orderService;
            _logger = logger;
            _mapper = mapper;
        }

        #endregion


        #region orderList

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

        #endregion

        #region Edit Actions
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Need to populate dropdowns or other view data
            ViewBag.KitStates = Enum.GetValues(typeof(KitState));
            ViewBag.StateStatuses = Enum.GetValues(typeof(StateStatus));

            var orderVM = _mapper.Map<OrderVM>(order);

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderVM orderVM)
        {
            if (ModelState.IsValid)
            {
                var order = _mapper.Map<Order>(orderVM);
                try
                {
                    await _orderService.UpdateOrderAsync(order);
                    TempData["SuccessMessage"] = "Order updated successfully!";
                    _logger.LogInformation($"Order {orderVM.Id} updated successfully.");
                    return RedirectToAction(nameof(DisplayOrderList));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Error updating order. Please try again.";
                        _logger.LogError(ex, $"Error updating order {orderVM.Id}.");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Error updating order. Please try again.";
                    _logger.LogError(ex, $"Error updating order {orderVM.Id}.");
                }
            }

            // If we got this far, something failed; redisplay form
            ViewBag.KitStates = Enum.GetValues(typeof(KitState));
            ViewBag.StateStatuses = Enum.GetValues(typeof(StateStatus));
            return View(orderVM);
        }

        private async Task<bool> OrderExists(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return order != null;
        }
        #endregion

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

