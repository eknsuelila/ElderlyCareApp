using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public class VitalSignsLog
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
        
        public DateTime MeasurementTime { get; set; }
        
        // Blood Pressure
        public int? SystolicPressure { get; set; } // mmHg
        public int? DiastolicPressure { get; set; } // mmHg
        
        // Heart Rate
        public int? HeartRate { get; set; } // beats per minute
        
        // Temperature
        [Range(35.0, 42.0)]
        public decimal? Temperature { get; set; } // Celsius
        
        // Weight
        [Range(20.0, 300.0)]
        public decimal? Weight { get; set; } // kg
        
        // Oxygen Saturation
        [Range(70, 100)]
        public int? OxygenSaturation { get; set; } // %
        
        // Respiratory Rate
        [Range(8, 40)]
        public int? RespiratoryRate { get; set; } // breaths per minute
        
        // Blood Sugar (for diabetics)
        [Range(20, 600)]
        public int? BloodSugar { get; set; } // mg/dL
        
        // Pain Level
        [Range(0, 10)]
        public int? PainLevel { get; set; } // 0-10 scale
        
        [StringLength(100)]
        public string? MeasurementLocation { get; set; }
        
        [StringLength(100)]
        public string? MeasurementMethod { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool IsNormal { get; set; } = true;
        
        [StringLength(500)]
        public string? Abnormalities { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 