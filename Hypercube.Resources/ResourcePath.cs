using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Hypercube.Resources;

[PublicAPI]
public readonly struct ResourcePath
{
    public const char Separator = '/';
    public const string SeparatorStr = "/";
    
    public const char WinSeparator = '\\';
    public const string WinSeparatorStr = @"\";
    
    public static readonly char SystemSeparator;
    public static readonly string SystemSeparatorStr;

    public static readonly ResourcePath Self = ".";
    
    static ResourcePath()
    {
        SystemSeparator = OperatingSystem.IsWindows() ? '\\' : '/';
        SystemSeparatorStr = OperatingSystem.IsWindows() ? "\\" : "/";
    }
    
    public ResourcePath(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            Path = path.Replace('\\', '/');
            return;
        }

        Path = path;
    }

    public readonly string Path { get; }

    public bool Rooted => Path.Length > 0 && Path[0] == Separator;
    public bool Relative => !Rooted;
    public bool IsSelf => Path == Self.Path;

    public string FilenameWithExt
    {
        get
        {
            var sepIndex = Path.LastIndexOf('/') + 1;
            return sepIndex == -1 ? string.Empty : Path[sepIndex..];
        }
    }

    public string Extension
    {
        get
        {
            var filename = FilenameWithExt;
            
            var ind = filename.LastIndexOf('.');
            return ind <= 1
                ? string.Empty
                : filename[ind..];
        }
    }

    public string Filename
    {
        get
        {
            var filename = FilenameWithExt;

            var ind = filename.LastIndexOf('.');
            return ind <= 0
                ? filename
                : filename[..ind];
        }
    }

    public ResourcePath ParentDirectory
    {
        get
        {
            if (IsSelf)
                return Self;
            
            var idx = Path.Length > 1 && Path[^1] == '/'
                ? Path[..^1].LastIndexOf('/')
                : Path.LastIndexOf('/');

            return idx switch
            {
                -1 => Self,
                0 => new ResourcePath(Path[..1]),
                _ => new ResourcePath(Path[..idx])
            };
        }
    }

    public static implicit operator ResourcePath(string path)
    {
        return new ResourcePath(path);
    }
    public static implicit operator string(ResourcePath path)
    {
        return path.Path;
    }
    
    public static ResourcePath operator +(ResourcePath l, ResourcePath r)
    {
        if (r.IsSelf)
            return l;
        
        if (r.Rooted)
            return r;
        
        if (l.Path == string.Empty)
            return new ResourcePath($"/{r.Path}");

        if (l.Path.EndsWith('/'))
            return new ResourcePath($"{l.Path}{r.Path}");

        return new ResourcePath($"{l.Path}/{r.Path}");
    }
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

        if (Path.StartsWith(basePath.Path))
        {
            var x = Path[basePath.Path.Length..].Trim('/');
            relative = x == string.Empty ? Self : new ResourcePath(x);
            return true;
        }

        relative = null;
        return false;
    }
    
    public static ResourcePath FromRelativeSystemPath(string path, char newSeparator)
    {
        // ReSharper disable once RedundantArgumentDefaultValue
        return new ResourcePath(path.Replace(newSeparator, '/'));
    }

    public static bool Equals(ResourcePath a, ResourcePath b)
    {
        return a.Path == b.Path;
    }

    public bool Equals(ResourcePath b)
    {
        return Path == b.Path;
    }

    public override bool Equals(object? other)
    {
        if (other is null || other is not ResourcePath b)
            return false;

        return Path == b.Path;
    }

    public override int GetHashCode()
    {
        return Path.GetHashCode();
    }

    public override string ToString()
    {
        return Path;
    }

    public static bool operator ==(ResourcePath a, ResourcePath b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(ResourcePath a, ResourcePath b)
    {
        return !a.Equals(b);
    }
}