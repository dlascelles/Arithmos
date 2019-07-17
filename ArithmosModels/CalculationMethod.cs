/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;

namespace ArithmosModels
{
    /// <summary>
    /// The different methods that can be used to determine a character's numerical value.
    /// </summary>
    [Flags]
    public enum CalculationMethod
    {
        None = 0,
        Gematria = 1,
        Ordinal = 2,
        Reduced = 4,
        Sumerian = 8,
        Primes = 16,
        Squared = 32,
        MisparGadol = 64,
        MisparShemi = 128,
        All = ~None
    }
}