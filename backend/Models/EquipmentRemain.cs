using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class EquipmentRemain
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Warehouse Warehouse { get; set; }
    public Equipment Equipment { get; set; }
    public int OnStock { get; set; }
    public int OnLoan { get; set; }
}