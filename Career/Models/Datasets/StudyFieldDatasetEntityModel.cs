using System.ComponentModel.DataAnnotations;

namespace Career.Models.Datasets;

public class StudyFieldDatasetEntityModel : IDatasetBaseEntityModel
{
    [Key]
    public int StudyFieldId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}
