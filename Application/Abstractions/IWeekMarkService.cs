using Application.DTO.WeekMark.Request;
using Application.DTO.WeekMark.Response;
using Domain.StronglyTypedIds;

namespace Application.Abstractions;

public interface IWeekMarkService
{
    Task<WeekMarkResponse> GetByIdAsync(WeekMarkId id, CancellationToken token = default);
    Task<IEnumerable<WeekMarkResponse>> GetWeekMarksByWeekInfoId(HouseWeekInfoId id, CancellationToken token = default);
    Task<WeekMarkResponse> AddAsync(CreateWeekMarkRequest request, CancellationToken token = default);
    Task RemoveByIdAsync(WeekMarkId id, CancellationToken token = default);
    Task<WeekMarkResponse> UpdateAsync(UpdateWeekMarkRequest request, CancellationToken token = default);
}