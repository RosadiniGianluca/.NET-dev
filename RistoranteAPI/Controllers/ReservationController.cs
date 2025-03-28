using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RistoranteAPI.Data;
using RistoranteAPI.Models;
using RistoranteAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace RistoranteAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ValidationService _validationService;
    private readonly EmailService _emailService;

    public ReservationController(AppDbContext context, ValidationService validationService, EmailService emailService)
    {
        _context = context;
        _validationService = validationService;
        _emailService = emailService;
    }

    [Authorize] // Authorization via Jwt token required to use this method
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
    {
        return await _context.Reservations.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> GetReservationById(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound($"Reservation with id {id} not found");
        }

        return reservation;
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> CreateReservation(Reservation reservation)
    {
        var errors = _validationService.ValidateReservation(reservation);

        if (errors.Any())
        {
            return BadRequest(new { Errors = errors });
        }

        // Check if slot is available
        var isSlotAvailable = await _validationService.IsSlotAvailable(_context, reservation.ReservationDate);
        if (!isSlotAvailable)
        {
            return Conflict(new { Error = "Maximum reservations reached for this time slot." });
        }

        // Check for duplicate reservation
        var existingReservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Email == reservation.Email && r.ReservationDate == reservation.ReservationDate);

        if (existingReservation != null)
        {
            return Conflict(new { Error = "A reservation already exists with this email and date." });
        }

        _context.Reservations.Add(reservation);

        var emailBody = $"<h1>Reservation Confirmed!</h1><p>Dear {reservation.CustomerName}, your reservation on {reservation.ReservationDate} has been confirmed!</p>";

        await _emailService.SendEmailAsync(reservation.Email, "Reservation Confirmation", emailBody);

        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReservations), new { id = reservation.Id }, new
        {
            message = "Reservation created successfully",
            details = reservation
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Reservation>> DeleteReservationById(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound($"Reservation with id {id} not found");
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"Reservation with id {id} deleted", details = reservation });
    }
}
