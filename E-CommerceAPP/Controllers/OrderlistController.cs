using E_CommerceAPP.Data;
using E_CommerceAPP.Models;
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
        private readonly OrderDbContext _context;
        private readonly ILogger<OrderlistController> _logger;

        public OrderlistController(OrderDbContext context, ILogger<OrderlistController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
