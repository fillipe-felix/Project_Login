using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Project_Login.Models;

namespace Project_Login.Persistence.Configurations;

public class sConfiguration : IEntityTypeConfiguration<Produto>
{

    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.HasKey(s => s.Id);
    }
}
