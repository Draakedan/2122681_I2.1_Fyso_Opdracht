﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FysioAppUX.Data
{
    public class FysioIdentityDBContext : IdentityDbContext
    {
        public FysioIdentityDBContext()
        { 
        }

        public string GetUserEmail(string name)
        {
            var user = this.Users.FirstOrDefault(u => u.UserName == name);
            return user.Email;
        }

        public FysioIdentityDBContext(DbContextOptions<FysioIdentityDBContext> options)
            :base (options)
        { 

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=desktop-vh9b6rm;Initial Catalog=FysioIDentity;Integrated Security=True");
        }

        //

    }
}
