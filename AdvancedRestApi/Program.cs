using AdvancedRestApi.Data;
using AdvancedRestApi.Interfaces;
using AdvancedRestApi.Profiles;
using AdvancedRestApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserDbContext>(option => option.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UsersDb;"));
//Creating Dependency
builder.Services.AddScoped<IUser, UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();