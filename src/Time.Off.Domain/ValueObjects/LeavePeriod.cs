namespace Time.Off.Domain.ValueObjects;

public record LeavePeriod
{
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }

    public LeavePeriod() { }

    public LeavePeriod(DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.");

        StartDate = startDate;
        EndDate = endDate;
    }
}



