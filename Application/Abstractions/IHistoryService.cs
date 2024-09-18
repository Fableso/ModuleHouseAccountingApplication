using Application.DTO.History.Responses;
using Domain.StronglyTypedIds;

namespace Application.Abstractions;

public interface IHistoryService
{
    Task<IEnumerable<AuditResponse>>
        GetFullHouseHistoryLogByIdAsync(HouseId houseId, CancellationToken token = default);
}