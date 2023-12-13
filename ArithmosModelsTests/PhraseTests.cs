/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosModelsTests;

[TestClass]
public class PhraseTests
{
    [TestMethod]
    public void Constructor_NormalizesTextLatin()
    {
        // Arrange
        long id = 1;
        int operationId = 2;
        GematriaMethod gematriaMethod = new(new Cipher("a=1,b=2,c=3"))
        {
            Id = 1
        };
        string text = "The  Café ";

        // Act
        Phrase phrase = new(text, new List<GematriaMethod> { gematriaMethod }, id, operationId);

        // Assert
        Assert.AreEqual("The Cafe", phrase.Content);
    }

    [TestMethod]
    public void Constructor_NormalizesTextGreek()
    {
        // Arrange
        long id = 1;
        int operationId = 2;
        GematriaMethod gematriaMethod = new(new Cipher("a=1,b=2,c=3"))
        {
            Id = 1
        };
        string text = "  ἐν ἀρχῇ ἦν ὁ λόγος  ";

        // Act
        Phrase phrase = new(text, new List<GematriaMethod> { gematriaMethod }, id, operationId);

        // Assert
        Assert.AreEqual("εν αρχη ην ο λογος", phrase.Content);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForEqualPhrases()
    {
        // Arrange
        Phrase phrase1 = new("Hello", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 1, 2);
        Phrase phrase2 = new("Hello", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 3, 4);

        // Act
        bool result = phrase1.Equals(phrase2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_ReturnsFalseForDifferentPhrases()
    {
        // Arrange
        Phrase phrase1 = new("Hello", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 1, 2);
        Phrase phrase2 = new("World", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 3, 4);

        // Act
        bool result = phrase1.Equals(phrase2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetHashCode_ReturnsSameHashCodeForEqualPhrases()
    {
        // Arrange
        Phrase phrase1 = new("Hello", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 1, 2);
        Phrase phrase2 = new("Hello", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 3, 4);

        // Act
        int hashCode1 = phrase1.GetHashCode();
        int hashCode2 = phrase2.GetHashCode();

        // Assert
        Assert.AreEqual(hashCode1, hashCode2);
    }

    [TestMethod]
    public void GetHashCode_ReturnsDifferentHashCodeForDifferentPhrases()
    {
        // Arrange
        Phrase phrase1 = new("Hello", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 1, 2);
        Phrase phrase2 = new("World", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 3, 4);

        // Act
        int hashCode1 = phrase1.GetHashCode();
        int hashCode2 = phrase2.GetHashCode();

        // Assert
        Assert.AreNotEqual(hashCode1, hashCode2);
    }

    [TestMethod]
    public void ToString_ReturnsNormalizedText()
    {
        // Arrange
        Phrase phrase = new("Café", new List<GematriaMethod> { new(new Cipher("a=1,b=2,c=3")) }, 1, 2);

        // Act
        string result = phrase.ToString();

        // Assert
        Assert.AreEqual("Cafe", result);
    }
}
