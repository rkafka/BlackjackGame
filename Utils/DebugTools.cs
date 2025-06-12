namespace BlackjackGame.DebugTools;

class DebugTools
{
    public static int titleNumber = 1;
    public static Dictionary<string, int> titleCounts = [];
    public static void PrintSectionHeader(string title, bool addNumber = true)
    {
        if (addNumber)
        {
            titleNumber++;
            // if title is repeated, indicate so
            if (!titleCounts.ContainsKey(title))
            {
                titleCounts[title] = 1;
                title = $" {titleNumber}. {title} ";
            }
            else
            {
                titleCounts[title]++;
                title = $" {titleNumber}. {title} ({titleCounts[title]}) ";
            }
        }
        else
            title = $" {title} ";

        int numLines = Console.WindowWidth - title.Length;
        // if numLines odd, truncation occurs below
        Console.Write("[".PadLeft(numLines / 2, '-') + title);
        Console.Write("]".PadRight(numLines / 2, '-'));
        // Adjust title for numLines' truncation, if needed
        Console.WriteLine(((numLines % 2 == 1) ? "-" : "") + "\n");
    }
}