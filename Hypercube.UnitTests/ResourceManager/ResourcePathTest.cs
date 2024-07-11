using Hypercube.Shared.Resources;

namespace Hypercube.UnitTests.ResourceManager;

public class ResourcePathTest
{
    [Test]
    public void GetFilenameTest()
    {
        var resPath = new ResourcePath("/Rooted/path.txt");
        Assert.That(resPath.Filename == "path");
        
        Assert.Pass($"ResPath file name is {resPath.Filename}");
    }

    [Test]
    public void GetExtensionTest()
    {
        var resPath = new ResourcePath("/Rooted/path.txt");
        Assert.That(resPath.Extension == ".txt");
        Assert.Pass($"ResPath ext is {resPath.Extension}");
    }

    [Test]
    public void RootedTest()
    {
        var resPath = new ResourcePath("/Rooted/path.txt");
        Assert.That(resPath.Rooted);
        Assert.Pass("Res path is rooted");
    }

    [Test]
    public void ParentDirTest()
    {
        var resPath = new ResourcePath("/Rooted/path.txt");
        Assert.That(resPath.ParentDirectory.Path == "/Rooted");
        Assert.Pass("Parent directory passed");
    }

    [Test]
    public void PathConcatTest()
    {
        var resPath = new ResourcePath("/Rooted/");
        var resPath2 = new ResourcePath("path.txt");

        var concated = resPath + resPath2;
        Assert.That(concated.Path == "/Rooted/path.txt");
        
        Assert.Pass($"Concatenated path is equal to {concated.Path}");
    }

    [Test]
    public void PathEqualityTest()
    {
        var resPath = new ResourcePath("/Rooted/");
        var resPath2 = new ResourcePath("/Rooted/");
        var resPath3 = new ResourcePath("path.txt");
        
        Assert.That(resPath != resPath3);
        Assert.That(resPath == resPath2);
        
        Assert.Pass("Passed equality test");
    }
}