namespace Domain;

public static class DomainConstants
{
    public const int MaxWeekMarkCommentLength = 2000;
    public const int MaxBrigadeNameLength = 50;
    public const int MaxAuthorNameLength = 50;
    public const int MaxPostNameLength = 100;
    public static readonly DateOnly MinDate = new(2021, 1, 1);
}