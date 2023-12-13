/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
namespace ArithmosModels.Enums;

/// <summary>
/// Represents the supported alphabets.
/// </summary>
[Flags]
public enum Alphabet
{
    None = 0,
    Unknown = 1,
    English = 2,
    Greek = 4,
    Hebrew = 8,
    Arabic = 16,
    Cyrillic = 32,
    Coptic = 64,
    All = ~None
}
