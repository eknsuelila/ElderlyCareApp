using Microsoft.EntityFrameworkCore;

namespace ElderlyCareApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ElderlyPerson> ElderlyPeople { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<MedicationLog> MedicationLogs { get; set; }
        public DbSet<MealLog> MealLogs { get; set; }
        public DbSet<AppointmentLog> AppointmentLogs { get; set; }
        public DbSet<CareNote> CareNotes { get; set; }
        public DbSet<CaregiverAssignment> CaregiverAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ElderlyPerson configuration
            modelBuilder.Entity<ElderlyPerson>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            // ActivityLog configuration
            modelBuilder.Entity<ActivityLog>()
                .HasOne(a => a.ElderlyPerson)
                .WithMany()
                .HasForeignKey(a => a.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ActivityLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // MedicationLog configuration
            modelBuilder.Entity<MedicationLog>()
                .HasOne(m => m.ElderlyPerson)
                .WithMany()
                .HasForeignKey(m => m.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicationLog>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // MealLog configuration
            modelBuilder.Entity<MealLog>()
                .HasOne(m => m.ElderlyPerson)
                .WithMany()
                .HasForeignKey(m => m.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MealLog>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentLog configuration
            modelBuilder.Entity<AppointmentLog>()
                .HasOne(a => a.ElderlyPerson)
                .WithMany()
                .HasForeignKey(a => a.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppointmentLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // CareNote configuration
            modelBuilder.Entity<CareNote>()
                .HasOne(n => n.ElderlyPerson)
                .WithMany()
                .HasForeignKey(n => n.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CareNote>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // CaregiverAssignment configuration
            modelBuilder.Entity<CaregiverAssignment>()
                .HasOne(ca => ca.Caregiver)
                .WithMany()
                .HasForeignKey(ca => ca.CaregiverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CaregiverAssignment>()
                .HasOne(ca => ca.ElderlyPerson)
                .WithMany()
                .HasForeignKey(ca => ca.ElderlyPersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure unique assignments (one active assignment per caregiver per patient)
            modelBuilder.Entity<CaregiverAssignment>()
                .HasIndex(ca => new { ca.CaregiverId, ca.ElderlyPersonId, ca.IsActive })
                .IsUnique()
                .HasFilter("[IsActive] = 1");
        }
    }
} 