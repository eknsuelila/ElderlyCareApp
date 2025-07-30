using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public class MedicationLog
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
        public string MedicationName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Dosage { get; set; }
        
        public bool Taken { get; set; } = false;
        
        public DateTime Timestamp { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 