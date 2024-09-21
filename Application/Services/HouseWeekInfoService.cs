using System.Runtime.CompilerServices;
using Application.Abstractions;
using Application.DTO.HouseWeekInfo.Request;
using Application.DTO.HouseWeekInfo.Response;
using Application.Exceptions;
using Application.Services.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class HouseWeekInfoService : IHouseWeekInfoService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<HouseWeekInfoService> _logger;

    public HouseWeekInfoService(IApplicationDbContext context, IMapper mapper, ILogger<HouseWeekInfoService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
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
        ExceptionThrowingHelper
            .ThrowEntityNotFoundExceptionIfEntityDoesNotExist(houseId, await _context.Houses.FindAsync(houseId, token), _logger);
        var houseWeekInfos = await FetchHouseWeekInfosForHouseAsync(houseId, token);
        return _mapper.Map<IEnumerable<HouseWeekInfoResponse>>(houseWeekInfos);
    }


    public async Task<IEnumerable<HouseWeekInfoResponse>> GetHouseInfosForHouseInTimeSpanAsync(HouseId houseId, DateSpan dateSpan,
        CancellationToken token = default)
    {
        ExceptionThrowingHelper
            .ThrowEntityNotFoundExceptionIfEntityDoesNotExist(houseId, await _context.Houses.FindAsync(houseId, token), _logger);
        var houseWeekInfos = await FetchHouseWeekInfosForHouseInDateRangeAsync(houseId, dateSpan, token);
        return _mapper.Map<IEnumerable<HouseWeekInfoResponse>>(houseWeekInfos);
    }

    public async Task<HouseWeekInfoResponse> AddAsync(CreateHouseWeekInfoRequest request, CancellationToken token = default)
    {
        ExceptionThrowingHelper
            .ThrowEntityNotFoundExceptionIfEntityDoesNotExist(request.HouseId, await _context.Houses.FindAsync(request.HouseId, token), _logger);
        var houseWeekInfo = _mapper.Map<HouseWeekInfo>(request);
        await _context.HouseWeekInfos.AddAsync(houseWeekInfo, token);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<HouseWeekInfoResponse>(houseWeekInfo);
    }

    public async Task RemoveByIdAsync(HouseWeekInfoId id, CancellationToken token = default)
    {
        var houseWeekInfo = await _context.HouseWeekInfos.FindAsync(id, token);
        ExceptionThrowingHelper.ThrowEntityNotFoundExceptionIfEntityDoesNotExist(id, houseWeekInfo, _logger);
        _context.HouseWeekInfos.Remove(houseWeekInfo!);
        await _context.SaveChangesAsync(token);
    }

    public async Task<HouseWeekInfoResponse> UpdateStatusAsync(UpdateHouseWeekInfoRequest request, CancellationToken token = default)
    {
        var houseWeekInfo = await _context.HouseWeekInfos.FindAsync(request.Id, token);
        ExceptionThrowingHelper.ThrowEntityNotFoundExceptionIfEntityDoesNotExist(request.Id, houseWeekInfo, _logger);
        var updatedHouseWeekInfo = _mapper.Map(request, houseWeekInfo);
        _context.HouseWeekInfos.Update(updatedHouseWeekInfo!);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<HouseWeekInfoResponse>(updatedHouseWeekInfo);
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