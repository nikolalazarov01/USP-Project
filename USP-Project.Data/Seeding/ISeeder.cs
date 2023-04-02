namespace USP_Project.Data.Seeding;

public interface ISeeder
{
    public int Priority { get; }
    
    Task SeedAsync(CancellationToken cancellationToken = default);
}