/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ArithmosModels.Helpers
{
    /// <summary>
    /// Helper class to normalize text in FormD and to detect a character's or string's alphabet. It contains all available characters with their corresponding alphabet.
    /// </summary>
    public static class CharacterHandler
    {
        public static readonly Dictionary<char, Alphabet> Characters = new()
        {
            {'A', Alphabet.English}, {'B', Alphabet.English}, {'C', Alphabet.English}, {'D', Alphabet.English},
            {'E', Alphabet.English}, {'F', Alphabet.English}, {'G', Alphabet.English}, {'H', Alphabet.English},
            {'I', Alphabet.English}, {'J', Alphabet.English}, {'K', Alphabet.English}, {'L', Alphabet.English},
            {'M', Alphabet.English}, {'N', Alphabet.English}, {'O', Alphabet.English}, {'P', Alphabet.English},
            {'Q', Alphabet.English}, {'R', Alphabet.English}, {'S', Alphabet.English}, {'T', Alphabet.English},
            {'U', Alphabet.English}, {'V', Alphabet.English}, {'W', Alphabet.English}, {'X', Alphabet.English},
            {'Y', Alphabet.English}, {'Z', Alphabet.English},
            {'Α', Alphabet.Greek}, {'Β', Alphabet.Greek}, {'Γ', Alphabet.Greek}, {'Δ', Alphabet.Greek},
            {'Ε', Alphabet.Greek}, {'Ϛ', Alphabet.Greek}, {'Ζ', Alphabet.Greek}, {'Η', Alphabet.Greek},
            {'Θ', Alphabet.Greek}, {'Ι', Alphabet.Greek}, {'Κ', Alphabet.Greek}, {'Λ', Alphabet.Greek},
            {'Μ', Alphabet.Greek}, {'Ν', Alphabet.Greek}, {'Ξ', Alphabet.Greek}, {'Ο', Alphabet.Greek},
            {'Π', Alphabet.Greek}, {'Ϙ', Alphabet.Greek}, {'Ρ', Alphabet.Greek}, {'Σ', Alphabet.Greek},
            {'Τ', Alphabet.Greek}, {'Υ', Alphabet.Greek}, {'Φ', Alphabet.Greek}, {'Χ', Alphabet.Greek},
            {'Ψ', Alphabet.Greek}, {'Ω', Alphabet.Greek}, {'Ϡ', Alphabet.Greek},
            {'א', Alphabet.Hebrew}, {'ב', Alphabet.Hebrew}, {'ג', Alphabet.Hebrew}, {'ד', Alphabet.Hebrew},
            {'ה', Alphabet.Hebrew}, {'ו', Alphabet.Hebrew}, {'ז', Alphabet.Hebrew}, {'ח', Alphabet.Hebrew},
            {'ט', Alphabet.Hebrew}, {'י', Alphabet.Hebrew}, {'כ', Alphabet.Hebrew}, {'ל', Alphabet.Hebrew},
            {'מ', Alphabet.Hebrew}, {'נ', Alphabet.Hebrew}, {'ס', Alphabet.Hebrew}, {'ע', Alphabet.Hebrew},
            {'פ', Alphabet.Hebrew}, {'צ', Alphabet.Hebrew}, {'ק', Alphabet.Hebrew}, {'ר', Alphabet.Hebrew},
            {'ש', Alphabet.Hebrew}, {'ת', Alphabet.Hebrew}, {'ך', Alphabet.Hebrew}, {'ם', Alphabet.Hebrew},
            {'ן', Alphabet.Hebrew}, {'ף', Alphabet.Hebrew}, {'ץ', Alphabet.Hebrew}
        };

        private static readonly StringBuilder sb = new();

        /// <summary>
        /// Detects the alphabet of a character
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>Alphabet Enum</returns>
        public static Alphabet GetAlphabet(char c)
        {
            return Characters.ContainsKey(c) ? Characters[c] : Alphabet.None;
        }

        /// <summary>
        /// Detects the alphabet of a string
        /// </summary>
        /// <param name="text">The string to check</param>
        /// <returns>Alphabet Enum</returns>
        public static Alphabet GetAlphabet(string text)
        {
            if (String.IsNullOrWhiteSpace(text)) return Alphabet.None;

            Alphabet alphabet = Alphabet.None;

            foreach (char c in text)
            {
                alphabet |= GetAlphabet(c);
                if ((alphabet & (alphabet - 1)) != 0)
                {
                    return Alphabet.Mixed;
                }
            }

            return alphabet;
        }

        /// <summary>
        /// Will normalize text in FormD, transform all characters to uppercase and replace multiple spaces with just one.
        /// </summary>
        /// <param name="text">The string to be normalized</param>
        /// <returns>The normalized string</returns>
        public static string NormalizeText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            sb.Clear();
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            bool isPreviousSpace = false;
            foreach (char c in normalizedString)
            {
                if ((c == ' ' && !isPreviousSpace) || Characters.ContainsKey(char.ToUpper(c)))
                {
                    UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        sb.Append(char.ToUpper(c));
                    }
                    isPreviousSpace = c == ' ';
                }
            }

            return sb.ToString().Trim();
        }
    }
}