using System.ComponentModel.DataAnnotations;
using RistoranteAPI.Models;
using RistoranteAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RistoranteAPI.Settings;

namespace RistoranteAPI.Services;

// Validare i dati di input in fase di creazione o modifica di una prenotazione per evitare:
// Nomi vuoti
// Email non valide
// Numero di persone minore di 1
// Date di prenotazione nel passato
public class ValidationService
{
    // Verifica se il numero massimo di prenotazioni in una certa fascia oraria è stato raggiunto.
    private readonly int _maxReservations;

    public ValidationService(IOptions<ReservationSettings> options)
    {
        _maxReservations = options.Value.MaxReservationsPerSlot;
    }

    public List<string> ValidateReservation(Reservation reservation)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(reservation);

        Validator.TryValidateObject(reservation, validationContext, validationResults, true);

        return validationResults.Select(v => v.ErrorMessage!).ToList();
    }

    public async Task<bool> IsSlotAvailable(AppDbContext context, DateTime date)
    {
        var reservations = await context.Reservations
            .Where(r => r.ReservationDate.Date == date.Date)
            .CountAsync();

        return reservations < _maxReservations;
    }
}