using Career.Models.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Career.Models.FormModels;

public class ContactFormModel
{
    [Required]
    public UserContactEntityModel Contact { get; set; }

    public List<SelectListItem>? CountryDataset { get; set; }
    public List<SelectListItem>? CityDataset { get; set; }
}
