using Hypercube.Resources.Manager;

namespace Hypercube.UnitTests.ResourceManager;

public class ResourceManagerTests
{
    [Test]
    public void ReadFileTest()
    {
        var resourceMan = new ResourceLoader();
        resourceMan.MountContentFolder("Resources", "/");

        if (!resourceMan.TryReadFileContent("/Tests/testFile.txt", out var stream))
        {
            Assert.Fail("Unable to read file");
            return;
        }

        using var wrapped = resourceMan.WrapStream(stream);
        var read = wrapped.ReadToEnd();
        Assert.That(read == "hey");
        Assert.Pass("Read file successfully");
    }
}