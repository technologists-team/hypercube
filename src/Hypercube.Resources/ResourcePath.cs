using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Hypercube.Resources;

[PublicAPI]
public readonly struct ResourcePath : IEquatable<ResourcePath>
{
    /// <summary>
    /// Separator used in paths (Unix-style).
    /// </summary>
    public const char Separator = '/';
    
    /// <summary>
    /// Separator used in paths (Unix-style).
    /// </summary>
    public const string SeparatorStr = "/";

    /// <summary>
    /// Separator used in Windows paths.
    /// </summary>
    public const char WinSeparator = '\\';
    
    /// <summary>
    /// Separator used in Windows paths.
    /// </summary>
    public const string WinSeparatorStr = "\\";

    /// <summary>
    /// Platform-specific separator (depends on the operating system).
    /// </summary>
    public static readonly char SystemSeparator;

    /// <summary>
    /// Platform-specific separator as string (depends on the operating system).
    /// </summary>
    public static readonly string SystemSeparatorStr;

    /// <summary>
    /// Represents the current directory ("./").
    /// </summary>
    public static readonly ResourcePath Self = ".";

    public static readonly ResourcePath Empty = string.Empty;
    
    /// <summary>
    /// Static constructor to initialize platform-specific separators.
    /// </summary>
    static ResourcePath()
    {
        SystemSeparator = OperatingSystem.IsWindows() ? '\\' : '/';
        SystemSeparatorStr = OperatingSystem.IsWindows() ? "\\" : "/";
    }

    /// <summary>
    /// The actual path represented by this instance.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Indicates whether the path is rooted (starts with the separator).
    /// </summary>
    public bool Rooted => Value.Length > 0 && Value[0] == Separator;

    /// <summary>
    /// Indicates whether the path is relative (does not start with the separator).
    /// </summary>
    public bool Relative => !Rooted;

    /// <summary>
    /// Indicates whether the path refers to the current directory (".").
    /// </summary>
    public bool IsSelf => Value == Self;

    public bool IsEmpty => Value == Empty;
    
    /// <summary>
    /// Extracts the filename with its extension from the path.
    /// </summary>
    public string FilenameWithExt
    {
        get
        {
            var sepIndex = Value.LastIndexOf(Separator) + 1;
            return sepIndex == -1 ? string.Empty : Value[sepIndex..];
        }
    }

    /// <summary>
    /// Extracts the file extension from the filename.
    /// </summary>
    public string Extension
    {
        get
        {
            var filename = FilenameWithExt;
            var extIndex = filename.LastIndexOf('.');
            return extIndex > 0 ? filename[extIndex..] : string.Empty;
        }
    }

    /// <summary>
    /// Extracts the filename without its extension.
    /// </summary>
    public string Filename
    {
        get
        {
            var filename = FilenameWithExt;
            var extIndex = filename.LastIndexOf('.');
            return extIndex > 0 ? filename[..extIndex] : filename;
        }
    }

    /// <summary>
    /// Returns the parent directory of the current path.
    /// </summary>
    public ResourcePath ParentDirectory
    {
        get
        {
            if (IsSelf)
                return Self;

            var lastIndex = Value.Length > 1 && Value[^1] == Separator
                ? Value[..^1].LastIndexOf(Separator)
                : Value.LastIndexOf(Separator);

            return lastIndex switch
            {
                -1 => Self,
                0 => new ResourcePath(Value[..1]),
                _ => new ResourcePath(Value[..lastIndex])
            };
        }
    }

    /// <summary>
    /// Normalizes the path by removing redundant separators and resolving '.' and '..'.
    /// </summary>
    public ResourcePath Normalized
    {
        get
        {
            if (IsEmpty || IsSelf)
                return this;

            var segments = Value.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            var stack = new Stack<string>();

            foreach (var segment in segments)
            {
                if (segment == ".")
                    continue;
            
                if (segment == "..")
                {
                    if (stack.Count > 0 && stack.Peek() != "..")
                    {
                        stack.Pop();
                        continue;
                    }
                    
                    if (!Rooted)
                    {
                        stack.Push("..");
                        continue;
                    }
                    
                    continue;
                }

                stack.Push(segment);
            }

            var normalized = string.Join(SeparatorStr, stack.Reverse());
            return Rooted ? SeparatorStr + normalized : normalized;
        }
    }
    
    /// <summary>
    /// Constructor that initializes the path and converts it to the appropriate separator for the platform.
    /// </summary>
    /// <param name="value">The path string.</param>
    public ResourcePath(string value)
    {
        Value = OperatingSystem.IsWindows() ? value.Replace('\\', '/') : value;
    }
    
    /// <summary>
    /// Tries to calculate the relative path to a given base path.
    /// </summary>
    public bool TryRelativeTo(ResourcePath basePath, [NotNullWhen(true)] out ResourcePath? relative)
    {
        if (this == basePath)
        {
            relative = Self;
            return true;
        }

        if (basePath == Self && Relative)
        {
            relative = this;
            return true;
        }

        if (Value.StartsWith(basePath.Value))
        {
            var x = Value[basePath.Value.Length..].Trim(Separator);
            relative = string.IsNullOrEmpty(x) ? Self : new ResourcePath(x);
            return true;
        }

        relative = null;
        return false;
    }
    
    public bool IsChildOf(ResourcePath parent)
    {
        if (!parent.Rooted || !Rooted)
            return false;

        // Normalize both paths for comparison
        var parentPath = parent.Value.TrimEnd(Separator) + Separator;
        var thisPath = Value;

        // Check if this path starts with parent path and is longer
        return thisPath.StartsWith(parentPath) &&
               thisPath.Length > parentPath.Length;
    }

    /// <summary>
    /// Checks if the current ResourcePath is equal to another.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(ResourcePath other)
    {
        return Value == other.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object? obj)
    {
        if (obj is not ResourcePath other)
            return false;

        return Value == other.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Creates a ResourcePath from a relative system path, replacing the specified separator.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResourcePath FromRelativeSystemPath(string path, char newSeparator)
    {
        return new ResourcePath(path.Replace(newSeparator, Separator));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ResourcePath(string value)
    {
        return new ResourcePath(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator string(ResourcePath path)
    {
        return path.Value;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ResourcePath operator +(ResourcePath left, ResourcePath right)
    {
        if (right.IsSelf)
            return left;

        if (right.Rooted)
            return right;

        if (left.IsEmpty)
            return new ResourcePath($"/{right.Value}");

        return left.Value.EndsWith(Separator)
            ? new ResourcePath(left.Value + right.Value)
            : new ResourcePath(left.Value + Separator + right.Value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(ResourcePath left, ResourcePath right)
    {
        return left.Equals(right);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(ResourcePath left, ResourcePath right)
    {
        return !left.Equals(right);
    }
}