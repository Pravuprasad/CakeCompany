namespace CakeCompany.Provider.Time;

internal interface ITimeProvider
{
    DateTime Now { get; }
}