/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Enums;
using ArithmosModels.Extensions;

namespace ArithmosModelsTests;

[TestClass]
public class StringMethodsTests
{
    [TestMethod]
    public void RemoveNonUnicodeCharacters_WithUnicodeString_ShouldReturnSameString()
    {
        // Arrange
        string input = "Hello, こんにちは, Здравствуйте, Γειά σας, مرحبًا, שלום";

        // Act
        string result = input.RemoveNonUnicodeCharacters();

        // Assert
        Assert.AreEqual(input, result);
    }

    [TestMethod]
    public void RemoveNonUnicodeCharacters_WithNonUnicodeCharacters_ShouldRemoveNonUnicodeCharacters()
    {
        // Arrange
        string input = "Here are some characters that should be removed: \uFDEF\uFFFE";

        // Act
        string result = input.RemoveNonUnicodeCharacters();

        // Assert
        Assert.AreEqual("Here are some characters that should be removed: ", result);
    }

    [TestMethod]
    public void RemoveNonUnicodeCharacters_WithEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        string input = string.Empty;

        // Act
        string result = input.RemoveNonUnicodeCharacters();

        // Assert
        Assert.AreEqual(string.Empty, result);
    }

    [TestMethod]
    public void RemoveNonSpacingMarks_ShouldHandleEmptyString()
    {
        // Arrange
        string input = "";

        // Act
        string result = input.RemoveNonSpacingMarks();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveNonSpacingMarks_ShouldHandleStringWithNoMarks()
    {
        // Arrange
        string input = "hello world";

        // Act
        string result = input.RemoveNonSpacingMarks();

        // Assert
        Assert.AreEqual("hello world", result);
    }

    [TestMethod]
    public void RemoveNonSpacingMarks_ShouldRemoveMarks()
    {
        // Arrange
        string input = "בְּרֵאשִׁ֖ית בָּרָ֣א אֱלֹהִ֑ים";

        // Act
        string result = input.RemoveNonSpacingMarks();

        // Assert
        Assert.AreEqual("בראשית ברא אלהים", result);
    }

    [TestMethod]
    public void RemoveExtraSpaces_NullInput_ReturnsEmptyString()
    {
        // Arrange
        string? input = null;

        // Act
        string result = input.RemoveExtraSpaces();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveExtraSpaces_EmptyStringInput_ReturnsEmptyString()
    {
        // Arrange
        string input = "";

        // Act
        string result = input.RemoveExtraSpaces();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveExtraSpaces_OnlySpacesInput_ReturnsEmptyString()
    {
        // Arrange
        string input = "    ";

        // Act
        string result = input.RemoveExtraSpaces();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveExtraSpaces_SingleSpaceInput_ReturnsEmptyString()
    {
        // Arrange
        string input = " ";

        // Act
        string result = input.RemoveExtraSpaces();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveExtraSpaces_NoExtraSpacesInput_ReturnsInputString()
    {
        // Arrange
        string input = "I'm your huckleberry";

        // Act
        string result = input.RemoveExtraSpaces();

        // Assert
        Assert.AreEqual(input, result);
    }

    [TestMethod]
    public void RemoveExtraSpaces_WithExtraSpacesInput_RemovesExtraSpaces()
    {
        // Arrange
        string input = "  I'm    your       huckleberry  ";

        // Act
        string result = input.RemoveExtraSpaces();

        // Assert
        Assert.AreEqual(" I'm your huckleberry ", result);
    }

    [TestMethod]
    public void RemoveNewLines_NullInput_ReturnsEmptyString()
    {
        // Arrange
        string? input = null;

        // Act
        string result = input.RemoveNewLines();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveNewLines_EmptyStringInput_ReturnsEmptyString()
    {
        // Arrange
        string input = "";

        // Act
        string result = input.RemoveNewLines();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveNewLines_OnlySpacesInput_ReturnsEmptyString()
    {
        // Arrange
        string input = "    ";

        // Act
        string result = input.RemoveNewLines();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveNewLines_NoNewLinesInput_ReturnsInputString()
    {
        // Arrange
        string input = "I'm your huckleberry";

        // Act
        string result = input.RemoveExtraSpaces();

        // Assert
        Assert.AreEqual(input, result);
    }

    [TestMethod]
    public void RemoveNewLines_InputWithNewLines_ReturnsInputWithoutNewLines()
    {
        // Arrange
        string input = "\rI'm \nyo\nur\r\n\r huckleberry\r\n";

        // Act
        string result = input.RemoveNewLines();

        // Assert
        Assert.AreEqual("I'm your huckleberry", result);
    }

    [TestMethod]
    public void RemoveNewLines_InputWithNewLines_ReturnsReplacedNewLines()
    {
        // Arrange
        string input = "\rI'm\nyo\nur\r\n\rhuckleberry\r\n";

        // Act
        string result = input.RemoveNewLines(" ");

        // Assert
        Assert.AreEqual(" I'm yo ur   huckleberry  ", result);
    }

    [TestMethod]
    public void RemoveAccents_WithAccentedCharacters_ReturnsAccentsRemoved()
    {
        // Arrange
        string input = "Ἄνδρα μοι ἔννεπε, Μοῦσα, πολύτροπον, ὃς μάλα πολλὰ";
        string expected = "Ανδρα μοι εννεπε, Μουσα, πολυτροπον, ος μαλα πολλα";

        // Act
        string result = input.RemoveAccents();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void RemoveAccents_WithAccentedCharacters_ReturnsAccentsRemoved2()
    {
        // Arrange
        string input = "Μῆνιν ἄειδε, θεά, Πηληϊάδεω Ἀχιλῆος";
        string expected = "Μηνιν αειδε, θεα, Πηληιαδεω Αχιληος";

        // Act
        string result = input.RemoveAccents();

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void RemoveAccents_WithoutAccentedCharacters_ReturnsOriginalString()
    {
        // Arrange
        string input = "This should stay the same";

        // Act
        string result = input.RemoveAccents();

        // Assert
        Assert.AreEqual(input, result);
    }

    [TestMethod]
    public void RemoveAccents_EmptyString_ReturnsEmptyString()
    {
        // Arrange
        string input = "";

        // Act
        string result = input.RemoveAccents();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void RemoveAccents_NullString_ReturnsNull()
    {
        // Arrange
        string? input = null;

        // Act
        string result = input.RemoveAccents();

        // Assert
        Assert.AreEqual("", result);
    }

    [TestMethod]
    public void GetAlphabet_WhiteSpace_ReturnsNone()
    {
        // Arrange
        string input = "            ";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.None, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_Symbols_ReturnsNone()
    {
        // Arrange
        string input = " '=-_@%^* !>?<>{}||[}\\} }{&#$%^#%   ";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.None, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_English_ReturnsEnglish()
    {
        // Arrange
        string input = "Hello my friend, how are you?";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.English, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_Greek_ReturnsGreek()
    {
        // Arrange
        string input = "Γεια σου φιλε μου πως εισαι;";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.Greek, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_Hebrew_ReturnsHebrew()
    {
        // Arrange
        string input = "שלום ידידי מה שלומך?";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.Hebrew, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_Arabic_ReturnsArabic()
    {
        // Arrange
        string input = "مرحبا يا صديقي كيف حالك؟";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.Arabic, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_Cyrillic_ReturnsCyrillic()
    {
        // Arrange
        string input = "Привет, друг, ты как?";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.Cyrillic, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_Coptic_ReturnsCoptic()
    {
        // Arrange
        string input = "Ⲭⲁⲓⲣⲉ ⲛⲁ ⲏⲣ, ⲡⲟⲥ ⲉⲓⲛⲁⲓ ⲑⲟⲕ?";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.Coptic, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_EnglishGreek_ReturnsEnglishGreek()
    {
        // Arrange
        string input = "Hello my friend, how are you? Γεια σου φιλε μου πως εισαι;";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.English | Alphabet.Greek, alphabet);
    }

    [TestMethod]
    public void GetAlphabet_HebrewArabic_ReturnsHebrewArabic()
    {
        // Arrange
        string input = "שלום ידידי מה שלומך? مرحبا يا صديقي كيف حالك؟";

        // Act
        Alphabet alphabet = input.GetAlphabet();

        // Assert
        Assert.AreEqual(Alphabet.Hebrew | Alphabet.Arabic, alphabet);
    }

}
