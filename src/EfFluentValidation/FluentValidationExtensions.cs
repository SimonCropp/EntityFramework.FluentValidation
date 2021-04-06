using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EfFluentValidation
{
    /// <summary>
    /// Extensions to FluentValidation.
    /// </summary>
    public static class FluentValidationExtensions
    {
        public static T DbContext<T>(this IValidationContext context)
            where T : DbContext
        {
            Guard.AgainstNull(context, nameof(context));
            return (T) context.EfContext().DbContext;
        }

        public static EfContext EfContext(this IValidationContext context)
        {
            Guard.AgainstNull(context, nameof(context));
            return (EfContext) context.RootContextData["EfContext"];
        }
    }
}