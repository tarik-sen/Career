using System.ComponentModel.DataAnnotations;

namespace Career.Models.Datasets;

public class NationalityDatasetEntityModel: IDatasetBaseEntityModel
{
    [Key]
    public int NationalityId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
}
