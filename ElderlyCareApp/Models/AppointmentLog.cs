using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum AppointmentType
    {
        DoctorVisit,
        SpecialistConsultation,
        TherapySession,
        DentalAppointment,
        VisionAppointment,
        LabTest,
        Imaging,
        Surgery,
        FollowUp,
        Emergency,
        Other
    }

    public enum AppointmentStatus
    {
        Scheduled,
        Confirmed,
        InProgress,
        Completed,
        Cancelled,
        NoShow,
        Rescheduled
    }

    public class AppointmentLog
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
        public AppointmentType AppointmentType { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? ProviderName { get; set; }
        
        [StringLength(100)]
        public string? ProviderSpecialty { get; set; }
        
        [StringLength(200)]
        public string? Location { get; set; }
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        public DateTime ScheduledDateTime { get; set; }
        
        public DateTime? ActualDateTime { get; set; }
        
        public int? DurationMinutes { get; set; }
        
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        
        [StringLength(500)]
        public string? Reason { get; set; }
        
        [StringLength(500)]
        public string? Symptoms { get; set; }
        
        [StringLength(500)]
        public string? Diagnosis { get; set; }
        
        [StringLength(500)]
        public string? Treatment { get; set; }
        
        [StringLength(500)]
        public string? Prescriptions { get; set; }
        
        [StringLength(500)]
        public string? Recommendations { get; set; }
        
        [StringLength(500)]
        public string? FollowUpInstructions { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FollowUpDate { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool RequiresTransportation { get; set; } = false;
        
        [StringLength(500)]
        public string? TransportationNotes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 