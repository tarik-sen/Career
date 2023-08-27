using Career.Data;
using Career.Models.EntityModels;
using Career.Models;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Admin.Pages.Jobs.Manage;

public class UpdateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    [BindProperty]
    public JobsFormModel JobForm { get; set; }


    public UpdateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id == null)
            return NotFound();

        var job = await _context.Jobs.FirstOrDefaultAsync(j => j.JobId == id);

        if (job == null)
            return NotFound();

        JobForm = await BindDatasets(job);

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            JobForm = await BindDatasets(JobForm.Job);

            return Page();
        }

        _context.Jobs.Update(JobForm.Job);
        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }

    private async Task<JobsFormModel> BindDatasets(JobsEntityModel job)
    {
        var jobForm = new JobsFormModel
        {
            Job = job,
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
