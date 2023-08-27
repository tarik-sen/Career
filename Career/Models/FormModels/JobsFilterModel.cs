using Career.Data;
using Career.Models.EntityModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Career.Models.FormModels;

public class JobsFilterModel
{
    private readonly ApplicationDbContext _context;

    public List<SelectListItem> CountryDataset { get; set; }
    public List<SelectListItem> IndustryDataset { get; set; }
    public List<SelectListItem> SectorDataset { get; set; }
    public List<SelectListItem> WorkTypeList { get; set; }

    public List<SelectListItem>? CityDataset { get; set; }

    public List<JobsEntityModel>? Jobs;

    public string? JSearch { get; set; }
    public string? PSearch { get; set; }
    public WorkType? WorkType { get; set; }
    public int? SectorId { get; set; }
    public int? IndustryId { get; set; }
    public int? CountryId { get; set; }
    public int? CityId { get; set; }


    public JobsFilterModel(ApplicationDbContext context)
    {
        _context = context;

        CountryDataset = _context.CountryDataset
            .AsNoTracking()
            .OrderBy(c => c.Title)
            .Select(c => new SelectListItem { Value = c.CountryId.ToString(), Text = c.Title })
            .ToList();

        IndustryDataset = _context.IndustryDataset
            .AsNoTracking()
            .OrderBy(i => i.Title)
            .Select(i => new SelectListItem { Value = i.IndustryId.ToString(), Text = i.Title })
            .ToList();

        SectorDataset = _context.SectorDataset
            .AsNoTracking()
            .OrderBy(s => s.Title)
            .Select(s => new SelectListItem { Value = s.SectorId.ToString(), Text = s.Title })
            .ToList();

        WorkTypeList = EnumHelper.GetEnumSelectList<WorkType>();
    }
}
