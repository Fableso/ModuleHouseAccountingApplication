using Domain.Common;
using FluentResults;
using FluentValidation;

namespace Web.Validation.Extensions;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> MustBeValidValueObject<T, TProperty, TValueObject>(
        this IRuleBuilder<T, TProperty> ruleBuilder, Func<TProperty, Result<TValueObject>> factory)
        where TValueObject : ValueObject
    {
        return (IRuleBuilderOptions<T,TProperty>)ruleBuilder.Custom((value, context) =>
        {
            var result = factory(value);
            
            if (!result.IsFailed) return;
            
            foreach (var error in result.Errors)
            {
                context.AddFailure(error.Message);
            }
        });
    }

    public static IRuleBuilderOptions<T, IEnumerable<TElement>> MustBeUniqueCollection<T, TElement>(
        this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return (IRuleBuilderOptions<T,IEnumerable<TElement>>)ruleBuilder.Custom((collection, context) =>
        {
            if (collection is null) return;
            var duplicates = collection.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();

            if (duplicates.Count != 0)
            {
                context.AddFailure($"Collection contains duplicate values: {string.Join(", ", duplicates)}");
            }
        });
    }
}