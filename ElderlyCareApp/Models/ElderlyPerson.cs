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
        
        [StringLength(100)]
        public string? PrimaryCarePhysician { get; set; }
        
        [StringLength(20)]
        public string? PhysicianPhone { get; set; }
        
        [StringLength(100)]
        public string? InsuranceProvider { get; set; }
        
        [StringLength(50)]
        public string? InsurancePolicyNumber { get; set; }
        
        [StringLength(500)]
        public string? Allergies { get; set; }
        
        [StringLength(500)]
        public string? MedicalConditions { get; set; }
        
        [StringLength(10)]
        public string? BloodType { get; set; }
        
        [StringLength(500)]
        public string? CarePreferences { get; set; }
        
        [StringLength(500)]
        public string? DietaryRestrictions { get; set; }
        
        [StringLength(500)]
        public string? MobilityAids { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
        public virtual ICollection<MedicationLog> MedicationLogs { get; set; } = new List<MedicationLog>();
        public virtual ICollection<MealLog> MealLogs { get; set; } = new List<MealLog>();
        public virtual ICollection<SocialInteractionLog> SocialInteractionLogs { get; set; } = new List<SocialInteractionLog>();
        public virtual ICollection<VitalSignsLog> VitalSignsLogs { get; set; } = new List<VitalSignsLog>();
        public virtual ICollection<AppointmentLog> AppointmentLogs { get; set; } = new List<AppointmentLog>();
        public virtual ICollection<SymptomLog> SymptomLogs { get; set; } = new List<SymptomLog>();
    }
} 