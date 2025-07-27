using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum InteractionType
    {
        PhoneCall,
        VideoCall,
        InPersonVisit,
        GroupActivity,
        FamilyGathering,
        CommunityEvent,
        ReligiousService,
        Other
    }

    public enum InteractionQuality
    {
        Poor,
        Fair,
        Good,
        Excellent
    }

    public class SocialInteractionLog
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
        public InteractionType InteractionType { get; set; }
        
        [StringLength(200)]
        public string? InteractionTitle { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? Participants { get; set; }
        
        [StringLength(100)]
        public string? Relationship { get; set; } // Family, Friend, Neighbor, etc.
        
        public DateTime StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }
        
        public int? DurationMinutes { get; set; }
        
        [StringLength(100)]
        public string? Location { get; set; }
        
        public InteractionQuality Quality { get; set; } = InteractionQuality.Good;
        
        [Range(1, 10)]
        public int? MoodBefore { get; set; } // 1-10 scale
        
        [Range(1, 10)]
        public int? MoodAfter { get; set; } // 1-10 scale
        
        [Range(1, 10)]
        public int? EngagementLevel { get; set; } // 1-10 scale
        
        [StringLength(500)]
        public string? TopicsDiscussed { get; set; }
        
        [StringLength(500)]
        public string? Activities { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsCompleted { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 