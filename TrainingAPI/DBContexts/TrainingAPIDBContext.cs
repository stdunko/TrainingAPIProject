using Microsoft.EntityFrameworkCore;
using TrainingAPI.Models;

public class TrainingAPIDBContext : DbContext
{
    public TrainingAPIDBContext(DbContextOptions<TrainingAPIDBContext> options) : base(options)
    {

    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Name> Names { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=db;Database=trainingapi;User Id=sa;Password=1234567Qq");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.Name)
            .WithOne(n => n.Patient)
            .HasForeignKey<Name>(n => n.PatientId);

        modelBuilder.Entity<Name>()
            .Property(n => n.GivenSerialized)
            .HasColumnName("GivenSerialized");
    }
}