using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQWeb.CreateExcell.Models
{
    public class MyApplicationDbContext  : IdentityDbContext
    {
        public MyApplicationDbContext(DbContextOptions<MyApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<UserFileInfo> UserFileInfos { get; set; }

    }
}
