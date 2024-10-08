using Application.DTO.History.Responses;
using Application.DTO.House.Request;
using Application.DTO.House.Response;
using Application.DTO.HouseWeekInfo.Request;
using Application.DTO.HouseWeekInfo.Response;
using Application.DTO.Post.Request;
using Application.DTO.Post.Response;
using Application.DTO.WeekMark.Request;
using Application.DTO.WeekMark.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;

namespace Application.Mappers;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<House, HouseResponse>()
            .ForMember(hr => hr.Posts, h => h.MapFrom(x => x.HousePosts.Select(hp => hp.Post)))
            .ForMember(hr => hr.HouseWeekInfos, h => h.MapFrom(x => x.HouseWeekInfos))
            .ForMember(hr => hr.Model, h => h.MapFrom(x => x.Id));

        CreateMap<CreateHouseRequest, House>()
            .ConstructUsing(house => new House(house.Model, HouseMetrics.Create(house.Length, house.Width).Value,
                new Point(house.TopLeftCornerX, house.TopLeftCornerY), HouseStatus.Planned,
                DateSpan.Create(house.OfficialStartDate, house.OfficialEndDate).Value,
                Brigade.Create(house.Brigade).Value, null))
            .ForMember(h => h.CurrentState, hr => hr.Ignore())
            .ForMember(h => h.HousePosts, hr => hr.Ignore())
            .ForMember(h => h.HouseWeekInfos, hr => hr.Ignore())
            .ForMember(h => h.RealEndDate, hr => hr.Ignore())
            .ForMember(h => h.RealStartDate, hr => hr.Ignore());

        CreateMap<UpdateHouseRequest, House>()
            .ConstructUsing(house => new House(house.Model, HouseMetrics.Create(house.Length, house.Width).Value,
                new Point(house.TopLeftCornerX, house.TopLeftCornerY),
                house.CurrentState,
                DateSpan.Create(house.OfficialStartDate, house.OfficialEndDate).Value,
                Brigade.Create(house.Brigade).Value, DateSpan.Create(house.RealStartDate, house.RealEndDate).Value))
            .ForMember(h => h.HousePosts, hr => hr.Ignore())
            .ForMember(h => h.HouseWeekInfos, hr => hr.Ignore());

        CreateMap<Post, PostResponse>();
        
        CreateMap<CreatePostRequest, Post>()
            .ConstructUsing(post => new Post(PostName.Create(post.Name).Value))
            .ForMember(p => p.Id, pr => pr.Ignore())
            .ForMember(p => p.Houses, pr => pr.Ignore());
        
        CreateMap<UpdatePostRequest, Post>()
            .ConstructUsing(post => new Post(PostName.Create(post.Name).Value))
            .ForMember(p => p.Houses, pr => pr.Ignore());

        CreateMap<HouseWeekInfo, HouseWeekInfoResponse>()
            .ForMember(hwir => hwir.WeekMarkResponses, hwi => hwi.MapFrom(x => x.WeekComments));
        
        CreateMap<CreateHouseWeekInfoRequest, HouseWeekInfo>()
            .ConstructUsing(houseWeekInfoRequest => new HouseWeekInfo(
                houseWeekInfoRequest.HouseId,
                WeekStartDate.Create(houseWeekInfoRequest.StartDate).Value,
                houseWeekInfoRequest.Status,
                houseWeekInfoRequest.OnTime))
            .ForMember(hwi => hwi.Id, hwi => hwi.Ignore())
            .ForMember(hwi => hwi.House, hwi => hwi.Ignore())
            .ForMember(hwi => hwi.WeekComments, hwi => hwi.Ignore());
        
        CreateMap<UpdateHouseWeekInfoRequest, HouseWeekInfo>()
            .ForMember(hwi => hwi.House, hwi => hwi.Ignore())
            .ForMember(hwi => hwi.WeekComments, hwi => hwi.Ignore());
        
        CreateMap<CreateWeekMarkRequest, WeekMark>()
            .ConstructUsing(weekMarkRequest => new WeekMark(
                weekMarkRequest.HouseWeekInfoId,
                weekMarkRequest.MarkType,
                MarkComment.Create(weekMarkRequest.Comment ?? string.Empty).Value))
            .ForMember(wm => wm.HouseWeekInfo, wm => wm.Ignore())
            .ForMember(wm => wm.Id, wm => wm.Ignore());
        
        CreateMap<UpdateWeekMarkRequest, WeekMark>()
            .ForMember(wm => wm.Comment, request => request.MapFrom(weekMarkRequest => 
                MarkComment.Create(weekMarkRequest.Comment ?? string.Empty).Value.Value))
            .ForMember(wm => wm.HouseWeekInfo, wm => wm.Ignore());
        
        CreateMap<WeekMark, WeekMarkResponse>();

        CreateMap<AuditEntry, AuditEntryResponse>();
        
        CreateMap<Audit, AuditResponse>()
            .ForMember(ar => ar.Changes, a => a.MapFrom(x => x.Changes));
    }
}