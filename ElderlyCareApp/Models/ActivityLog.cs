using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum ActivityCategory
    {
        Exercise,
        Social,
        Cognitive,
        PersonalCare,
        Recreation,
        Medical,
        Other
    }

    public class ActivityLog
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
        public string ActivityType { get; set; } = string.Empty;
        
        public ActivityCategory Category { get; set; } = ActivityCategory.Other;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public int? DurationMinutes { get; set; }
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        [Range(1, 10)]
        public int? EnergyLevel { get; set; } // 1-10 scale
        
        [Range(1, 10)]
        public int? EnjoymentLevel { get; set; } // 1-10 scale
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsCompleted { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 