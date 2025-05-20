using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace backend.Models;

[Microsoft.EntityFrameworkCore.Index(nameof(Date))]
public class EquipmentWriteOff
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Warehouse From { get; set; }
    public Equipment Equipment { get; set; }
    public IdentityUser Initiator { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
}