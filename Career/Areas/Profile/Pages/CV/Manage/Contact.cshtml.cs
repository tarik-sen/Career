using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Career.Models.EntityModels;
using Career.Data;
using Microsoft.AspNetCore.Identity;
using Career.Models.Datasets;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Career.Models.FormModels;
using Career.DebugHelper;

namespace Career.Areas.Profile.Pages.CV.Manage;

public class ContactModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public bool IsExist { get; set; } = false;

    [BindProperty]
    public UserContactEntityModel Contact { get; set; }

    public List<SelectListItem> CountryDataset { get; set; }
    public List<SelectListItem> CityDataset { get; set; }

    public ContactModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;

        CountryDataset = _context.CountryDataset.OrderBy(c => c.Title).Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Title }).ToList();
    }

    public async Task<IActionResult> OnGet()
    {
        string? userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId) || _context.CountryDataset is null || !_context.CountryDataset.Any())
            return NotFound();

        await ConfigureContact(userId);

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid || Contact == null){
            ModelStateHelper.LogModelValidationDrawbacks(ModelState);

            if (Contact != null && Contact.CityId != null)
            {
                CityDatasetEntityModel city = await _context.CityDataset.FirstOrDefaultAsync(c => c.CityId == Contact.CityId) ?? throw new Exception("Invalid City Id");
                CityDataset = new() { new SelectListItem { Value = city.CityId.ToString(), Text = city.Title } };
            }

            return Page();
        }

        if (await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == Contact.UserId) != null)
            _context.Contacts.Update(Contact);
        else
            _context.Contacts.Add(Contact);

        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    public async Task ConfigureContact(string userId)
    {
        var contact = await _context.Contacts.Include(c => c.UserCity).FirstOrDefaultAsync(c => c.UserId == userId);

        IsExist = contact != null;
        Contact = contact ?? new UserContactEntityModel() { UserId = userId };

        if (Contact.UserCity != null)
        {
            CityDataset = new() { new SelectListItem { Value = Contact.UserCity.CityId.ToString(), Text = Contact.UserCity.Title } };
        }
    }
}
