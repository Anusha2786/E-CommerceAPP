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
    [Authorize] // Ensure endpoint requires authorization
    public class AddressController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<AddressController> _logger;

        public AddressController(OrderDbContext context, ILogger<AddressController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Addrees>>> GetAddresses()
        {
            try
            {
                var addresses = await _context.addrees.ToListAsync();
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting addresses.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting addresses.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Addrees>> GetAddressById(int id)
        {
            try
            {
                var address = await _context.addrees.FindAsync(id);

                if (address == null)
                {
                    return NotFound();
                }

                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting address by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting address by ID.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Addrees>> AddAddress(Addrees address)
        {
            try
            {
                if (address == null)
                {
                    return BadRequest("Address data is null.");
                }

                _context.addrees.Add(address);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAddressById), new { id = address.AddressId}, address);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving address to database.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving address to database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding address.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while adding address.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Addrees>> DeleteAddress(int id)
        {
            try
            {
                var address = await _context.addrees.FindAsync(id);
                if (address == null)
                {
                    return NotFound();
                }

                _context.addrees.Remove(address);
                await _context.SaveChangesAsync();

                return Ok(address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting address.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting address.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Addrees>> UpdateAddress(int id, Addrees address)
        {
            try
            {
                if (id != address.AddressId)
                {
                    return BadRequest("Address ID mismatch.");
                }

                var existingAddress = await _context.addrees.FindAsync(id);
                if (existingAddress == null)
                {
                    return NotFound();
                }

                existingAddress.AddressName=address.AddressName;
                existingAddress.Street = address.Street;
                existingAddress.State = address.State;
                existingAddress.Pincode = address.Pincode;
                existingAddress.City = address.City;
                existingAddress.Country = address.Country;

                _context.addrees.Update(existingAddress);
                await _context.SaveChangesAsync();

                return Ok(existingAddress);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating address.");
            }
        }
    }
}

