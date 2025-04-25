namespace backend.Models;

public class EquipmentRemain
{
    public Guid Id { get; set; }
    public Warehouse Warehouse { get; set; }
    public Equipment Equipment { get; set; }
    public int OnStock { get; set; }
    public int OnLoan { get; set; }
}