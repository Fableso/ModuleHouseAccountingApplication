using Application.Abstractions;
using Application.DTO.House.Request;
using Application.DTO.House.Response;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class HouseService : IHouseService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHousePostService _housePostService;
    public HouseService(IApplicationDbContext context, IMapper mapper, IHousePostService housePostService)
    {
        _context = context;
        _mapper = mapper;
        _housePostService = housePostService;
    }
    
    public async Task<IEnumerable<HouseResponse>> GetHousesInDateRangeAsync(DateSpan dateSpan, CancellationToken token = default)
    {
        var houses = await FetchHousesInDateRangeAsync(dateSpan, token);
        return _mapper.Map<IEnumerable<HouseResponse>>(houses);
    }


    public async Task<HouseResponse?> GetByIdAsync(HouseId id, CancellationToken token = default)
    {
        var house = await FetchHouseById(id, token);
        return _mapper.Map<HouseResponse?>(house);
    }

    public async Task<HouseResponse> AddAsync(CreateHouseRequest houseRequest, CancellationToken token = default)
    {
        if (await HouseExistsAsync(houseRequest.Model, token))
        {
            throw new EntityAlreadyExistsException($"House with model {houseRequest.Model.Value} already exists. The house model must be unique");
        }
        var house = _mapper.Map<House>(houseRequest);
        await _housePostService.AddHousePostRelationsForNewHouseAsync(house.Id, houseRequest.PostIds, token);
        await _context.Houses.AddAsync(house, token);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<HouseResponse>(house);
    }

    public async Task<HouseResponse> UpdateAsync(UpdateHouseRequest houseRequest, CancellationToken token = default)
    {
        var existingHouse = await FetchHouseById(houseRequest.Model, token);
        if (existingHouse == null)
        {
            throw new EntityNotFoundException($"House with id {houseRequest.Model.Value} not found");
        }
        var updatedHouse = _mapper.Map(houseRequest, existingHouse);
        await _housePostService.UpdatePostsForHouseAsync(updatedHouse.Id, houseRequest.PostIds, token);
        _context.Houses.Update(updatedHouse);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<HouseResponse>(updatedHouse);
    }

    public async Task RemoveByIdAsync(HouseId id, CancellationToken token = default)
    {
        var house = await _context.Houses.FindAsync([id], token);
        if (house is null)
        {
            throw new EntityNotFoundException($"House with id {id.Value} not found");
        }
        _context.Houses.Remove(house);
        await _context.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<HouseResponse>> GetHousesByStateAsync(HouseStatus state, CancellationToken token = default)
    {
        var houses = await _context.Houses.Where(house => house.CurrentState == state).ToListAsync(cancellationToken: token);
        return _mapper.Map<IEnumerable<HouseResponse>>(houses);
    }
    
    private async Task<List<House>> FetchHousesInDateRangeAsync(DateSpan dateSpan, CancellationToken token)
    {
        return await _context.Houses
            .Where(house =>
                house.OfficialStartDate >= dateSpan.StartDate &&
                house.OfficialEndDate <= (dateSpan.EndDate ?? DateOnly.MaxValue))
            .Include(h => h.HouseWeekInfos)
            .ThenInclude(hwi => hwi.WeekComments)
            .Include(h => h.HousePosts)
            .ThenInclude(h => h.Post)
            .AsNoTracking()
            .ToListAsync(cancellationToken: token);
    }
    
    private async Task<House?> FetchHouseById(HouseId id, CancellationToken token)
    {
        return await _context.Houses
            .Include(h => h.HouseWeekInfos)
            .ThenInclude(hwi => hwi.WeekComments)
            .Include(h => h.HousePosts)
            .ThenInclude(h => h.Post)
            .FirstOrDefaultAsync(h => h.Id == id, token);
    }
    
    private async Task<bool> HouseExistsAsync(HouseId id, CancellationToken token)
    {
        return await _context.Houses.AnyAsync(house => house.Id == id, token);
    }

}