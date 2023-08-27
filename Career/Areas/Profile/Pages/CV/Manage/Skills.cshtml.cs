using Career.Data;
using Career.Models.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Profile.Pages.CV.Manage;

public class SkillsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public List<UserAbilityEntityModel> Abilities { get; set; }

    public SkillsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet()
    {
        string? userId = _userManager.GetUserId(User);
        
        if (String.IsNullOrEmpty(userId))
            return NotFound();

        Abilities = await _context.Abilities.Where(a => a.UserId == userId).Include(a => a.UserSkill).ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnGetSkillsStartWith(string str)
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        List<int> userSkillIds = await _context.Abilities.Where(a => a.UserId == userId).Select(a => a.SkillId).ToListAsync();

        var skills = await _context.SkillDataset.AsNoTracking().Where(s => s.Title.StartsWith(str) && !userSkillIds.Contains(s.SkillId)).ToListAsync();

        return new JsonResult(skills);
    }

    public IActionResult OnPostAddSkill(int skillId)
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        UserAbilityEntityModel ability = new() { UserId = userId, SkillId = skillId, UserSkill = _context.SkillDataset.First(s => s.SkillId == skillId) };

        _context.Abilities.Add(ability);
        _context.SaveChanges();

        return Partial("_AbilityDeleteButton", ability);
    }

    public IActionResult OnPostDeleteAbility(int abilityId)
    {
        UserAbilityEntityModel? ability = _context.Abilities.FirstOrDefault(a => a.AbilityId == abilityId);

        if (ability is null)
            return NotFound();

        _context.Abilities.Remove(ability);
        _context.SaveChanges();

        return new JsonResult(new { success = true });
    }
}
