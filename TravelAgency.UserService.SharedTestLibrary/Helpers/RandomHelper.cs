namespace TravelAgency.UserService.SharedTestLibrary.Helpers;
public static class RandomHelper
{
    private static Random _random = new Random();

    public static int Next(int maxValue)
    {
        return _random.Next(maxValue);
    }

    public static int Next(int minValue, int maxValue)
    {
        return _random.Next(minValue, maxValue);
    }
}
