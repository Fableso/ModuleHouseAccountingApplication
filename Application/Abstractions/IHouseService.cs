using Application.DTO.House.Request;
using Application.DTO.House.Response;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;

namespace Application.Abstractions;

public interface IHouseService
{
    Task<HouseResponse?> GetByIdAsync(HouseId id, CancellationToken token = default);
    Task<HouseResponse> AddAsync(CreateHouseRequest houseRequest, CancellationToken token = default);
    Task<HouseResponse> UpdateAsync(UpdateHouseRequest houseRequest, CancellationToken token = default);
    Task RemoveByIdAsync(HouseId id, CancellationToken token = default);
    Task<IEnumerable<HouseResponse>> GetHousesByStateAsync(HouseStatus state, CancellationToken token = default);
    Task<IEnumerable<HouseResponse>> GetHousesInDateRangeAsync(DateSpan dateSpan, CancellationToken token = default);
}