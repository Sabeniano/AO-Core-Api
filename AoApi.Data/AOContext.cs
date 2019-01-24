using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AoApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using AoApi.Data.Common;

namespace AoApi.Data
{
    public class AOContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Workhours> Workhours { get; set; }


        public AOContext(DbContextOptions<AOContext> options) : base(options)
        {
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //  add audit information before saving
            AddAuditInformation();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Wallet>()
                .Property(e => e.PaymentMethod)
                .HasConversion(
                    v => v.ToString(),
                    v => (EnumPaymentMethod)Enum.Parse(typeof(EnumPaymentMethod), v));

            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = Guid.Parse("B48D780F-44AD-408F-B5F6-81BDFE15E617"), RoleTitle = "Master Administrator"},
                new Role() { Id = Guid.Parse("A3D1A284-6CE6-494A-A616-822239DF2799"), RoleTitle = "Administrator"},
                new Role() { Id = Guid.Parse("A105FA9D-8B3E-4A80-84E5-4A97C42ED931"), RoleTitle = "Employee"}
            );

            modelBuilder.Entity<Job>().HasData(
                new Job() { Id = Guid.Parse("8068CBF6-C595-4733-9C24-8104E8454B4C"), JobTitle = "CEO", Description = "Chief Executive Officer" },
                new Job() { Id = Guid.Parse("6E0A64BC-7A50-4A6C-9125-8CCF6E54BF70"), JobTitle = "CIO", Description = "Chief Information Officer" },
                new Job() { Id = Guid.Parse("72163C34-3D32-4A78-9701-1F708053978F"), JobTitle = "Administrator", Description = "IT Administrator" },
                new Job() { Id = Guid.Parse("E143EBFF-A0BD-4107-889F-9BFF26EDA916"), JobTitle = "Sales Manager", Description = "" },
                new Job() { Id = Guid.Parse("01C66E3E-8C25-4F5C-A2C5-512C79D09AA6"), JobTitle = "Accountant", Description = "" },
                new Job() { Id = Guid.Parse("976A7A24-1C25-4A7F-97C6-1A019C5C148D"), JobTitle = "IT-Support", Description = "" },
                new Job() { Id = Guid.Parse("0532F0DF-C92D-4A10-9D1A-8A5935C541A2"), JobTitle = "Sales Assitant", Description = "" },
                new Job() { Id = Guid.Parse("76C2FAB8-7161-49B7-88C6-F3AAF484EA64"), JobTitle = "Janitor", Description = "" }
            );

            modelBuilder.Entity<Employee>().HasData(
                // Employee 1
                new Employee()
                {
                    Id = Guid.Parse("9D90A452-9547-4D04-98ED-7D617E64AE1E"),
                    JobId = Guid.Parse("8068CBF6-C595-4733-9C24-8104E8454B4C"),
                    FirstName = "Mikkel",
                    LastName = "Hammer",
                    Birthday = new DateTimeOffset(new DateTime(1980, 11, 11)),
                    Email = "MikkelHammer@gmail.com",
                    City = "Copenhagen",
                    Country = "Denmark",
                    Street = "Telegrafvej 9",
                    PhoneNumber = "29482948",
                    //StartDate = new DateTimeOffset(new DateTime(2000, 5, 12)),
                },
                // Employee 2
                new Employee()
                {
                    Id = Guid.Parse("45C0C223-DE18-4E6E-99EA-AED94E7469F1"),
                    JobId = Guid.Parse("8068CBF6-C595-4733-9C24-8104E8454B4C"),
                    FirstName = "Balen",
                    LastName = "Dezai",
                    Birthday = new DateTimeOffset(new DateTime(1980, 11, 11)),
                    Email = "BalenDezai@gmail.com",
                    City = "Copenhagen",
                    Country = "Denmark",
                    Street = "Telegrafvej 9",
                    PhoneNumber = "29482949",
                    //StartDate = new DateTimeOffset(new DateTime(2000, 5, 12)),
                },
                // Employee 3
                new Employee()
                {
                    Id = Guid.Parse("DE9B842D-531A-4F17-AD69-0D3E11CB911D"),
                    JobId = Guid.Parse("8068CBF6-C595-4733-9C24-8104E8454B4C"),
                    FirstName = "Jason",
                    LastName = "Sabeniano",
                    Birthday = new DateTimeOffset(new DateTime(1997, 7, 23)),
                    Email = "JasonSabeniano@gmail.com",
                    City = "Copenhagen",
                    Country = "Denmark",
                    Street = "Telegrafvej 9",
                    PhoneNumber = "29482950",
                    //StartDate = new DateTimeOffset(new DateTime(2000, 5, 12)),

                },
                // Employee 4
                new Employee()
                {
                    Id = Guid.Parse("195C5A46-96F9-446B-B4F7-864AB2DC49DE"),
                    JobId = Guid.Parse("72163C34-3D32-4A78-9701-1F708053978F"),
                    FirstName = "Franz",
                    LastName = "Kafka",
                    Birthday = new DateTimeOffset(new DateTime(1883, 07, 3)),
                    Email = "FranzKafka@gmail.com",
                    City = "Prague",
                    Country = "Czech republic",
                    Street = "Telegrafvej 9",
                    PhoneNumber = "29482948",
                    //StartDate = new DateTimeOffset(new DateTime(1924, 6, 2)),
                },
                // Employee 5
                new Employee()
                {
                    Id = Guid.Parse("174FD8D4-F72B-4059-A7EA-05E687026B0D"),
                    JobId = Guid.Parse("0532F0DF-C92D-4A10-9D1A-8A5935C541A2"),
                    FirstName = "Fjodor",
                    LastName = "Dostojevskij",
                    Birthday = new DateTimeOffset(new DateTime(1821, 11, 11)),
                    Email = "FjordorDostojevskij@gmail.com",
                    City = "Moskva",
                    Country = "Russia",
                    Street = "Telegrafvej 9",
                    PhoneNumber = "29482948",
                    //StartDate = new DateTimeOffset(new DateTime(2000, 5, 12)),
                },
                // Employee 6
                new Employee()
                {
                    Id = Guid.Parse("163C03B3-A057-426D-AFA3-1A2631A693E2"),
                    JobId = Guid.Parse("76C2FAB8-7161-49B7-88C6-F3AAF484EA64"),
                    FirstName = "Ernest",
                    LastName = "Hemingway",
                    Birthday = new DateTimeOffset(new DateTime(1899, 07, 21)),
                    Email = "ErnestHemingway@gmail.com",
                    City = "Springfield",
                    Country = "USA",
                    Street = "Telegrafvej 9",
                    PhoneNumber = "29482948",
                    //StartDate = new DateTimeOffset(new DateTime(2000, 5, 12)),
                }
                );

            //User
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = Guid.Parse("9A28EA13-7A24-4A4C-8394-37605FF69C82"),
                    EmployeeId = Guid.Parse("DE9B842D-531A-4F17-AD69-0D3E11CB911D"),
                    Username = "Superuser",
                    Email = "SU@gmail.com",
                    RoleId = Guid.Parse("B48D780F-44AD-408F-B5F6-81BDFE15E617"),
                    Password = "SU1234"
                },
                new User()
                {
                    Id = Guid.Parse("A3D1A284-6CE6-494A-A616-822239DF2799"),
                    EmployeeId = Guid.Parse("195C5A46-96F9-446B-B4F7-864AB2DC49DE"),
                    Username = "Administrativeuser",
                    Email = "AU@gmail.com",
                    RoleId = Guid.Parse("A3D1A284-6CE6-494A-A616-822239DF2799"),
                    Password = "AU1234"
                },
                new User()
                {
                    Id = Guid.Parse("A105FA9D-8B3E-4A80-84E5-4A97C42ED931"),
                    EmployeeId = Guid.Parse("163C03B3-A057-426D-AFA3-1A2631A693E2"),
                    Username = "Employeeuser",
                    Email = "EU@gmail.com",
                    RoleId = Guid.Parse("A105FA9D-8B3E-4A80-84E5-4A97C42ED931"),
                    Password = "EU1234"
                }
                );

            // Schedules
            modelBuilder.Entity<Schedule>().HasData(
                new Schedule()
                {
                    Id = Guid.Parse("092CA7C5-AE83-4A52-A38B-CFC7C8E40E9A"),
                    EmployeeId = Guid.Parse("9D90A452-9547-4D04-98ED-7D617E64AE1E"),
                    IsHoliday = false,
                    IsWeekend = false,
                    WorkDate = new DateTimeOffset(new DateTime(2018, 12, 23)),
                    StartHour = new DateTimeOffset(new DateTime(2018, 12, 23, 6, 0, 0)),
                    EndHour = new DateTimeOffset(new DateTime(2018, 12, 23, 12, 0, 0))
                },
                new Schedule()
                {
                    Id = Guid.Parse("5E0D5AD3-22B0-4BDC-808C-62B8F50D0796"),
                    EmployeeId = Guid.Parse("45C0C223-DE18-4E6E-99EA-AED94E7469F1"),
                    IsHoliday = true,
                    IsWeekend = true,
                    WorkDate = new DateTimeOffset(new DateTime(2019, 1, 1)),
                    StartHour = new DateTimeOffset(new DateTime(2019, 1, 1, 6, 0, 0)),
                    EndHour = new DateTimeOffset(new DateTime(2019, 1, 1, 12, 0, 0))
                },
                new Schedule()
                {
                    Id = Guid.Parse("CF3C5F8E-94EE-494A-B0F1-4A48D9D8291F"),
                    EmployeeId = Guid.Parse("DE9B842D-531A-4F17-AD69-0D3E11CB911D"),
                    IsHoliday = true,
                    IsWeekend = true,
                    WorkDate = new DateTimeOffset(new DateTime(2019, 1, 1)),
                    StartHour = new DateTimeOffset(new DateTime(2019, 1, 1, 12, 0, 0)),
                    EndHour = new DateTimeOffset(new DateTime(2019, 1, 1, 18, 0, 0))
                }
                );

            // Ændrer ID 
            modelBuilder.Entity<Wallet>().HasData(
                new Wallet()
                {
                    Id = Guid.Parse("303814CA-54F0-4FBB-955B-7FFD33B10B9D"),
                    EmployeeId = Guid.Parse("9D90A452-9547-4D04-98ED-7D617E64AE1E"),
                    Wage = 50000,
                    Salary = 0,
                    PaymentMethod = EnumPaymentMethod.Monthly,
                },
                new Wallet()
                {
                    Id = Guid.Parse("CE442AD4-37A4-43F4-9A6D-5F7AB15DF011"),
                    EmployeeId = Guid.Parse("45C0C223-DE18-4E6E-99EA-AED94E7469F1"),
                    Wage = 50000,
                    Salary = 0,
                    PaymentMethod = EnumPaymentMethod.Monthly,
                },
                new Wallet()
                {
                    Id = Guid.Parse("7F36E8E7-B5CD-43EF-A71D-8CFA2355D8AB"),
                    EmployeeId = Guid.Parse("DE9B842D-531A-4F17-AD69-0D3E11CB911D"),
                    Wage = 50000,
                    Salary = 0,
                    PaymentMethod = EnumPaymentMethod.Monthly,
                },
                new Wallet()
                {
                    Id = Guid.Parse("68ACCFC2-B922-4519-9BD2-20E235B6DB2E"),
                    EmployeeId = Guid.Parse("195C5A46-96F9-446B-B4F7-864AB2DC49DE"),
                    Wage = 0,
                    Salary = 600,
                    PaymentMethod = EnumPaymentMethod.Monthly,
                },
                new Wallet()
                {
                    Id = Guid.Parse("F2D86EC1-0735-4F47-8087-0C5C311F3B74"),
                    EmployeeId = Guid.Parse("174FD8D4-F72B-4059-A7EA-05E687026B0D"),
                    Wage = 0,
                    Salary = 400,
                    PaymentMethod = EnumPaymentMethod.Hourly
                },
                new Wallet()
                {
                    Id = Guid.Parse("8AE640BB-1534-4E25-AA97-D85128D50AA8"),
                    EmployeeId = Guid.Parse("163C03B3-A057-426D-AFA3-1A2631A693E2"),
                    Wage = 0,
                    Salary = 300,
                    PaymentMethod = EnumPaymentMethod.Hourly
                }
                );

            modelBuilder.Entity<Workhours>().HasData(
                new Workhours()
                {
                    Id = Guid.Parse("044B879D-1486-4BC5-9907-2A7E14C84F7C"),
                    EmployeeId = Guid.Parse("9D90A452-9547-4D04-98ED-7D617E64AE1E"),
                    TotalHoursThisPaycheck = 32,
                    TotalOvertimeHoursThisPaycheck = 0
                },
                new Workhours()
                {
                    Id = Guid.Parse("1D0D61A3-9DFF-4F0B-ABC3-524B310D6FE4"),
                    EmployeeId = Guid.Parse("45C0C223-DE18-4E6E-99EA-AED94E7469F1"),
                    TotalHoursThisPaycheck = 32,
                    TotalOvertimeHoursThisPaycheck = 0
                },
                new Workhours()
                {
                    Id = Guid.Parse("9AF65A2D-A8BD-410C-A3A2-61B8B2427F5E"),
                    EmployeeId = Guid.Parse("DE9B842D-531A-4F17-AD69-0D3E11CB911D"),
                    TotalHoursThisPaycheck = 32,
                    TotalOvertimeHoursThisPaycheck = 0
                },
                new Workhours()
                {
                    Id = Guid.Parse("AD0A45A1-59E0-47AC-9132-B3F4AEA940F9"),
                    EmployeeId = Guid.Parse("195C5A46-96F9-446B-B4F7-864AB2DC49DE"),
                    TotalHoursThisPaycheck = 32,
                    TotalOvertimeHoursThisPaycheck = 5
                },
                new Workhours()
                {
                    Id = Guid.Parse("40AC68BF-F764-4E0F-9197-DCF365C493AF"),
                    EmployeeId = Guid.Parse("174FD8D4-F72B-4059-A7EA-05E687026B0D"),
                    TotalHoursThisPaycheck = 32,
                    TotalOvertimeHoursThisPaycheck = 5
                },
                new Workhours()
                {
                    Id = Guid.Parse("1A7824D4-E3D3-4BE7-BF6E-3A2C52583628"),
                    EmployeeId = Guid.Parse("163C03B3-A057-426D-AFA3-1A2631A693E2"),
                    TotalHoursThisPaycheck = 32,
                    TotalOvertimeHoursThisPaycheck = 20
                }
                );

            //  get all the deleteable entity types
            var deleteableEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(x => x.ClrType != null && typeof(IDeleteableEntity).IsAssignableFrom(x.ClrType));

            foreach (var deletedentityType in deleteableEntityTypes)
            {
                //  add indexer on the IsDeleted property on each of them
                modelBuilder.Entity(deletedentityType.ClrType).HasIndex(nameof(IDeleteableEntity.IsDeleted));
            }

        }

        private void AddAuditInformation()
        {
            //  get the changed entries from the changed tracker
            //  where they are of type IAduit
            //  and their state is either added or modified
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditInfo && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in changedEntities)
            {
                //  type cast the entity in the entry to IAduit
                var entity = (IAuditInfo)entry.Entity;

                //  if the entity state is added and the createdon property on the entity is default
                //  default of thet type which in this case is null
                //  then assign created on, otherwise assign modified on
                if (entry.State == EntityState.Added && entity.CreatedOn == default(DateTimeOffset))
                {
                    entity.CreatedOn = DateTimeOffset.Now;
                }
                else
                {
                    entity.ModifiedOn = DateTimeOffset.Now;
                }
            }
        }
    }
}
