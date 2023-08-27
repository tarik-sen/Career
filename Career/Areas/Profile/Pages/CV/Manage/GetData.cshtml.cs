using Career.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Profile.Pages.CV.Manage;

public class GetDataModel : PageModel
{
    public ApplicationDbContext _context { get; set; }

    public GetDataModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return NotFound("Page not found");
    }

    public async Task<IActionResult> OnPostCityOf(int countryId)
    {
        var data = await _context.CityDataset.
                                  Where(city => city.CountryId == countryId).
                                  OrderBy(city => city.Title).
                                  Select(city => new { value = city.CityId, text = city.Title }).
                                  ToListAsync();

        return new JsonResult(data);
    }
}
