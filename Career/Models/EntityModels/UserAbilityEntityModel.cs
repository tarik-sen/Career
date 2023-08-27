using Career.Models.Datasets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Career.Models.EntityModels;

public class UserAbilityEntityModel
{
    [Key]
    public int AbilityId { get; set; }

    [Required]
    public string? UserId { get; set; }

    [Required]
    public int SkillId { get; set; }  // Foreign Key

    // Relations
    [ForeignKey("SkillId")]
    public virtual SkillDatasetEntityModel? UserSkill { get; set; }
}
