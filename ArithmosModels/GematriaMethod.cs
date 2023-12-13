/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Extensions;
using System.Collections.Immutable;

namespace ArithmosModels;

/// <summary>
/// Represents a gematria method used for calculating values based on a given cipher.
/// </summary>
public class GematriaMethod
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GematriaMethod"/> class with the specified cipher.
    /// </summary>
    /// <param name="cipher">The cipher used for gematria calculations.</param>
    /// <exception cref="ArgumentNullException">Thrown when the cipher is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the cipher is not valid.</exception>
    public GematriaMethod(Cipher cipher)
    {
        if (cipher == null) throw new ArgumentNullException(nameof(cipher), "The Cipher was null");
        if (!cipher.IsValid()) throw new ArgumentException("The Cipher does not seem to be valid", nameof(cipher));

        Cipher = cipher;
        ValueMapper = GetValueMapper().ToImmutableDictionary();
    }

    /// <summary>
    /// Gets the gematria value of the specified text based on its assigned cipher.
    /// </summary>
    /// <param name="text">The text for which to calculate the gematria value.</param>
    /// <returns>The gematria value of the text.</returns>
    public int GetTextValue(string text)
    {
        if (ValueMapper == null || string.IsNullOrWhiteSpace(text)) return 0;

        int total = text.Sum(c => ValueMapper.TryGetValue(c, out int value) ? value : 0);
        total += AddsTotalNumberOfCharacters ? text.Sum(c => c.IsNonLetterCharacter() ? 0 : 1) : 0;
        total += AddsTotalNumberOfWords ? text.Split(' ').Length : 0;
        return total;
    }

    /// <summary>
    /// Extracts characters from a Cipher's body and adds them to a dictionary with their corresponding gematria values.
    /// </summary>
    /// <returns>A dictionary where each character is associated with its corresponding integer value.</returns>
    private Dictionary<char, int> GetValueMapper()
    {
        Dictionary<char, int> valueMapper = [];
        string[] values = Cipher.Body.Split(Cipher.PairSeparator);
        foreach (string value in values)
        {
            string[] pair = value.Split(Cipher.ValueSeparator);
            char c = Convert.ToChar(pair[0]);
            int v = Convert.ToInt32(pair[1]);
            valueMapper.Add(c, v);
        }
        return valueMapper;
    }

    /// <summary>
    /// Gets or sets the unique identifier for the gematria method.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the gematria method.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the sorting order of the gematria method.
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include the total number of characters in the gematria calculation.
    /// </summary>
    public bool AddsTotalNumberOfCharacters { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to include the total number of words in the gematria calculation.
    /// </summary>
    public bool AddsTotalNumberOfWords { get; set; } = false;

    /// <summary>
    /// Gets the cipher associated with the gematria method.
    /// </summary>
    public Cipher Cipher { get; private set; }

    /// <summary>
    /// Gets the immutable dictionary representing the mapping between characters and their gematria values.
    /// </summary>
    public readonly ImmutableDictionary<char, int> ValueMapper;
}
