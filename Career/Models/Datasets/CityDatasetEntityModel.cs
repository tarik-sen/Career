using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Career.Models.Datasets;

[Index("CountryId", Name = "CountryId")]
public class CityDatasetEntityModel: IDatasetBaseEntityModel
{
    [Key]
    public int CityId { get; set; }

    public int CountryId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [ForeignKey("CountryId")]
    public virtual CountryDatasetEntityModel? CountryDatasetEntityModel { get; set; }
}
