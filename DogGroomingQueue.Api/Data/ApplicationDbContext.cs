using DogGroomingQueue.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DogGroomingQueue.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<GroomingType> DogGroomingTypes => Set<GroomingType>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User_Ta");
            entity.HasKey(x => x.UserId_Int);
        });

        modelBuilder.Entity<GroomingType>(entity =>
        {
            entity.ToTable("DogGroomingTypes_Ta");
            entity.HasKey(x => x.GroomingTypeId_Int);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointments_Ta");
            entity.HasKey(x => x.AppointmentId_Int);
        });

        base.OnModelCreating(modelBuilder);
    }
}