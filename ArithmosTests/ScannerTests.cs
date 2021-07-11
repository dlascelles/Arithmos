/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosModels.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArithmosTests
{
    [TestClass]
    public class ScannerTests
    {
        [TestMethod]
        public async Task ScanTestAsync()
        {
            string[] words = { "NEW", "YORK", "CITY", "IS", "THE", "BEST", "SUN", "MOON" };
            int[] values = { 276, 666, 888, 474, 510 };

            HashSet<Phrase> matched = new HashSet<Phrase>(await Scanner.ScanAsync(words, PhraseSeparator.All, values, CalculationMethod.Sumerian, 1, 1, 5));

            Assert.IsTrue(matched.Count == 5);
            Assert.IsTrue(matched.Contains(new Phrase("NEW YORK")));
            Assert.IsTrue(matched.Contains(new Phrase("CITY IS")));
            Assert.IsTrue(matched.Contains(new Phrase("THE BEST")));
            Assert.IsTrue(matched.Contains(new Phrase("BEST")));
            Assert.IsTrue(matched.Contains(new Phrase("SUN MOON")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 276 ||
                    phrase.Values[CalculationMethod.Sumerian] == 666 ||
                    phrase.Values[CalculationMethod.Sumerian] == 888 ||
                    phrase.Values[CalculationMethod.Sumerian] == 474 ||
                    phrase.Values[CalculationMethod.Sumerian] == 510);
            }

            words = new string[] { "FISH", "LAUGH", "LIKENED", "COZY", "MOADIAH", "AGONE", "JERIAH", "MOON" };

            values = new int[] { 123 };
            matched = new HashSet<Phrase>(await Scanner.ScanAsync(words, PhraseSeparator.All, values, CalculationMethod.Gematria, 1, 1, 1));
            Assert.IsTrue(matched.Count == 5);
            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 123);
            }

        }

        [TestMethod]
        public async Task EnglishScanFileTestAsync()
        {
            HashSet<Phrase> matched = new HashSet<Phrase>();
            string path = @$"{Environment.CurrentDirectory}\\Assets\\bible.txt";
            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 123 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 15);
            Assert.IsTrue(matched.Contains(new Phrase("FISH")));
            Assert.IsTrue(matched.Contains(new Phrase("HEIFER")));
            Assert.IsTrue(matched.Contains(new Phrase("MOCK")));
            Assert.IsTrue(matched.Contains(new Phrase("CAMPED")));
            Assert.IsTrue(matched.Contains(new Phrase("GAMALIEL")));
            Assert.IsTrue(matched.Contains(new Phrase("PEDAHEL")));
            Assert.IsTrue(matched.Contains(new Phrase("AGONE")));
            Assert.IsTrue(matched.Contains(new Phrase("MALCHAM")));
            Assert.IsTrue(matched.Contains(new Phrase("JERIAH")));
            Assert.IsTrue(matched.Contains(new Phrase("PILEHA")));
            Assert.IsTrue(matched.Contains(new Phrase("MOADIAH")));
            Assert.IsTrue(matched.Contains(new Phrase("LIKENED")));
            Assert.IsTrue(matched.Contains(new Phrase("DECEASE")));
            Assert.IsTrue(matched.Contains(new Phrase("CREEK")));
            Assert.IsTrue(matched.Contains(new Phrase("DEACON")));
            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 123);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 123 }, CalculationMethod.Sumerian, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 0);

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 123 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 118);
            Assert.IsTrue(matched.Contains(new Phrase("HE LODGED")));
            Assert.IsTrue(matched.Contains(new Phrase("JACOB HELD")));
            Assert.IsTrue(matched.Contains(new Phrase("MOCK")));
            Assert.IsTrue(matched.Contains(new Phrase("ENDED AND")));
            Assert.IsTrue(matched.Contains(new Phrase("NEED IN")));
            Assert.IsTrue(matched.Contains(new Phrase("HANG HIM")));
            Assert.IsTrue(matched.Contains(new Phrase("ON HE")));
            Assert.IsTrue(matched.Contains(new Phrase("HE SEE")));
            Assert.IsTrue(matched.Contains(new Phrase("GALL AND")));
            Assert.IsTrue(matched.Contains(new Phrase("AND HE AND")));
            Assert.IsTrue(matched.Contains(new Phrase("HIM MAKE")));
            Assert.IsTrue(matched.Contains(new Phrase("AGONE")));
            Assert.IsTrue(matched.Contains(new Phrase("CHILD DEAD AND")));
            Assert.IsTrue(matched.Contains(new Phrase("HAD ON")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 123);
                int count = phrase.NormalizedText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Count();
                Assert.IsTrue(count >= 1 && count <= 5);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 123 }, CalculationMethod.Sumerian, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 0);

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 324, 206, 241, 263, 255 }, CalculationMethod.Ordinal, PhraseSeparator.All, 1, 6, 6));
            Assert.IsTrue(matched.Count == 16636);
            Assert.IsTrue(matched.Contains(new Phrase("INTO THE CONDEMNATION OF THE DEVIL")));
            Assert.IsTrue(matched.Contains(new Phrase("AND JACOB SWARE BY THE FEAR")));
            Assert.IsTrue(matched.Contains(new Phrase("JACOB SAID TO SIMEON AND LEVI")));
            Assert.IsTrue(matched.Contains(new Phrase("BUTTER AND HONEY SHALL HE EAT")));
            Assert.IsTrue(matched.Contains(new Phrase("YE KNOW NOT WHAT YE ASK")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.NormalizedText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Count() == 6);
            }

            path = @$"{Environment.CurrentDirectory}\\Assets\\moby-dick.txt";
            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 444 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 26);
            Assert.IsTrue(matched.Contains(new Phrase("EVIL")));
            Assert.IsTrue(matched.Contains(new Phrase("GOODHEARTED")));
            Assert.IsTrue(matched.Contains(new Phrase("PANTHEON")));
            Assert.IsTrue(matched.Contains(new Phrase("FLUSH")));
            Assert.IsTrue(matched.Contains(new Phrase("BEEFSTEAKS")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 444);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 444 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 231);
            Assert.IsTrue(matched.Contains(new Phrase("ON ALL SIDES AND")));
            Assert.IsTrue(matched.Contains(new Phrase("TO BE AFRAID OF")));
            Assert.IsTrue(matched.Contains(new Phrase("TO ESCAPE")));
            Assert.IsTrue(matched.Contains(new Phrase("FLUSH")));
            Assert.IsTrue(matched.Contains(new Phrase("HAND AND FOOT")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 444);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }

            path = @$"{Environment.CurrentDirectory}\\Assets\\paradise-lost.txt";
            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 444, 555, 666, 777, 888 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 42);
            Assert.IsTrue(matched.Contains(new Phrase("WINGS")));
            Assert.IsTrue(matched.Contains(new Phrase("WAND")));
            Assert.IsTrue(matched.Contains(new Phrase("OBSERVED")));
            Assert.IsTrue(matched.Contains(new Phrase("DAWN")));
            Assert.IsTrue(matched.Contains(new Phrase("BROTHERS")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 444 ||
                    phrase.Values[CalculationMethod.Gematria] == 555 ||
                    phrase.Values[CalculationMethod.Gematria] == 666 ||
                    phrase.Values[CalculationMethod.Gematria] == 777 ||
                    phrase.Values[CalculationMethod.Gematria] == 888);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 444, 555, 666, 777, 888 }, CalculationMethod.Sumerian, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 209);
            Assert.IsTrue(matched.Contains(new Phrase("HEAVENS")));
            Assert.IsTrue(matched.Contains(new Phrase("ETHEREAL")));
            Assert.IsTrue(matched.Contains(new Phrase("SPEEDY")));
            Assert.IsTrue(matched.Contains(new Phrase("SOMEWHERE")));
            Assert.IsTrue(matched.Contains(new Phrase("SPEECHLESS")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 444 ||
                    phrase.Values[CalculationMethod.Sumerian] == 555 ||
                    phrase.Values[CalculationMethod.Sumerian] == 666 ||
                    phrase.Values[CalculationMethod.Sumerian] == 777 ||
                    phrase.Values[CalculationMethod.Sumerian] == 888);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 40, 50, 60, 70, 111 }, CalculationMethod.Ordinal, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 427);
            Assert.IsTrue(matched.Contains(new Phrase("WHOSE")));
            Assert.IsTrue(matched.Contains(new Phrase("MIND")));
            Assert.IsTrue(matched.Contains(new Phrase("HILLS")));
            Assert.IsTrue(matched.Contains(new Phrase("DUTY")));
            Assert.IsTrue(matched.Contains(new Phrase("ABANDONED")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 40 ||
                    phrase.Values[CalculationMethod.Ordinal] == 50 ||
                    phrase.Values[CalculationMethod.Ordinal] == 60 ||
                    phrase.Values[CalculationMethod.Ordinal] == 70 ||
                    phrase.Values[CalculationMethod.Ordinal] == 111);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 40, 50, 60, 70, 111 }, CalculationMethod.Ordinal, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 2317);
            Assert.IsTrue(matched.Contains(new Phrase("OREB")));
            Assert.IsTrue(matched.Contains(new Phrase("ENRAGED MIGHT")));
            Assert.IsTrue(matched.Contains(new Phrase("DARK DESIGNS")));
            Assert.IsTrue(matched.Contains(new Phrase("SHOULD BE ALL")));
            Assert.IsTrue(matched.Contains(new Phrase("PROUDLY")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 40 ||
                    phrase.Values[CalculationMethod.Ordinal] == 50 ||
                    phrase.Values[CalculationMethod.Ordinal] == 60 ||
                    phrase.Values[CalculationMethod.Ordinal] == 70 ||
                    phrase.Values[CalculationMethod.Ordinal] == 111);
                Assert.IsTrue(phrase.Alphabet == Alphabet.English);
            }


        }

        [TestMethod]
        public async Task EnglishScanTextTestAsync()
        {
            string allStates = @"Alabama
Alaska
Arizona
Arkansas
California
Colorado
Connecticut
Delaware
Florida
Georgia
Hawaii
Idaho
Illinois
Indiana
Iowa
Kansas
Kentucky
Louisiana
Maine
Maryland
Massachusetts
Michigan
Minnesota
Mississippi
Missouri
Montana
Nebraska
Nevada
New Hampshire
New Jersey
New Mexico
New York
North Carolina
North Dakota
Ohio
Oklahoma
Oregon
Pennsylvania
Rhode Island
South Carolina
South Dakota
Tennessee
Texas
Utah
Vermont
Virginia
Washington
West Virginia
Wisconsin
Wyoming";
            PhraseSeparator currentSeparator = PhraseSeparator.NewLine;
            HashSet<Phrase> matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(allStates, currentSeparator, 1, 1, 3));
            Assert.IsTrue(matched.Count == 50);

            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(allStates, new int[] { 666 }, CalculationMethod.Sumerian, currentSeparator, 1, 1, 3));
            Assert.IsTrue(matched.Count == 2);
            Assert.IsTrue(matched.Contains(new Phrase("NEW MEXICO")));
            Assert.IsTrue(matched.Contains(new Phrase("NEW YORK")));

            allStates = "Alabama,Alaska,Arizona,Arkansas,California,Colorado,Connecticut,Delaware,Florida,Georgia,Hawaii,Idaho,Illinois,Indiana,Iowa,Kansas,Kentucky,Louisiana,Maine,Maryland,Massachusetts,Michigan,Minnesota,Mississippi,Missouri,Montana,Nebraska,Nevada,New Hampshire,New Jersey,New Mexico,New York,North Carolina,North Dakota,Ohio,Oklahoma,Oregon,Pennsylvania,Rhode Island,South Carolina,South Dakota,Tennessee,Texas,Utah,Vermont,Virginia,Washington,West Virginia,Wisconsin,Wyoming";

            currentSeparator = PhraseSeparator.Comma;
            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(allStates, currentSeparator, 1, 1, 3));
            Assert.IsTrue(matched.Count == 50);

            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(allStates, new int[] { 666 }, CalculationMethod.Sumerian, currentSeparator, 1, 1, 3));
            Assert.IsTrue(matched.Count == 2);
            Assert.IsTrue(matched.Contains(new Phrase("NEW MEXICO")));
            Assert.IsTrue(matched.Contains(new Phrase("NEW YORK")));

            string str = "THE DIE HAS BEEN CAST";
            currentSeparator = PhraseSeparator.Comma;
            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(str, new int[] { 888 }, CalculationMethod.Sumerian, currentSeparator, 1, 1, 5));
            Assert.IsTrue(matched.Count == 1);
            Assert.IsTrue(matched.Contains(new Phrase("THE DIE HAS BEEN CAST")));

            currentSeparator = PhraseSeparator.AllExceptSpace;
            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(str, new int[] { 888 }, CalculationMethod.Sumerian, currentSeparator, 1, 1, 5));
            Assert.IsTrue(matched.Count == 1);
            Assert.IsTrue(matched.Contains(new Phrase("THE DIE HAS BEEN CAST")));

            currentSeparator = PhraseSeparator.AllExceptSpace;
            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(str, currentSeparator, 1, 1, 5));
            Assert.IsTrue(matched.Count == 1);
            Assert.IsTrue(matched.Contains(new Phrase("THE DIE HAS BEEN CAST")));

            currentSeparator = PhraseSeparator.All;
            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(str, currentSeparator, 1, 1, 5));
            Assert.IsTrue(matched.Count == 15);
            Assert.IsTrue(matched.Contains(new Phrase("THE DIE HAS BEEN CAST")));

            currentSeparator = PhraseSeparator.All;
            matched = new HashSet<Phrase>(await Scanner.ScanTextAsync(str, new int[] { 888 }, CalculationMethod.Sumerian, currentSeparator, 1, 1, 5));
            Assert.IsTrue(matched.Count == 1);
            Assert.IsTrue(matched.Contains(new Phrase("THE DIE HAS BEEN CAST")));
        }

        [TestMethod]
        public async Task GreekScanFileTestAsync()
        {
            HashSet<Phrase> matched = new HashSet<Phrase>();
            string path = @$"{Environment.CurrentDirectory}\\Assets\\apocalypse.txt";
            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 444 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 3);
            Assert.IsTrue(matched.Contains(new Phrase("ΒΑΣΑΝΙΖΟΜΕΝΗ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΠΟΛΕΜΗΣΑΙ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΘΕΡΙΣΟΝ")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 444);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 555 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 2);
            Assert.IsTrue(matched.Contains(new Phrase("ΔΥΝΑΜΙΝ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΔΡΑΚΟΝΤΙ")));
            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 555);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 888 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 12);
            Assert.IsTrue(matched.Contains(new Phrase("ΗΜΑΣ ΒΑΣΙΛΕΙΑΝ ΙΕΡΕΙΣ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΘΕΟΥ ΚΑΙ ΔΙΑ ΤΗΝ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΛΕΓΩΝ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΖΩΝ ΚΑΙ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΕΛΑΒΕ ΤΟ ΒΙΒΛΙΟΝ ΤΑ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΕΠΙ ΤΑΣ ΠΗΓΑΣ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΚΑΘΗΜΕΝΟΥΣ ΕΠ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΚΑΙ ΛΕΓΟΥΣΙ ΜΟΙ ΔΕΙ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΚΑΙ ΑΠΗΛΘΕ ΠΟΙΗΣΑΙ ΠΟΛΕΜΟΝ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΤΗΝ ΓΗΝ ΚΑΙ ΕΓΕΝΕΤΟ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΚΑΙ ΤΕΛΟΣ ΜΑΚΑΡΙΟΙ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΙΗΣΟΥΣ")));
            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 888);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);
            }

            path = @$"{Environment.CurrentDirectory}\\Assets\\prometheus.txt";
            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 666 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 2);
            Assert.IsTrue(matched.Contains(new Phrase("ΕΚΛΥΣΑΙ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΖΕΥΓΛΑΙΣΙ")));
            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 666);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 999 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 10);
            Assert.IsTrue(matched.Contains(new Phrase("ΧΘΟΝΟΣ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΠΥΡΟΣ ΠΗΓΗΝ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΖΕΥΣ ΑΛΛ ΕΜΠΑΣ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΜΟΧΘΟΙΣ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΕΓΩ ΜΕΝ ΕΙΜΙ ΚΑΙ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΛΕΥΡΟΝ ΓΑΡ ΟΙΜΟΝ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΤΙ ΓΕΝΟΣ ΤΙΝΑ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΕΙ Δ ΕΧΕΙΣ ΕΙΠΕΙΝ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΠΕΛΑΓΟΣ ΑΤΗΡΑΣ")));
            Assert.IsTrue(matched.Contains(new Phrase("ΣΗΜΗΝΟΝ ΕΙ ΜΗ ΤΙΣ")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 999);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);
            }
        }

        [TestMethod]
        public async Task HebrewScanFileTestAsync()
        {
            HashSet<Phrase> matched = new HashSet<Phrase>();
            string path = @$"{Environment.CurrentDirectory}\\Assets\\hunger.txt";
            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 444 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 8);
            Assert.IsTrue(matched.Contains(new Phrase("מדת")));
            Assert.IsTrue(matched.Contains(new Phrase("ותחל")));
            Assert.IsTrue(matched.Contains(new Phrase("כחותי")));
            Assert.IsTrue(matched.Contains(new Phrase("להגות")));
            Assert.IsTrue(matched.Contains(new Phrase("דלתי")));
            Assert.IsTrue(matched.Contains(new Phrase("ולבקשו")));
            Assert.IsTrue(matched.Contains(new Phrase("ילדת")));
            Assert.IsTrue(matched.Contains(new Phrase("ולזאת")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 444);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 444 }, CalculationMethod.Gematria, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 51);
            Assert.IsTrue(matched.Contains(new Phrase("בגדי השמיע")));
            Assert.IsTrue(matched.Contains(new Phrase("אני מכיסי כסף לבטלה או")));
            Assert.IsTrue(matched.Contains(new Phrase("יורד בהצלחה למטה")));
            Assert.IsTrue(matched.Contains(new Phrase("ואולי ירע לי בגללו")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 444);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 666 }, CalculationMethod.MisparGadol, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 7);
            Assert.IsTrue(matched.Contains(new Phrase("תכריכיבד")));
            Assert.IsTrue(matched.Contains(new Phrase("ותמכר")));
            Assert.IsTrue(matched.Contains(new Phrase("השושנה")));
            Assert.IsTrue(matched.Contains(new Phrase("שרעפיו")));
            Assert.IsTrue(matched.Contains(new Phrase("כאלהים")));
            Assert.IsTrue(matched.Contains(new Phrase("יומים")));
            Assert.IsTrue(matched.Contains(new Phrase("ושרעפי")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 666);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 666 }, CalculationMethod.MisparGadol, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 33);
            Assert.IsTrue(matched.Contains(new Phrase("להכיר את")));
            Assert.IsTrue(matched.Contains(new Phrase("נפלאה ער כרי")));
            Assert.IsTrue(matched.Contains(new Phrase("חפצו מאתי לא")));
            Assert.IsTrue(matched.Contains(new Phrase("הסוחר אולי איש טוב הוא")));
            Assert.IsTrue(matched.Contains(new Phrase("מעוני ואותי יכלאו")));
            Assert.IsTrue(matched.Contains(new Phrase("אך ננסה")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 666);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 123 }, CalculationMethod.Ordinal, PhraseSeparator.All, 1, 1, 1));
            Assert.IsTrue(matched.Count == 8);
            Assert.IsTrue(matched.Contains(new Phrase("תמהוןראשי")));
            Assert.IsTrue(matched.Contains(new Phrase("ברכתהשלום")));
            Assert.IsTrue(matched.Contains(new Phrase("בנפשיואחלוף")));
            Assert.IsTrue(matched.Contains(new Phrase("גרנלנדסלרט")));
            Assert.IsTrue(matched.Contains(new Phrase("בדקדוקהלשון")));
            Assert.IsTrue(matched.Contains(new Phrase("יועץהמשרה")));
            Assert.IsTrue(matched.Contains(new Phrase("בהתמרמרות")));
            Assert.IsTrue(matched.Contains(new Phrase("המפורסמים")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 123);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
            }

            matched = new HashSet<Phrase>(await Scanner.ScanFileAsync(path, new int[] { 123 }, CalculationMethod.Ordinal, PhraseSeparator.All, 1, 1, 5));
            Assert.IsTrue(matched.Count == 393);
            Assert.IsTrue(matched.Contains(new Phrase("את חותמה המיוחד")));
            Assert.IsTrue(matched.Contains(new Phrase("הדבר האחד אשר מצא")));
            Assert.IsTrue(matched.Contains(new Phrase("ויאמר כי לא אצלח")));
            Assert.IsTrue(matched.Contains(new Phrase("ביום נחמד אשר")));
            Assert.IsTrue(matched.Contains(new Phrase("מיום אתמול לא בא")));
            Assert.IsTrue(matched.Contains(new Phrase("יותר בהירות")));
            Assert.IsTrue(matched.Contains(new Phrase("רבה באכילה מה טוב היה")));
            Assert.IsTrue(matched.Contains(new Phrase("הזמן והמקום")));

            foreach (Phrase phrase in matched)
            {
                Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 123);
                Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
            }

        }
    }
}