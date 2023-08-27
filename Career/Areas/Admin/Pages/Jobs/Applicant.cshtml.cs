using Career.Data;
using Career.Models.EntityModels;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Admin.Pages.Jobs;

public class ApplicantModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public Profile.Pages.CV.Manage.IndexModel UserData { get; set; }

    public UserAppliedJobsEntityModel UserJobApplication { get; set; }

    public ApplicantModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet(int? id)
    {
        if (id == null)
            return NotFound();

        var userJobApplication = await _context.AppliedJobs.FirstOrDefaultAsync(j => j.AppliedJobId == id);

        if (userJobApplication == null || userJobApplication.UserId == null)
            return NotFound();

        UserJobApplication = userJobApplication;

        UserData = new(_context, _userManager);
        await UserData.ConfigureEntries(UserJobApplication.UserId);

        return Page();
    }

    public async Task<IActionResult> OnPostSendNotification(int? appliedJobId)
    {
        if (appliedJobId == null)
            return NotFound();

        var jobApplication = await _context.AppliedJobs
            .Include(a => a.JobsEntityModel)
            .FirstOrDefaultAsync(a => a.AppliedJobId == appliedJobId);

        if (jobApplication == null)
            return NotFound();

        var lastMsg = await _context.Messages
            .Where(m => m.AppliedJobId == appliedJobId)
            .OrderByDescending(a => a.MsgDate)
            .FirstOrDefaultAsync();

        string jobText = $"({jobApplication.JobsEntityModel!.CompanyName} | {jobApplication.JobsEntityModel!.Title})";

        if (lastMsg?.MsgDate.AddDays(30) > DateTime.Now)
            return Partial("_StatusMsg", new StatusMsgFormModel()
            {
                Status = "danger",
                Message = jobText + "Your job notification could NOT be delivered. At least one month must pass after your last notification."
            });


        UserMessageEntityModel msg = new()
        {
            UserId = jobApplication.UserId,
            AppliedJobId = (int)appliedJobId,
            Content = $"You have been accepted by {jobApplication.JobsEntityModel!.CompanyName} for {jobApplication.JobsEntityModel!.Title} job!",
            MsgDate = DateTime.Now
        };

        await _context.Messages.AddAsync(msg);
        await _context.SaveChangesAsync();

        return Partial("_StatusMsg", new StatusMsgFormModel()
        {
            Status = "success",
            Message = jobText + "Your job hiring notification is delivered successfully!"
        });
    }
}
