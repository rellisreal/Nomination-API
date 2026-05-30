using Microsoft.EntityFrameworkCore;
using nomination_api.models;

namespace nomination_api.DataBaseContext
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Nomination>()
                .HasOne(n => n.Nominator)
                .WithMany(u => u.NominationsGiven)
                .HasForeignKey(n => n.NominatorId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Nomination>()
                .HasOne(n => n.Nominated)
                .WithMany(u => u.NominationsReceived)
                .HasForeignKey(n => n.NominatedId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany (u => u.EventsGenerated)
                .HasForeignKey (e => e.UserId)
                .HasPrincipalKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.UsersInRole)
                .HasForeignKey(u => u.RoleId)
                .HasPrincipalKey(r => r.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);
            
            modelBuilder.Entity<Nomination>()
            .HasOne(n => n.Category)
            .WithMany(c => c.NominationsInCategory)
            .HasForeignKey(n => n.CategoryId)
            .HasPrincipalKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            // Errors with FKEY references, due to statically declaring a default Admin/Role
            var guidConverter = new Microsoft.EntityFrameworkCore.Storage.ValueConversion.GuidToStringConverter();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties().Where(p => p.ClrType == typeof(Guid)))
                {
                    property.SetValueConverter(guidConverter);
                }
            }
        }

        public DbSet<Nomination> Nominations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
    }   
}