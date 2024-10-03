using Application.DTO.House.Response;
using Application.DTO.HouseWeekInfo.Response;
using Application.DTO.Post.Response;
using Application.DTO.WeekMark.Response;

namespace ModuleHouseAccountingApplication.Application.Tests;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class WeekMarkResponseComparer : IEqualityComparer<WeekMarkResponse>
{
    public bool Equals(WeekMarkResponse? x, WeekMarkResponse? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return Equals(x.Id, y.Id)
            && Equals(x.HouseWeekInfoId, y.HouseWeekInfoId)
            && x.MarkType == y.MarkType
            && string.Equals(x.Comment, y.Comment, StringComparison.Ordinal);
    }

    public int GetHashCode(WeekMarkResponse obj)
    {
        int hash = 17;
        hash = hash * 23 + obj.Id.GetHashCode();
        hash = hash * 23 + obj.HouseWeekInfoId.GetHashCode();
        hash = hash * 23 + obj.MarkType.GetHashCode();
        hash = hash * 23 + (obj.Comment != null ? obj.Comment.GetHashCode(StringComparison.Ordinal) : 0);
        return hash;
    }
}

// Comparer for HouseWeekInfoResponse
public sealed class HouseWeekInfoResponseComparer : IEqualityComparer<HouseWeekInfoResponse>
{

    public bool Equals(HouseWeekInfoResponse? x, HouseWeekInfoResponse? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return Equals(x.Id, y.Id)
               && Equals(x.HouseModel, y.HouseModel)
               && x.StartDate == y.StartDate
               && x.OnTime == y.OnTime
               && x.Status == y.Status;
    }

    public int GetHashCode(HouseWeekInfoResponse obj)
    {
        var hash = 17;
        hash = hash * 23 + obj.Id.GetHashCode();
        hash = hash * 23 + obj.HouseModel.GetHashCode();
        hash = hash * 23 + obj.StartDate.GetHashCode();
        hash = hash * 23 + obj.OnTime.GetHashCode();
        hash = hash * 23 + obj.Status.GetHashCode();
        return hash;
    }
}

// Comparer for PostResponse
public sealed class PostResponseComparer : IEqualityComparer<PostResponse>
{
    public bool Equals(PostResponse? x, PostResponse? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return Equals(x.Id, y.Id)
            && string.Equals(x.Name, y.Name, StringComparison.Ordinal)
            && Nullable.Equals(x.Area, y.Area);
    }

    public int GetHashCode(PostResponse obj)
    {
        int hash = 17;
        hash = hash * 23 + obj.Id.GetHashCode();
        hash = hash * 23 + obj.Name.GetHashCode(StringComparison.Ordinal);
        hash = hash * 23 + (obj.Area.HasValue ? obj.Area.GetHashCode() : 0);
        return hash;
    }
}

// Comparer for HouseResponse
public sealed class HouseResponseComparer : IEqualityComparer<HouseResponse>
{
    private const double Tolerance = 0.0001;

    public bool Equals(HouseResponse? x, HouseResponse? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;


        return Equals(x.Model, y.Model)
               && Math.Abs(x.Length - y.Length) < Tolerance
               && Math.Abs(x.Width - y.Width) < Tolerance
               && x.TopLeftCornerX == y.TopLeftCornerX
               && x.TopLeftCornerY == y.TopLeftCornerY
               && x.OfficialStartDate == y.OfficialStartDate
               && Nullable.Equals(x.OfficialEndDate, y.OfficialEndDate)
               && Nullable.Equals(x.RealStartDate, y.RealStartDate)
               && Nullable.Equals(x.RealEndDate, y.RealEndDate)
               && x.CurrentState == y.CurrentState
               && string.Equals(x.Brigade, y.Brigade, StringComparison.Ordinal);
    }

    public int GetHashCode(HouseResponse obj)
    {
        int hash = 17;
        hash = hash * 23 + obj.Model.GetHashCode();
        hash = hash * 23 + obj.Length.GetHashCode();
        hash = hash * 23 + obj.Width.GetHashCode();
        hash = hash * 23 + obj.TopLeftCornerX.GetHashCode();
        hash = hash * 23 + obj.TopLeftCornerY.GetHashCode();
        hash = hash * 23 + obj.OfficialStartDate.GetHashCode();
        hash = hash * 23 + (obj.OfficialEndDate.HasValue ? obj.OfficialEndDate.Value.GetHashCode() : 0);
        hash = hash * 23 + (obj.RealStartDate.HasValue ? obj.RealStartDate.Value.GetHashCode() : 0);
        hash = hash * 23 + (obj.RealEndDate.HasValue ? obj.RealEndDate.Value.GetHashCode() : 0);
        hash = hash * 23 + obj.CurrentState.GetHashCode();
        hash = hash * 23 + obj.Brigade.GetHashCode(StringComparison.Ordinal);
        return hash;
    }
}
