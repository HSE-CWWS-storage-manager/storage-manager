using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend;

public class StorageManagerDbContext(DbContextOptions<StorageManagerDbContext> options)
    : IdentityDbContext<IdentityUser>(options)
{
}