using Career.Models.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Career.Models.FormModels;

public class ExperiencesFormModel
{
    [Required]
    public UserExperienceEntityModel Experience { get; set; }

    public List<SelectListItem>? IndustriesDataset { get; set; }
    public List<SelectListItem>? SectorDataset { get; set; }
    public List<SelectListItem>? WorkType { get; set; }

}
