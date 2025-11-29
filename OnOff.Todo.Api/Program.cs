using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnOff.Todo.Api.Application.Services;
using OnOff.Todo.Api.Domain.Entities;
using OnOff.Todo.Api.Infrastructure.Data;
using Microsoft.AspNetCore.Cors;


var builder = WebApplication.CreateBuilder(args);

// 1. Swagger: registra generador de documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// 2. Leer configuración JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");

// 3. DbContext con base de datos en memoria
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("OnOffTodoDb");
});

// 4. Identity con ApplicationUser
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 5. Servicios de aplicación
builder.Services.AddScoped<ITodoTaskService, TodoTaskService>();
builder.Services.AddScoped<IUserService, UserService>();

// 6. Autenticación con JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? string.Empty);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 7. Controllers
builder.Services.AddControllers();

var app = builder.Build();

// 8. Activar Swagger (UI + JSON) en ambiente Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();     // <- ESTO GENERA /swagger/v1/swagger.json
    app.UseSwaggerUI();  // <- ESTO GENERA /swagger/index.html
}

app.UseHttpsRedirection();

// Activamos CORS para permitir que Angular llame a la API
app.UseCors("AllowAngular");
// 9. Puedes comentar HTTPS redirection mientras no tengas puerto HTTPS configurado
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

