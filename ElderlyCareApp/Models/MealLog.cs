using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum MealType
    {
        Breakfast,
        Lunch,
        Dinner,
        Snack
    }

    public class MealLog
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
        public MealType MealType { get; set; }
        
        [StringLength(200)]
        public string? MealName { get; set; }
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public DateTime MealTime { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 