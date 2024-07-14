namespace Hypercube.Shared.Utilities.Extensions;

public static class StringExtension
{
    public static string RemoveChar(this string str, char @char) {
        var len = str.Length;
        var chars = str.ToCharArray();
        var destinationIndex = 0;
        
        for (var i = 0; i < len; i++) {
            var currentChar = chars[i];
            if (currentChar == @char)
                continue;
            
            chars[destinationIndex++] = currentChar;
        }
        
        return new string(chars, 0, destinationIndex);
    }
}