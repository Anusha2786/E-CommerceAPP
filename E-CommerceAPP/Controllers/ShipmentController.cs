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
    public class ShipmentController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<ShipmentController> _logger;

        public ShipmentController(OrderDbContext context, ILogger<ShipmentController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipments()
        {
            try
            {
                var shipments = await _context.shipment.ToListAsync();
                return Ok(shipments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting shipments.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting shipments.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> GetShipmentById(int id)
        {
            try
            {
                var shipment = await _context.shipment.FindAsync(id);
                if (shipment == null)
                {
                    return NotFound();
                }

                return Ok(shipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting shipment by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting shipment by ID.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> AddShipment(Shipment shipment)
        {
            try
            {
                if (shipment == null)
                {
                    return BadRequest("Shipment data is null.");
                }

                _context.shipment.Add(shipment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetShipmentById), new { id = shipment.shipmentId}, shipment);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving shipment to database.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving shipment to database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding shipment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while adding shipment.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> DeleteShipment(int id)
        {
            try
            {
                var shipment = await _context.shipment.FindAsync(id);
                if (shipment == null)
                {
                    return NotFound();
                }

                _context.shipment.Remove(shipment);
                await _context.SaveChangesAsync();

                return Ok(shipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting shipment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting shipment.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> UpdateShipment(int id, Shipment shipment)
        {
            try
            {
                if (id != shipment.shipmentId)
                {
                    return BadRequest("Shipment ID mismatch.");
                }

                var existingShipment = await _context.shipment.FindAsync(id);
                if (existingShipment == null)
                {
                    return NotFound();
                }

                existingShipment.shipmentdate = shipment.shipmentdate;
              
               

                _context.shipment.Update(existingShipment);
                await _context.SaveChangesAsync();

                return Ok(existingShipment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating shipment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating shipment.");
            }
        }
    }
}
