namespace TrackMyShiftClient;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(Helpers.Message);
        UserInput.ShowMenu();
    }
}