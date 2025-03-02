using System.ComponentModel.DataAnnotations;
using RistoranteAPI.Models;

namespace RistoranteAPI.Services;

public class ValidationService
{
    public List<string> ValidateReservation(Reservation reservation)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(reservation);

        Validator.TryValidateObject(reservation, validationContext, validationResults, true);

        return validationResults.Select(v => v.ErrorMessage!).ToList();
    }
}