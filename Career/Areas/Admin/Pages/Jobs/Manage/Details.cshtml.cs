using Career.Data;
using Career.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Admin.Pages.Jobs.Manage;

public class DetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public JobsEntityModel? JobModel { get; set; }

    public DetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id == null)
            return NotFound();

        JobModel = await _context.Jobs
            .Include(j => j.UserIndustry)
            .Include(j => j.UserSector)
            .Include(j => j.UserCountry)
            .Include(j => j.UserCity)
            .FirstOrDefaultAsync(j => j.JobId == id);

        return Page();
    }
}
