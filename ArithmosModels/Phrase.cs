/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Enums;
using ArithmosModels.Extensions;
using System.Text;

namespace ArithmosModels;

/// <summary>
/// Represents a phrase with content, associated gematria values, alphabet(s) and information about the phrase.
/// </summary>
public readonly struct Phrase : IEquatable<Phrase>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Phrase"/> struct with the specified content, gematria methods, and optional identifiers.
    /// </summary>
    /// <param name="content">The content of the phrase.</param>
    /// <param name="methods">The list of gematria methods used to calculate values for the phrase.</param>
    /// <param name="id">The unique identifier for the phrase. Defaults to 0.</param>
    /// <param name="operationId">The identifier of the associated operation. Defaults to 0.</param>
    /// <exception cref="ArgumentNullException">Thrown when the list of methods is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the list of methods is empty.</exception>
    public Phrase(string content, List<GematriaMethod> methods, long id = 0, int operationId = 0)
    {
        if (methods == null) throw new ArgumentNullException(nameof(methods), "The list of methods was null");
        if (methods.Count == 0) throw new ArgumentException("A Phrase must contain at least one method", nameof(methods));

        Id = id;
        OperationId = operationId;
        Content = CleanText(content.RemoveAccents()).RemoveExtraSpaces();
        Alphabet = Content.GetAlphabet();
        Values = new ((int, string), int)[methods.Count];
        for (int i = 0; i < methods.Count; ++i)
        {
            Values[i] = new(new(methods[i].Id, methods[i].Name), methods[i].GetTextValue(Content));
        }
    }

    /// <summary>
    /// Gets the gematria value for the specified method identifier.
    /// </summary>
    /// <param name="methodId">The identifier of the gematria method.</param>
    /// <returns>The gematria value for the specified method identifier.</returns>
    public int GetValue(int methodId)
    {
        return Values.SingleOrDefault(v => v.GematriaMethod.Id == methodId).Value;
    }

    /// <summary>
    /// Gets the gematria value for the specified method name.
    /// </summary>
    /// <param name="methodName">The name of the gematria method.</param>
    /// <returns>The gematria value for the specified method name.</returns>
    public int GetValue(string methodName)
    {
        return Values.SingleOrDefault(v => v.GematriaMethod.Name == methodName).Value;
    }

    private static string CleanText(string text)
    {
        StringBuilder cleanedString = new(text.Length);
        foreach (char c in text)
        {
            cleanedString.Append(c.IsNonLetterCharacter(allowedExceptions) ? ' ' : c);
        }
        return cleanedString.ToString().Trim().Trim(trimChars);
    }

    /// <summary>
    /// Returns a string that represents the content of the phrase.
    /// </summary>    
    public override string ToString()
    {
        return Content;
    }

    /// <summary>
    /// Indicates whether the current phrase is equal to another phrase.
    /// </summary>
    /// <param name="other">The phrase to compare with the current phrase.</param>
    /// <returns>True if the current phrase is equal to the other phrase; otherwise, false.</returns>
    public bool Equals(Phrase other)
    {
        return Content == other.Content;
    }

    /// <summary>
    /// Indicates whether the current phrase is equal to another object.
    /// </summary>
    /// <param name="obj">The object to compare with the current phrase.</param>
    /// <returns>True if the current phrase is equal to the specified object; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
        if (obj is Phrase == false) return false;
        return Content == ((Phrase)obj).Content;
    }

    /// <summary>
    /// Determines whether two phrases are equal.
    /// </summary>
    /// <param name="left">The first phrase to compare.</param>
    /// <param name="right">The second phrase to compare.</param>
    /// <returns>True if the phrases are equal; otherwise, false.</returns>
    public static bool operator ==(Phrase left, Phrase right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two phrases are not equal.
    /// </summary>
    /// <param name="left">The first phrase to compare.</param>
    /// <param name="right">The second phrase to compare.</param>
    /// <returns>True if the phrases are not equal; otherwise, false.</returns>
    public static bool operator !=(Phrase left, Phrase right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current phrase.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Content);
    }

    /// <summary>
    /// Represents characters that we allow to exist in a phrase. For example the phrases "Don't do that" or "A double-edged sword" will be allowed as is.
    /// </summary>
    private static readonly char[] allowedExceptions = ['-', '\'', '’'];

    /// <summary>
    /// Represents characters that we don't allow to exist at the beginning or the end of a phrase, but are allowed anywhere else.
    /// </summary>
    private static readonly char[] trimChars = ['\'', '-', '’'];

    /// <summary>
    /// Gets the unique identifier for the phrase.
    /// </summary>
    public readonly long Id;

    /// <summary>
    /// Gets the identifier of the associated operation.
    /// </summary>
    public readonly int OperationId;

    /// <summary>
    /// Gets the content of the phrase.
    /// </summary>
    public readonly string Content;

    /// <summary>
    /// Gets the gematria values associated with the phrase and gematria methods.
    /// </summary>
    public readonly ((int Id, string Name) GematriaMethod, int Value)[] Values;

    /// <summary>
    /// Gets the alphabet of the phrase.
    /// </summary>
    public readonly Alphabet Alphabet;
}
