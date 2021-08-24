using System;
using System.Collections.Generic;

namespace EfFluentValidation
{
    public class EntityValidationFailure
    {
        public object Entity { get; }
        public Type EntityType { get; }
        public IReadOnlyList<TypeValidationFailure> Failures { get; }

        public EntityValidationFailure(object entity,Type entityType, IReadOnlyList<TypeValidationFailure> failures)
        {
            Entity = entity;
            EntityType = entityType;
            Failures = failures;
        }
    }
}