using System.ComponentModel.DataAnnotations;

namespace Career.Models.EntityModels;

public class UserSummaryEntityModel
{
    [Key]
    public string? UserId { get; set; }

    [Required]
    public string Summary { get; set; } = null!;
}
