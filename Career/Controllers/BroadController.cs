using Career.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Career.Controllers;

public class BroadController : Controller
{
    private readonly ApplicationDbContext _context;

    public BroadController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCitiesOf(int? countryId)
    {
        if (countryId == null)
            return NotFound();

        var data = await _context.CityDataset.
                                  Where(city => city.CountryId == countryId).
                                  OrderBy(city => city.Title).
                                  Select(city => new { value = city.CityId, text = city.Title }).
                                  ToListAsync();

        return new JsonResult(data);
    }

    [HttpGet]
    public async Task<IActionResult> GetNumberOfApplicants(int? jobId)
    {
        if (jobId == null)
            return NotFound();

        var count = await _context.AppliedJobs.Where(a => a.JobId == jobId).CountAsync();

        return new JsonResult(new { Count = count });
    }
}
