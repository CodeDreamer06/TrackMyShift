﻿using ConsoleTableExt;
using System.Globalization;

namespace TrackMyShiftClient
{
    internal class Helpers
    {
        public const string Message = @"
# Welcome to your TrackMyShift Client!
  Type 'help' to show this message again.
 * exit or 0: stop the program
 * show [optional: id]: display all your shifts
 * add: log a new shift
 * update [id]: edit an existing log
 * remove [id]: delete a log
";

        public const string NoResultsMessage = @"There are no logs currently saved. Type 'help' to learn how to log a new shift.";

        private static readonly Dictionary<HeaderCharMapPositions, char> HeaderCharacterMap = new()
        {
            { HeaderCharMapPositions.TopLeft, '╒' },
            { HeaderCharMapPositions.TopCenter, '╤' },
            { HeaderCharMapPositions.TopRight, '╕' },
            { HeaderCharMapPositions.BottomLeft, '╞' },
            { HeaderCharMapPositions.BottomCenter, '╪' },
            { HeaderCharMapPositions.BottomRight, '╡' },
            { HeaderCharMapPositions.BorderTop, '═' },
            { HeaderCharMapPositions.BorderRight, '│' },
            { HeaderCharMapPositions.BorderBottom, '═' },
            { HeaderCharMapPositions.BorderLeft, '│' },
            { HeaderCharMapPositions.Divider, '│' },
        };

        internal static Shift GetShiftDetails(int? id = null, bool existingShift = false)
        {
            var shift = new Shift();

            if (existingShift) Console.WriteLine("Press enter if you don't want to change the value.");

            foreach (var property in shift.GetType().GetProperties())
            {
                if (property.Name == "Id")
                {
                    if (id.HasValue) shift.Id = id.Value;
                    continue;
                }

                if (property.Name == "Duration")
                {
                    if (shift.End is not null || shift.Start is not null)
                        shift.Duration = (decimal) (shift.End - shift.Start)?.TotalMinutes!;
                    continue;
                }

                Console.Write(property.Name + ": ");
                var userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput)) continue;

                var value = Convert.ChangeType(userInput, Nullable.GetUnderlyingType(property.PropertyType)!);

                property.SetValue(shift, value);
            }

            return new ShiftsService().ReplaceEmptyFields(shift);
        }

        public static void DisplayTable<T>(List<T> records, string emptyMessage) where T : class
        {
            if (records.Count == 0)
            {
                Console.WriteLine(emptyMessage);
                return;
            }

            ConsoleTableBuilder.From(records)
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition, HeaderCharacterMap)
                .ExportAndWriteLine();
        }

        public static string CorrectSpelling(string command)
        {
            var definitions = new List<string> { "exit", "help", "show", "add", "update", "remove" };
            string correctDefinition = "";
            int maxPercentage = 0;

            try
            {
                foreach (var definition in definitions)
                {
                    int matchPercentage = FuzzySharp.Fuzz.Ratio(command, definition);

                    if (matchPercentage > maxPercentage)
                    {
                        maxPercentage = matchPercentage;
                        correctDefinition = definition;
                    }
                }

                return maxPercentage > 40 ? $"Did you mean {correctDefinition}?" : "";
            }

            catch
            {
                return "";
            }
        }

        public static (bool, decimal) IsNumber(object testObject)
        {
            bool isNum = decimal.TryParse(
                Convert.ToString(testObject),
                NumberStyles.Any,
                NumberFormatInfo.InvariantInfo,
                out decimal number);

            return (isNum, number);
        }
    }

    public static class Extensions
    {
        public static string RemoveKeyword(this string str, string keyword) => str.Replace(keyword + " ", "");

        public static int GetNumber(this string str, string keyword) => 
            (int)Helpers.IsNumber(str.Replace(keyword + " ", "")).Item2;
    }
}