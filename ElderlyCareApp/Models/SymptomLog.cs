using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElderlyCareApp.Models
{
    public enum SymptomSeverity
    {
        Mild,
        Moderate,
        Severe,
        Critical
    }

    public enum MoodLevel
    {
        VeryPoor,
        Poor,
        Fair,
        Good,
        Excellent
    }

    public class SymptomLog
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
        
        public DateTime LogTime { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Symptom { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        public SymptomSeverity Severity { get; set; } = SymptomSeverity.Mild;
        
        [Range(0, 10)]
        public int? PainLevel { get; set; } // 0-10 scale
        
        [StringLength(100)]
        public string? Location { get; set; } // Where the symptom is located
        
        [StringLength(500)]
        public string? Triggers { get; set; }
        
        [StringLength(500)]
        public string? ReliefMethods { get; set; }
        
        public bool IsOngoing { get; set; } = true;
        
        public DateTime? ResolvedTime { get; set; }
        
        // Mood and Mental Health
        public MoodLevel Mood { get; set; } = MoodLevel.Good;
        
        [Range(1, 10)]
        public int? AnxietyLevel { get; set; } // 1-10 scale
        
        [Range(1, 10)]
        public int? DepressionLevel { get; set; } // 1-10 scale
        
        [Range(1, 10)]
        public int? SleepQuality { get; set; } // 1-10 scale
        
        public int? HoursSlept { get; set; }
        
        // Cognitive Symptoms
        [StringLength(500)]
        public string? CognitiveSymptoms { get; set; }
        
        [StringLength(500)]
        public string? MemoryIssues { get; set; }
        
        [StringLength(500)]
        public string? Confusion { get; set; }
        
        // Physical Symptoms
        [StringLength(500)]
        public string? PhysicalSymptoms { get; set; }
        
        [StringLength(500)]
        public string? MobilityIssues { get; set; }
        
        [StringLength(500)]
        public string? BalanceIssues { get; set; }
        
        [StringLength(500)]
        public string? DigestiveIssues { get; set; }
        
        [StringLength(500)]
        public string? RespiratoryIssues { get; set; }
        
        [StringLength(500)]
        public string? CardiovascularIssues { get; set; }
        
        // Medication Related
        [StringLength(500)]
        public string? MedicationSideEffects { get; set; }
        
        [StringLength(500)]
        public string? MedicationCompliance { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public bool RequiresMedicalAttention { get; set; } = false;
        
        [StringLength(500)]
        public string? MedicalActionTaken { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
} 