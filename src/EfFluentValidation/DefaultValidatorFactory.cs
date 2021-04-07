using System;
using System.Collections.Generic;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EfFluentValidation
{
    public static class DefaultValidatorFactory<T>
        where T : DbContext
    {
        public static Func<Type, CachedValidators> Factory { get; }

        static DefaultValidatorFactory()
        {
            var validators = ValidationFinder.FromAssemblyContaining<T>();

            ValidatorTypeCache typeCache = new(validators);
            Factory = type => typeCache.GetValidators(type);
        }
    }

    public class CachedValidators
    {
        public IReadOnlyList<IValidator> Validators { get; }
        public bool HasAsyncCondition { get; }

        public CachedValidators(IReadOnlyList<IValidator> validators, bool hasAsyncCondition)
        {
            Validators = validators;
            HasAsyncCondition = hasAsyncCondition;
        }
    }
}