/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Enums;
using System.Globalization;

namespace ArithmosModels.Extensions;

public static class CharMethods
{
    /// <summary>
    /// Determines whether the specified character is a non-letter character, considering optional allowed characters.
    /// </summary>
    /// <param name="c">The character to check.</param>
    /// <param name="allowedCharacters">An optional array of characters that are considered allowed, exempting them from being treated as non-letter characters.</param>
    /// <returns>
    ///   <c>true</c> if the specified character is a non-letter character; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNonLetterCharacter(this char c, char[] allowedCharacters = null)
    {
        if (char.IsLetter(c)) return false;
        if (allowedCharacters != null && allowedCharacters.Contains(c)) return false;

        UnicodeCategory category = char.GetUnicodeCategory(c);
        if (category == UnicodeCategory.OtherLetter || category == UnicodeCategory.LetterNumber) return false;

        return true;
    }

    /// <summary>
    /// Determines the alphabet to which the specified character belongs.
    /// </summary>
    /// <param name="c">The character for which to determine the alphabet.</param>
    /// <returns>
    /// The <see cref="Alphabet"/> to which the character belongs. If the character is a non-letter character,
    /// <see cref="Alphabet.None"/> is returned. If the character's alphabet is not explicitly defined in the method,
    /// <see cref="Alphabet.Unknown"/> is returned.
    /// </returns>
    public static Alphabet GetAlphabet(this char c)
    {
        if (c.IsNonLetterCharacter()) return Alphabet.None;

        if (Constants.Alphabets.English.Contains(c)) return Alphabet.English;
        if (Constants.Alphabets.Greek.Contains(c)) return Alphabet.Greek;
        if (Constants.Alphabets.Hebrew.Contains(c)) return Alphabet.Hebrew;
        if (Constants.Alphabets.Arabic.Contains(c)) return Alphabet.Arabic;
        if (Constants.Alphabets.Cyrillic.Contains(c)) return Alphabet.Cyrillic;
        if (Constants.Alphabets.Coptic.Contains(c)) return Alphabet.Coptic;

        return Alphabet.Unknown;
    }
}
