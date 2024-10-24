using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

class Program
{
    static void Main(string[] args)
    {
        using (ApplicationContext db = new ApplicationContext())
        {
            /*db.Database.EnsureDeleted();
            db.Database.EnsureCreated();*/


            //var obj = db.UserCompanyViewModels.FromSqlRaw("EXEC GetDataFromTables").ToList();

            //var users = db.Users.FromSqlRaw("EXEC GetUsersByNamePattern 'o'").ToList();

            /*SqlParameter avgAge = new SqlParameter("@AvgAge", System.Data.SqlDbType.Decimal)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            db.Database.ExecuteSqlRaw($"EXEC GetAverageAgeOfUsers @AvgAge OUT", avgAge);*/
        }
    }
}

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<User> Users { get; set; }
}

public class UserCompanyViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<UserCompanyViewModel> UserCompanyViewModels { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId);

        modelBuilder.Entity<Company>().HasData(
           new Company { Id = 1, Name = "Tech Solutions", Description = "Providing tech solutions for businesses." },
           new Company { Id = 2, Name = "Creative Agency", Description = "A creative agency focused on marketing." },
           new Company { Id = 3, Name = "Health Corp", Description = "Healthcare services and products." }
       );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, FullName = "Alice Johnson", Age = 30, Email = "alice.johnson@example.com", CompanyId = 1 },
            new User { Id = 2, FullName = "Bob Smith", Age = 25, Email = "bob.smith@example.com", CompanyId = 2 },
            new User { Id = 3, FullName = "Charlie Brown", Age = 28, Email = "charlie.brown@example.com", CompanyId = 3 },
            new User { Id = 4, FullName = "Diana Prince", Age = 35, Email = "diana.prince@example.com", CompanyId = 1 },
            new User { Id = 5, FullName = "Ethan Hunt", Age = 32, Email = "ethan.hunt@example.com", CompanyId = 2 }
        );

        base.OnModelCreating(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EFCoreDB;Trusted_Connection=True;");
    }
}