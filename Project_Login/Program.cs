using FluentValidation.AspNetCore;

using Microsoft.EntityFrameworkCore;

using Project_Login.Configuration;
using Project_Login.Filters;
using Project_Login.Persistence;
using Project_Login.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MeuDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.WebApiConfig();

builder.Services.ResolveDependencies();

builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilters)))
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterUserViewModelValidator>());



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMvcConfiguration();

app.MapControllers();

app.Run();
