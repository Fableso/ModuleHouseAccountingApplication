using Application.Abstractions;
using Application.DTO.HouseWeekInfo.Request;
using Application.DTO.HouseWeekInfo.Response;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class HouseWeekInfoService : IHouseWeekInfoService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public HouseWeekInfoService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task <HouseWeekInfoResponse?> GetByIdAsync(HouseWeekInfoId id, CancellationToken token = default)
    {
        var houseWeekInfo = await _context.HouseWeekInfos.FindAsync(id, token);
        return _mapper.Map<HouseWeekInfoResponse?>(houseWeekInfo);
    }
    
    public async Task<IEnumerable<HouseWeekInfoResponse>> GetHouseInfosInTimeSpanAsync(DateSpan dateSpan, CancellationToken token = default)
    {
        var houseWeekInfos = await FetchHouseInfosInDateRangeAsync(dateSpan, token);
        return _mapper.Map<IEnumerable<HouseWeekInfoResponse>>(houseWeekInfos);
    }

    public async Task<IEnumerable<HouseWeekInfoResponse>> GetHouseInfosForHouseAsync(HouseId houseId, CancellationToken token = default)
    {
        await ThrowEntityNotFoundExceptionIfHouseDoesNotExist(houseId);
        var houseWeekInfos = await FetchHouseWeekInfosForHouseAsync(houseId, token);
        return _mapper.Map<IEnumerable<HouseWeekInfoResponse>>(houseWeekInfos);
    }


    public async Task<IEnumerable<HouseWeekInfoResponse>> GetHouseInfosForHouseInTimeSpanAsync(HouseId houseId, DateSpan dateSpan,
        CancellationToken token = default)
    {
        await ThrowEntityNotFoundExceptionIfHouseDoesNotExist(houseId);
        var houseWeekInfos = await FetchHouseWeekInfosForHouseInDateRangeAsync(houseId, dateSpan, token);
        return _mapper.Map<IEnumerable<HouseWeekInfoResponse>>(houseWeekInfos);
    }

    public async Task<HouseWeekInfoResponse> AddAsync(CreateHouseWeekInfoRequest request, CancellationToken token = default)
    {
        if (!await _context.Houses.AnyAsync(h => h.Id == request.HouseId, token))
        {
            throw new EntityNotFoundException($"House with ID {request.HouseId.Value} not found");
        }
        var houseWeekInfo = _mapper.Map<HouseWeekInfo>(request);
        await _context.HouseWeekInfos.AddAsync(houseWeekInfo, token);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<HouseWeekInfoResponse>(houseWeekInfo);
    }

    public async Task RemoveByIdAsync(HouseWeekInfoId id, CancellationToken token = default)
    {
        var houseWeekInfo = await _context.HouseWeekInfos.FindAsync(id, token);
        if (houseWeekInfo is null)
        {
            throw new EntityNotFoundException($"HouseWeekInfo with ID {id.Value} not found");
        }
        _context.HouseWeekInfos.Remove(houseWeekInfo);
        await _context.SaveChangesAsync(token);
    }

    public async Task<HouseWeekInfoResponse> UpdateStatusAsync(UpdateHouseWeekInfoRequest request, CancellationToken token = default)
    {
        var houseWeekInfo = await _context.HouseWeekInfos.FindAsync(request.Id, token);
        if (houseWeekInfo is null)
        {
            throw new EntityNotFoundException($"HouseWeekInfo with ID {request.Id.Value} not found");
        }
        var updatedHouseWeekInfo = _mapper.Map(request, houseWeekInfo);
        _context.HouseWeekInfos.Update(updatedHouseWeekInfo);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<HouseWeekInfoResponse>(updatedHouseWeekInfo);
    }

    private async Task ThrowEntityNotFoundExceptionIfHouseDoesNotExist(HouseId houseId)
    {
        var house = await _context.Houses.FindAsync(houseId);
        if (house is null)
        {
            throw new EntityNotFoundException($"House with ID {houseId.Value} not found");
        }
    }
    private async Task<IEnumerable<HouseWeekInfo>> FetchHouseInfosInDateRangeAsync(DateSpan dateSpan,
        CancellationToken token = default)
    {
        return await _context.HouseWeekInfos
            .Where(
                info => info.StartDate >= dateSpan.StartDate &&
                        info.StartDate <= (dateSpan.EndDate ?? DateOnly.MaxValue))
            .Include(hwi => hwi.WeekComments)
            .AsNoTracking()
            .ToListAsync(token);
    }
    
    private async Task<IEnumerable<HouseWeekInfo>> FetchHouseWeekInfosForHouseAsync(HouseId houseId, CancellationToken token = default)
    {
        return await _context.HouseWeekInfos
            .Where(info => info.HouseId == houseId)
            .Include(hwi => hwi.WeekComments)
            .AsNoTracking()
            .ToListAsync(token);
    }
    
    private async Task<IEnumerable<HouseWeekInfo>> FetchHouseWeekInfosForHouseInDateRangeAsync(HouseId houseId, DateSpan dateSpan,
        CancellationToken token = default)
    {
        return await _context.HouseWeekInfos
            .Where(info => info.HouseId == houseId &&
                           info.StartDate >= dateSpan.StartDate &&
                           info.StartDate <= (dateSpan.EndDate ?? DateOnly.MaxValue))
            .Include(hwi => hwi.WeekComments)
            .AsNoTracking()
            .ToListAsync(token);
    }
}