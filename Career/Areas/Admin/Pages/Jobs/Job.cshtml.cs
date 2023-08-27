using Career.Data;
using Career.Models;
using Career.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Collections.Specialized.BitVector32;

namespace Career.Areas.Admin.Pages.Jobs;

public class JobModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public List<ApplicantSummary> DataTable { get; set; }

    public List<SelectListItem> SelectTwo { get; set; }
    
    public int JobId { get; set; }

    [BindProperty]
    public List<int> ApplicantIds { get; set; }


    public class ApplicantSummary
    {
        [Required]
        public int AppliedJobId { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ApplicationDate { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [Display(Name = "Exp. Salary")]
        [DecimalRange(1_000, 100_000, DecimalRangeAttribute.Mode.Money)]
        public decimal ExpectedSalary { get; set; }
        
        [Required]
        public Gender Gender { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }
    }

    public JobModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id == null)
            return NotFound();

        JobId = (int)id;

        DataTable = await _context.AppliedJobs
            .Where(a => a.JobId == id)
            .Join(
                _context.Contacts.Include(c => c.UserCountry).Include(c => c.UserCity),
                a => a.UserId,
                c => c.UserId,
                (a, c) => new
                {
                    a.AppliedJobId,
                    a.UserId,
                    a.ApplicationDate,
                    c.Name,
                    c.Surname,
                    Country = c.UserCountry!.Title,
                    City = c.UserCity!.Title
                })
            .Join(
                _context.PrivateData,
                x => x.UserId,
                p => p.UserId,
                (x, p) => new ApplicantSummary() 
                {
                    AppliedJobId = x.AppliedJobId,
                    ApplicationDate = x.ApplicationDate,
                    Name = x.Name,
                    Surname = x.Surname,
                    Country = x.Country,
                    City = x.City,
                    Gender = p.Gender,
                    ExpectedSalary =  p.ExpectedSalary
                })
            .ToListAsync();

        SelectTwo = DataTable.Select(d => new SelectListItem() { Value = d.AppliedJobId.ToString(), Text = d.Name }).ToList();

        return Page();

    }
}