/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosModels.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ArithmosTests
{
    [TestClass]
    public class PhraseHelperTests
    {
        [TestMethod]
        public void PhraseContainsAnyValueSingleValue()
        {
            Phrase phrase = new("The revolution will not be televised");

            HashSet<int> singleValue = new HashSet<int> { 100, 146, 2000, 1500, 300, 3061 };
            CalculationMethod method = CalculationMethod.Gematria | CalculationMethod.Ordinal | CalculationMethod.Primes;

            Assert.IsTrue(PhraseHelper.PhraseContainsAnyValue(phrase, singleValue, method, out int found));
            Assert.IsTrue(found == 3061);
        }
        [TestMethod]
        public void PhraseContainsAnyValueMultipleValues()
        {
            Phrase phrase = new("The revolution will not be televised");

            HashSet<int> singleValue = new HashSet<int> { 100, 146, 397, 1500, 300, 3061 };
            CalculationMethod method = CalculationMethod.Gematria | CalculationMethod.Ordinal | CalculationMethod.Primes;

            Assert.IsTrue(PhraseHelper.PhraseContainsAnyValue(phrase, singleValue, method, out int found));
            Assert.IsTrue(found == 3061);
        }
        [TestMethod]
        public void PhraseContainsAnyValueNoValues()
        {
            Phrase phrase = new("The revolution will not be televised");

            HashSet<int> singleValue = new HashSet<int> { 100, 146, 324, 1500, 300, 1212 };
            CalculationMethod method = CalculationMethod.Gematria | CalculationMethod.Ordinal | CalculationMethod.Primes;

            Assert.IsFalse(PhraseHelper.PhraseContainsAnyValue(phrase, singleValue, method, out int found));
            Assert.IsTrue(found == -1);
        }
        [TestMethod]
        public void PhraseExceedsMaxValue()
        {
            Phrase phrase = new("We are not in Kansas any more");

            CalculationMethod method = CalculationMethod.Gematria | CalculationMethod.Ordinal | CalculationMethod.Reduced | CalculationMethod.Sumerian;

            int maxValue = 3000;
            Assert.IsFalse(PhraseHelper.PhraseExceedsMaxValue(phrase, method, maxValue));
            maxValue = 2000;
            Assert.IsFalse(PhraseHelper.PhraseExceedsMaxValue(phrase, method, maxValue));
            maxValue = 99;
            Assert.IsTrue(PhraseHelper.PhraseExceedsMaxValue(phrase, method, maxValue));
            maxValue = 100;
            Assert.IsFalse(PhraseHelper.PhraseExceedsMaxValue(phrase, method, maxValue));

            method = CalculationMethod.Primes;
            maxValue = 908;
            Assert.IsTrue(PhraseHelper.PhraseExceedsMaxValue(phrase, method, maxValue));
            maxValue = 909;
            Assert.IsFalse(PhraseHelper.PhraseExceedsMaxValue(phrase, method, maxValue));
            maxValue = 910;
            Assert.IsFalse(PhraseHelper.PhraseExceedsMaxValue(phrase, method, maxValue));
        }
    }
}