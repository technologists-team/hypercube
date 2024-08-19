using JetBrains.Annotations;

namespace Hypercube.Utilities;

[PublicAPI]
public class ArgumentParser
{
    private readonly string[] _args;
    
    public ArgumentParser(string[] args)
    {
        _args = args;
    }

    public bool TryParse()
    {
        foreach (var arg in _args)
        {
            Console.WriteLine(arg);
        }
        
        return true;
    }
}