using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum NoteType
    {
        General,
        Health,
        Behavior,
        Mood,
        Important
    }

    public class CareNote
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
        public NoteType NoteType { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 