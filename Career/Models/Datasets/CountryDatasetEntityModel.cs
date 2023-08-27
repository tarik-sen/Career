using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Career.Models.Datasets;

public class CountryDatasetEntityModel : IDatasetBaseEntityModel
{
    [Key]
    public int CountryId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public virtual ICollection<CityDatasetEntityModel> Cities { get; } = new List<CityDatasetEntityModel>();
}
