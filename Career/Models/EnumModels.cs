using Microsoft.AspNetCore.Mvc.Rendering;

namespace Career.Models;

public enum WorkType
{
    Freelance,
    PartTime,
    FullTime,
    Temporary,
    Intern,
    Volunteer
};

public enum EducationLevel
{
    Associate,
    Bachelor,
    Master,
    PhD
};

public enum Gender
{
    Undefined,
    Female,
    Male
};

public enum MilitaryServices
{
    Undefined,
    Completed,
    Uncompleted
};

public enum Handicap
{
    Undefined,
    Disabled,
    NonDisabled
};

public enum Retired
{
    Undefined,
    Retired,
    NotRetired
};

public enum ApplicationStatus
{
    Submitted,
    Approved,
    Rejected
};

public enum MessageSeen
{
    Unseen,
    Seen
};

public static class EnumHelper
{
    public static List<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem() { Value=e.ToString(), Text=Enum.GetName(typeof(TEnum), e) })
                .ToList();
    }
}