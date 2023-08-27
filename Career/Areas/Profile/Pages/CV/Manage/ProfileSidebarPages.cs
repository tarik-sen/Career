using Microsoft.AspNetCore.Mvc.Rendering;

namespace Career.Areas.Profile.Pages.CV.Manage;

public static class ProfileSidebarPages
{
    public static string Overview => "Overview";

    public static string Contact => "Contact";
    
    public static string Personal => "Personal";
    
    public static string Experiences => "Experiences";
    
    public static string Educations => "Educations";

    public static string Projects => "Projects";
    
    public static string Skills => "Skills";


    public static string OverviewNav(ViewContext viewContext) => PageNav(viewContext, Overview);
    public static string ContactNav(ViewContext viewContext) => PageNav(viewContext, Contact);
    public static string PersonalNav(ViewContext viewContext) => PageNav(viewContext, Personal);
    public static string ExperiencesNav(ViewContext viewContext) => PageNav(viewContext, Experiences);
    public static string EducationsNav(ViewContext viewContext) => PageNav(viewContext, Educations);
    public static string ProjectsNav(ViewContext viewContext) => PageNav(viewContext, Projects);
    public static string SkillsNav(ViewContext viewContext) => PageNav(viewContext, Skills);

    
    public static string PageNav(ViewContext viewContext, string page)
    {
        string activePage = viewContext.ViewData["ActivePage"] as string
            ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName)!;

        if (String.Equals(activePage, page))
            return "link-light active";

        return "link-dark";
    }
}
