using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api;

public class StorageManagerDbContext(DbContextOptions<StorageManagerDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
    
    public DbSet<User> Users { get; set; }
}