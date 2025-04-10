using System.Text;
using System.Text.RegularExpressions;
using Hypercube.Graphics.Rendering.Shaders;

namespace Hypercube.Graphics.Rendering.Api.Components;

public sealed class RenderingApiShaderLoader
{
    private const string SectionHeader = "header";
    private const string SectionType = "type";
    private const string SectionCode = "code";
    
    private static readonly Regex Regex;
    private static readonly Dictionary<string, ShaderType> SectionTags = new()
    {
        { "vertex", ShaderType.Vertex },
        { "fragment", ShaderType.Fragment },
        { "geometry", ShaderType.Geometry },
        { "compute", ShaderType.Compute },
        { "tessellation", ShaderType.Tessellation }
    };

    static RenderingApiShaderLoader()
    {
        Regex = new Regex(BuildRegexPattern(), RegexOptions.Multiline);
    }

    public static List<Section> ParseSections(string source)
    {
        var sections = new List<Section>();
     
        if (Regex.Matches(source) is not { } matches)
            return [];
        
        var header = matches.Count > 0 ?
            matches[0].Groups[SectionHeader].Value.Trim() : string.Empty;

        foreach (Match match in matches)
        {
            if (!SectionTags.TryGetValue(match.Groups[SectionType].Value, out var type))
                continue;

            var code = $"{header}{Environment.NewLine}{match.Groups[SectionCode].Value.Trim()}"; 
            sections.Add(new Section(
                type,
                code
            ));
        }

        return sections;
    }
    
    private static string BuildRegexPattern()
    {
        var builder = new StringBuilder();
        
        builder.Append($"^(?<{SectionHeader}>.*?)");
        builder.Append(@$"\[(?<{SectionType}>");
        
        var first = true;
        foreach (var tag in SectionTags.Keys)
        {
            if (!first)
                builder.Append('|');
            
            builder.Append(tag);
            first = false;
        }
        
        builder.Append(@$")\](?<{SectionCode}>.*?)(?=\[|$)");
            return builder.ToString();
    }
    
    public readonly struct Section
    {
        public readonly ShaderType Type;
        public readonly string Source;

        public Section(ShaderType type, string source)
        {
            Type = type;
            Source = source;
        }
    }
}