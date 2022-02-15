using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Project_Login.Persistence;

public class LoginDbContext : IdentityDbContext
{
    public LoginDbContext(DbContextOptions<LoginDbContext> options) : base(options)
    {
        
    }
}
