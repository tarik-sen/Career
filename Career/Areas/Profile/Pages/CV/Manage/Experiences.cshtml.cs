using Career.Data;
using Career.DebugHelper;
using Career.Models;
using Career.Models.EntityModels;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Profile.Pages.CV.Manage;

public class ExperiencesModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public List<SelectListItem> SectorDataset { get; set; }
    public List<SelectListItem> IndustriesDataset { get; set; }
    public List<SelectListItem> WorkType { get; set; }

    public List<ExperiencesFormModel> ExperienceForms { get; set; }

    [BindProperty]
    public ExperiencesFormModel? ActiveExperienceForm { get; set; }


    public ExperiencesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;

        SectorDataset = _context.SectorDataset.Select(s => new SelectListItem { Value = s.SectorId.ToString(), Text = s.Title }).ToList();
        IndustriesDataset = _context.IndustryDataset.Select(s => new SelectListItem { Value = s.IndustryId.ToString(), Text = s.Title }).ToList();
        WorkType = EnumHelper.GetEnumSelectList<WorkType>();

    }

    public async Task<IActionResult> OnGet()
    {
        string? userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId))
            return NotFound();

        await ConfigureExperienceForms(userId);

        return Page();
    }

    public IActionResult OnPost()
    {
        ActiveExperienceForm!.SectorDataset = SectorDataset;
        ActiveExperienceForm!.IndustriesDataset = IndustriesDataset;
        ActiveExperienceForm!.WorkType = WorkType;

        if (!ModelState.IsValid)
        {
            ModelStateHelper.LogModelValidationDrawbacks(ModelState);

            return Partial("_ExperienceForm", ActiveExperienceForm);
        }

        if (ActiveExperienceForm!.Experience.ExperienceId == default)
            _context.Experiences.Add(ActiveExperienceForm.Experience);
        else
            _context.Experiences.Update(ActiveExperienceForm.Experience);

        _context.SaveChanges();

        return Partial("_ExperienceForm", ActiveExperienceForm);
    }

    public IActionResult OnPostDeleteExperience(int experienceId)
    {
        var experience = _context.Experiences.First(e => e.ExperienceId == experienceId)!;

        _context.Experiences.Remove(experience);
        _context.SaveChanges();

        return new JsonResult(new { success = true });
    }

    public IActionResult OnGetNewExperience()
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        ExperiencesFormModel experienceForm = new()
        {
            Experience = new() { UserId = userId },
            IndustriesDataset = IndustriesDataset,
            SectorDataset = SectorDataset,
            WorkType = WorkType
        };

        return Partial("_ExperienceForm", experienceForm);
    }

    public IActionResult OnGetNthExperience(int experienceId)
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        var experience = _context.Experiences.AsNoTracking().First(e => e.ExperienceId == experienceId);

        ExperiencesFormModel experienceForm = new()
        {
            Experience = experience,
            IndustriesDataset = IndustriesDataset,
            SectorDataset = SectorDataset,
            WorkType = WorkType
        };

        return Partial("_ExperienceForm", experienceForm);
    }

    public async Task ConfigureExperienceForms(string userId)
    {
        ExperienceForms = await _context.Experiences.AsNoTracking()
            .OrderByDescending(e => e.ExperienceId)
            .Where(e => e.UserId == userId)
            .Select(e => new ExperiencesFormModel
            {
                Experience = e,
                IndustriesDataset = IndustriesDataset,
                SectorDataset = SectorDataset,
                WorkType = WorkType
            })
            .ToListAsync();
    }
}
