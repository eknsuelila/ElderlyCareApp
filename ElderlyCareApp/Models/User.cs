using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum UserRole
    {
        Family,
        Caregiver
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        
        [Required]
        public UserRole Role { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime? LastLoginAt { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class CaregiverAssignment
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CaregiverId { get; set; }
        
        [Required]
        public int ElderlyPersonId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        [ForeignKey("CaregiverId")]
        public virtual User Caregiver { get; set; } = null!;
        
        [ForeignKey("ElderlyPersonId")]
        public virtual ElderlyPerson ElderlyPerson { get; set; } = null!;
    }
} 