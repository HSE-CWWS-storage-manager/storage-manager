using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Microsoft.EntityFrameworkCore.Index(nameof(Name))]
public class Equipment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string? Model { get; set; }
    public string Name { get; set; }
    public string? SerialNumber { get; set; }
    public string? InventoryNumber { get; set; }
    public string? BalanceCost { get; set; }
    public DateTime? AcceptDate { get; set; }
}