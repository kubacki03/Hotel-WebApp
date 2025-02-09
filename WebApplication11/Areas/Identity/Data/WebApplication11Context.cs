using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication11.Areas.Identity.Data;
using WebApplication11.Models;

namespace WebApplication11.Data;

public class WebApplication11Context : IdentityDbContext<User>
{
    public WebApplication11Context(DbContextOptions<WebApplication11Context> options)
        : base(options)
    {
    }


    public DbSet<User> Users {  get; set; }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

 

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        

        builder.Entity<Room>()
            .HasMany(s => s.Reservations)
            .WithMany(c => c.Rooms)
            .UsingEntity(j => j.ToTable("ReservationRooms"));

        builder.Entity<Reservation>()
      .HasOne(p => p.User)
      .WithMany(b => b.Reservations)
      .HasForeignKey(p => p.UserId); 



    }
}
