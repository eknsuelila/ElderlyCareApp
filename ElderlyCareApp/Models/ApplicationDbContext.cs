using Microsoft.EntityFrameworkCore;

namespace ElderlyCareApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Core entities
        public DbSet<User> Users { get; set; }
        public DbSet<ElderlyPerson> ElderlyPeople { get; set; }
        
        // Log entities
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<MedicationLog> MedicationLogs { get; set; }
        public DbSet<MealLog> MealLogs { get; set; }
        public DbSet<SocialInteractionLog> SocialInteractionLogs { get; set; }
        public DbSet<VitalSignsLog> VitalSignsLogs { get; set; }
        public DbSet<AppointmentLog> AppointmentLogs { get; set; }
        public DbSet<SymptomLog> SymptomLogs { get; set; }
        
        // Care management entities
        public DbSet<CarePlan> CarePlans { get; set; }
        public DbSet<TaskSchedule> TaskSchedules { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure relationships and constraints
            
            // User - ElderlyPerson relationship (many-to-many through logs)
            modelBuilder.Entity<User>()
                .HasMany(u => u.ActivityLogs)
                .WithOne(al => al.User)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.MedicationLogs)
                .WithOne(ml => ml.User)
                .HasForeignKey(ml => ml.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.MealLogs)
                .WithOne(ml => ml.User)
                .HasForeignKey(ml => ml.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.SocialInteractionLogs)
                .WithOne(sil => sil.User)
                .HasForeignKey(sil => sil.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.VitalSignsLogs)
                .WithOne(vsl => vsl.User)
                .HasForeignKey(vsl => vsl.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.AppointmentLogs)
                .WithOne(al => al.User)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.SymptomLogs)
                .WithOne(sl => sl.User)
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<User>()
                .HasMany(u => u.CarePlans)
                .WithOne(cp => cp.User)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // ElderlyPerson - Logs relationships
            modelBuilder.Entity<ElderlyPerson>()
                .HasMany(ep => ep.ActivityLogs)
                .WithOne(al => al.ElderlyPerson)
                .HasForeignKey(al => al.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ElderlyPerson>()
                .HasMany(ep => ep.MedicationLogs)
                .WithOne(ml => ml.ElderlyPerson)
                .HasForeignKey(ml => ml.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ElderlyPerson>()
                .HasMany(ep => ep.MealLogs)
                .WithOne(ml => ml.ElderlyPerson)
                .HasForeignKey(ml => ml.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ElderlyPerson>()
                .HasMany(ep => ep.SocialInteractionLogs)
                .WithOne(sil => sil.ElderlyPerson)
                .HasForeignKey(sil => sil.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ElderlyPerson>()
                .HasMany(ep => ep.VitalSignsLogs)
                .WithOne(vsl => vsl.ElderlyPerson)
                .HasForeignKey(vsl => vsl.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ElderlyPerson>()
                .HasMany(ep => ep.AppointmentLogs)
                .WithOne(al => al.ElderlyPerson)
                .HasForeignKey(al => al.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<ElderlyPerson>()
                .HasMany(ep => ep.SymptomLogs)
                .WithOne(sl => sl.ElderlyPerson)
                .HasForeignKey(sl => sl.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // CarePlan - TaskSchedule relationship
            modelBuilder.Entity<CarePlan>()
                .HasMany(cp => cp.TaskSchedules)
                .WithOne(ts => ts.CarePlan)
                .HasForeignKey(ts => ts.CarePlanId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // TaskSchedule - ElderlyPerson relationship (change to NO ACTION to avoid cascade cycles)
            modelBuilder.Entity<TaskSchedule>()
                .HasOne(ts => ts.ElderlyPerson)
                .WithMany()
                .HasForeignKey(ts => ts.ElderlyPersonId)
                .OnDelete(DeleteBehavior.NoAction);
            
            // TaskSchedule - User relationship (change to NO ACTION to avoid cascade cycles)
            modelBuilder.Entity<TaskSchedule>()
                .HasOne(ts => ts.User)
                .WithMany()
                .HasForeignKey(ts => ts.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            // Configure indexes for better performance
            modelBuilder.Entity<ActivityLog>()
                .HasIndex(al => new { al.ElderlyPersonId, al.StartTime });
                
            modelBuilder.Entity<MedicationLog>()
                .HasIndex(ml => new { ml.ElderlyPersonId, ml.ScheduledTime });
                
            modelBuilder.Entity<MealLog>()
                .HasIndex(ml => new { ml.ElderlyPersonId, ml.MealTime });
                
            modelBuilder.Entity<VitalSignsLog>()
                .HasIndex(vsl => new { vsl.ElderlyPersonId, vsl.MeasurementTime });
                
            modelBuilder.Entity<AppointmentLog>()
                .HasIndex(al => new { al.ElderlyPersonId, al.ScheduledDateTime });
                
            modelBuilder.Entity<TaskSchedule>()
                .HasIndex(ts => new { ts.ElderlyPersonId, ts.ScheduledTime });
                
            modelBuilder.Entity<TaskSchedule>()
                .HasIndex(ts => new { ts.Status, ts.ScheduledTime });
        }
    }
} 