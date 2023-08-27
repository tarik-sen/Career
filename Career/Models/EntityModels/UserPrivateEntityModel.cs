using Career.Models.Datasets;
using Career.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Career.Models.EntityModels;

[Index("NationalityId", Name = "NationalityId")]
public class UserPrivateEntityModel
{
    [Key]
    public string UserId { get; set; }  // Foreign Key

    [PersonalData]
    [Column(TypeName = "money")]
    [DecimalRange(1_000, 100_000, DecimalRangeAttribute.Mode.Money)]
    public decimal ExpectedSalary { get; set; }

    public int? NationalityId { get; set; }  // Foreign Key

    [PersonalData]
    public Gender Gender { get; set; }

    [PersonalData]
    public MilitaryServices MilitaryServices { get; set; }

    [PersonalData]
    public Handicap Handicap { get; set; }

    [PersonalData]
    public Retired Retired { get; set; }

    // Relations
    [PersonalData]
    [ForeignKey("NationalityId")]
    public virtual NationalityDatasetEntityModel? UserNationality { get; set; }
}
