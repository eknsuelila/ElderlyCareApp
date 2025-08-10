using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum ActivityType
    {
        Walk,
        Exercise,
        Reading,
        Social,
        Rest,
        Other
    }

    public class ActivityLog
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ElderlyPersonId { get; set; }
        [ForeignKey("ElderlyPersonId")]
        public virtual ElderlyPerson? ElderlyPerson { get; set; }
        
        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        
        [Required]
        public ActivityType ActivityType { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public int? DurationMinutes { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 