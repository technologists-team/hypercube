using Hypercube.Shared.Resources;

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

        using var wrapped = resourceMan.WrapStream(stream);
        var read = wrapped.ReadToEnd();
        Assert.That(read == "hey");
        Assert.Pass("Read file successfully");
    }

    [Test]
    public void VFSTest()
    {
        var vfs = new VirtualContentRoot();
        
        Assert.Multiple(() =>
        {
            Assert.That(vfs.TryGetFile("/testFile.txt", out var stream));
            using var reader = new StreamReader(stream!);
            var read = reader.ReadToEnd();
            Assert.That(read == "hey");
            
            var found = vfs.FindFiles("/").ToList();
            Assert.That(found.Count == 1);
            Assert.That(vfs.TryGetFile(found.First(), out var stream2));
            using var reader2 = new StreamReader(stream2!);
            read = reader2.ReadToEnd();
            Assert.That(read == "hey");
        });
        
        Assert.Pass("VFS passed");
    }

    [Test]
    public void ResourceManWithVFSTest()
    {
        var resourceMan = new Shared.Resources.Manager.ResourceManager();
        resourceMan.AddRoot("/", new VirtualContentRoot());
        
        if(!resourceMan.TryReadFileContent("/testFile.txt", out var stream))
        {
            Assert.Fail("File not found");
            return;
        }

        using var wrap = resourceMan.WrapStream(stream);
        var content = wrap.ReadToEnd();
        Assert.That(content == "hey");
        Assert.Pass("VFS with resource manager passed");
    }
}