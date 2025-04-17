using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend;

public class StorageManagerDbContext(DbContextOptions<StorageManagerDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<EquipmentTransfer> EquipmentTransfers { get; set; }
    public DbSet<EquipmentWriteOff> EquipmentWriteOffs { get; set; }
    public DbSet<Student> Students { get; set; }
    
}