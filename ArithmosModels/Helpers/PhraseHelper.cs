using System;
using System.Collections.Generic;

namespace ArithmosModels.Helpers
{
    public static class PhraseHelper
    {
        /// <summary>
        /// Checks whether the phrase contains any of the given values
        /// </summary>
        /// <param name="phrase">The phrase to check</param>
        /// <param name="values">The values to check for</param>
        /// <param name="calculationMethod">The CalculationMethod to use</param>
        /// <param name="value">The found value</param>
        /// <returns>Boolean</returns>
        public static bool PhraseContainsAnyValue(Phrase phrase, HashSet<int> values, CalculationMethod calculationMethod, out int value)
        {
            value = -1;

            if (values.Contains(phrase.Gematria) && calculationMethod.HasFlag(CalculationMethod.Gematria))
            {
                value = phrase.Gematria;
                return true;
            }
            if (values.Contains(phrase.Ordinal) && calculationMethod.HasFlag(CalculationMethod.Ordinal))
            {
                value = phrase.Ordinal;
                return true;
            }
            if (values.Contains(phrase.Reduced) && calculationMethod.HasFlag(CalculationMethod.Reduced))
            {
                value = phrase.Reduced;
                return true;
            }
            if (values.Contains(phrase.Sumerian) && calculationMethod.HasFlag(CalculationMethod.Sumerian))
            {
                value = phrase.Sumerian;
                return true;
            }
            if (values.Contains(phrase.Primes) && calculationMethod.HasFlag(CalculationMethod.Primes))
            {
                value = phrase.Primes;
                return true;
            }
            if (values.Contains(phrase.Squared) && calculationMethod.HasFlag(CalculationMethod.Squared))
            {
                value = phrase.Squared;
                return true;
            }
            if (values.Contains(phrase.MisparGadol) && calculationMethod.HasFlag(CalculationMethod.MisparGadol))
            {
                value = phrase.MisparGadol;
                return true;
            }
            if (values.Contains(phrase.MisparShemi) && calculationMethod.HasFlag(CalculationMethod.MisparShemi))
            {
                value = phrase.MisparShemi;
                return true;
            }

            return false;
        }

        /// <summary>
        /// While scanning for a specific value we check if the phrase values already exceed that value
        /// </summary>
        /// <param name="phrase">The phrase we want to check</param>
        /// <param name="calculationMethod">The calculation method(s) we are using</param>
        /// <param name="maxValue">The maximum vcalue we compare to the phrase values</param>
        /// <returns>Boolean</returns>
        public static bool PhraseExceedsMaxValue(Phrase phrase, CalculationMethod calculationMethod, int maxValue)
        {
            foreach (CalculationMethod c in Enum.GetValues(typeof(CalculationMethod)))
            {
                if (c != CalculationMethod.None && c != CalculationMethod.All && calculationMethod.HasFlag(c))
                {
                    if (c == CalculationMethod.Gematria && phrase.Gematria <= maxValue) return false;
                    if (c == CalculationMethod.Ordinal && phrase.Ordinal <= maxValue) return false;
                    if (c == CalculationMethod.Reduced && phrase.Reduced <= maxValue) return false;
                    if (c == CalculationMethod.Sumerian && phrase.Sumerian <= maxValue) return false;
                    if (c == CalculationMethod.Primes && phrase.Primes <= maxValue) return false;
                    if (c == CalculationMethod.Squared && phrase.Squared <= maxValue) return false;
                    if (c == CalculationMethod.MisparGadol && phrase.MisparGadol <= maxValue) return false;
                    if (c == CalculationMethod.MisparShemi && phrase.MisparShemi <= maxValue) return false;
                }
            }

            return true;
        }
    }
}