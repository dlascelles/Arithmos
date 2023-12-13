/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
namespace ArithmosModels.Enums;

/// <summary>
/// Represents separators used for text formatting or data parsing.
/// </summary>
[Flags]
public enum Separator
{
    None = 0,
    NewLine = 1,
    Comma = 2,
    Semicolon = 4,
    GreekSemicolon = 8,
    Tab = 16,
    Colon = 32,
    FullStop = 64,
    Pipe = 128,
    Space = 256,
    AllExceptSpace = NewLine | Colon | Comma | FullStop | GreekSemicolon | Semicolon | Tab | Pipe,
    All = NewLine | Colon | Comma | FullStop | GreekSemicolon | Semicolon | Space | Tab | Pipe
}
