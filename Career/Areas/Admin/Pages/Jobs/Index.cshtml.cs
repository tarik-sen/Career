using Career.Data;
using Career.Models.EntityModels;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Admin.Pages.Jobs;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public List<JobsEntityModel>? Jobs { get; set; }

    public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet()
    {
        if (_context.Jobs is null)
            return NotFound("Jobs table not found.");

        Jobs = await _context.Jobs.ToListAsync();

        return Page();
    }

    public async Task<ActionResult> OnGetGenderChartData(int? jobId)
    {
        IQueryable<UserAppliedJobsEntityModel> query = _context.AppliedJobs;

        if (jobId != null)
            query = query.Where(a => a.JobId == jobId);

        var data = await query
            .Join(
                _context.PrivateData,
                job => job.UserId,
                personal => personal.UserId,
                (job, personal) => new
                {
                    Gender = personal.Gender
                }
            )
            .GroupBy(r => r.Gender)
            .Select(group => new ChartModel { Name = group.Key.ToString(), Count = group.Count() })
            .ToListAsync();

        var jsonData = ConvertToJson(data, "Gender");

        return new JsonResult(jsonData);
    }

    public async Task<IActionResult> OnGetCountryChartData(int? jobId)
    {
        IQueryable<UserAppliedJobsEntityModel> query = _context.AppliedJobs;

        if (jobId != null)
            query = query.Where(a => a.JobId == jobId);

        var data = await query
            .Join(
                _context.Contacts.Include(c => c.UserCountry),
                job => job.UserId,
                contact => contact.UserId,
                (job, contact) => new { Country = contact.UserCountry!.Title }
            )
            .GroupBy(r => r.Country)
            .Select(group => new ChartModel { Name = group.Key, Count = group.Count() })
            .ToListAsync();

        var jsonData = ConvertToJson(data, "Country");

        return new JsonResult(jsonData);
    }

    public async Task<IActionResult> OnGetStudyFieldChartData(int? jobId)
    {
        IQueryable<UserAppliedJobsEntityModel> query = _context.AppliedJobs;

        if (jobId != null)
            query = query.Where(a => a.JobId == jobId);

        var data = await query
            .Join(
                _context.Educations.Include(e => e.UserStudyField),
                job => job.UserId,
                education => education.UserId,
                (job, education) => new { StudyField = education.UserStudyField!.Title }
            )
            .GroupBy(r => r.StudyField)
            .Select(group => new ChartModel { Name = group.Key, Count = group.Count() })
            .ToListAsync();

        var jsonData = ConvertToJson(data, "Study Field");

        return new JsonResult(jsonData);
    }

    public async Task<IActionResult> OnGetExperienceChartData(int? jobId)
    {
        List<ChartModel> data = new()
        {
            new ChartModel { Name = "0-5" },
            new ChartModel { Name = "5-10" },
            new ChartModel { Name = "10-?" }
        };

        IQueryable<UserAppliedJobsEntityModel> query = _context.AppliedJobs;
        if (jobId != null)
            query = query.Where(a => a.JobId == jobId);

        var userIdList = await query.Select(j => j.UserId).ToListAsync();

        foreach (var userId in userIdList)
        {
            var timeSpanList = await _context.Experiences.Where(e => e.UserId == userId).Select(e => e.EndDate - e.StartDate).ToListAsync();

            int i = (int) timeSpanList.Select(t => t.TotalDays / 365.25).Sum() % 5;

            data[(i >= 2) ? 2 : i].Count++;
        }

        var jsonData = ConvertToJson(data, "Experience Amount");

        return new JsonResult(jsonData);
    }

    private class ChartModel
    {
        public string Name { get; set; } = String.Empty;
        public int Count { get; set; }
    }

    private static string ConvertToJson(List<ChartModel> chartData, string columnName)
    {
        var jsonData = chartData.ToGoogleDataTable()
            .NewColumn(new Column(ColumnType.String, columnName), d => d.Name)
            .NewColumn(new Column(ColumnType.Number, "Count"), d => d.Count)
            .Build()
            .GetJson();

        return jsonData;
    }
}
