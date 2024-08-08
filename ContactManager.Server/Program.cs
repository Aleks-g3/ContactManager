
using ContactManager.Server.Contexts;
using ContactManager.Server.Entities;
using ContactManager.Server.Extensions;
using ContactManager.Server.Repositories;
using ContactManager.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContactManager.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultChallengeScheme = IdentityConstants.BearerScheme;
                opt.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme)
                .AddBearerToken(IdentityConstants.BearerScheme);

            builder.Services.AddIdentityCore<Contact>(opt => opt.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddApiEndpoints();

            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                app.ApplyMigrations();
            }

            app.UseHttpsRedirection();

            app.MapCustomIdentityApi<Contact>();

            app.UseAuthorization();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
