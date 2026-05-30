using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using nomination_api.DataBaseContext;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5001", "http://localhost:3000", "http://localhost:3001")
              .AllowAnyMethod()
              .WithHeaders("Content-Type", "Authorization")
              .AllowCredentials()
              .WithExposedHeaders("Content-Length", "X-JSON-Response");
    });
});

builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = false; 
    options.Cookie.SameSite = SameSiteMode.Lax; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
}); 

builder.Services.AddDataProtection();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options =>
        options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// builder.Services.AddScoped<Mail_Manager>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
    {
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = false;
    options.Cookie.IsEssential = false;
    }
);
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rewards API V1");
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();