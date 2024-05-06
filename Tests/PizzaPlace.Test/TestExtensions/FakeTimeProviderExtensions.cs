namespace PizzaPlace.Test.TestExtensions;

public static class FakeTimeProviderExtensions
{
    public static void PassTimeInMinuteIntervals(this FakeTimeProvider fakeTimeProvider, int minutesToPass)
    {
        while (minutesToPass-- > 0)
        {
            fakeTimeProvider.Advance(TimeSpan.FromMinutes(1));
        }
    }
}
