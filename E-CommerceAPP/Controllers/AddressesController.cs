using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_CommerceAPP.Data;
using E_CommerceAPP.Models;
using System.Text.Json;


namespace E_CommerceAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public AddressesController(EcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of addresses.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/Address
        /// </remarks>
        /// <returns>A list of addresses.</returns>
        /// <response code="200">Returns the list of addresses.</response>
        /// <response code="500">If there was an error while retrieving addresses.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddress()
        {
            return await _context.Address.ToListAsync();
        }

        // GET: api/Addresses/ByCustomerId/{customerId}

        /// <summary>
        /// Retrieves addresses associated with a specific customer ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// </remarks>
        /// <param name="customerId">The ID of the customer whose addresses to retrieve.</param>
        /// <returns>A list of addresses associated with the specified customer ID.</returns>
        /// 
        /*
        [HttpGet("ByCustomerId/{customerId?}")]
        /// 
        /// <response code="200">Returns the list of addresses.</response>
        /// <response code="404">If no addresses are found for the given customer ID.</response>
        /// <response code="500">If there was an error while retrieving addresses.</response>
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressByCustomerId(int? customerId)
        {
            if (customerId.HasValue)
            {
                var addresses = await _context.Address
                                            .Where(a => a.CustomeID == customerId)
                                            .ToListAsync();

                if (addresses == null || addresses.Count == 0)
                {
                    return NotFound(); // If no addresses found for the customer
                }

                return addresses;
            }
            else
            {
                var allAddresses = await _context.Address.ToListAsync();
                return allAddresses;
            }
        }
        */
        /// <summary>
        /// Retrieves a specific address by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET /api/Address/5
        /// </remarks>
        /// <param name="id">The ID of the address to retrieve.</param>
        /// <returns>The address with the specified ID.</returns>
        /// <response code="200">Returns the address.</response>
        /// <response code="404">If no address is found for the given ID.</response>
        /// <response code="500">If there was an error while retrieving the address.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await _context.Address.FindAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        /// <summary>
        /// Updates an existing address.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT /api/Address/5
        ///     {
        ///         "addressId": 5,
        ///         "addressName": "Updated Address",
        ///         "street": "Updated Street",
        ///         "state": "Updated State",
        ///         "pincode": "12345",
        ///         "city": "Updated City",
        ///         "country": "Updated Country"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the address to update.</param>
        /// <param name="address">The updated address object.</param>
        /// <returns>The updated address.</returns>
        /// <response code="200">Returns the updated address.</response>
        /// <response code="400">If the request body is null or the address ID does not match the route ID.</response>
        /// <response code="404">If no address is found for the given ID.</response>
        /// <response code="500">If there was an error while updating the address.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutAddress(int id, Address address)
        {
            if (id != address.AddressID)
            {
                return BadRequest();
            }

            _context.Entry(address).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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
        /// Adds a new address.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/Address
        ///     {
        ///         "addressName": "New Address",
        ///         "street": "New Street",
        ///         "state": "New State",
        ///         "pincode": "12345",
        ///         "city": "New City",
        ///         "country": "New Country"
        ///     }
        /// </remarks>
        /// <param name="address">The address object to add.</param>
        /// <returns>The newly created address.</returns>
        /// <response code="201">Returns the newly created address.</response>
        /// <response code="400">If the request body is null or invalid.</response>
        /// <response code="500">If there was an error while adding the address.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Address), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Address>> PostAddress(Address address)
        {
            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAddress", new { id = address.AddressID }, address);
        }

        /// <summary>
        /// Deletes an address by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     DELETE /api/Address/5
        /// </remarks>
        /// <param name="id">The ID of the address to delete.</param>
        /// <returns>The deleted address.</returns>
        /// <response code="200">Returns the deleted address.</response>
        /// <response code="404">If no address is found for the given ID.</response>
        /// <response code="500">If there was an error while deleting the address.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            _context.Address.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(int id)
        {
            return _context.Address.Any(e => e.AddressID == id);
        }
    }
}
