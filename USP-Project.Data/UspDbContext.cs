using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using USP_Project.Data.Models;

namespace USP_Project.Data;

public class UspDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public UspDbContext(DbContextOptions<UspDbContext> options)
        : base(options)
    {
    }

    public int Levenshtein(string stringOne, string stringTwo)
        => throw new NotImplementedException();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(UspDbContext).Assembly);
        base.OnModelCreating(builder);
        
        builder.HasDbFunction(
                typeof(UspDbContext).GetMethod(nameof(Levenshtein),
                new []{ typeof(string), typeof(string) })!)
            .HasName("levenshtein");

        builder.Entity<Car>()
            .HasOne(c => c.Brand)
            .WithMany(b => b.Cars)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Entity<Brand>()
            .HasMany(b => b.Cars)
            .WithOne(c => c.Brand);

        builder.Entity<Car>()
            .HasOne(c => c.Model)
            .WithMany(m => m.Cars)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Model>()
            .HasMany(m => m.Cars)
            .WithOne(c => c.Model);

        builder.Entity<CarsExtras>()
            .HasKey(ce => new { ce.CarId, ce.ExtraId });
            
        builder.Entity<CarsExtras>()
            .HasOne(ce => ce.Car)
            .WithMany(c => c.CarsExtras)
            .HasForeignKey(ce => ce.CarId);
        
        builder.Entity<CarsExtras>()
            .HasOne(ce => ce.Extra)
            .WithMany(c => c.CarsExtras)
            .HasForeignKey(ce => ce.ExtraId);
    }
}