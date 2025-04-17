using Microsoft.AspNetCore.Identity;

namespace backend.Models;

public class EquipmentWriteOff
{
    public Guid Id { get; set; }
    public Equipment Equipment { get; set; }
    public IdentityUser Initiator { get; set; }
    public int Quantity { get; set; }
    public DateTime Date { get; set; }
}