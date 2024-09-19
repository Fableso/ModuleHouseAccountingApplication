using Application.DTO.HouseWeekInfo.Request;
using Application.DTO.HouseWeekInfo.Response;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;

namespace Application.Abstractions;

public interface IHouseWeekInfoService
{
    Task<HouseWeekInfoResponse?> GetByIdAsync(HouseWeekInfoId id, CancellationToken token = default);
    Task<IEnumerable<HouseWeekInfoResponse>> GetHouseInfosInTimeSpanAsync(DateSpan dateSpan, CancellationToken token = default);
    Task<IEnumerable<HouseWeekInfoResponse>> GetHouseInfosForHouseAsync(HouseId houseId, CancellationToken token = default);
    Task<IEnumerable<HouseWeekInfoResponse>> GetHouseInfosForHouseInTimeSpanAsync(HouseId houseId, DateSpan dateSpan, CancellationToken token = default);
    Task<HouseWeekInfoResponse> AddAsync(CreateHouseWeekInfoRequest request, CancellationToken token = default);
    Task RemoveByIdAsync(HouseWeekInfoId id, CancellationToken token = default);
    Task<HouseWeekInfoResponse> UpdateStatusAsync(UpdateHouseWeekInfoRequest request, CancellationToken token = default);
}