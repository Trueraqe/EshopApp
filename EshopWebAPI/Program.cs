using EshopCore.Data;
using EshopCore.Database;
using EshopCore.Interfaces;
using EshopCore.InterfacesWeb;
using EshopCore.Models;
using EshopCore.Services;
using EshopCore.ServicesWeb;
using EshopCore.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
namespace EshopWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            {
                var builder = WebApplication.CreateBuilder(args);

                // JWT
                var key = Encoding.UTF8.GetBytes("super_secret_jwt_key_for_Eshop_Console_and_Web_App");

                builder.Services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key)
                        };
                    });

                builder.Services.AddAuthorization();
                //builder.Services.AddAuthorization(options =>
                //{
                //    options.AddPolicy("RequireManagementRole", policy =>
                //        policy.RequireRole("Admin", "Manager", "Director"));
                //});

                //[Authorize(Policy = "RequireManagementRole")]
                //[HttpGet]
                //public IActionResult GetManagementData()
                //{
                //    return Ok("Dostęp przyznany na podstawie polityki zarządzania.");
                //}

                builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Wprowadź token JWT (\"Podaj token\")"
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                });

                // Services

                builder.Services.AddHttpContextAccessor();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll",
                        policy => policy.AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod());
                });


                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddScoped<IUserServiceWeb, UserServiceWeb>();
                builder.Services.AddScoped<IProductServiceWeb, ProductServiceWeb>();
                builder.Services.AddScoped<ICartServiceWeb, CartServiceWeb>();
                builder.Services.AddScoped<IAuthServiceWeb, AuthServiceWeb>();
                builder.Services.AddScoped<IOrderServiceWeb, OrderServiceWeb>();
                builder.Services.AddScoped<IValidator<User>, RegisterUserValidator>();
                builder.Services.AddScoped<ShopContext>();
                builder.Services.AddDbContext<ShopContext>(options => options.UseSqlite($"Data Source={EshopCore.Utils.Validators.SQL_DB_Path("shop.db")}"));

                // Ignorowanie cykli " Twój model Order ma listę Items. Każdy OrderItem ma właściwość Order. Podczas próby zamiany na JSON, serwer wpada w pętlę: Order -> Items -> Order -> Items... "
                builder.Services.AddControllers().AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

                var app = builder.Build();

                // Pipeline
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors("AllowAll");
                app.UseRouting();
                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();

                app.Run();
            }
        }
    }
}
