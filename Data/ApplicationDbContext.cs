using JokesWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JokesWebApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Joke> Jokes { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(@"Data Source=/home/malcolm/portfolio/JokesWebApp/app.db");
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
