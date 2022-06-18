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

            if (command == "help") Console.WriteLine(Helpers.Message);

            else if (string.IsNullOrWhiteSpace(command)) continue;

            else if (command.StartsWith("show"))
                api.Get((int)Helpers.IsNumber(command.RemoveKeyword("show ")).Item2);

            else if (command.StartsWith("add"))
            {
                var shift = new Shift();

                foreach (var property in shift.GetType().GetProperties())
                {
                    Console.Write(property.Name + ": ");
                    var value = Convert.ChangeType(Console.ReadLine()!, property.GetType());
                    property.SetValue(shift, value);
                }

                api.Post(shift);
            }

            else if (command.StartsWith("update")) api.Put();

            else if (command.StartsWith("remove")) api.Delete();

            else
            {
                string suggestion = Helpers.CorrectSpelling(command.Split()[0]);
                Console.WriteLine($"Not a command. Type 'help' for the list of all commands. {suggestion}");
            }
        }
    }
}
