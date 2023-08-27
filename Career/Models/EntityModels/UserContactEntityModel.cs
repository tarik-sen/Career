using Career.Models.Datasets;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Career.Models.EntityModels;

public class UserContactEntityModel
{
    [Key]
    public string UserId { get; set; } = null!;  // Foreign Key

    [Required]
    [PersonalData]
    [StringLength(128, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [Required]
    [PersonalData]
    [StringLength(128, MinimumLength = 3)]
    public string Surname { get; set; } = null!;

    [Column(TypeName = "image")]
    public byte[]? Image { get; set; }

    [Phone]
    [PersonalData]
    public string? Phone { get; set; }

    [Required]
    [PersonalData]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [PersonalData]
    [DataType(DataType.Date)]
    [DisplayName("Birth Day")]
    public DateTime BirthDay { get; set; }

    public int? CountryId { get; set; }  // Foreign Key

    public int? CityId { get; set; }  // Foreign Key

    [PersonalData]
    public string? LinkedInUrl { get; set; }

    [PersonalData]
    public string? GithubUrl { get; set; }

    [PersonalData]
    public string? PersonalUrl { get; set; }

    // Relations
    [PersonalData]
    [ForeignKey("CountryId")]
    public virtual CountryDatasetEntityModel? UserCountry { get; set; }

    [PersonalData]
    [ForeignKey("CityId")]
    public virtual CityDatasetEntityModel? UserCity { get; set; }
}
