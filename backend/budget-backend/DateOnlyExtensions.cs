namespace budget_backend;

public static class DateOnlyExtensions
{
    public static DateOnly Today()
    {
        return DateOnly.FromDateTime(DateTime.Now);
    }
}