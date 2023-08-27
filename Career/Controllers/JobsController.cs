using Career.Authorization;
using Career.Data;
using Career.Models;
using Career.Models.EntityModels;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Career.Controllers;

[Authorize(Policy = "RequireUserOrAnonymous")]
public class JobsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public JobsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? jSearch, string? pSearch, WorkType? workType, int? sectorId, int? industryId, int? countryId, int? cityId)
    {
        if (_context.Jobs is null)
            return NotFound("Jobs table not found");

        IQueryable<JobsEntityModel> jobs = _context.Jobs;
        List<SelectListItem>? cityDataset = default;

        if (!jSearch.IsNullOrEmpty())
            jobs = jobs.Where(j => j.Title.Contains(jSearch!) || j.CompanyName.Contains(jSearch!));

        if (!pSearch.IsNullOrEmpty())
            jobs = jobs.Where(j => j.Position.Contains(pSearch!));

        if (workType is not null)
            jobs = jobs.Where(j => j.WorkType == workType);

        if (sectorId is not null)
            jobs = jobs.Where(j => j.SectorId == sectorId);

        if (industryId is not null)
            jobs = jobs.Where(j => j.IndustryId == industryId);

        if (countryId is not null)
        {
            jobs = jobs.Where(j => j.CountryId == countryId);
            cityDataset = await _context.CityDataset
                .AsNoTracking()
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.Title)
                .Select(c => new SelectListItem { Value = c.CityId.ToString(), Text = c.Title })
                .ToListAsync();
        }

        if (cityId is not null)
            jobs = jobs.Where(j => j.CityId == cityId);

        JobsFilterModel model = new(_context)
        {
            JSearch = jSearch,
            PSearch = pSearch,
            WorkType = workType,
            SectorId = sectorId,
            IndustryId = industryId,
            CountryId = countryId,
            CityId = cityId,

            CityDataset = cityDataset,
            Jobs = await jobs.ToListAsync()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Details(int? id)
    {
        if (id is null)
            return NotFound();

        JobsEntityModel model = _context.Jobs
            .Include(j => j.UserCountry)
            .Include(j => j.UserCity)
            .Include(j => j.UserIndustry)
            .Include(j => j.UserSector)
            .First(j => j.JobId == id);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Details(int jobId, string? coverLetter)
    {
        string? userId = _userManager.GetUserId(User);
        var user = await _userManager.GetUserAsync(User);

        if (string.IsNullOrEmpty(userId) || user == null)
            return NotFound();

        if (!await _userManager.IsInRoleAsync(user, Constants.UserRole))
            return Challenge();

        var prevApplication = await _context.AppliedJobs
            .Where(j => j.UserId == userId && j.JobId == jobId)
            .OrderByDescending(j => j.ApplicationDate)
            .FirstOrDefaultAsync();

        if (prevApplication?.ApplicationDate.AddDays(30) > DateTime.Now)
            return PartialView("_StatusMsg", new StatusMsgFormModel() 
            { 
                Status = "danger", 
                Message = "Your job application could NOT be delivered. At least one month must pass after your last application."
                //Message = "You need to log in before to apply any job."
            });

        UserAppliedJobsEntityModel appliedJob = new()
        {
            UserId = userId,
            JobId = jobId,
            CoverLetter = coverLetter,
            ApplicationDate = DateTime.Now,
            ApplicationStatus = ApplicationStatus.Submitted
        };

        _context.AppliedJobs.Add(appliedJob);
        _context.SaveChanges();

        return PartialView("_StatusMsg", new StatusMsgFormModel() 
        { 
            Status = "success", 
            Message = "Your job application is delivered successfully, any changes will be notified. Check your meesages frequently!"
        });
    }

    [HttpGet]
    public async Task<IActionResult> CitiesOf(int? countryId)
    {
        var data = await _context.CityDataset.
                                  Where(city => city.CountryId == countryId).
                                  OrderBy(city => city.Title).
                                  Select(city => new { value = city.CityId, text = city.Title }).
                                  ToListAsync();

        return new JsonResult(data);
    }

    [HttpGet]
    public IActionResult NumberOfApplicants(int jobId)
    {
        int count = _context.AppliedJobs.Where(a => a.JobId == jobId).Count();

        return new JsonResult(new { Count = count });
    }

}
