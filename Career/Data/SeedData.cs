using Career.Authorization;
using Career.Models;
using Career.Models.Datasets;
using Career.Models.EntityModels;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Career.Data;

public static class SeedData
{
    private static int _numberOfCountries;
    private static int _numberOfNationalities;
    private static int _numberOfSectors;
    private static int _numberOfIndustries;
    private static int _numberOfUniversities;
    private static int _numberOfStudyFields;

    private static string[] CompanyNames = { 
        "Microsoft", 
        "Apple", 
        "Google", 
        "Amazon", 
        "Facebook", 
        "Intel", 
        "IBM", 
        "NVIDIA", 
        "Adobe", 
        "Tesla", 
        "Netflix", 
        "Salesforce", 
        "Oracle", 
        "Twitter", 
        "HP Inc."
    };

    private static string[] Professions = {
        "Software Engineer",
        "Web Developer",
        "Data Scientist",
        "Network Administrator",
        "Systems Analyst",
        "Database Administrator",
        "UX/UI Designer",
        "Cybersecurity Analyst",
        "Machine Learning Engineer",
        "DevOps Engineer",
        "Game Developer",
        "Cloud Architect",
        "Artificial Intelligence Specialist",
        "Mobile App Developer",
        "IT Project Manager"
    };

    public static async Task Initialize(IServiceProvider serviceProvider, string adminPW, string userPW)
    {
        using (var _context = serviceProvider.GetRequiredService<ApplicationDbContext>())
        {
            // Admin Account & Role Initialization
            var adminId = await EnsureUser(serviceProvider, adminPW, "admin@career.com");
            await EnsureRole(serviceProvider, adminId, Constants.AdministratorRole);
            
            // Database Initialization
            await SeedDatasets(_context);

            await SeedJobs(_context);

            if (_context.Users.Count() < 2)
            {
                var userIdList = await CreateRandomAccounts(serviceProvider, userPW, 5);
                foreach (var userId in userIdList)
                {
                    await SeedUserAccount(serviceProvider, _context, userId);
                }
            }


        }
    }

    private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string adminPW, string username)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var user = await userManager.FindByNameAsync(username);

        if (user is null)
        {
            user = new IdentityUser
            {
                UserName = username,
                Email = username,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(user, adminPW);
        }

        if (user is null)
            throw new Exception("The password probably is not enough!");

        return user.Id;
    }

    private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string userId, string role)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (roleManager is null)
            throw new Exception("Role Manager is null!");

        if (userManager is null)
            throw new Exception("User Manager is null!");

        IdentityResult IR;
        if (!await roleManager.RoleExistsAsync(role))
            IR = await roleManager.CreateAsync(new IdentityRole(role));

        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            throw new Exception("The password probably is not enough!");

        IR = await userManager.AddToRoleAsync(user, role);

        return IR;
    }

    public static async Task SeedDatasets(ApplicationDbContext context)
    {
        string csvBasePath = "wwwroot\\csv\\";

        if (!context.NationalityDataset.Any())
            await LoadEntityModel(context.NationalityDataset, Path.Combine(csvBasePath, "country-nationality.csv"), 0);

        if (!context.IndustryDataset.Any())
            await LoadEntityModel(context.IndustryDataset, Path.Combine(csvBasePath, "industries.csv"), 0);

        if (!context.SectorDataset.Any())
            await LoadEntityModel(context.SectorDataset, Path.Combine(csvBasePath, "sectors.csv"), 0);

        if (!context.SkillDataset.Any())
            await LoadEntityModel(context.SkillDataset, Path.Combine(csvBasePath, "skills.csv"), 0);

        if (!context.CountryDataset.Any())
            await LoadEntityModel(context.CountryDataset, Path.Combine(csvBasePath, "countries.csv"), 0);

        if (!context.StudyFieldDataset.Any())
            await LoadEntityModel(context.StudyFieldDataset, Path.Combine(csvBasePath, "majors-list.csv"), 1);

        if (!context.UniversityDataset.Any())
            await LoadEntityModel(context.UniversityDataset, Path.Combine(csvBasePath, "world-universities.csv"), 1);

        await context.SaveChangesAsync();

        if (!context.CityDataset.Any())
        {
            List<string[]> data = ReadCsvFile(Path.Combine(csvBasePath, "worldcities.csv"));
            List<CityDatasetEntityModel> cities = new();

            foreach (var items in data)
            {
                CountryDatasetEntityModel? country = await context.CountryDataset.FirstOrDefaultAsync(c => c.Title == items[1]) ?? throw new Exception("Country not found!");
                cities.Add(new CityDatasetEntityModel() { Title = items[0], CountryId = country.CountryId });
            }

            await context.AddRangeAsync(cities);
            await context.SaveChangesAsync();
        }

        _numberOfCountries = await context.CountryDataset.CountAsync();
        _numberOfNationalities = await context.NationalityDataset.CountAsync();
        _numberOfSectors = await context.SectorDataset.CountAsync();
        _numberOfIndustries = await context.IndustryDataset.CountAsync();
        _numberOfUniversities = await context.UniversityDataset.CountAsync();
        _numberOfStudyFields = await context.StudyFieldDataset.CountAsync();
    }


    public static async Task LoadEntityModel<T>(DbSet<T> dbSet, string csvPath, int ind) where T : class, IDatasetBaseEntityModel, new()
    {
        List<string[]> data = ReadCsvFile(csvPath);
        List<T> entityList = new ();

        foreach (string[] items in data)
        {
            entityList.Add(new T { Title = items[ind] });
        }

        await dbSet.AddRangeAsync(entityList);
    }

    public static List<string[]> ReadCsvFile(string filePath)
    {
        using TextFieldParser parser = new(filePath);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");

        List<string[]> data = new();

        if (!parser.EndOfData)
            parser.ReadLine();

        while (!parser.EndOfData)
        {
            data.Add(parser.ReadFields()!);
        }

        return data;

    }

    public static async Task<List<string>> CreateRandomAccounts(IServiceProvider serviceProvider, string userPW, int acctNum)
    {
        List<string> userIdList = new(); 

        for (int i = 1; i <= acctNum; i++)
        {
            string userId = await EnsureUser(serviceProvider, userPW, "UserSeedAccount" + i + "@gmail.com");
            await EnsureRole(serviceProvider, userId, Constants.UserRole);

            userIdList.Add(userId);
        }

        return userIdList;
    }

    public static async Task SeedUserAccount(IServiceProvider serviceProvider, ApplicationDbContext context, string userId)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var User = await userManager.FindByIdAsync(userId) ?? throw new Exception($"User with id: ${userId} not found!");

        await SeedContactData(context, User);
        await SeedPersonalData(context, User);
        await SeedExperienceData(context, User);
        await SeedEducationData(context, User);

        await SeedJobApplicationData(context, User);
    }

    public static async Task SeedContactData(ApplicationDbContext context, IdentityUser user)
    {
        string[] Names = { "Tarik", "Sevilay", "Zehra", "Zeynep", "Mustafa", "Bekir", "Bekri", "Kemalettin", "Burhanettin", "Fehmi" };
        string[] Surnames = { "Sen", "Smith", "Johnson", "Williams", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor" };

        Random random = new Random();

        int countryId = random.Next(1, _numberOfCountries);

        List<int> cityIdList = await context.CityDataset.Where(c => c.CountryId == countryId).Select(c => c.CityId).ToListAsync();
        int cityId = cityIdList[random.Next(0, cityIdList.Count)];

        UserContactEntityModel contact = new()
        {
            UserId = user.Id,
            Name = Names[random.Next(0, 10)],
            Surname = Surnames[random.Next(0, 10)],
            Email = user.Email!,
            BirthDay = new DateTime(random.Next(1950, 2016), random.Next(1, 13), random.Next(1, 29)),
            CountryId = countryId,
            CityId = cityId,
            LinkedInUrl = "https://www.linkedin.com/in/" + user.UserName!,
            GithubUrl = "https://www.github.com/" + user.UserName!
        };

        await context.Contacts.AddAsync(contact);
        await context.SaveChangesAsync();
    }


    public static async Task SeedPersonalData(ApplicationDbContext context, IdentityUser user)
    {
        Random random = new();

        UserPrivateEntityModel personal = new()
        {
            UserId = user.Id,
            ExpectedSalary = random.Next(1000, 100000),
            NationalityId = random.Next(1, _numberOfNationalities),
            Gender = (Gender)random.Next(0, 3),
            MilitaryServices = (MilitaryServices)random.Next(0, 3),
            Handicap = (Handicap)random.Next(0, 3),
            Retired = (Retired)random.Next(0, 3)
        };

        await context.PrivateData.AddAsync(personal);
        await context.SaveChangesAsync();
    }

    public static async Task SeedExperienceData(ApplicationDbContext context, IdentityUser user, int re = 3)
    {
        Random random = new();

        List<UserExperienceEntityModel> experiences = new();

        for(int i = 0; i < re; i++)
        {
            var startDate = new DateTime(random.Next(1990, 2010), random.Next(1, 13), random.Next(1, 29));
            var endDate = startDate.AddYears(random.Next(1, 4));

            UserExperienceEntityModel experience = new()
            {
                UserId = user.Id,
                CompanyName = CompanyNames[random.Next(0, 15)],
                Profession = Professions[random.Next(0, 15)],
                StartDate = startDate,
                EndDate = endDate,
                SectorId = random.Next(1, _numberOfSectors),
                IndustryId = random.Next(1, _numberOfIndustries),
                WorkType = (WorkType)random.Next(0, 6),
            };

            experiences.Add(experience);
        }

        await context.Experiences.AddRangeAsync(experiences);
        await context.SaveChangesAsync();
    }

    public static async Task SeedEducationData(ApplicationDbContext context, IdentityUser user, int re = 2)
    {
        Random random = new();

        List<UserEducationEntityModel> educations = new();

        for (int i = 0; i < re; i++)
        {
            var startDate = new DateTime(random.Next(1965, 2000), random.Next(1, 13), random.Next(1, 29));
            var endDate = startDate.AddYears(random.Next(2, 6)).AddMonths(random.Next(1, 13)).AddDays(random.Next(0, 29));

            UserEducationEntityModel education = new()
            {
                UserId = user.Id,
                Level = (EducationLevel)random.Next(0, 4),
                StartDate = startDate,
                EndDate = endDate,
                UniversityId = random.Next(1, _numberOfUniversities),
                StudyField = random.Next(1, _numberOfStudyFields),
                MaxDegree = 4,
                Degree = random.Next(2, 5)
            };

            educations.Add(education);
        }

        await context.Educations.AddRangeAsync(educations);
        await context.SaveChangesAsync();
    }


    public static async Task SeedJobApplicationData(ApplicationDbContext context, IdentityUser user, int sub = 3)
    {
        Random random = new();

        List<UserAppliedJobsEntityModel> jobApplications = new();
        List<int> jobIdList = await context.Jobs.Select(j => j.JobId).ToListAsync();
        
        for (int i = 0; i < sub; i++)
        {
            UserAppliedJobsEntityModel application = new()
            {
                UserId = user.Id,
                JobId = jobIdList[random.Next(0, jobIdList.Count)],
                ApplicationDate = DateTime.Now,
                ApplicationStatus = ApplicationStatus.Submitted
            };

            jobApplications.Add(application);
        }

        await context.AppliedJobs.AddRangeAsync(jobApplications);
        await context.SaveChangesAsync();
    }


    public static async Task SeedJobs(ApplicationDbContext context, int re = 9)
    {
        if (context.Jobs.Any())
            return;
        
        string[] JobDescriptions = {
            "Design and develop scalable software solutions, collaborate with cross-functional teams, and optimize code performance. Experience in coding languages and problem-solving skills required.",
            "Create innovative iOS applications, work on user interface and functionality, troubleshoot bugs, and collaborate with backend teams. Proficiency in Swift and app development required.",
            "Develop machine learning models, analyze data, and improve algorithms for predictive insights. Strong programming skills, expertise in TensorFlow, and statistical knowledge are essential.",
            "Design and implement cloud-based architectures, manage data, and ensure scalability and security. Proficiency in cloud platforms like AWS, Azure, or GCP required.",
            "Build responsive and user-friendly web interfaces, collaborate with designers, and ensure seamless user experiences. Proficiency in HTML, CSS, and JavaScript frameworks necessary.",
            "Conduct research in artificial intelligence, develop algorithms, and contribute to advancements in natural language processing or computer vision. Strong mathematical and programming skills required.",
            "Automate deployment pipelines, monitor system performance, and ensure continuous integration and delivery. Expertise in tools like Jenkins, Docker, and Kubernetes is necessary.",
            "Craft engaging user experiences, design intuitive interfaces, and collaborate with cross-functional teams. Proficiency in design tools, wireframing, and user-centered design required.",
            "Monitor network activities, detect and mitigate security breaches, and develop protocols for data protection. Strong knowledge of cybersecurity practices and tools necessary.",
            "Analyze large datasets, build predictive models, and provide data-driven insights. Proficiency in data analysis tools (Python, R) and experience with machine learning techniques essential."
        };

        Random random = new();

        List<JobsEntityModel> jobs = new();

        for (int i = 0; i < re; i++)
        {
            string profession = Professions[random.Next(0, 15)];
            string description = JobDescriptions[random.Next(0, 10)];

            JobsEntityModel job = new()
            {
                Title = profession,
                Description = description,
                Summary = description.Substring(0, 30),
                CompanyName = CompanyNames[random.Next(0, 15)],
                Position = profession,
                SectorId = random.Next(1, _numberOfSectors),
                IndustryId = random.Next(1, _numberOfIndustries),
                WorkType = (WorkType)random.Next(0, 6),
                CountryId = 17,
                CityId = 226
            };

            jobs.Add(job);
        }

        await context.Jobs.AddRangeAsync(jobs);
        await context.SaveChangesAsync();
    }
}


