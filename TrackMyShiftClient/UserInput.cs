﻿namespace TrackMyShiftClient;

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

            else if (command.StartsWith("show")) api.Get(command.GetNumber("show"));

            else if (command == "add") api.Post(Helpers.GetShiftDetails());

            else if (command.StartsWith("update ")) api.Put(Helpers.GetShiftDetails());

            else if (command.StartsWith("remove ")) api.Delete(command.GetNumber("remove"));

            else
            {
                string suggestion = Helpers.CorrectSpelling(command.Split()[0]);
                Console.WriteLine($"Not a command. Type 'help' for the list of all commands. {suggestion}");
            }
        }
    }
}
