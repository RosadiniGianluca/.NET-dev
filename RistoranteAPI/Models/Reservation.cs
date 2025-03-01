namespace RistoranteAPI.Models;
using System.ComponentModel.DataAnnotations;

public class Reservation
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Customer name is required")]
    public string CustomerName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string Phone { get; set; }

    [Range(1, 20, ErrorMessage = "People count must be between 1 and 20")]
    public int NumberOfPeople { get; set; }

    [Required(ErrorMessage = "Reservation date is required")]
    [DataType(DataType.DateTime)]
    public DateTime ReservationDate { get; set; }
}