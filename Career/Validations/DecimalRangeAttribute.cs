using System.ComponentModel.DataAnnotations;

namespace Career.Validations;

public class DecimalRangeAttribute: ValidationAttribute
{
    private readonly decimal _min;
    private readonly decimal _max;

    private readonly Mode _mode;

    public DecimalRangeAttribute(double min, double max, Mode mode = Mode.Decimal)
    {
        _min = (decimal) min;
        _max = (decimal) max;
        _mode = mode;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || value is not decimal)
            return new ValidationResult("Invalid value");

        decimal val = (decimal)value;

        if (val < _min || val > _max)
        {
            switch (_mode)
            {
                case Mode.Decimal: return new ValidationResult($"The value must be between {_min} and {_max}.");

                case Mode.Money: return new ValidationResult($"The value must be between {_min:C} and {_max:C}.");
            }
        }

        return ValidationResult.Success!;
    }

    public enum Mode
    {
        Money,
        Decimal
    }
}
