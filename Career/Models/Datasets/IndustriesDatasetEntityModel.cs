using System.ComponentModel.DataAnnotations;

namespace Career.Models.Datasets;

public class IndustriesDatasetEntityModel: IDatasetBaseEntityModel
{
    [Key]
    public int IndustryId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}
