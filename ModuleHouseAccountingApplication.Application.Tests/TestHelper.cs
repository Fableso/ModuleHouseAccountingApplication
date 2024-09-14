using Application.Abstractions;
using Application.DTO.House.Request;
using Application.DTO.House.Response;
using Application.DTO.Post.Response;
using Application.DTO.WeekMark.Response;
using Application.Mappers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Infrastructure.Data;

namespace ModuleHouseAccountingApplication.Application.Tests;

public static class TestHelper
{
    public static IMapper CreateMapperProfile()
    {
        var myProfile = new AutomapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

        return new Mapper(configuration);
    }

    public static void SeedData(MhDbContext context)
    {
        var post1 = new Post(PostName.Create("FirstPost").Value) { Id = new PostId(1), Area = 50 };
        var post2 = new Post(PostName.Create("SecondPost").Value) { Id = new PostId(2) };
        var post3 = new Post(PostName.Create("ThirdPost").Value) { Id = new PostId(3), Area = 140 };
        context.Posts.AddRange(post1, post2, post3);

        var house1 = new House(new HouseId("MB 110-1"),
            HouseMetrics.Create(10.5, 16.34).Value, new Point(4, 8), HouseStatus.Planned,
            DateSpan.Create(new DateOnly(2024, 7, 15), new DateOnly(2024, 9, 12)).Value,
            Brigade.Create("MarkusBrigade").Value);

        var house2 = new House(new HouseId("MB 140-1"),
            HouseMetrics.Create(15, 20.67).Value,
            new Point(-5, 19), HouseStatus.InProcess,
            DateSpan.Create(new DateOnly(2023, 9, 8), new DateOnly(2024, 1, 19)).Value,
            Brigade.Create("Productive brigade").Value,
            DateSpan.Create(new DateOnly(2023, 9, 8), new DateOnly(2024, 1, 10)).Value);

        var house3 = new House(new HouseId("MB 56-1"),
            HouseMetrics.Create(8, 7.5).Value,
            new Point(15, 13), HouseStatus.InProcess,
            DateSpan.Create(new DateOnly(2025, 8, 20), new DateOnly(2025, 10, 1)).Value,
            Brigade.Create("MarkusBrigade").Value);

        context.Houses.AddRange(house1, house2, house3);

        context.HousePosts.AddRange(
            new HousePost(house1.Id, post1.Id),
            new HousePost(house1.Id, post2.Id),
            new HousePost(house2.Id, post1.Id),
            new HousePost(house2.Id, post2.Id),
            new HousePost(house2.Id, post3.Id),
            new HousePost(house3.Id, post1.Id)
        );

        var houseWeekInfo1 = new HouseWeekInfo(house1.Id, new DateOnly(2024, 8, 1), WeekStatus.InProcess, true);
        var houseWeekInfo2 = new HouseWeekInfo(house1.Id, new DateOnly(2024, 8, 8), WeekStatus.OnHold, true);
        var houseWeekInfo3 = new HouseWeekInfo(house1.Id, new DateOnly(2024, 8, 15), WeekStatus.OnHold, false);
        var houseWeekInfo4 = new HouseWeekInfo(house2.Id, new DateOnly(2023, 9, 8), WeekStatus.InProcess, false);
        var houseWeekInfo5 = new HouseWeekInfo(house2.Id, new DateOnly(2023, 9, 15), WeekStatus.InProcess, true);
        var houseWeekInfo6 = new HouseWeekInfo(house3.Id, new DateOnly(2025, 8, 20), WeekStatus.OnHold, false);
        var houseWeekInfo7 = new HouseWeekInfo(house3.Id, new DateOnly(2025, 8, 27), WeekStatus.OnHold, true);
        var houseWeekInfo8 = new HouseWeekInfo(house3.Id, new DateOnly(2025, 9, 5), WeekStatus.OnHold, false);

        context.HouseWeekInfos.AddRange(
            houseWeekInfo1, houseWeekInfo2, houseWeekInfo3, houseWeekInfo4, houseWeekInfo5, houseWeekInfo6,
            houseWeekInfo7, houseWeekInfo8
        );

        context.SaveChanges();

        context.WeekMarks.AddRange(
            new WeekMark(houseWeekInfo1.Id, MarkType.BlueMark, MarkComment.Create("Adjustment comment").Value),
            new WeekMark(houseWeekInfo2.Id, MarkType.RedMark, MarkComment.Create("Notes").Value),
            new WeekMark(houseWeekInfo2.Id, MarkType.ProductionStart, MarkComment.Create("Production started").Value),
            new WeekMark(houseWeekInfo2.Id, MarkType.ProductionEnd, MarkComment.Create("Production ended").Value),
            new WeekMark(houseWeekInfo3.Id, MarkType.GreenMark, MarkComment.Create("Requirements notes").Value),
            new WeekMark(houseWeekInfo4.Id, MarkType.BlueMark, MarkComment.Create("Other").Value),
            new WeekMark(houseWeekInfo4.Id, MarkType.GreenMark, MarkComment.Create("Green mark Test comment").Value),
            new WeekMark(houseWeekInfo5.Id, MarkType.RedMark, MarkComment.Create("Bricks needed").Value),
            new WeekMark(houseWeekInfo6.Id, MarkType.PurpleMark, MarkComment.Create("A large comment").Value),
            new WeekMark(houseWeekInfo6.Id, MarkType.GreenMark, MarkComment.Create("Test comment").Value),
            new WeekMark(houseWeekInfo7.Id, MarkType.BlueMark, MarkComment.Create("I love .NET").Value),
            new WeekMark(houseWeekInfo8.Id, MarkType.RedMark, MarkComment.Create("Another test comment").Value),
            new WeekMark(houseWeekInfo8.Id, MarkType.YellowMark, MarkComment.Create("Yellow comment").Value)
        );

        context.SaveChanges();
    }

    public static IEnumerable<HouseResponse> ExpectedHouseResponses()
    {
        yield return new HouseResponse
        {
            Model = new HouseId("MB 110-1"),
            Length = 10.5,
            Width = 16.34,
            TopLeftCornerX = 4,
            TopLeftCornerY = 8,
            CurrentState = HouseStatus.Planned,
            OfficialStartDate = new DateOnly(2024, 7, 15),
            OfficialEndDate = new DateOnly(2024, 9, 12),
            Brigade = "MarkusBrigade",
        };
        yield return new HouseResponse
        {
            Model = new HouseId("MB 140-1"),
            Length = 15,
            Width = 20.67,
            TopLeftCornerX = -5,
            TopLeftCornerY = 19,
            CurrentState = HouseStatus.InProcess,
            OfficialStartDate = new DateOnly(2023, 9, 8),
            OfficialEndDate = new DateOnly(2024, 1, 19),
            RealStartDate = new DateOnly(2023, 9, 8),
            RealEndDate = new DateOnly(2024, 1, 10),
            Brigade = "Productive brigade",
        };
        yield return new HouseResponse
        {
            Model = new HouseId("MB 56-1"),
            Length = 8,
            Width = 7.5,
            TopLeftCornerX = 15,
            TopLeftCornerY = 13,
            CurrentState = HouseStatus.InProcess,
            OfficialStartDate = new DateOnly(2025, 8, 20),
            OfficialEndDate = new DateOnly(2025, 10, 1),
            Brigade = "MarkusBrigade",
        };
    }
    
    public static CreateHouseRequest GetCreateHouseRequest() => new()
    {
        Id = new HouseId("MB 200-1"),
        Length = 20.0,
        Width = 30.0,
        TopLeftCornerX = 10,
        TopLeftCornerY = 20,
        OfficialStartDate = new DateOnly(2024, 1, 1),
        OfficialEndDate = new DateOnly(2024, 12, 31),
        Brigade = "NewBrigade",
        PostIds = new List<PostId> { new PostId(1), new PostId(2) }
    };
    
    public static UpdateHouseRequest GetUpdateHouseRequest() => new()
    {
        Id = new HouseId("MB 110-1"),
        Length = 20.0,
        Width = 30.0,
        TopLeftCornerX = 10,
        TopLeftCornerY = 20,
        OfficialStartDate = new DateOnly(2024, 1, 1),
        OfficialEndDate = new DateOnly(2024, 12, 31),
        Brigade = "NewBrigade",
        PostIds = new List<PostId> { new PostId(3), new PostId(2) }
    };
}