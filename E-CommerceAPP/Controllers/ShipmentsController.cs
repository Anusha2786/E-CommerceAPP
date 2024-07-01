using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_CommerceAPP.Data;
using E_CommerceAPP.Models;

namespace E_CommerceAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public ShipmentsController(EcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all shipments.
        /// </summary>
        /// <returns>A list of all shipments.</returns>
        /// <response code="200">Returns the list of shipments.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Shipment>>> GetShipment()
        {
            return await _context.Shipment.ToListAsync();
        }
        /// <summary>
        /// Retrieves a shipment by its ID.
        /// </summary>
        /// <param name="id">The ID of the shipment to retrieve.</param>
        /// <returns>The shipment with the specified ID.</returns>
        /// <response code="200">Returns the shipment with the specified ID.</response>
        /// <response code="404">If no shipment with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> GetShipment(int id)
        {
            var shipment = await _context.Shipment.FindAsync(id);

            if (shipment == null)
            {
                return NotFound();
            }

            return shipment;
        }

        /// <summary>
        /// Updates a shipment by its ID.
        /// </summary>
        /// <param name="id">The ID of the shipment to update.</param>
        /// <param name="updatedShipment">The updated shipment object.</param>
        /// <returns>No content if the shipment was successfully updated.</returns>
        /// <response code="200">Returns no content if the shipment was successfully updated.</response>
        /// <response code="400">If the request body or parameters are invalid.</response>
        /// <response code="404">If no shipment with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutShipment(int id, Shipment shipment)
        {
            if (id != shipment.ShipmentID)
            {
                return BadRequest();
            }

            _context.Entry(shipment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new shipment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Shipments
        ///     {
        ///         "shipmentName": "Sample Shipment",
        ///         "recipientName": "John Doe",
        ///         "address": "123 Main St, Anytown, USA",
        ///         "weight": 10.5
        ///     }
        /// </remarks>
        /// <param name="newShipment">The shipment object to create.</param>
        /// <returns>A newly created shipment.</returns>
        /// <response code="201">Returns the newly created shipment.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Shipment>> PostShipment(Shipment shipment)
        {
            _context.Shipment.Add(shipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShipment", new { id = shipment.ShipmentID }, shipment);
        }

        /// <summary>
        /// Deletes a shipment by its ID.
        /// </summary>
        /// <param name="id">The ID of the shipment to delete.</param>
        /// <returns>No content if the shipment was successfully deleted.</returns>
        /// <response code="204">No content if the shipment was successfully deleted.</response>
        /// <response code="404">If no shipment with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteShipment(int id)
        {
            var shipment = await _context.Shipment.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _context.Shipment.Remove(shipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipment.Any(e => e.ShipmentID == id);
        }
    }
}
