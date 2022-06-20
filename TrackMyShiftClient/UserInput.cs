namespace TrackMyShiftClient;

internal class UserInput
{
    internal static void ShowMenu()
    {
        while (true)
        {
            var rawCommand = Console.ReadLine()!;
            var command = rawCommand.Trim().ToLower();
            var api = new ShiftsService();

            if (command is "exit" or "0") break;

            if (command is "help")
                Console.WriteLine(Helpers.Message);

            else if (string.IsNullOrWhiteSpace(command)) continue;

            else if (command.StartsWith("show")) 
                api.Get(api.GetAbsoluteId(command.GetNumber("show")));

            else if (command is "add")
                api.Post(Helpers.GetShiftDetails());

            else if (command.StartsWith("update"))
                api.Put(Helpers.GetShiftDetails(command.GetNumber("update"), true));

            else if (command.StartsWith("remove"))
                api.Delete(api.GetAbsoluteId(command.GetNumber("remove")));

            else
            {
                string suggestion = Helpers.CorrectSpelling(command.Split()[0]);
                Console.WriteLine($"Not a command. Type 'help' for the list of all commands. {suggestion}");
            }
        }
    }
}
