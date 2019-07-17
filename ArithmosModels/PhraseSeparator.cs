/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;

namespace ArithmosModels
{
    /// <summary>
    /// The separators that can be used to split phrases when scanning text.
    /// </summary>
    [Flags]
    public enum PhraseSeparator
    {
        None = 0,
        NewLine = 1,
        Comma = 2,
        Semicolon = 4,
        GreekSemicolon = 8,
        Tab = 16,
        Colon = 32,
        FullStop = 64,
        Space = 128,
        AllExceptSpace = PhraseSeparator.NewLine | PhraseSeparator.Colon | PhraseSeparator.Comma | PhraseSeparator.FullStop | PhraseSeparator.GreekSemicolon | PhraseSeparator.Semicolon | PhraseSeparator.Tab,
        All = PhraseSeparator.NewLine | PhraseSeparator.Colon | PhraseSeparator.Comma | PhraseSeparator.FullStop | PhraseSeparator.GreekSemicolon | PhraseSeparator.Semicolon | PhraseSeparator.Space | PhraseSeparator.Tab
    }
}
