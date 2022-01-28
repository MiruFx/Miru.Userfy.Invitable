using Microsoft.EntityFrameworkCore.Diagnostics;
using Miru.Userfy;

namespace AltBank.Database;

public class AppDbContext : UserfyDbContext<User>
{
    public AppDbContext(
        DbContextOptions options,
        IEnumerable<IInterceptor> interceptors) : base(options, interceptors)
    {
    }
        
    // Your entities
    // public DbSet<>  { get; set; } 
    
}
