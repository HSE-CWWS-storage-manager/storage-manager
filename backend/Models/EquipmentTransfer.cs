using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace backend.Models;

[Microsoft.EntityFrameworkCore.Index(nameof(IssueDate))]
public class EquipmentTransfer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Warehouse From { get; set; }
    public Equipment Equipment { get; set; }
    public Student Recipient { get; set; }
    public IdentityUser Initiator { get; set; }
    public DateTime IssueDate { get; set; }
}