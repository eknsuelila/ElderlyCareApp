using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum CarePlanStatus
    {
        Draft,
        Active,
        OnHold,
        Completed,
        Cancelled
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class CarePlan
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
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        public CarePlanStatus Status { get; set; } = CarePlanStatus.Draft;
        
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? TargetDate { get; set; }
        
        [StringLength(500)]
        public string? Goals { get; set; }
        
        [StringLength(500)]
        public string? Objectives { get; set; }
        
        [StringLength(1000)]
        public string? Interventions { get; set; }
        
        [StringLength(500)]
        public string? SuccessCriteria { get; set; }
        
        [StringLength(500)]
        public string? RiskFactors { get; set; }
        
        [StringLength(500)]
        public string? Precautions { get; set; }
        
        [StringLength(500)]
        public string? Equipment { get; set; }
        
        [StringLength(500)]
        public string? Resources { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsRecurring { get; set; } = false;
        
        [StringLength(100)]
        public string? RecurrencePattern { get; set; }
        
        public int? ProgressPercentage { get; set; }
        
        [StringLength(500)]
        public string? ProgressNotes { get; set; }
        
        [StringLength(500)]
        public string? Barriers { get; set; }
        
        [StringLength(500)]
        public string? Solutions { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        // Navigation properties for related entities
        public virtual ICollection<TaskSchedule> TaskSchedules { get; set; } = new List<TaskSchedule>();
    }
} 