using Career.Models.Datasets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Career.Models.EntityModels;

public class JobsEntityModel
{
    [Key]
    public int JobId { get; set; }

    [Required]
    [StringLength(128)]
    public string? Title { get; set; }

    [Required]
    public string? Description { get; set; }

    [Required]
    public string? Summary { get; set; }

    [Required]
    [StringLength(128)]
    public string? CompanyName { get; set; }

    [Required]
    [StringLength(128)]
    public string? Position { get; set; }

    public string? Address { get; set; }

    [Required]
    public WorkType WorkType { get; set; }

    [Required]
    public int? CountryId { get; set; }  // Foreign Key

    [Required]
    public int? CityId { get; set; }  // Foreign Key

    [Required]
    public int SectorId { get; set; }  // Foreign Key

    [Required]
    public int IndustryId { get; set; }  // Foreign Key

    // Relations
    [ForeignKey("SectorId")]
    public virtual SectorDatasetEntityModel? UserSector { get; set; }

    [ForeignKey("IndustryId")]
    public virtual IndustriesDatasetEntityModel? UserIndustry { get; set; }

    [ForeignKey("CountryId")]
    public virtual CountryDatasetEntityModel? UserCountry { get; set; }

    [ForeignKey("CityId")]
    public virtual CityDatasetEntityModel? UserCity { get; set; }
}
