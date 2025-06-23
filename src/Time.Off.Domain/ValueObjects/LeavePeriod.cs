namespace Time.Off.Domain.ValueObjects;

public record LeavePeriod
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public LeavePeriod() { }

    public LeavePeriod(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.");

        StartDate = startDate;
        EndDate = endDate;
    }
}



