using BlackjackGame.Models;

namespace BlackjackGame.UI;

public static class UIHelper
{
    private static int titleNumber = 0;
    private static Dictionary<string, int> _titleCounts = [];

    /// <summary> Method for breaking up sections and adding a title </summary>
    /// <param name="title"></param>
    /// <param name="addNumber">Optional toggle for whether to enumerate the section title</param>
    public static void PrintSectionHeader(string header, bool addNumber = false, bool markDuplicates=false)
    {
        if (addNumber)
        {
            titleNumber++;
            header = $" {titleNumber}. {header} ";
            // if title is repeated, indicate so
            if (!_titleCounts.ContainsKey(header))
            {
                _titleCounts[header] = 1;
            }
            else
            {
                _titleCounts[header]++;
                if (markDuplicates)
                    header += $"({_titleCounts[header]}) ";

            }
        }
        else
            header = $" {header} ";

        int numLines = Console.WindowWidth - header.Length;
        // if numLines odd, truncation occurs below
        Console.Write("[".PadLeft(numLines / 2, '-') + header);
        Console.Write("]".PadRight(numLines / 2, '-'));
        // Adjust title for numLines' truncation, if needed
        Console.WriteLine(((numLines % 2 == 1) ? "-" : "") + "\n");
    }

    // method for capitalizing the first letter of a string
    public static string CapitalizeFirstLetter(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }
}