using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQWeb.CreateExcell.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQWeb.CreateExcell
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host =  CreateHostBuilder(args).Build();

   
            using (var scope = host.Services.CreateScope())
            {

                var appDbContext = scope.ServiceProvider.GetRequiredService<MyApplicationDbContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                appDbContext.Database.Migrate();


                if (!appDbContext.Users.Any())
                {
                    userManager.CreateAsync(new IdentityUser() { UserName = "deneme", Email = "deneme@outlook.com" }, "Password12*").Wait();


                    userManager.CreateAsync(new IdentityUser() { UserName = "deneme2", Email = "deneme2@outlook.com" }, "Password12*").Wait();
                }


            }



            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
