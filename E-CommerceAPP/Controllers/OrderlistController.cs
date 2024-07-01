using E_CommerceAPP.Data;
using E_CommerceAPP.Models;
using E_CommerceAPP.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_CommerceAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class OrderlistController : ControllerBase
    {
        private readonly EcommerceDbContext _context;
        private readonly ILogger<OrderlistController> _logger;

        public OrderlistController(EcommerceDbContext context, ILogger<OrderlistController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Retrieves a list of orders.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/Orderlist
        /// </remarks>
        /// <returns>A list of orders.</returns>
        /// <response code="200">Returns the list of orders.</response>
        /// <response code="500">If there was an error while retrieving orders.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Orderlist>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Orderlist>>> GetOrder()
        {
            try
            {
                var orders = await _context.orderlists.ToListAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting orders.");
            }
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/Orderlist/5
        /// </remarks>
        /// <param name="id">The ID of the order to retrieve.</param>
        /// <returns>The order matching the given ID.</returns>
        /// <response code="200">Returns the order with the specified ID.</response>
        /// <response code="404">If no order with the given ID exists.</response>
        /// <response code="500">If there was an error while retrieving the order.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Orderlist), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Orderlist>> GetOrderById(int id)
        {
            try
            {
                var order = await _context.orderlists.FindAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting order by ID.");
            }
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Orderlist
        ///     {
        ///         "orderStatus": "Pending",
        ///         "orderDate": "2024-07-01T10:00:00",
        ///         "quantity": 1,
        ///         "paymentId": 1,
        ///         "shipmentId": 1,
        ///         "addressId": 1
        ///     }
        ///     
        /// </remarks>
        /// <param name="order">The order details to be created.</param>
        /// <returns>The newly created order.</returns>
        /// <response code="201">Returns the newly created order.</response>
        /// <response code="400">If the request body is invalid or missing required fields.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Orderlist), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Orderlist>> AddOrder(Orderlist order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest("Order data is null.");
                }

                _context.orderlists.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderlistId }, order);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving order to database.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving order to database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while adding order.");
            }
        }

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/Orderlist/5
        ///     
        /// </remarks>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="200">If the order is successfully deleted.</response>
        /// <response code="404">If no order with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the deletion request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Orderlist>> DeleteOrder(int id)
        {
            try
            {
                var order = await _context.orderlists.FindAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                _context.orderlists.Remove(order);
                await _context.SaveChangesAsync();

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting order.");
            }
        }

        /// <summary>
        /// Updates an order by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/Orderlist/5
        ///     {
        ///         "orderlistId": 5,
        ///         "orderStatus": "Shipped",
        ///         "orderDate": "2024-07-01T10:00:00",
        ///         "quantity": 2,
        ///         "paymentId": 1,
        ///         "shipmentId": 1,
        ///         "addressId": 1
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="order">The updated order details.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="200">If the order is successfully updated.</response>
        /// <response code="400">If the request body is invalid or missing required fields.</response>
        /// <response code="404">If no order with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the update request.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Orderlist>> UpdateOrder(int id, Orderlist order)
        {
            try
            {
                if (id != order.OrderlistId)
                {
                    return BadRequest("Order ID mismatch.");
                }

                var existingOrder = await _context.orderlists.FindAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }

                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.Quantity = order.Quantity;
                existingOrder.PaymentId = order.PaymentId;
                existingOrder.ShipmentId = order.ShipmentId;
                existingOrder.AddressId = order.AddressId;

                _context.orderlists.Update(existingOrder);
                await _context.SaveChangesAsync();

                return Ok(existingOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating order.");
            }
        }
    }
}
