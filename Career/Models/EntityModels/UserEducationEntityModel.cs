using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Career.Models;
using Career.Models.Datasets;
using Career.Validations;
using Microsoft.EntityFrameworkCore;

namespace Career.Models.EntityModels;

[Index("UserId", Name = "UserId")]
public class UserEducationEntityModel
{
    [Key]
    public int EducationId { get; set; }

    [Required]
    public string? UserId { get; set; }  // Foreign Key

    [Required]
    public EducationLevel? Level { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DateRange(nameof(StartDate))]
    public DateTime EndDate { get; set; }

    [Required]
    public int? UniversityId { get; set; }  // Foreign Key

    [Required]
    public int? StudyField { get; set; }  // Foreign Key

    [DecimalRange(0, 100)]
    public decimal MaxDegree { get; set; }

    [DecimalRange(0, 100)]
    public decimal Degree { get; set; }

    public string? Description { get; set; }

    // Relations
    [ForeignKey("UniversityId")]
    public virtual UniversityDatasetEntityModel? UserUniversity { get; set; }

    [ForeignKey("StudyField")]
    public virtual StudyFieldDatasetEntityModel? UserStudyField { get; set; }

}
