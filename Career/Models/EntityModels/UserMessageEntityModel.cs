using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Career.Models.EntityModels;

public class UserMessageEntityModel
{
    [Key]
    public int MessageId { get; set; }

    public string? UserId { get; set; }  // Foreign Key

    public int AppliedJobId { get; set; }  // Foreign Key

    public string? Content { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime MsgDate { get; set; }

    // Relations
    [ForeignKey("AppliedJobId")]
    public virtual UserAppliedJobsEntityModel? UserAppliedJob { get; set; }
}
