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
        private readonly OrderDbContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(OrderDbContext context, ILogger<PaymentController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

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
