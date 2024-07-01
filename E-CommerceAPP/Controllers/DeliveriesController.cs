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
    public class DeliveriesController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public DeliveriesController(EcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all deliveries.
        /// </summary>
        /// <returns>A list of deliveries.</returns>
        /// <response code="200">Returns a list of deliveries.</response>
        /// <response code="404">If no deliveries are found.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDelivery()
        {
            return await _context.Delivery.ToListAsync();
        }

        /// <summary>
        /// Retrieves a delivery by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery to retrieve.</param>
        /// <returns>The delivery with the specified ID.</returns>
        /// <response code="200">Returns the delivery with the specified ID.</response>
        /// <response code="404">If no delivery with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Delivery>> GetDelivery(int id)
        {
            var delivery = await _context.Delivery.FindAsync(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return delivery;
        }

        /// <summary>
        /// Updates a delivery by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT /api/Deliveries/5
        ///     {
        ///         "deliveryId": 5,
        ///         "deliveryStatus": "Delivered",
        ///         "deliveryDate": "2024-07-01T10:00:00",
        ///         "recipientName": "John Doe",
        ///         "recipientAddress": "123 Main St",
        ///         "recipientPhone": "555-1234"
        ///     }
        ///     
        /// </remarks>
        /// <param name="id">The ID of the delivery to update.</param>
        /// <param name="delivery">The updated delivery details.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="200">If the delivery is successfully updated.</response>
        /// <response code="400">If the request body is invalid or missing required fields.</response>
        /// <response code="404">If no delivery with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the update request.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutDelivery(int id, Delivery delivery)
        {
            if (id != delivery.DeliveryID)
            {
                return BadRequest();
            }

            _context.Entry(delivery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id))
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
        /// Creates a new delivery.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Deliveries
        ///     {
        ///         "deliveryStatus": "Pending",
        ///         "deliveryDate": "2024-07-01T10:00:00",
        ///         "recipientName": "John Doe",
        ///         "recipientAddress": "123 Main St",
        ///         "recipientPhone": "555-1234"
        ///     }
        ///     
        /// </remarks>
        /// <param name="delivery">The delivery details to create.</param>
        /// <returns>A newly created delivery.</returns>
        /// <response code="201">Returns the newly created delivery.</response>
        /// <response code="400">If the request body is invalid or missing required fields.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Delivery>> PostDelivery(Delivery delivery)
        {
            _context.Delivery.Add(delivery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDelivery", new { id = delivery.DeliveryID }, delivery);
        }

        /// <summary>
        /// Deletes a delivery by its ID.
        /// </summary>
        /// <param name="id">The ID of the delivery to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="200">If the delivery is successfully deleted.</response>
        /// <response code="404">If no delivery with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the delete request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            var delivery = await _context.Delivery.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            _context.Delivery.Remove(delivery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryExists(int id)
        {
            return _context.Delivery.Any(e => e.DeliveryID == id);
        }
    }
}
 