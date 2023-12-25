/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Enums;

namespace ArithmosModels;

/// <summary>
/// Represents a utility class for handling text separators based on specified separator options.
/// </summary>
public class TextSeparator(Separator separator)
{
    /// <summary>
    /// Gets an array of selected separators based on the configured options.
    /// </summary>
    /// <returns>An array of selected separator characters as strings.</returns>
    public string[] GetSelectedSeparators()
    {
        List<string> separators = [];
        if (NewLine) separators.AddRange(new List<string> { Constants.Characters.NewLineN, Constants.Characters.NewLineR, Constants.Characters.NewLineRN, Constants.Characters.NewLineNR });
        if (Comma) separators.Add(Constants.Characters.Comma);
        if (Semicolon) separators.Add(Constants.Characters.Semicolon);
        if (GreekSemicolon) separators.Add(Constants.Characters.GreekSemicolon);
        if (Tab) separators.Add(Constants.Characters.Tab);
        if (Colon) separators.Add(Constants.Characters.Colon);
        if (FullStop) separators.Add(Constants.Characters.FullStop);
        if (Pipe) separators.Add(Constants.Characters.Pipe);
        if (Space) separators.Add(Constants.Characters.Space);
        return separators.ToArray();
    }

    /// <summary>
    /// Gets an array of unselected separators based on the configured options.
    /// </summary>
    /// <returns>An array of unselected separator characters as strings.</returns>
    public string[] GetUnSelectedSeparators()
    {
        List<string> separators = [];
        if (!NewLine) separators.AddRange(new List<string> { Constants.Characters.NewLineN, Constants.Characters.NewLineR, Constants.Characters.NewLineRN, Constants.Characters.NewLineNR });
        if (!Comma) separators.Add(Constants.Characters.Comma);
        if (!Semicolon) separators.Add(Constants.Characters.Semicolon);
        if (!GreekSemicolon) separators.Add(Constants.Characters.GreekSemicolon);
        if (!Tab) separators.Add(Constants.Characters.Tab);
        if (!Colon) separators.Add(Constants.Characters.Colon);
        if (!FullStop) separators.Add(Constants.Characters.FullStop);
        if (!Pipe) separators.Add(Constants.Characters.Pipe);
        if (!Space) separators.Add(Constants.Characters.Space);
        return separators.ToArray();
    }

    public readonly bool NewLine = (separator & Separator.NewLine) != 0;

    public readonly bool Comma = (separator & Separator.Comma) != 0;

    public readonly bool Semicolon = (separator & Separator.Semicolon) != 0;

    public readonly bool GreekSemicolon = (separator & Separator.GreekSemicolon) != 0;

    public readonly bool Tab = (separator & Separator.Tab) != 0;

    public readonly bool Colon = (separator & Separator.Colon) != 0;

    public readonly bool FullStop = (separator & Separator.FullStop) != 0;

    public readonly bool Pipe = (separator & Separator.Pipe) != 0;

    public readonly bool Space = (separator & Separator.Space) != 0;
    
    /// <summary>
    /// Gets an array containing all possible separators, including space.
    /// </summary>
    /// <returns>An array of all possible separator strings.</returns>
    public static string[] GetAllSeparators()
    {
        return [Constants.Characters.NewLineN, Constants.Characters.NewLineR, Constants.Characters.NewLineRN, Constants.Characters.NewLineNR, Constants.Characters.Comma, Constants.Characters.Semicolon, Constants.Characters.GreekSemicolon, Constants.Characters.Tab, Constants.Characters.Colon, Constants.Characters.FullStop, Constants.Characters.Pipe, Constants.Characters.Space];
    }

    /// <summary>
    /// Gets an array containing all possible separators except space.
    /// </summary>
    /// <returns>An array of all possible separator strings excluding space.</returns>
    public static string[] GetAllSeparatorsExceptSpace()
    {
        return [Constants.Characters.NewLineN, Constants.Characters.NewLineR, Constants.Characters.NewLineRN, Constants.Characters.NewLineNR, Constants.Characters.Comma, Constants.Characters.Semicolon, Constants.Characters.GreekSemicolon, Constants.Characters.Tab, Constants.Characters.Colon, Constants.Characters.FullStop, Constants.Characters.Pipe];
    }
}
