using System;
using System.Threading;
using System.Threading.Tasks;
using EfFluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Custom
{
    #region CustomDbContext

    public class SampleDbContext :
        DbContext
    {
        Func<Type, CachedValidators> validatorFactory;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;

        public SampleDbContext(
            DbContextOptions options,
            Func<Type, CachedValidators> validatorFactory) :
            base(options)
        {
            this.validatorFactory = validatorFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.Company)
                .IsRequired();
            modelBuilder.Entity<Employee>();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            DbContextValidator.Validate(this, validatorFactory).GetAwaiter().GetResult();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            await DbContextValidator.Validate(this, validatorFactory);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }

    #endregion
}