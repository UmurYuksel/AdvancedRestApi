using AdvancedRestApi.Data;
using AdvancedRestApi.Interfaces;
using AdvancedRestApi.Profiles;
using AdvancedRestApi.Services;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddOData(option=>option.Select().Filter().OrderBy());
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<UserDbContext>(option => option.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UsersDb;"));
//builder.Services

//Creating Dependency
builder.Services.AddScoped<IUser, UserService>();

//Rate Limiting Related Configurations=>
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>((options) =>
{
    options.GeneralRules = new List<RateLimitRule>()
    {
        new RateLimitRule()
        {
            Endpoint= "*",
            Limit=10,
            Period="3m"
        }
    };
});
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

//Rate Limiting Related Configuration.
app.UseIpRateLimiting();
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