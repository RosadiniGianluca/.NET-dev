namespace RistoranteAPI.Models;

public class Reservation
{
    public int Id { get; set; }
    public string? CustomerName { get; set; }
    public int NumberOfPeople { get; set; }
    public DateTime ReservationTime { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}