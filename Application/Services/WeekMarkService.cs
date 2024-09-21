using System.Runtime.CompilerServices;
using Application.Abstractions;
using Application.DTO.WeekMark.Request;
using Application.DTO.WeekMark.Response;
using Application.Exceptions;
using Application.Services.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class WeekMarkService : IWeekMarkService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<WeekMarkService> _logger;

    public WeekMarkService(IApplicationDbContext context, IMapper mapper, ILogger<WeekMarkService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<WeekMarkResponse?> GetByIdAsync(WeekMarkId id, CancellationToken token = default)
    {
        var weekMark = await _context.WeekMarks.FindAsync(id, token);
        return _mapper.Map<WeekMarkResponse>(weekMark);
    }

    public async Task<IEnumerable<WeekMarkResponse>> GetWeekMarksByWeekInfoId(HouseWeekInfoId id, CancellationToken token = default)
    {
        var weekMarks = await FetchWeekMarksByWeekInfoIdAsync(id, token);
        return _mapper.Map<IEnumerable<WeekMarkResponse>>(weekMarks);
    }


    public async Task<WeekMarkResponse> AddAsync(CreateWeekMarkRequest request, CancellationToken token = default)
    {
        ExceptionCasesHandlingHelper
            .ThrowEntityNotFoundExceptionIfEntityDoesNotExist(
                request.HouseWeekInfoId,
                await _context.HouseWeekInfos.FindAsync(request.HouseWeekInfoId, token),
                _logger);
        var weekMark = _mapper.Map<WeekMark>(request);
        _context.WeekMarks.Add(weekMark);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<WeekMarkResponse>(weekMark);
    }

    public async Task RemoveByIdAsync(WeekMarkId id, CancellationToken token = default)
    {
        var weekMark = await _context.WeekMarks.FindAsync(id, token);
        ExceptionCasesHandlingHelper.ThrowEntityNotFoundExceptionIfEntityDoesNotExist(id, weekMark, _logger);
        _context.WeekMarks.Remove(weekMark!);
        await _context.SaveChangesAsync(token);
    }

    public async Task<WeekMarkResponse> UpdateAsync(UpdateWeekMarkRequest request, CancellationToken token = default)
    {
        var weekMark = await _context.WeekMarks.FindAsync(request.Id, token);
        ExceptionCasesHandlingHelper.ThrowEntityNotFoundExceptionIfEntityDoesNotExist(request.Id, weekMark, _logger);
        _mapper.Map(request, weekMark);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<WeekMarkResponse>(weekMark);
    }
    private async Task<object> FetchWeekMarksByWeekInfoIdAsync(HouseWeekInfoId id, CancellationToken token)
    {
        return await _context.WeekMarks.Where(wm => wm.HouseWeekInfoId == id).AsNoTracking().ToListAsync(token);
    }
}