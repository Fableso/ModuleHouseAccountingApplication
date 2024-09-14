using Application.Abstractions;
using Application.DTO.WeekMark.Request;
using Application.DTO.WeekMark.Response;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class WeekMarkService : IWeekMarkService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public WeekMarkService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<WeekMarkResponse> GetByIdAsync(WeekMarkId id, CancellationToken token = default)
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
        var weekMark = _mapper.Map<WeekMark>(request);
        _context.WeekMarks.Add(weekMark);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<WeekMarkResponse>(weekMark);
    }

    public async Task RemoveByIdAsync(WeekMarkId id, CancellationToken token = default)
    {
        var weekMark = await _context.WeekMarks.FindAsync(id, token);
        if (weekMark is null)
        {
            throw new EntityNotFoundException($"WeekMark with ID {id.Value} not found");
        }
        _context.WeekMarks.Remove(weekMark);
        await _context.SaveChangesAsync(token);
    }

    public async Task<WeekMarkResponse> UpdateAsync(UpdateWeekMarkRequest request, CancellationToken token = default)
    {
        var weekMark = await _context.WeekMarks.FindAsync(request.Id, token);
        if (weekMark is null)
        {
            throw new EntityNotFoundException($"WeekMark with ID {request.Id.Value} not found");
        }
        _mapper.Map(request, weekMark);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<WeekMarkResponse>(weekMark);
    }
    
    private async Task<object> FetchWeekMarksByWeekInfoIdAsync(HouseWeekInfoId id, CancellationToken token)
    {
        return await _context.WeekMarks.Where(wm => wm.HouseWeekInfoId == id).AsNoTracking().ToListAsync(token);
    }
}