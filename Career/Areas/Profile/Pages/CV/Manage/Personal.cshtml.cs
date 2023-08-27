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

public class PersonalModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public bool IsExists { get; set; } = false;

    [BindProperty]
    public UserPrivateEntityModel Personal { get; set; }

    public List<SelectListItem> NationalityDataset { get; set; }
    public List<SelectListItem> GenderOptions { get; set; }
    public List<SelectListItem> MilitaryServicesOptions { get; set; }
    public List<SelectListItem> HandicapOptions { get; set; }
    public List<SelectListItem> RetiredOptions { get; set; }

    public PersonalModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;

        NationalityDataset = _context.NationalityDataset.Select(n => new SelectListItem { Value = n.NationalityId.ToString(), Text = n.Title }).ToList();
        GenderOptions = EnumHelper.GetEnumSelectList<Gender>();
        MilitaryServicesOptions = EnumHelper.GetEnumSelectList<MilitaryServices>();
        HandicapOptions = EnumHelper.GetEnumSelectList<Handicap>();
        RetiredOptions = EnumHelper.GetEnumSelectList<Retired>();
    }

    public async Task<IActionResult> OnGet()
    {
        string? userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId) || _context.NationalityDataset is null || !_context.NationalityDataset.Any())
            return NotFound();

        await ConfigurePersonal(userId);

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid || Personal == null)
        {
            ModelStateHelper.LogModelValidationDrawbacks(ModelState);
            return Page();
        }

        if (await _context.PrivateData.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == Personal.UserId) != null)
            _context.PrivateData.Update(Personal);
        else
            _context.PrivateData.Add(Personal);

        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    public async Task ConfigurePersonal(string userId)
    {
        var personal = await _context.PrivateData.FirstOrDefaultAsync(p => p.UserId == userId);
        
        IsExists = personal != null;
        Personal = personal ?? new UserPrivateEntityModel { UserId = userId };
    }
}
