using Career.Models.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Career.Models.FormModels;

public class EducationFormModel
{
    [Required]
    public UserEducationEntityModel Education { get; set; }

    public List<SelectListItem>? EducationLevels { get; set; }
    public List<SelectListItem>? UniversityDataset { get; set; }
    public List<SelectListItem>? StudyFieldDataset { get; set; }
}
