/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Enums;
using System.Globalization;
using System.Text;

namespace ArithmosModels.Extensions;

public static class StringMethods
{
    /// <summary>
    /// Removes non-Unicode characters from the specified string.
    /// </summary>
    /// <param name="value">The string from which to remove non-Unicode characters.</param>
    /// <returns>
    /// A new string with non-Unicode characters removed. If the input string is null, empty, or contains only white spaces,
    /// an empty string is returned.
    /// </returns>
    public static string RemoveNonUnicodeCharacters(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "";

        StringBuilder stringBuilder = new(value.Length);
        foreach (char c in value)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.OtherNotAssigned)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Removes characters with the Unicode category of Non-Spacing Mark from the specified string.
    /// </summary>
    /// <param name="value">The string from which to remove characters with the Non-Spacing Mark Unicode category.</param>
    /// <returns>
    /// A new string with characters having the Non-Spacing Mark Unicode category removed. If the input string is null,
    /// empty, or contains only white spaces, an empty string is returned.
    /// </returns>
    public static string RemoveNonSpacingMarks(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "";

        StringBuilder stringBuilder = new(value.Length);
        foreach (char c in value)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Removes extra spaces from the specified string, reducing consecutive spaces to a single space.
    /// </summary>
    /// <param name="value">The string from which to remove extra spaces.</param>
    /// <returns>
    /// A new string with consecutive spaces reduced to a single space. If the input string is null,
    /// empty, or contains only white spaces, an empty string is returned.
    /// </returns>
    public static string RemoveExtraSpaces(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "";

        StringBuilder sb = new(value.Length);
        bool isPreviousSpace = false;
        foreach (char c in value)
        {
            if (c == ' ' && !isPreviousSpace || !char.IsWhiteSpace(c))
            {
                sb.Append(c);
                isPreviousSpace = c == ' ';
            }
        }
        return sb.ToString();
    }

    /// <summary>
    /// Removes newline characters from the given string and replaces them with the specified string or an empty string by default.
    /// </summary>
    /// <param name="value">The input string containing newline characters.</param>
    /// <param name="replaceWith">The string to replace newline characters with. If null, newline characters are replaced with an empty string.</param>
    /// <returns>A new string with newline characters removed and replaced with the specified string or an empty string.</returns>
    public static string RemoveNewLines(this string value, string replaceWith = null)
    {
        if (string.IsNullOrWhiteSpace(value)) return "";

        replaceWith ??= "";
        return value
            .Replace(TextSeparator.CharNewLineN, replaceWith)
            .Replace(TextSeparator.CharNewLineNR, replaceWith)
            .Replace(TextSeparator.CharNewLineR, replaceWith)
            .Replace(TextSeparator.CharNewLineRN, replaceWith);
    }

    /// <summary>
    /// Removes accents from characters in the specified string.
    /// </summary>
    /// <param name="value">The string from which to remove accents.</param>
    /// <returns>
    /// A new string with accents removed. If the input string is null, empty, or contains only white spaces,
    /// an empty string is returned.
    /// </returns>
    public static string RemoveAccents(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return "";

        string normalized = value.RemoveNonUnicodeCharacters().Normalize(NormalizationForm.FormKD);
        normalized = normalized.RemoveNonSpacingMarks();
        return normalized;
    }

    /// <summary>
    /// Determines the combined alphabet of the characters in the specified string.
    /// </summary>
    /// <param name="value">The string for which to determine the combined alphabet.</param>
    /// <returns>
    /// The <see cref="Alphabet"/> representing the combined alphabet of the characters in the string.
    /// If the string is null, empty, or contains only white spaces, <see cref="Alphabet.None"/> is returned.
    /// </returns>
    public static Alphabet GetAlphabet(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return Alphabet.None;

        Alphabet alphabet = Alphabet.None;
        foreach (char c in value)
        {
            alphabet |= c.GetAlphabet();
        }
        return alphabet;
    }
}
