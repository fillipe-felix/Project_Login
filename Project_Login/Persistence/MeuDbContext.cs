using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Project_Login.Models;

namespace Project_Login.Persistence;

public class MeuDbContext : DbContext
{
    
    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    {
        
    }

    public DbSet<Produto> Produtos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
