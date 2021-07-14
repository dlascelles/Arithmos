/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Helpers;
using System;

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
        /// <exception cref="Argument">Thrown when the originalText parameter is null</exception>
        public Phrase(string originalText, long phraseId = 0, int operationId = 0)
        {
            if (originalText == null) { throw new ArgumentNullException(nameof(originalText), "Original Text cannot be null"); }

            Id = phraseId;
            OperationId = operationId;
            NormalizedText = CharacterHandler.NormalizeText(originalText.Trim());
            Alphabet = CharacterHandler.GetAlphabet(NormalizedText);
            Gematria = ValueMapper.GetValue(NormalizedText, CalculationMethod.Gematria);
            Ordinal = ValueMapper.GetValue(NormalizedText, CalculationMethod.Ordinal);
            Reduced = ValueMapper.GetValue(NormalizedText, CalculationMethod.Reduced);
            Sumerian = ValueMapper.GetValue(NormalizedText, CalculationMethod.Sumerian);
            Primes = ValueMapper.GetValue(NormalizedText, CalculationMethod.Primes);
            Squared = ValueMapper.GetValue(NormalizedText, CalculationMethod.Squared);
            MisparGadol = ValueMapper.GetValue(NormalizedText, CalculationMethod.MisparGadol);
            MisparShemi = ValueMapper.GetValue(NormalizedText, CalculationMethod.MisparShemi);
        }

        public override string ToString()
        {
            return NormalizedText;
        }

        public bool Equals(Phrase other)
        {
            return NormalizedText == other.NormalizedText;
        }

        public override bool Equals(object obj)
        {
            if (obj is Phrase == false) return false;
            return NormalizedText == ((Phrase)obj).NormalizedText;
        }

        public static bool operator ==(Phrase left, Phrase right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Phrase left, Phrase right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NormalizedText);
        }

        public readonly Alphabet Alphabet;

        public readonly string NormalizedText;

        public readonly long Id;

        public readonly int OperationId;

        public readonly int Gematria;

        public readonly int Ordinal;

        public readonly int Reduced;

        public readonly int Primes;

        public readonly int Squared;

        public readonly int Sumerian;

        public readonly int MisparGadol;

        public readonly int MisparShemi;
    }
}