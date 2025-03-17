using FluentValidation;
using IdentitiyExample.Context;
using IdentitiyExample.DTOs;
using IdentitiyExample.Mapping;
using IdentitiyExample.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//InMemory DBContext 
builder.Services.AddDbContext<ApplicationDbContext>(p =>
{
    p.UseInMemoryDatabase("AppDb");
});
//Add Identity
builder.Services.AddIdentity<AppUser,AppRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
