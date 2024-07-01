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
    public class CustomersController : ControllerBase
    {
        private readonly EcommerceDbContext _context;

        public CustomersController(EcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of customers.
        /// </summary>
        /// <returns>A list of customers.</returns>
        /// <response code="200">Returns the list of customers.</response>
        /// <response code="500">If there was an error while retrieving the customers.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            return await _context.Customer.ToListAsync();
        }

        /// <summary>
        /// Retrieves a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to retrieve.</param>
        /// <returns>The customer with the specified ID.</returns>
        /// <response code="200">Returns the customer with the specified ID.</response>
        /// <response code="404">If no customer is found for the given ID.</response>
        /// <response code="500">If there was an error while retrieving the customer.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        /// <summary>
        /// Updates a customer by ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     PUT /api/Customers/5
        ///     {
        ///         "customerId": 5,
        ///         "customerName": "Updated Customer Name",
        ///         "email": "updated@example.com",
        ///         "phoneNumber": "1234567890"
        ///     }
        /// </remarks>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer data.</param>
        /// <returns>The updated customer.</returns>
        /// <response code="200">Returns the updated customer.</response>
        /// <response code="400">If the request body is null or invalid.</response>
        /// <response code="404">If no customer is found for the given ID.</response>
        /// <response code="500">If there was an error while updating the customer.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
        /// Creates a new customer.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST /api/Customers
        ///     {
        ///         "customerName": "New Customer",
        ///         "email": "new@example.com",
        ///         "phoneNumber": "9876543210"
        ///     }
        /// </remarks>
        /// <param name="customer">The customer data to create.</param>
        /// <returns>The newly created customer.</returns>
        /// <response code="201">Returns the newly created customer.</response>
        /// <response code="400">If the request body is null or invalid.</response>
        /// <response code="500">If there was an error while creating the customer.</response>
        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerID }, customer);
        }

        //// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>The deleted customer.</returns>
        /// <response code="200">Returns the deleted customer.</response>
        /// <response code="404">If no customer is found for the given ID.</response>
        /// <response code="500">If there was an error while deleting the customer.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerID == id);
        }
    }
}
