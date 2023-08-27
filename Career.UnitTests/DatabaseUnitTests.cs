using Career;
using Career.Data;
using Career.Models.Datasets;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Career.UnitTests;

public class DatabaseUnitTests
{
    [Fact]
    public void DatabaseConnectionTest()
    {
        using (ApplicationDbContext context = new(GetDbOptions()))
        {
            Assert.True(context.Database.CanConnect());
        }
    }

    [Fact]
    public void CountryCountTest()
    {
        using (ApplicationDbContext context = new(GetDbOptions()))
        {
            int expected = context.CountryDataset.Count();
            int actual = 242;

            Assert.Equal(expected, actual);
        }
    }

    [Fact]
    public void CountryCitiesTest()
    {
        using (ApplicationDbContext context = new(GetDbOptions()))
        {
            var Turkey = context.CountryDataset.Where(c => c.Title == "Turkey").Include(c => c.Cities).First();
            List<string> cityNames = Turkey.Cities.Select(c => c.Title).ToList();

            Assert.Contains("Ankara", cityNames);
        }
    }

    private static DbContextOptions<ApplicationDbContext> GetDbOptions()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-Career-1a6d03a9-bc9d-4373-b29d-61fc5c127e74;Trusted_Connection=True;MultipleActiveResultSets=true");

        return optionsBuilder.Options;
    }
}