namespace Hypercube.Shared.Utilities.ArgumentsParser;

public class ArgumentParser(string[] args)
{
    public bool TryParse()
    {
        foreach (var arg in args)
        {
            Console.WriteLine(arg);
        }
        
        return true;
    }
}