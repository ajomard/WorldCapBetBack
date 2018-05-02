﻿using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Models;

namespace WorldCapBetService.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
        }

        public DbSet<Team> Team { get; set; }
        public DbSet<Match> Match { get; set; }
        public DbSet<Pronostic> Pronostic { get; set; }
        public DbSet<User> User { get; set; }
    }
}