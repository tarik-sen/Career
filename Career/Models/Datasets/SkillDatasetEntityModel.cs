using System.ComponentModel.DataAnnotations;

namespace Career.Models.Datasets;

public class SkillDatasetEntityModel : IDatasetBaseEntityModel
{
    [Key]
    public int SkillId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}
