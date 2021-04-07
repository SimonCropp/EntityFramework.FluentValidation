using System.Collections.Generic;
using System.Threading.Tasks;
using EfFluentValidation;
using EfLocalDb;
using FluentValidation;

// LocalDb is used to make the sample simpler.
// Replace with a real DbContext
public static class DbContextBuilder
{
    static DbContextBuilder()
    {
        sqlInstance = new(
            buildTemplate: CreateDb,
            constructInstance: builder => new SampleDbContext(
                builder.Options,
                _ => new CachedValidators(new List<IValidator>(),false)));
    }

    static SqlInstance<SampleDbContext> sqlInstance;

    static async Task CreateDb(SampleDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        Company company1 = new()
        {
            Id = 1,
            Content = "Company1"
        };
        Employee employee1 = new()
        {
            Id = 2,
            CompanyId = company1.Id,
            Content = "Employee1",
            Age = 25
        };
        Employee employee2 = new()
        {
            Id = 3,
            CompanyId = company1.Id,
            Content = "Employee2",
            Age = 31
        };
        Company company2 = new()
        {
            Id = 4,
            Content = "Company2"
        };
        Employee employee4 = new()
        {
            Id = 5,
            CompanyId = company2.Id,
            Content = "Employee4",
            Age = 34
        };
        Company company3 = new()
        {
            Id = 6,
            Content = "Company3"
        };
        Company company4 = new()
        {
            Id = 7,
            Content = "Company4"
        };
        context.AddRange(company1, employee1, employee2, company2, company3, company4, employee4);
        await context.SaveChangesAsync();
    }

    public static Task<SqlDatabase<SampleDbContext>> GetDatabase(string suffix)
    {
        return sqlInstance.Build(suffix);
    }
}