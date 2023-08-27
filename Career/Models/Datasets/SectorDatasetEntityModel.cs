using System.ComponentModel.DataAnnotations;

namespace Career.Models.Datasets;

public class SectorDatasetEntityModel : IDatasetBaseEntityModel
{
    [Key]
    public int SectorId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}
