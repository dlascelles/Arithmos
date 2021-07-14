/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;

namespace ArithmosModels
{
    /// <summary>
    /// The alphabet of a character or a phrase
    /// </summary>
    [Flags]
    public enum Alphabet : sbyte
    {
        None = 0,
        Mixed = 1,
        English = 2,
        Greek = 4,
        Hebrew = 8,
        All = ~None
    }
}