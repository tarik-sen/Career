using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Career.Models;
using Career.Models.Datasets;
using Career.Validations;
using Microsoft.EntityFrameworkCore;

namespace Career.Models.EntityModels;

[Index("UserId", Name = "UserId")]
public class UserExperienceEntityModel
{
    [Key]
    public int ExperienceId { get; set; }

    public string? UserId { get; set; }  // Foreign Key

    [Required]
    [StringLength(256)]
    public string CompanyName { get; set; } = null!;

    [Required]
    [StringLength(256)]
    public string Profession { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DateRange(nameof(StartDate))]
    public DateTime EndDate { get; set; }

    public int SectorId { get; set; }  // Foreign Key
    
    public int IndustryId { get; set; }  // Foreign Key

    public WorkType WorkType { get; set; }

    public string? JobDefinition { get; set; }

    // Relations
    [ForeignKey("SectorId")]
    public virtual SectorDatasetEntityModel? UserSector { get; set; }

    [ForeignKey("IndustryId")]
    public virtual IndustriesDatasetEntityModel? UserIndustry { get; set; }

}
