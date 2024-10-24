using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW_23_10
{
    public class AdditionalTask4
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                /*db.Database.EnsureDeleted();
                db.Database.EnsureCreated();*/

                var statistics = db.Departments.Select(d => new
                {
                    DepartmentName = d.Name,
                    AverageSalary = d.Employees.Average(e => e.Salary),
                    TopEmployees = d.Employees.OrderByDescending(e => e.Salary).Take(3).Select(e => new
                    {
                        EmployeeName = e.Name,
                        Salary = e.Salary
                    }).ToList()
                }).ToList();
            }
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }

    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }

    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            modelBuilder.Entity<Department>().HasData(
                new Department() { Id = 1, Name = "Human Resources" },
                new Department() { Id = 2, Name = "IT" },
                new Department() { Id = 3, Name = "Marketing" });

            modelBuilder.Entity<Employee>().HasData(
                new Employee()
                {
                    Id = 1,
                    Name = "Alice Johnson",
                    Salary = 55000,
                    DepartmentId = 1
                },
                new Employee()
                {
                    Id = 2,
                    Name = "Bob Smith",
                    Salary = 75000,
                    DepartmentId = 2
                },
                new Employee()
                {
                    Id = 3,
                    Name = "Carol Davis",
                    Salary = 65000,
                    DepartmentId = 3
                },
                new Employee()
                {
                    Id = 4,
                    Name = "David Brown",
                    Salary = 72000,
                    DepartmentId = 2
                });

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EFCoreDB;Trusted_Connection=True;");
        }
    }
}
