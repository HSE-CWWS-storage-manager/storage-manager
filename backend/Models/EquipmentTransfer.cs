using Microsoft.AspNetCore.Identity;

namespace backend.Models;

public class EquipmentTransfer
{
    public Guid Id { get; set; }
    public Equipment Equipment { get; set; }
    public Student Recipient { get; set; }
    public IdentityUser Initiator { get; set; }
    public DateTime IssueDate { get; set; }
}