namespace Hypercube.UnitTests.ResourceManager;

public class ResourceManagerTests
{
    [Test]
    public void ReadFileTest()
    {
        var resourceMan = new Shared.Resources.Manager.ResourceManager();
        resourceMan.MountContentFolder("Resources", "/");

        if (!resourceMan.TryReadFileContent("/Tests/testFile.txt", out var stream))
        {
            Assert.Fail("Unable to read file");
            return;
        }

        var wrapped = resourceMan.WrapStream(stream);
        var read = wrapped.ReadToEnd();
        Assert.That(read == "hey");
        Assert.Pass("Read file successfully");
    }
}