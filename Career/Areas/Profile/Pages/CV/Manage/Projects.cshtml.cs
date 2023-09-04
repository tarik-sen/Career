using Career.Data;
using Career.DebugHelper;
using Career.Models;
using Career.Models.EntityModels;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Career.Areas.Profile.Pages.CV.Manage;

public class ProjectsModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public List<UserProjectEntityModel> Projects { get; set; }

    [BindProperty]
    public UserProjectEntityModel? ActiveProject { get; set; }


    public ProjectsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }

    public async Task<IActionResult> OnGet()
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        await ConfigureProjects(userId);

        return Page();
    }

    public IActionResult OnGetNewProject()
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        return Partial("_ProjectForm", new UserProjectEntityModel() { UserId = userId });
    }

    public IActionResult OnGetProjectWithId(int projectId)
    {
        UserProjectEntityModel? project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);

        if (project is null)
            return NotFound();

        return Partial("_ProjectForm", project);
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ModelStateHelper.LogModelValidationDrawbacks(ModelState);
            return Partial("_ProjectForm", ActiveProject);
        }

        if (ActiveProject!.ProjectId == default)
            _context.Projects.Add(ActiveProject);
        else
            _context.Projects.Update(ActiveProject);

        _context.SaveChanges();

        return Partial("_ProjectForm", ActiveProject);
    }

    public IActionResult OnPostDeleteProject(int projectId)
    {
        UserProjectEntityModel? project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);

        if (project is null)
            return NotFound();

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return new JsonResult(new { success = true });
    }

    public async Task ConfigureProjects(string userId)
    {
        Projects = await _context.Projects.AsNoTracking()
            .OrderByDescending(p => p.ProjectId)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }
}
