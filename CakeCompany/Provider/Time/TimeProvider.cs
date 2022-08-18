namespace CakeCompany.Provider.Time;

internal class TimeProvider : ITimeProvider
{
    public DateTime Now { get; } = DateTime.Now;
}