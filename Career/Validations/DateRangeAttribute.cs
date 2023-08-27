using System.ComponentModel.DataAnnotations;

namespace Career.Validations;

public class DateRangeAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public DateRangeAttribute(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);

        if (startDateProperty == null)
        {
            return new ValidationResult($"Unknown property: {_startDatePropertyName}");
        }

        var startDateValue = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance)!;
        var endDateValue = (DateTime)value!;

        if (endDateValue <= startDateValue)
        {
            return new ValidationResult($"End Date must be greater than Start Date.");
        }

        return ValidationResult.Success!;
    }

}
