using System.ComponentModel.DataAnnotations;

namespace Career.Models.EntityModels;

public class UserProjectEntityModel
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    public string? UserId { get; set; }  // Foreign Key

    [Required]
    [StringLength(128)]
    public string? Name { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? Date { get; set; }

    [Required]
    [Url]
    public string? Url { get; set; }

    public string? Description { get; set; }
}
