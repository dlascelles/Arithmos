﻿/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArithmosModels
{
    /// <summary>
    /// A struct representing a phrase and its gematria values based on the various Calculation Methods
    /// </summary>
    public readonly struct Phrase : IEquatable<Phrase>
    {
        /// <summary>
        /// Constructs a new instance of a readonly struct of type Phrase 
        /// </summary>
        /// <param name="originalText">The original text from which we will derive a new phrase</param>
        /// <param name="operationId">The id of the operation associated with this phrase</param>
        /// <param name="phraseId">The id of the phrase</param>
        /// <exception cref="Argument">Thrown when the originalText parameter is null</exception>
        public Phrase(string originalText, int operationId = 0, long phraseId = 0)
        {
            if (originalText == null) throw new ArgumentNullException("Original Text cannot be null");
            Id = phraseId;
            OperationId = operationId;
            NormalizedText = CharacterHandler.NormalizeText(originalText.Trim());
            Alphabet = CharacterHandler.GetAlphabet(NormalizedText);
            Values = new Dictionary<CalculationMethod, int>() {
                { CalculationMethod.Gematria, ValueMapper.GetValue(NormalizedText, CalculationMethod.Gematria) }, { CalculationMethod.Ordinal, ValueMapper.GetValue(NormalizedText, CalculationMethod.Ordinal) },
                { CalculationMethod.Reduced, ValueMapper.GetValue(NormalizedText, CalculationMethod.Reduced) }, { CalculationMethod.Primes, ValueMapper.GetValue(NormalizedText, CalculationMethod.Primes) },
                { CalculationMethod.Squared, ValueMapper.GetValue(NormalizedText, CalculationMethod.Squared) }, { CalculationMethod.Sumerian, ValueMapper.GetValue(NormalizedText, CalculationMethod.Sumerian) },
                { CalculationMethod.MisparGadol, ValueMapper.GetValue(NormalizedText, CalculationMethod.MisparGadol) }, { CalculationMethod.MisparShemi, ValueMapper.GetValue(NormalizedText, CalculationMethod.MisparShemi) }
            };
        }

        /// <summary>
        /// Checks whether the phrase contains any of the given values
        /// </summary>
        /// <param name="values">The values to check</param>
        /// <param name="calculationMethod">The CalculationMethod to use</param>
        /// <param name="value">The found value</param>
        /// <returns>Boolean</returns>
        public bool ContainsAnyValue(int[] values, CalculationMethod calculationMethod, out int value)
        {
            value = -1;

            foreach (CalculationMethod c in Values.Keys)
            {
                if (calculationMethod.HasFlag(c) && values.Contains(Values[c]))
                {
                    value = Values[c];
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return NormalizedText;
        }

        public bool Equals(Phrase phrase)
        {
            return NormalizedText == phrase.NormalizedText;
        }

        public override bool Equals(object obj)
        {
            if (obj is Phrase == false) return false;
            return NormalizedText == ((Phrase)obj).NormalizedText;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = NormalizedText != null ? hash * 23 + NormalizedText.GetHashCode() : 0;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + OperationId.GetHashCode();
                return hash;
            }
        }

        public Alphabet Alphabet { get; }

        public string NormalizedText { get; }

        public long Id { get; }

        public int OperationId { get; }

        public Dictionary<CalculationMethod, int> Values { get; }
    }
}