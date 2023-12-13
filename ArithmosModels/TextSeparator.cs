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
        if (NewLine) separators.AddRange(new List<string> { CharNewLineN, CharNewLineR, CharNewLineRN, CharNewLineNR });
        if (Comma) separators.Add(CharComma);
        if (Semicolon) separators.Add(CharSemicolon);
        if (GreekSemicolon) separators.Add(CharGreekSemicolon);
        if (Tab) separators.Add(CharTab);
        if (Colon) separators.Add(CharColon);
        if (FullStop) separators.Add(CharFullStop);
        if (Pipe) separators.Add(CharPipe);
        if (Space) separators.Add(CharSpace);
        return separators.ToArray();
    }

    /// <summary>
    /// Gets an array of unselected separators based on the configured options.
    /// </summary>
    /// <returns>An array of unselected separator characters as strings.</returns>
    public string[] GetUnSelectedSeparators()
    {
        List<string> separators = [];
        if (!NewLine) separators.AddRange(new List<string> { CharNewLineN, CharNewLineR, CharNewLineRN, CharNewLineNR });
        if (!Comma) separators.Add(CharComma);
        if (!Semicolon) separators.Add(CharSemicolon);
        if (!GreekSemicolon) separators.Add(CharGreekSemicolon);
        if (!Tab) separators.Add(CharTab);
        if (!Colon) separators.Add(CharColon);
        if (!FullStop) separators.Add(CharFullStop);
        if (!Pipe) separators.Add(CharPipe);
        if (!Space) separators.Add(CharSpace);
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


    public const string CharNewLineN = "\n";

    public const string CharNewLineR = "\r";

    public const string CharNewLineRN = "\r\n";

    public const string CharNewLineNR = "\n\r";

    public const string CharComma = ",";

    public const string CharSemicolon = ";";

    public const string CharGreekSemicolon = "·";

    public const string CharTab = "\t";

    public const string CharColon = ":";

    public const string CharFullStop = ".";

    public const string CharPipe = "|";

    public const string CharSpace = " ";

    /// <summary>
    /// Gets an array containing all possible separators, including space.
    /// </summary>
    /// <returns>An array of all possible separator strings.</returns>
    public static string[] GetAllSeparators()
    {
        return [CharNewLineN, CharNewLineR, CharNewLineRN, CharNewLineNR, CharComma, CharSemicolon, CharGreekSemicolon, CharTab, CharColon, CharFullStop, CharPipe, CharSpace];
    }

    /// <summary>
    /// Gets an array containing all possible separators except space.
    /// </summary>
    /// <returns>An array of all possible separator strings excluding space.</returns>
    public static string[] GetAllSeparatorsExceptSpace()
    {
        return [CharNewLineN, CharNewLineR, CharNewLineRN, CharNewLineNR, CharComma, CharSemicolon, CharGreekSemicolon, CharTab, CharColon, CharFullStop, CharPipe];
    }
}
