using E_CommerceAPP.Data;
using E_CommerceAPP.Models;
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
    public class DeliveryController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController(OrderDbContext context, ILogger<DeliveryController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliveries()
        {
            try
            {
                var deliveries = await _context.deliverylists.ToListAsync();
                return Ok(deliveries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deliveries.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting deliveries.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Delivery>> AddDelivery(Delivery delivery)
        {
            try
            {
                if (delivery == null)
                {
                    return BadRequest("Delivery data is null.");
                }

                _context.deliverylists.Add(delivery);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDeliveryById), new { id = delivery.DeliveryId }, delivery);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving delivery to database.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving delivery to database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding delivery.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while adding delivery.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Delivery>> GetDeliveryById(int id)
        {
            try
            {
                var delivery = await _context.deliverylists.FindAsync(id);

                if (delivery == null)
                {
                    return NotFound();
                }

                return Ok(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting delivery by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting delivery by ID.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Delivery>> DeleteDelivery(int id)
        {
            try
            {
                var delivery = await _context.deliverylists.FindAsync(id);
                if (delivery == null)
                {
                    return NotFound();
                }

                _context.deliverylists.Remove(delivery);
                await _context.SaveChangesAsync();

                return Ok(delivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting delivery.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting delivery.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Delivery>> UpdateDelivery(int id, Delivery delivery)
        {
            try
            {
                if (id != delivery.DeliveryId)
                {
                    return BadRequest("Delivery ID mismatch.");
                }

                var existingDelivery = await _context.deliverylists.FindAsync(id);
                if (existingDelivery == null)
                {
                    return NotFound();
                }

                existingDelivery.Status = delivery.Status;
                existingDelivery.Estimateddeliver = delivery.Estimateddeliver;
                existingDelivery.Deliverydate = delivery.Deliverydate;
       

                _context.deliverylists.Update(existingDelivery);
                await _context.SaveChangesAsync();

                return Ok(existingDelivery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating delivery.");
            }
        }
    }
}
