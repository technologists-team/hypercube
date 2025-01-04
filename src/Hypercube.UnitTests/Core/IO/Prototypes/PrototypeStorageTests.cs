using Hypercube.Core.IO.Prototypes;
using Hypercube.Core.IO.Prototypes.Storage;

namespace Hypercube.UnitTests.Core.IO.Prototypes;

[TestFixture]
public sealed class PrototypeStorageTests
{
    private readonly string _yamlData = 
    """
    # Also parser test
    example1:
      type: example
      Name: 'Prototype One' # Parser test
      Value: 100

    example2:
      type: example
      Name: 'Prototype Two'
      Value: 200
    """;
    
    [Test]
    public void LoadPrototypes_ShouldLoadPrototypesCorrectly()
    {
        // Arrange
        var storage = new PrototypeStorage();

        // Act
        storage.LoadPrototypes(_yamlData);

        // Assert
        Assert.That(storage.HasPrototype(new PrototypeId<ExamplePrototype>("example1")), Is.True);
        Assert.That(storage.HasPrototype(new PrototypeId<ExamplePrototype>("example2")), Is.True);
    }

    [Test]
    public void GetPrototype_ShouldReturnCorrectPrototype()
    {
        // Arrange
        var storage = new PrototypeStorage();
        storage.LoadPrototypes(_yamlData);

        // Act
        var prototype = storage.GetPrototype(new PrototypeId<ExamplePrototype>("example1"));

        // Assert
        Assert.That(prototype, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(prototype.Id, Is.EqualTo("example1"));
            Assert.That(prototype.Name, Is.EqualTo("Prototype One"));
            Assert.That(prototype.Value, Is.EqualTo(100));
        });
    }

    [Test]
    public void TryGetPrototype_ShouldReturnTrue_WhenPrototypeExists()
    {
        // Arrange
        var storage = new PrototypeStorage();
        storage.LoadPrototypes(_yamlData);

        // Act
        var result = storage.TryGetPrototype(new PrototypeId<ExamplePrototype>("example2"), out var prototype);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(prototype, Is.Not.Null);
        });
        
        Assert.Multiple(() =>
        {
            Assert.That(prototype.Id, Is.EqualTo("example2"));
            Assert.That(prototype.Name, Is.EqualTo("Prototype Two"));
            Assert.That(prototype.Value, Is.EqualTo(200));
        });
    }

    [Test]
    public void TryGetPrototype_ShouldReturnFalse_WhenPrototypeDoesNotExist()
    {
        // Arrange
        var storage = new PrototypeStorage();
        storage.LoadPrototypes(_yamlData);

        // Act
        var result = storage.TryGetPrototype(new PrototypeId<ExamplePrototype>("nonexistent"), out var prototype);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(prototype, Is.Null);
        });
    }

    [Test]
    public void HasPrototype_ShouldReturnTrue_WhenPrototypeExists()
    {
        // Arrange
        var storage = new PrototypeStorage();
        storage.LoadPrototypes(_yamlData);

        // Act
        var result = storage.HasPrototype(new PrototypeId<ExamplePrototype>("example1"));

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void HasPrototype_ShouldReturnFalse_WhenPrototypeDoesNotExist()
    {
        // Arrange
        var storage = new PrototypeStorage();
        storage.LoadPrototypes(_yamlData);

        // Act
        var result = storage.HasPrototype(new PrototypeId<ExamplePrototype>("nonexistent"));

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void EnumeratePrototypes_ShouldReturnAllPrototypesOfType()
    {
        // Arrange
        var storage = new PrototypeStorage();
        storage.LoadPrototypes(_yamlData);

        // Act
        var prototypes = storage.EnumeratePrototypes<ExamplePrototype>().ToList();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(prototypes.Count, Is.EqualTo(2));
            Assert.That(prototypes.Any(p => p.Id == "example1"), Is.True);
            Assert.That(prototypes.Any(p => p.Id == "example2"), Is.True);
        });
    }

    [Test]
    public void GetPrototype_ShouldThrowException_WhenPrototypeDoesNotExist()
    {
        // Arrange
        var storage = new PrototypeStorage();
        storage.LoadPrototypes(_yamlData);

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() =>
            storage.GetPrototype(new PrototypeId<ExamplePrototype>("nonexistent"))
        );
    }

    [Test]
    public void LoadPrototypes_ShouldThrowException_WhenTypeIsMissing()
    {
        // Arrange
        var invalidYamlData = @"
example1:
  Name: 'Invalid Prototype'
  Value: 50
";

        var storage = new PrototypeStorage();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => storage.LoadPrototypes(invalidYamlData));
    }

    [Test]
    public void LoadPrototypes_ShouldThrowException_WhenTypeIsUnknown()
    {
        // Arrange
        var invalidYamlData = @"
example1:
  type: UnknownPrototype
  Name: 'Invalid Prototype'
  Value: 50
";

        var storage = new PrototypeStorage();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => storage.LoadPrototypes(invalidYamlData));
    }
    
    [Prototype("example")]
    private class ExamplePrototype : IPrototype
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
    }
}