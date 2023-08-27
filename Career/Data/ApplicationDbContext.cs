using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Career.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Career.Models.EntityModels;

namespace Career.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserEducationEntityModel>(entity => 
        { 
            entity.Property(e => e.Degree).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MaxDegree).HasColumnType("decimal(5, 2)");
        });

        builder.Entity<JobsEntityModel>()
            .HasOne(j => j.UserCity)
            .WithMany()
            .HasForeignKey(j => j.CityId)
            .OnDelete(DeleteBehavior.NoAction);

    }

    // Entity Models

    public virtual DbSet<Models.EntityModels.UserExperienceEntityModel> Experiences { get; set; }

    public virtual DbSet<Models.EntityModels.UserEducationEntityModel> Educations { get; set; }

    public virtual DbSet<Models.EntityModels.UserContactEntityModel> Contacts { get; set; }

    public virtual DbSet<Models.EntityModels.UserProjectEntityModel> Projects { get; set; }

    public virtual DbSet<Models.EntityModels.UserPrivateEntityModel> PrivateData { get; set; }

    public virtual DbSet<Models.EntityModels.UserSummaryEntityModel> Summaries { get; set; }

    public virtual DbSet<Models.EntityModels.UserAbilityEntityModel> Abilities { get; set; }
    
    public virtual DbSet<Models.EntityModels.UserAppliedJobsEntityModel> AppliedJobs { get; set; }

    public virtual DbSet<Models.EntityModels.UserMessageEntityModel> Messages { get; set; }
    
    public virtual DbSet<Models.EntityModels.JobsEntityModel> Jobs { get; set; }

    // Datasets

    public virtual DbSet<Models.Datasets.SkillDatasetEntityModel> SkillDataset { get; set; }

    public virtual DbSet<Models.Datasets.SectorDatasetEntityModel> SectorDataset { get; set; }

    public virtual DbSet<Models.Datasets.IndustriesDatasetEntityModel> IndustryDataset { get; set; }

    public virtual DbSet<Models.Datasets.UniversityDatasetEntityModel> UniversityDataset { get; set; }
    
    public virtual DbSet<Models.Datasets.StudyFieldDatasetEntityModel> StudyFieldDataset { get; set; }
    
    public virtual DbSet<Models.Datasets.CountryDatasetEntityModel> CountryDataset { get; set; }
    
    public virtual DbSet<Models.Datasets.CityDatasetEntityModel> CityDataset { get; set; }

    public virtual DbSet<Models.Datasets.NationalityDatasetEntityModel> NationalityDataset { get; set; }
}