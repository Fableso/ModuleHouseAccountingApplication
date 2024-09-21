using System.Reflection;
using Application.Abstractions;
using Application.DTO.History.Responses;
using Application.Exceptions;
using Application.Services.Helpers;
using AutoMapper;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class HistoryService : IHistoryService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<HistoryService> _logger;

    public HistoryService(IApplicationDbContext context, IMapper mapper, ILogger<HistoryService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AuditResponse>> GetMainHouseHistoryLogByIdAsync(
        HouseId houseId, CancellationToken token = default)
    {
        var houseAudits = await GetMainHouseLogs(houseId, token);
        return _mapper.Map<IEnumerable<AuditResponse>>(houseAudits);
    }

    public async Task<IEnumerable<AuditResponse>> GetHouseWeekHistoryLogByIdAsync(HouseWeekInfoId houseWeekInfoId, CancellationToken token = default)
    {
        var weekInfoAuditLogs= await GetHouseWeekInfoHistoryLogs(houseWeekInfoId, token);
        return _mapper.Map<IEnumerable<AuditResponse>>(weekInfoAuditLogs);
    }


    public async Task<IEnumerable<AuditResponse>> GetFullHouseHistoryLogByIdAsync(
        HouseId houseId, CancellationToken token = default)
    {
        var house = await _context.Houses
            .Include(h => h.HousePosts)
                .ThenInclude(hp => hp.Post)
            .Include(h => h.HouseWeekInfos)
                .ThenInclude(hwi => hwi.WeekComments)
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Id == houseId, token);

        ExceptionCasesHandlingHelper.ThrowEntityNotFoundExceptionIfEntityDoesNotExist(houseId, house, _logger);

        var houseAudits = await GetMainHouseLogs(houseId, token);

        var houseWeekInfoIds = house!.HouseWeekInfos.Select(hwi => hwi.Id.ToString());
        var houseWeekInfoAudits = await GetRecordsLogsAsync(
            houseWeekInfoIds, nameof(_context.HouseWeekInfos), token);

        var weekCommentIds = house.HouseWeekInfos
            .SelectMany(hwi => hwi.WeekComments.Select(wc => wc.Id.ToString()));
        var weekCommentAudits = await GetRecordsLogsAsync(
            weekCommentIds, nameof(_context.WeekMarks), token);

        var housePostIds = house.HousePosts.Select(hp => hp.Id.ToString());
        var housePostsAudits = await GetRecordsLogsAsync(
            housePostIds, nameof(_context.HousePosts), token);

        var allAudits = houseAudits
            .Concat(houseWeekInfoAudits)
            .Concat(weekCommentAudits)
            .Concat(housePostsAudits);

        return _mapper.Map<IEnumerable<AuditResponse>>(allAudits);
    }
    private async Task<IEnumerable<Audit>> GetHouseWeekInfoHistoryLogs(HouseWeekInfoId houseWeekInfoId, CancellationToken token)
    {
        var houseWeekInfo = await _context.HouseWeekInfos
            .AsNoTracking()
            .Include(hwi => hwi.WeekComments)
            .FirstOrDefaultAsync(hwi => hwi.Id == houseWeekInfoId, token);
        
        ExceptionCasesHandlingHelper.ThrowEntityNotFoundExceptionIfEntityDoesNotExist(houseWeekInfoId, houseWeekInfo, _logger);
        
        
        var weekLogs = await GetRecordsLogsAsync(
            new[] { houseWeekInfoId.ToString() }, nameof(_context.HouseWeekInfos), token);
        
        var weekCommentIds = houseWeekInfo!.WeekComments.Select(wc => wc.Id.ToString());
        var weekCommentLogs = await GetRecordsLogsAsync(
            weekCommentIds, nameof(_context.WeekMarks), token);
        
        return weekLogs.Concat(weekCommentLogs);
    }

    private async Task<IEnumerable<Audit>> GetMainHouseLogs(
        HouseId houseId, CancellationToken token = default)
    {
        return await GetRecordsLogsAsync(
            new[] { houseId.ToString() }, nameof(_context.Houses), token);
    }

    private async Task<IEnumerable<Audit>> GetRecordsLogsAsync(
        IEnumerable<string> recordIds, string tableName, CancellationToken token = default)
    {
        return await _context.Audits
            .Where(a => a.TableName == tableName && recordIds.Contains(a.RecordId))
            .Include(a => a.Changes)
            .ToListAsync(token);
    }
}
