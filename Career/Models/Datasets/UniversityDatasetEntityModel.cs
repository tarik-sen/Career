using System.ComponentModel.DataAnnotations;

namespace Career.Models.Datasets;

public class UniversityDatasetEntityModel : IDatasetBaseEntityModel
{
    [Key]
    public int UniversityId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}
