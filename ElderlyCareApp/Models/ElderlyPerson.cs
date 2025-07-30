using System;
using System.ComponentModel.DataAnnotations;

namespace ElderlyCareApp.Models
{
    public class ElderlyPerson
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(100)]
        public string? EmergencyContactName { get; set; }
        
        [StringLength(20)]
        public string? EmergencyContactPhone { get; set; }
        
        [StringLength(500)]
        public string? Allergies { get; set; }
        
        [StringLength(500)]
        public string? MedicalConditions { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
        public virtual ICollection<MedicationLog> MedicationLogs { get; set; } = new List<MedicationLog>();
        public virtual ICollection<MealLog> MealLogs { get; set; } = new List<MealLog>();
        public virtual ICollection<AppointmentLog> AppointmentLogs { get; set; } = new List<AppointmentLog>();
        public virtual ICollection<CareNote> CareNotes { get; set; } = new List<CareNote>();
    }
} 