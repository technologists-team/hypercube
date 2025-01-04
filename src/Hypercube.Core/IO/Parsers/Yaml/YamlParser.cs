using System.Text;

namespace Hypercube.Core.IO.Parsers.Yaml;

public static class YamlParser
{
    // Private constants for quotes, escape characters, and comment symbol
    private const char DoubleQuote = '"';
    private const char SingleQuote = '\'';
    private const char EscapeChar = '\\';
    private const char CommentChar = '#';

    /// <summary>
    /// Parses the provided YAML string into a dictionary of prototypes.
    /// </summary>
    /// <param name="yamlData">The YAML string to be parsed.</param>
    /// <returns>A dictionary with prototype IDs as keys and field dictionaries as values.</returns>
    public static Dictionary<string, Dictionary<string, string>> ParseYaml(string yamlData)
    {
        var result = new Dictionary<string, Dictionary<string, string>>();
        var lines = yamlData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        string? currentId = null;
        
        var currentFields = new Dictionary<string, string>();
        var inString = false;

        foreach (var line in lines)
        {
            var cleanedLine = ProcessLine(line, ref inString);

            // Ignore empty or commented-out lines
            if (string.IsNullOrWhiteSpace(cleanedLine))
                continue;

            var parts = cleanedLine.Split(':', 2, StringSplitOptions.TrimEntries);
            
            // Process a new prototype section (ID and fields)
            if (!line.StartsWith(" ") && cleanedLine.Contains(":"))
            {
                if (currentId != null)
                    result[currentId] = currentFields;
                
                currentId = parts[0];
                currentFields = new Dictionary<string, string>();
                continue;
            }

            if (!line.StartsWith(" "))
                continue;

            if (parts.Length != 2)
                continue;
            
            currentFields[parts[0]] = RemoveQuotesAndEscape(parts[1]);
        }

        // Add the last prototype if present
        if (currentId != null)
            result[currentId] = currentFields;

        return result;
    }

    /// <summary>
    /// Processes a line by stripping out comments and handling quote and escape characters.
    /// </summary>
    /// <param name="line">The line to process.</param>
    /// <param name="inString">Indicates whether we are currently inside a string.</param>
    /// <returns>A cleaned-up line with comments removed and quotes handled.</returns>
    private static string ProcessLine(string line, ref bool inString)
    {
        var cleanedLine = string.Empty;
        foreach (var ch in line)
        {
            // Toggle string state on encountering a quote
            if (ch is DoubleQuote or SingleQuote)
                inString = !inString;

            // Ignore comments if not inside a string
            // And stop processing the line at the comment symbol
            if (ch == CommentChar && !inString)
                break;

            cleanedLine += ch;
        }

        return cleanedLine.Trim();
    }

    /// <summary>
    /// Removes surrounding quotes and handles escape sequences inside the string.
    /// </summary>
    /// <param name="value">The value to process.</param>
    /// <returns>The value with quotes removed and escape sequences properly handled.</returns>
    private static string RemoveQuotesAndEscape(string value)
    {
        // Check for open/close quote mismatch
        var startsWithQuote = value.StartsWith(DoubleQuote) || value.StartsWith(SingleQuote);
        var endsWithQuote = value.EndsWith(DoubleQuote) || value.EndsWith(SingleQuote);

        if (startsWithQuote && !endsWithQuote)
            throw new FormatException("String starts with a quote but does not end with one.");

        if (!startsWithQuote && endsWithQuote)
            throw new FormatException("String ends with a quote but does not start with one.");

        // Remove surrounding quotes if they exist
        if ((value.StartsWith(DoubleQuote) && value.EndsWith(DoubleQuote)) || 
            (value.StartsWith(SingleQuote) && value.EndsWith(SingleQuote)))
        {
            value = value.Substring(1, value.Length - 2);  // Remove quotes
        }

        var result = string.Empty;
        var isEscaped = false;

        // Process each character in the string
        foreach (var currentChar in value)
        {
            // If we're escaping the next character
            if (isEscaped)
            {
                result += currentChar switch
                {
                    'n' => "\n", // Handle newline escape
                    't' => "\t", // Handle tab escape
                    'r' => "\r", // Handle carriage return escape
                    '\\' => "\\", // Handle escaped backslash
                    '"' => "\"", // Handle escaped double quote
                    '\'' => "'", // Handle escaped single quote
                    _ => throw new FormatException($"Invalid escape sequence: \\{currentChar}")
                };

                isEscaped = false;
                continue;
            }

            // Detect escape sequence
            if (currentChar == EscapeChar)
            {
                isEscaped = true;
                continue;
            }

            result += currentChar;
        }

        // If we're still escaping after the loop, it's an invalid escape sequence
        if (isEscaped)
            throw new FormatException("String ends with an incomplete escape sequence.");

        return result;
    }
}