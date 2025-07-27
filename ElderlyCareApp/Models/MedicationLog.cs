using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum MedicationStatus
    {
        Scheduled,
        Taken,
        Missed,
        Skipped,
        Refused
    }

    public class MedicationLog
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ElderlyPersonId { get; set; }
        [ForeignKey("ElderlyPersonId")]
        public virtual ElderlyPerson ElderlyPerson { get; set; } = null!;
        
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string MedicationName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Dosage { get; set; }
        
        [StringLength(100)]
        public string? Frequency { get; set; }
        
        [StringLength(100)]
        public string? Route { get; set; } // Oral, Topical, Injection, etc.
        
        [StringLength(100)]
        public string? PrescribedBy { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? PrescriptionDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }
        
        [StringLength(100)]
        public string? Pharmacy { get; set; }
        
        [StringLength(20)]
        public string? PharmacyPhone { get; set; }
        
        [StringLength(500)]
        public string? Instructions { get; set; }
        
        [StringLength(500)]
        public string? SideEffects { get; set; }
        
        [StringLength(500)]
        public string? Interactions { get; set; }
        
        public MedicationStatus Status { get; set; } = MedicationStatus.Scheduled;
        
        public DateTime ScheduledTime { get; set; }
        
        public DateTime? ActualTime { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool RequiresRefill { get; set; } = false;
        
        [DataType(DataType.Date)]
        public DateTime? RefillReminderDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 