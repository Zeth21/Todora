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
        public DbSet<Role> Rolles { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<WorkTask> Tasks { get; set; }
        public DbSet<TaskOwning> TaskOwnings { get; set; }
        public DbSet<TaskStage> TaskStages { get; set; }
        public DbSet<StageNote> StageNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //USER SETTINGS
            builder.Entity<User>(entity =>
            {
                //Setting rules for Name property
                entity.Property(x => x.Name)
                    .HasMaxLength(30);

                //Setting rules for Surname property
                entity.Property(x => x.Surname)
                    .HasMaxLength(30);

                //Building relationships with RepositoryRoles
                entity.HasMany(u => u.RepositoryRoles)
                      .WithOne(r => r.User)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                //Building relationships with TaskStages
                entity.HasMany(u => u.TaskStages)
                      .WithOne(ts => ts.User)
                      .HasForeignKey(ts => ts.UserId)
                      .OnDelete(DeleteBehavior.NoAction);

                //Building relationships with TaskOwnings
                entity.HasMany(u => u.TaskOwnings)
                      .WithOne(to => to.User)
                      .HasForeignKey(to => to.UserId)
                      .OnDelete(DeleteBehavior.NoAction);


                //Building relationships with Notifications
                entity.HasMany(u => u.Notifications)
                      .WithOne(n => n.User)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.NoAction);


                //Building relationships with StageNotes
                entity.HasMany(u => u.StageNotes)
                      .WithOne(st => st.User)
                      .HasForeignKey(st => st.UserId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            //NOTIFICATION SETTINGS
            builder.Entity<Notification>(entity => 
            {
                //Setting primary key
                entity.HasKey(n => n.NotificationId);

                //Setting primary key auto increment
                entity.Property(n => n.NotificationId)
                .ValueGeneratedOnAdd();

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

                entity.Property(n => n.NotificationTime)
                .HasColumnType("datetime2(0)");
            });

            //REPOSITORY SETTINGS
            builder.Entity<Repository>(entity => 
            {
                //Setting primary key
                entity.HasKey(r => r.RepositoryId);

                //Setting primary key auto increment
                entity.Property(r => r.RepositoryId)
                .ValueGeneratedOnAdd();

                //Building relationships with Task
                entity.HasMany(r => r.Tasks)
                .WithOne(t => t.Repository)
                .HasForeignKey(t => t.RepositoryId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationships with RepositoryRoles
                entity.HasMany(r => r.RepositoryRoles)
                .WithOne(t => t.Repository)
                .HasForeignKey(t => t.RepositoryRoleId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(n => n.RepositoryCreateDate)
                .HasColumnType("datetime2(0)");

                //Setting maxlength for title property
                entity.Property(n => n.RepositoryTitle)
                .IsRequired()
                .HasMaxLength(75);

                //Setting maxlength for description property
                entity.Property(n => n.RepositoryDescription)
                .IsRequired()
                .HasMaxLength(200);
            });

            //REPOSITORYROLE SETTINGS
            builder.Entity<RepositoryRole>(entity => 
            {
                //Setting primary key
                entity.HasKey(rp => rp.RepositoryRoleId);

                //Setting primary key auto increment
                entity.Property(rp => rp.RepositoryRoleId)
                .ValueGeneratedOnAdd();

                //Building relationshipts with User
                entity.HasOne(rp => rp.User)
                .WithMany(u => u.RepositoryRoles)
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationshipts with Role
                entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RepositoryRoles)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationshipts with Role
                entity.HasOne(rp => rp.Repository)
                .WithMany(r => r.RepositoryRoles)
                .HasForeignKey(rp => rp.RepositoryId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasIndex(rp => new { rp.RepositoryId, rp.UserId })
                .IsUnique();
            });

            //ROLE SETTINGS
            builder.Entity<Role>(entity => 
            {
                //Setting primary key
                entity.HasKey(r => r.RoleId);

                //Setting primary key auto increment
                entity.Property(r => r.RoleId)
                .ValueGeneratedOnAdd();

                //Building relationshipts with RepositoryRoles
                entity.HasMany(r => r.RepositoryRoles)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

                //Setting maxlength for name property
                entity.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(75);

                //Setting unique value for name property
                entity.HasIndex(r => r.RoleName)
                .IsUnique();
            });

            //STAGE SETTINGS
            builder.Entity<Stage>(entity =>
            {
                //Setting primary key
                entity.HasKey(s => s.StageId);

                //Setting primary key auto increment
                entity.Property(s => s.StageId)
                .ValueGeneratedOnAdd();

                //Building relationshipts with RepositoryRoles
                entity.HasMany(s => s.TaskStages)
                .WithOne(ts => ts.Stage)
                .HasForeignKey(ts => ts.StageId)
                .OnDelete(DeleteBehavior.NoAction);

                //Setting maxlength for name property
                entity.Property(s => s.StageName)
                .IsRequired()
                .HasMaxLength(75);

                //Setting unique value for name property
                entity.HasIndex(s => s.StageName)
                .IsUnique();
            });

            //TASK SETTINGS
            builder.Entity<Domain.Entities.WorkTask>(entity => 
            {
                //Setting primary key
                entity.HasKey(t => t.TaskId);

                //Setting primary key auto increment
                entity.Property(t => t.TaskId)
                .ValueGeneratedOnAdd();

                //Building relationship with User
                entity.HasOne(t => t.User)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.TaskCreatedUserId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationship with Repository
                entity.HasOne(t => t.Repository)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.RepositoryId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationships with TaskStages
                entity.HasMany(t => t.TaskStages)
                .WithOne(s => s.Task)
                .HasForeignKey(t => t.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

                // The title value must be unique within its repository
                entity.HasIndex(t => new {t.RepositoryId, t.TaskTitle})
                .IsUnique();

                //Setting maxlength for title property
                entity.Property(t => t.TaskTitle)
                .HasMaxLength(75)
                .IsRequired();

                //Setting maxlength for description property
                entity.Property(t => t.TaskDescription)
                .HasMaxLength(200)
                .IsRequired();

                entity.Property(n => n.TaskCreateDate)
                .HasColumnType("datetime2(0)");

                entity.Property(n => n.TaskStartDate)
                .HasColumnType("datetime2(0)"); 

                entity.Property(n => n.TaskEndDate)
                .HasColumnType("datetime2(0)");
            });

            //TASKOWNING SETTINGS
            builder.Entity<TaskOwning>(entity => 
            {
                //Setting primary key
                entity.HasKey(to => to.TaskOwningId);

                //Setting primary key auto increment
                entity.Property(to => to.TaskOwningId)
                .ValueGeneratedOnAdd();

                //Building relationship with Task
                entity.HasOne(to => to.Task)
                .WithMany(t => t.TaskOwnings)
                .HasForeignKey(to => to.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationship with User
                entity.HasOne(to => to.User)
                .WithMany(u => u.TaskOwnings)
                .HasForeignKey(to => to.UserId)
                .OnDelete(DeleteBehavior.NoAction);

                // Create a composite index on TaskId and UserId to prevent duplicate entries
                entity.HasIndex(to => new { to.TaskId, to.UserId })
                .IsUnique()
                .HasFilter("[TaskOwningIsActive] = 1");

                entity.Property(n => n.TaskOwningDate)
                .HasColumnType("datetime2(0)");

                entity.Property(n => n.TaskOwningEndDate)
                .HasColumnType("datetime2(0)");
            });

            //TASKSTAGE SETTINGS
            builder.Entity<TaskStage>(entity => 
            {
                //Setting primary key
                entity.HasKey(ts => ts.TaskStageId);

                //Setting primary key auto increment
                entity.Property(ts => ts.TaskStageId)
                .ValueGeneratedOnAdd();

                //Building relationship with User
                entity.HasOne(ts => ts.User)
                .WithMany(u => u.TaskStages)
                .HasForeignKey(ts => ts.UserId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationship with Task
                entity.HasOne(ts => ts.Task)
                .WithMany(t => t.TaskStages)
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationship with Stage
                entity.HasOne(ts => ts.Stage)
                .WithMany(s => s.TaskStages)
                .HasForeignKey(ts => ts.StageId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationships with StageNotes
                entity.HasMany(ts => ts.StageNotes)
                .WithOne(sn => sn.TaskStage)
                .HasForeignKey(sn => sn.TaskStageId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(n => n.TaskStageDate)
                .HasColumnType("datetime2(0)");
            });

            //STAGENOTE SETTINGS
            builder.Entity<StageNote>(entity => 
            {
                //Setting primary key
                entity.HasKey(sn => sn.StageNoteId);

                //Setting primary key auto increment
                entity.Property(sn => sn.StageNoteId)
                .ValueGeneratedOnAdd();

                //Building relationship with User
                entity.HasOne(sn => sn.User)
                .WithMany(u => u.StageNotes)
                .HasForeignKey(sn => sn.UserId)
                .OnDelete(DeleteBehavior.NoAction);

                //Building relationship with TaskStage
                entity.HasOne(sn => sn.TaskStage)
                .WithMany(ts => ts.StageNotes)
                .HasForeignKey(sn => sn.TaskStageId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.Property(sn => sn.StageNoteText)
                .HasMaxLength(200)
                .IsRequired();

                entity.Property(n => n.StageNoteDate)
                .HasColumnType("datetime2(0)");
            });
        }

    }
}
