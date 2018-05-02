using Microsoft.EntityFrameworkCore;
using WorldCapBetService.Models;

namespace WorldCapBetService.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
        }

        public DbSet<Team> TodoItems { get; set; }
    }
}