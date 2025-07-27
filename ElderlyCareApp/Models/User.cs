using System;
using System.ComponentModel.DataAnnotations;

namespace ElderlyCareApp.Models
{
    public enum UserRole
    {
        Family,
        Caregiver
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public UserRole Role { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        [StringLength(100)]
        public string? EmergencyContactName { get; set; }
        
        [StringLength(20)]
        public string? EmergencyContactPhone { get; set; }
        
        [StringLength(100)]
        public string? EmergencyContactRelationship { get; set; }
        
        [StringLength(500)]
        public string? Specializations { get; set; }
        
        [StringLength(100)]
        public string? LicenseNumber { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
        public virtual ICollection<MedicationLog> MedicationLogs { get; set; } = new List<MedicationLog>();
        public virtual ICollection<MealLog> MealLogs { get; set; } = new List<MealLog>();
        public virtual ICollection<SocialInteractionLog> SocialInteractionLogs { get; set; } = new List<SocialInteractionLog>();
        public virtual ICollection<VitalSignsLog> VitalSignsLogs { get; set; } = new List<VitalSignsLog>();
        public virtual ICollection<AppointmentLog> AppointmentLogs { get; set; } = new List<AppointmentLog>();
        public virtual ICollection<SymptomLog> SymptomLogs { get; set; } = new List<SymptomLog>();
        public virtual ICollection<CarePlan> CarePlans { get; set; } = new List<CarePlan>();
    }
} 