using Career.Data;
using Career.Models.EntityModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Profile.Pages.Messages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public List<UserMessageEntityModel> Messages { get; set; }

    public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;

        Messages = new();
    }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return NotFound();

        Messages = await _context.Messages.Where(m => m.UserId == user.Id).ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteMsg(int? msgId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (msgId == null || user == null)
            return NotFound();

        var msg = await _context.Messages.FirstOrDefaultAsync(m => m.UserId == user.Id && m.MessageId == msgId);

        if (msg is null)
            return NotFound();

        _context.Messages.Remove(msg);
        await _context.SaveChangesAsync();

        return new JsonResult(new { success = true });
    }
}
