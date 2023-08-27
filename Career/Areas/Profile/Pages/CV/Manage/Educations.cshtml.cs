using Career.Data;
using Career.Debug;
using Career.Models;
using Career.Models.EntityModels;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Profile.Pages.CV.Manage;

public class EducationsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public List<SelectListItem> UniversityDataset { get; set; }
    public List<SelectListItem> StudyFieldDataset { get; set; }
    public List<SelectListItem> EducationLevels { get; set; }

    public List<EducationFormModel> EducationForms { get; set; }

    [BindProperty]
    public EducationFormModel? ActiveEducationForm { get; set; }


    public EducationsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;

        UniversityDataset = _context.UniversityDataset.Select(u => new SelectListItem { Value = u.UniversityId.ToString(), Text = u.Title }).ToList();
        StudyFieldDataset = _context.StudyFieldDataset.Select(s => new SelectListItem { Value = s.StudyFieldId.ToString(), Text = s.Title }).ToList();
        EducationLevels = EnumHelper.GetEnumSelectList<EducationLevel>();

    }

    public async Task<IActionResult> OnGet()
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        await ConfigureEducationForms(userId);

        return Page();
    }

    public IActionResult OnGetNewEducation()
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        EducationFormModel educationForm = new()
        {
            Education = new() { UserId = userId },
            EducationLevels = EducationLevels,
            UniversityDataset = UniversityDataset,
            StudyFieldDataset = StudyFieldDataset
        };

        return Partial("_EducationForm", educationForm);
    }


    public IActionResult OnGetEducationWithId(int educationId)
    {
        UserEducationEntityModel? education = _context.Educations.FirstOrDefault(e => e.EducationId == educationId);

        if (education is null)
            return NotFound();

        EducationFormModel educationForm = new()
        {
            Education = education,
            EducationLevels = EducationLevels,
            UniversityDataset = UniversityDataset,
            StudyFieldDataset = StudyFieldDataset
        };

        return Partial("_EducationForm", educationForm);
    }

    public IActionResult OnPost()
    {
        ActiveEducationForm!.UniversityDataset = UniversityDataset;
        ActiveEducationForm!.StudyFieldDataset = StudyFieldDataset;
        ActiveEducationForm!.EducationLevels = EducationLevels;

        if (!ModelState.IsValid)
        {
            ModelStateHelper.LogModelValidationDrawbacks(ModelState);

            return Partial("_EducationForm", ActiveEducationForm);
        }

        if (ActiveEducationForm!.Education.EducationId == default)
            _context.Educations.Add(ActiveEducationForm.Education);
        else
            _context.Educations.Update(ActiveEducationForm.Education);

        _context.SaveChanges();

        return Partial("_EducationForm", ActiveEducationForm);
    }

    public IActionResult OnPostDeleteEducation(int educationId)
    {
        UserEducationEntityModel? education = _context.Educations.FirstOrDefault(e => e.EducationId == educationId);

        if (education is null)
            return NotFound();

        _context.Educations.Remove(education);
        _context.SaveChanges();

        return new JsonResult(new { success = true });
    }

    public async Task ConfigureEducationForms(string userId)
    {
        EducationForms = await _context.Educations.AsNoTracking()
            .OrderByDescending(e => e.EducationId)
            .Where(e => e.UserId == userId)
            .Select(e => new EducationFormModel
            {
                Education = e,
                EducationLevels = EducationLevels,
                UniversityDataset = UniversityDataset,
                StudyFieldDataset = StudyFieldDataset
            })
            .ToListAsync();
    }
}
