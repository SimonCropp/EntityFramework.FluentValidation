using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Result = FluentValidation.AssemblyScanner.AssemblyScanResult;

namespace EfFluentValidation
{
    public class ValidatorTypeCache
    {
        ConcurrentDictionary<Type, CachedValidators> entityMapCache = new();
        Dictionary<Type, CachedValidators> instanceCache = new();

        public ValidatorTypeCache(IEnumerable<Result> scanResults)
        {
            foreach (var result in scanResults.GroupBy(x => x.InterfaceType.GenericTypeArguments.Single()))
            {
                var validators = result
                    .Select(x => Activator.CreateInstance(x.ValidatorType))
                    .Cast<IValidator>()
                    .ToList();
                var hasAsyncCondition = validators.Any(x => x.CreateDescriptor().Rules.Any(y => y.HasAsyncCondition));
                instanceCache[result.Key] = new(validators, hasAsyncCondition);
            }
        }

        public CachedValidators GetValidators(Type entityType)
        {
            CachedValidators empty = new(new List<IValidator>(), false);
            return entityMapCache.GetOrAdd(entityType, x =>
            {
                var list = FindValidatorsForEntity(x);
                if (list.Validators.Any())
                {
                    return new(list.Validators, list.HasAsyncCondition);
                }

                return empty;
            });
        }

        CachedValidators FindValidatorsForEntity(Type entityType)
        {
            List<IValidator> list = new();
            var hasAsyncCondition = false;
            foreach (var typeToValidators in instanceCache)
            {
                var targetType = typeToValidators.Key;
                var cachedValidators = typeToValidators.Value;
                if (targetType.IsAssignableFrom(entityType))
                {
                    if (cachedValidators.HasAsyncCondition)
                    {
                        hasAsyncCondition = true;
                    }
                    list.AddRange(cachedValidators.Validators);
                }
            }

            return new(list, hasAsyncCondition);
        }
    }
}