/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ArithmosTests
{
    [TestClass]
    public class ExporterTests
    {
        [TestMethod]
        public void GetPhrasesForExport()
        {
            List<Phrase> phrases = new()
            {
                new("And that's the way it is"),
                new("Bazinga!"),
                new("Did I do that?"),
                new("Eat my shorts"),
                new("Good night, and good luck"),
                new("Here it is, your moment of Zen"),
                new("Oh my God, they killed Kenny"),
            };

            string expected = @"Phrase,Gematria,Ordinal,Reduced,Sumerian,Primes,Squared,MisparGadol,MisparShemi
AND THATS THE WAY IT IS‎,2296,226,73,1356,749,922834,0,0
BAZINGA‎,870,60,33,360,191,642636,0,0
DID I DO THAT‎,499,94,49,564,277,83875,0,0
EAT MY SHORTS‎,1504,163,46,978,554,603390,0,0
GOOD NIGHT AND GOOD LUCK‎,944,206,98,1236,607,151050,0,0
HERE IT IS YOUR MOMENT OF ZEN‎,2892,318,129,1908,1033,1345562,0,0
OH MY GOD THEY KILLED KENNY‎,2715,267,114,1602,844,1526765,0,0
";
            string result = Exporter.GetPhrasesForExport(phrases, ',');
            Assert.IsTrue(result == expected);
        }
    }
}