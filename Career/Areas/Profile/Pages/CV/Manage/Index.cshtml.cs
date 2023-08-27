using Career.Data;
using Career.Models;
using Career.Models.Datasets;
using Career.Models.FormModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Career.Areas.Profile.Pages.CV.Manage;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ContactModel? ContactForm { get; set; }

    public PersonalModel? PersonalForm { get; set; }

    public ExperiencesModel? ExperiencesForm { get; set; }

    public EducationsModel? EducationsForm { get; set; }

    public ProjectsModel? ProjectsForm { get; set; }

    public List<string>? SkillList { get; set; }

    public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet()
    {
        string? userId = _userManager.GetUserId(User);

        if (String.IsNullOrEmpty(userId))
            return NotFound();

        await ConfigureEntries(userId);

        return Page();
    }

    public async Task ConfigureEntries(string userId)
    {
        await ConfigureContactForm(userId);
        await ConfigurePersonalForm(userId);
        await ConfigureEducationsForm(userId);
        await ConfigureExperiencesForm(userId);
        await ConfigureProjectsForm(userId);
        await ConfigureSkillList(userId);
    }

    private async Task ConfigureContactForm(string userId)
    {
        ContactModel contactModel = new(_context, _userManager);
        await contactModel.ConfigureContact(userId);

        if (contactModel.IsExist)
            ContactForm = contactModel;
    }

    private async Task ConfigurePersonalForm(string userId)
    {
        PersonalModel personalModel = new(_context, _userManager);
        await personalModel.ConfigurePersonal(userId);

        if (personalModel.IsExists)
            PersonalForm = personalModel;
    }

    private async Task ConfigureEducationsForm(string userId)
    {
        EducationsModel educationsModel = new(_context, _userManager);
        await educationsModel.ConfigureEducationForms(userId);

        if (educationsModel.EducationForms.Count > 0)
            EducationsForm = educationsModel;
    }

    private async Task ConfigureExperiencesForm(string userId)
    {
        ExperiencesModel experiencesModel = new(_context, _userManager);
        await experiencesModel.ConfigureExperienceForms(userId);

        if (experiencesModel.ExperienceForms.Count > 0)
            ExperiencesForm = experiencesModel;
    }

    private async Task ConfigureProjectsForm(string userId)
    {
        ProjectsModel projectsModel = new(_context, _userManager);
        await projectsModel.ConfigureProjects(userId);

        if (projectsModel.Projects.Count > 0)
            ProjectsForm = projectsModel;
    }

    private async Task ConfigureSkillList(string userId)
    {
        List<string>? skillList = await _context.Abilities
            .Where(a => a.UserId == userId)
            .Include(a => a.UserSkill)
            .Select(a => a.UserSkill!.Title)
            .ToListAsync();

        if (skillList.Any())
            SkillList = skillList;

    }
}
