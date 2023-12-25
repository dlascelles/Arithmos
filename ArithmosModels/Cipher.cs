/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System.Text.RegularExpressions;

namespace ArithmosModels;

/// <summary>
/// Represents a cipher with a specified body, value separator, and pair separator.
/// </summary>
public record Cipher
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Cipher"/> class.
    /// </summary>
    /// <param name="body">The body of the cipher.</param>
    public Cipher(string body)
    {
        Body = body;
    }

    /// <summary>
    /// Checks whether the cipher is valid according to the specified pattern and constraints.
    /// </summary>
    /// <returns><c>true</c> if the cipher is valid; otherwise, <c>false</c>.</returns>
    public bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(Body)) return false;
        if (ValueSeparator == PairSeparator) return false;

        string pattern = $@"^[\p{{L}}\p{{Lu}}]{Regex.Escape(ValueSeparator.ToString())}\d{{1,6}}({Regex.Escape(PairSeparator.ToString())}[\p{{L}}\p{{Lu}}]{Regex.Escape(ValueSeparator.ToString())}\d{{1,6}})*$";
        if (!Regex.IsMatch(Body, pattern, RegexOptions.IgnoreCase)) return false;

        string[] pairs = Body.Split(PairSeparator);
        HashSet<char> chars = [];
        foreach (string pair in pairs)
        {
            char currentChar = Convert.ToChar(pair.Split(ValueSeparator)[0]);
            if (!chars.Add(currentChar)) return false;
        }
        return true;
    }
    
    /// <summary>
    /// Gets or sets the body of the cipher.
    /// </summary>
    public string Body { get; init; }

    /// <summary>
    /// Gets or sets the character used as the value separator.
    /// </summary>
    public char ValueSeparator { get; init; } = '=';

    /// <summary>
    /// Gets or sets the character used as the pair separator.
    /// </summary>
    public char PairSeparator { get; init; } = ',';
}