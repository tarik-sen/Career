using Career.Data;
using Career.Models;
using Career.Models.EntityModels;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Career.Areas.Admin.Pages.Jobs.Manage;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public JobsFormModel JobForm { get; set; }

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet() 
    {
        JobForm = await CreateJobForm();

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            JobForm = await CreateJobForm(JobForm.Job);

            return Page();
        }

        await _context.Jobs.AddAsync(JobForm.Job);
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    private async Task<JobsFormModel> CreateJobForm(JobsEntityModel? job = null)
    {
        var jobForm = new JobsFormModel
        {
            Job = job ?? new JobsEntityModel(),
            IndustriesDataset = await _context.IndustryDataset.Select(i => new SelectListItem { Value = i.IndustryId.ToString(), Text = i.Title }).ToListAsync(),
            SectorDataset = await _context.SectorDataset.Select(s => new SelectListItem { Value = s.SectorId.ToString(), Text = s.Title }).ToListAsync(),
            CountryDataset = await _context.CountryDataset.Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Title }).ToListAsync(),
            WorkType = EnumHelper.GetEnumSelectList<WorkType>()
        };

        if (jobForm.Job.CountryId != null)
        {
            jobForm.CityDataset = await _context.CityDataset
                .Where(c => c.CountryId == jobForm.Job.CountryId)
                .Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Title })
                .ToListAsync();
        }

        return jobForm;
    }

}
