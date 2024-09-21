using System.Runtime.CompilerServices;
using Application.Exceptions;
using Domain.StronglyTypedIds.Interface;
using Microsoft.Extensions.Logging;

namespace Application.Services.Helpers;

public static class ExceptionCasesHandlingHelper
{
    public static void ThrowEntityNotFoundExceptionIfEntityDoesNotExist<TEntity, TId>(IStronglyTypedId<TId> id, TEntity? entity, ILogger? logger = default,
        [CallerMemberName] string actionName = "ExceptionCasesHandlingHelper")
        where TId : notnull
        where TEntity : class
    {
        if (entity is not null) return;
        
        logger?.LogWarning("{ActionName}: {Entity} with ID {EntityId} not found, EntityNotFound exception was thrown",
            actionName, typeof(TEntity).Name, id.Value);
        throw new EntityNotFoundException($"{typeof(TEntity).Name} with ID {id.Value} not found");
    }
}