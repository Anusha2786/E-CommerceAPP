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
    public class PaymentController : ControllerBase
    {
        private readonly EcommerceDbContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(EcommerceDbContext context, ILogger<PaymentController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Retrieves a list of payments.
        /// </summary>
        /// <returns>A list of payments.</returns>
        /// <response code="200">Returns a list of payments.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            try
            {
                var payments = await _context.paymentlists.ToListAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payments.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting payments.");
            }
        }
        /// <summary>
        /// Creates a new payment.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Payments
        ///     {
        ///         "amount": 100.00,
        ///         "paymentDate": "2024-07-01T10:00:00",
        ///         "description": "Payment for services",
        ///         "customerId": 123
        ///     }
        ///     
        /// </remarks>
        /// <param name="payment">The payment details to create.</param>
        /// <returns>A newly created payment.</returns>
        /// <response code="201">Returns the newly created payment.</response>
        /// <response code="400">If the request body is invalid or missing required fields.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> AddPayment(Payment payment)
        {
            try
            {
                if (payment == null)
                {
                    return BadRequest("Payment data is null.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.paymentlists.Add(payment);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Payment successfully added. Payment ID: {PaymentId}", payment.paymentId);

                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.paymentId }, payment);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving payment to database. Payment ID: {PaymentId}", payment?.paymentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving payment to database.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding payment. Payment ID: {PaymentId}", payment?.paymentId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while adding payment.");
            }
        }



        /// <summary>
        /// Retrieves a payment by its ID.
        /// </summary>
        /// <param name="id">The ID of the payment to retrieve.</param>
        /// <returns>The payment with the specified ID.</returns>
        /// <response code="200">Returns the payment with the specified ID.</response>
        /// <response code="404">If no payment with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            try
            {
                var payment = await _context.paymentlists.FindAsync(id);

                if (payment == null)
                {
                    return NotFound();
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting payment by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting payment by ID.");
            }
        }

        /// <summary>
        /// Deletes a payment by its ID.
        /// </summary>
        /// <param name="id">The ID of the payment to delete.</param>
        /// <returns>No content if the payment was successfully deleted.</returns>
        /// <response code="200">Returns no content if the payment was successfully deleted.</response>
        /// <response code="404">If no payment with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> DeletePayment(int id)
        {
            try
            {
                var payment = await _context.paymentlists.FindAsync(id);
                if (payment == null)
                {
                    return NotFound();
                }

                _context.paymentlists.Remove(payment);
                await _context.SaveChangesAsync();

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting payment.");
            }
        }

        /// <summary>
        /// Updates a payment by its ID.
        /// </summary>
        /// <param name="id">The ID of the payment to update.</param>
        /// <param name="updatedPayment">The updated payment object.</param>
        /// <returns>No content if the payment was successfully updated.</returns>
        /// <response code="200">Returns no content if the payment was successfully updated.</response>
        /// <response code="400">If the request body or parameters are invalid.</response>
        /// <response code="404">If no payment with the given ID exists.</response>
        /// <response code="500">If there was an error while processing the request.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Payment>> UpdatePayment(int id, Payment payment)
        {
            try
            {
                if (id != payment.paymentId)
                {
                    return BadRequest("Payment ID mismatch.");
                }

                var existingPayment = await _context.paymentlists.FindAsync(id);
                if (existingPayment == null)
                {
                    return NotFound();
                }

                existingPayment.paymentdate = payment.paymentdate;
                existingPayment.paymenttype = payment.paymenttype;
                existingPayment.amount = payment.amount;
                existingPayment.paymentstatus = payment.paymentstatus;

                _context.paymentlists.Update(existingPayment);
                await _context.SaveChangesAsync();

                return Ok(existingPayment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating payment.");
            }
        }
    }
}
