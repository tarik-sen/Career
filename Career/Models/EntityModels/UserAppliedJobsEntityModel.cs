using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Career.Models.EntityModels;

public class UserAppliedJobsEntityModel
{
    [Key]
    public int AppliedJobId { get; set; }

    public string? UserId { get; set; }  // Foreign Key

    public int JobId { get; set; }  // Foreign Key

    [Required]
    [DataType(DataType.Date)]
    public DateTime ApplicationDate { get; set; }

    public string? CoverLetter { get; set; }

    public ApplicationStatus ApplicationStatus { get; set; }

    // Relations
    [ForeignKey("JobId")]
    public virtual JobsEntityModel? JobsEntityModel { get; set;}
}
