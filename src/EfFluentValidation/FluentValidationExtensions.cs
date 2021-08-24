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
            return (T) context.EfContext().DbContext;
        }

        public static EfContext EfContext(this IValidationContext context)
        {
            return (EfContext) context.RootContextData["EfContext"];
        }
    }
}