using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum TaskType
    {
        Medication,
        Meal,
        Exercise,
        PersonalCare,
        Medical,
        Social,
        Appointment,
        Maintenance,
        Other
    }

    public enum TaskStatus
    {
        Scheduled,
        InProgress,
        Completed,
        Skipped,
        Cancelled,
        Overdue
    }

    public enum RecurrenceType
    {
        None,
        Daily,
        Weekly,
        Monthly,
        Custom
    }

    public class TaskSchedule
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
        
        public int? CarePlanId { get; set; }
        [ForeignKey("CarePlanId")]
        public virtual CarePlan? CarePlan { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public TaskType TaskType { get; set; }
        
        public TaskStatus Status { get; set; } = TaskStatus.Scheduled;
        
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        
        public DateTime ScheduledTime { get; set; }
        
        public DateTime? CompletedTime { get; set; }
        
        public int? DurationMinutes { get; set; }
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        [StringLength(500)]
        public string? Instructions { get; set; }
        
        [StringLength(500)]
        public string? Requirements { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        // Recurrence settings
        public bool IsRecurring { get; set; } = false;
        
        public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;
        
        public int? RecurrenceInterval { get; set; } // Every X days/weeks/months
        
        [StringLength(100)]
        public string? RecurrenceDays { get; set; } // For weekly recurrence (Mon,Tue,Wed...)
        
        [DataType(DataType.Date)]
        public DateTime? RecurrenceEndDate { get; set; }
        
        public int? MaxOccurrences { get; set; }
        
        // Reminder settings
        public bool HasReminder { get; set; } = false;
        
        public int? ReminderMinutesBefore { get; set; }
        
        public DateTime? LastReminderSent { get; set; }
        
        // Completion tracking
        public bool IsCompleted { get; set; } = false;
        
        [StringLength(500)]
        public string? CompletionNotes { get; set; }
        
        public int? CompletionRating { get; set; } // 1-10 scale
        
        [StringLength(500)]
        public string? Barriers { get; set; }
        
        [StringLength(500)]
        public string? Solutions { get; set; }
        
        // Related task information
        [StringLength(100)]
        public string? RelatedMedication { get; set; }
        
        [StringLength(100)]
        public string? RelatedAppointment { get; set; }
        
        [StringLength(100)]
        public string? RelatedActivity { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 