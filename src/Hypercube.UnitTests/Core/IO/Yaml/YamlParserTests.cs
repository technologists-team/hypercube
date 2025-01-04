using Hypercube.Core.IO.Parsers.Yaml;

namespace Hypercube.UnitTests.Core.IO.Yaml;

[TestFixture]
public class YamlParserTests
{
    [Test]
    public void ParseYaml_ValidYaml_ShouldParseCorrectly()
    {
        // Arrange
        const string yamlData =
        """      
        # This is a comment
        prototype1:
          type: SomeType
          field1: 'Value with # and \" quotes'
          field2: "Escaped value\\nNew line"
        prototype2:
          type: AnotherType
          field1: 'Another field';
        """;

        var expected = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "prototype1", new Dictionary<string, string>
                {
                    { "type", "SomeType" },
                    { "field1", "Value with # and \" quotes" },
                    { "field2", "Escaped value\nNew line" }
                }
            },
            {
                "prototype2", new Dictionary<string, string>
                {
                    { "type", "AnotherType" },
                    { "field1", "Another field" }
                }
            }
        };

        // Act
        var result = YamlParser.ParseYaml(yamlData);

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void ParseYaml_YamlWithComments_ShouldIgnoreComments()
    {
        // Arrange
        const string yamlData =
        """
        # This is a comment
        prototype1:
          type: SomeType
          field1: 'Field1 value'
        # Another comment
        prototype2:
          type: AnotherType
          field2: 'Field2 value'
        """;

        var expected = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "prototype1", new Dictionary<string, string>
                {
                    { "type", "SomeType" },
                    { "field1", "Field1 value" }
                }
            },
            {
                "prototype2", new Dictionary<string, string>
                {
                    { "type", "AnotherType" },
                    { "field2", "Field2 value" }
                }
            }
        };

        // Act
        var result = YamlParser.ParseYaml(yamlData);

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void ParseYaml_EmptyString_ShouldReturnEmptyDictionary()
    {
        // Arrange
        var yamlData = string.Empty;

        // Act
        var result = YamlParser.ParseYaml(yamlData);

        // Assert
        Assert.IsEmpty(result);
    }

    [Test]
    public void ParseYaml_SinglePrototype_ShouldParseCorrectly()
    {
        // Arrange
        const string yamlData =
        """
        prototype1:
          type: MyType
          field1: 'Some value'
        """;

        var expected = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "prototype1", new Dictionary<string, string>
                {
                    { "type", "MyType" },
                    { "field1", "Some value" }
                }
            }
        };

        // Act
        var result = YamlParser.ParseYaml(yamlData);

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void ParseYaml_ValidYamlWithEscapedCharacters_ShouldHandleEscapeSequences()
    {
        // Arrange
        const string yamlData =
        """
        prototype1:
          type: SomeType
          field1: 'Value with escaped \\n new line'
          field2: 'Another value with \\t tab'
        """;

        var expected = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "prototype1", new Dictionary<string, string>
                {
                    { "type", "SomeType" },
                    { "field1", "Value with escaped \n new line" },
                    { "field2", "Another value with \t tab" }
                }
            }
        };

        // Act
        var result = YamlParser.ParseYaml(yamlData);

        // Assert
        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void ParseYaml_InvalidYaml_ShouldThrowException()
    {
        // Arrange
        const string yamlData =
        """
        prototype1:
          type: SomeType
          field1: 'Unmatched quote
        prototype2:
          type: AnotherType
          field2: 'Valid value'
        """;

        // Act & Assert
        Assert.Throws<FormatException>(() => YamlParser.ParseYaml(yamlData));
    }
}