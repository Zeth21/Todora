using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=ZEYIT;initial Catalog=Todora;trust server certificate=true;integrated security=true");
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<RepositoryRole> RepositoryRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<TaskOwning> TaskOwnings { get; set; }
        public DbSet<TaskStage> TaskStages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //USER SETTINGS
            builder.Entity<User>(entity =>
            {
                //Building relationships
                entity.HasMany(u => u.RepositoryRoles)
                      .WithOne(r => r.User)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.TaskStages)
                      .WithOne(ts => ts.User)
                      .HasForeignKey(ts => ts.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.TaskOwnings)
                      .WithOne(to => to.User)
                      .HasForeignKey(to => to.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(u => u.Notifications)
                      .WithOne(n => n.User)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            //NOTIFICATION SETTINGS
            builder.Entity<Notification>(entity => 
            {
                //Building relationships with User
                entity.HasOne(u => u.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

                //Setting maxlength for title property
                entity.Property(n => n.NotificationTitle)
                .IsRequired()
                .HasMaxLength(75);

                //Setting maxlength for description property
                entity.Property(n => n.NotificationDescription)
                .IsRequired()
                .HasMaxLength(200);
            });

            //REPOSITORY SETTINGS
            builder.Entity<Repository>(entity => 
            {
                //Building relationships with Task
                entity.HasMany(r => r.Tasks)
                .WithOne(t => t.Repository)
                .HasForeignKey(t => t.RepositoryId)
                .OnDelete(DeleteBehavior.NoAction);

                //Setting maxlength for title property
                entity.Property(n => n.RepositoryTitle)
                .IsRequired()
                .HasMaxLength(75);

                //Setting maxlength for description property
                entity.Property(n => n.RepositoryDescription)
                .IsRequired()
                .HasMaxLength(200);
            });

        }

    }
}
