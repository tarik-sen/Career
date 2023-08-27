using Career.Models.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Career.Models.FormModels;

public class JobsFormModel
{
    [Required]
    public JobsEntityModel Job { get; set; }

    public List<SelectListItem>? IndustriesDataset { get; set; }
    public List<SelectListItem>? SectorDataset { get; set; }
    public List<SelectListItem>? CountryDataset { get; set; }
    public List<SelectListItem>? CityDataset { get; set; }
    public List<SelectListItem>? WorkType { get; set; }
}
