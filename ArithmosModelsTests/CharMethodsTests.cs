/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosModels.Enums;
using ArithmosModels.Extensions;

namespace ArithmosModelsTests;

[TestClass]
public class CharMethodsTests
{
    [TestMethod]
    public void IsNonLetterCharacter_WithNonLetterCharacter_ShouldReturnTrue()
    {
        // Arrange
        char nonLetterCharacter = '@';

        // Act
        bool result = nonLetterCharacter.IsNonLetterCharacter();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsNonLetterCharacter_WithLetterCharacter_ShouldReturnFalse()
    {
        // Arrange
        char letterCharacter = 'A';

        // Act
        bool result = letterCharacter.IsNonLetterCharacter();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNonLetterCharacter_WithAllowedCharacter_ShouldReturnFalse()
    {
        // Arrange
        char allowedCharacter = '$';
        char[] allowedCharacters = ['$', '#'];

        // Act
        bool result = allowedCharacter.IsNonLetterCharacter(allowedCharacters);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNonLetterCharacter_WithOtherLetterUnicodeCategory_ShouldReturnFalse()
    {
        // Arrange
        char otherLetterCharacter = 'é';

        // Act
        bool result = otherLetterCharacter.IsNonLetterCharacter();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsNonLetterCharacter_WithLetterNumberUnicodeCategory_ShouldReturnFalse()
    {
        // Arrange
        char letterNumberCharacter = 'Ⅳ';

        // Act
        bool result = letterNumberCharacter.IsNonLetterCharacter();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetAlphabet_OtherCharacter_ShouldReturnUnknown()
    {
        // Arrange
        char letterNumberCharacter = 'Ⅳ';

        // Act
        Alphabet alphabet = letterNumberCharacter.GetAlphabet();

        // Assert
        Assert.IsTrue(alphabet == Alphabet.Unknown);
    }

    [TestMethod]
    public void GetAlphabet_SpaceCharacter_ShouldReturnNone()
    {
        // Arrange
        char spaceCharacter = ' ';

        // Act
        Alphabet alphabet = spaceCharacter.GetAlphabet();

        // Assert
        Assert.IsTrue(alphabet == Alphabet.None);
    }

    [TestMethod]
    public void GetAlphabet_WhiteSpaceCharacter_ShouldReturnNone()
    {
        // Arrange
        char whiteSpaceCharacter = '\t';

        // Act
        Alphabet alphabet = whiteSpaceCharacter.GetAlphabet();

        // Assert
        Assert.IsTrue(alphabet == Alphabet.None);
    }

    [TestMethod]
    public void GetAlphabet_NonLetterCharacter_ShouldReturnNone()
    {
        // Arrange
        char symbolCharacter = '=';

        // Act
        Alphabet alphabet = symbolCharacter.GetAlphabet();

        // Assert
        Assert.IsTrue(alphabet == Alphabet.None);
    }

    [TestMethod]
    public void GetAlphabet_Number_ShouldReturnNone()
    {
        // Arrange
        char numberCharacter = '5';

        // Act
        Alphabet alphabet = numberCharacter.GetAlphabet();

        // Assert
        Assert.IsTrue(alphabet == Alphabet.None);
    }

    [TestMethod]
    public void GetAlphabet_EnglishCharacters_ShouldReturnEnglish()
    {
        // Arrange
        Alphabet alphabet = Alphabet.None;

        // Act
        foreach (char c in Constants.Alphabets.English)
        {
            alphabet |= c.GetAlphabet();
        }

        // Assert
        Assert.IsTrue(alphabet == Alphabet.English);
    }

    [TestMethod]
    public void GetAlphabet_GreekCharacters_ShouldReturnGreek()
    {
        // Arrange
        Alphabet alphabet = Alphabet.None;

        // Act
        foreach (char c in Constants.Alphabets.Greek)
        {
            alphabet |= c.GetAlphabet();
        }

        // Assert
        Assert.IsTrue(alphabet == Alphabet.Greek);
    }

    [TestMethod]
    public void GetAlphabet_HebrewCharacters_ShouldReturnHebrew()
    {
        // Arrange
        Alphabet alphabet = Alphabet.None;

        // Act
        foreach (char c in Constants.Alphabets.Hebrew)
        {
            alphabet |= c.GetAlphabet();
        }

        // Assert
        Assert.IsTrue(alphabet == Alphabet.Hebrew);
    }

    [TestMethod]
    public void GetAlphabet_ArabicCharacters_ShouldReturnArabic()
    {
        // Arrange
        Alphabet alphabet = Alphabet.None;

        // Act
        foreach (char c in Constants.Alphabets.Arabic)
        {
            alphabet |= c.GetAlphabet();
        }

        // Assert
        Assert.IsTrue(alphabet == Alphabet.Arabic);
    }

    [TestMethod]
    public void GetAlphabet_CyrillicCharacters_ShouldReturnCyrillic()
    {
        // Arrange
        Alphabet alphabet = Alphabet.None;

        // Act
        foreach (char c in Constants.Alphabets.Cyrillic)
        {
            alphabet |= c.GetAlphabet();
        }

        // Assert
        Assert.IsTrue(alphabet == Alphabet.Cyrillic);
    }

    [TestMethod]
    public void GetAlphabet_CopticCharacters_ShouldReturnCoptic()
    {
        // Arrange
        Alphabet alphabet = Alphabet.None;

        // Act
        foreach (char c in Constants.Alphabets.Coptic)
        {
            alphabet |= c.GetAlphabet();
        }

        // Assert
        Assert.IsTrue(alphabet == Alphabet.Coptic);
    }
}
