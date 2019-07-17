/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArithmosTests
{
    [TestClass]
    public class ValueMapperTests
    {
        [TestMethod]
        public void EnglishLanguageTest()
        {
            Phrase phrase = new Phrase("World Wide Web");
            Assert.IsTrue(phrase.NormalizedText == "WORLD WIDE WEB");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 858);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 143);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 1709);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 62);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("The quick brown fox");
            Assert.IsTrue(phrase.NormalizedText == "THE QUICK BROWN FOX");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 1266);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 211);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 1993);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 85);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("The#$( *&^$&^*(     quick            brown fox     ;'[]/..,<>?:      ");
            Assert.IsTrue(phrase.NormalizedText == "THE QUICK BROWN FOX");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 1266);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 211);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 1993);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 85);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase(@"   HELLO, MY 
NAME IS INIGO 
MONTOYA                                                                                                                             ");
            Assert.IsTrue(phrase.NormalizedText == "HELLO MY NAME IS INIGO MONTOYA");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 1848);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 308);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 2324);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 128);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("serendipity");
            Assert.IsTrue(phrase.NormalizedText == "SERENDIPITY");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 864);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 144);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 1242);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 63);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.IsTrue(phrase.NormalizedText == "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 2106);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 351);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 4095);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 126);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.");
            Assert.IsTrue(phrase.NormalizedText == "LOREM IPSUM DOLOR SIT AMET CONSECTETUR ADIPISCING ELIT SED DO EIUSMOD TEMPOR");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 4752);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 792);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 4221);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 306);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);
        }

        [TestMethod]
        public void HebrewLanguageTest()
        {
            Phrase phrase = new Phrase("גימטריה");
            Assert.IsTrue(phrase.NormalizedText == "גימטריה");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 70);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 277);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 277);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 25);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("ראשית");
            Assert.IsTrue(phrase.NormalizedText == "ראשית");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 74);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 911);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 911);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 11);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("אבטיח");
            Assert.IsTrue(phrase.NormalizedText == "אבטיח");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 30);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 30);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 30);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 21);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("אוניברסיטה");
            Assert.IsTrue(phrase.NormalizedText == "אוניברסיטה");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 92);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 353);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 353);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 38);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("אגוז מלך");
            Assert.IsTrue(phrase.NormalizedText == "אגוז מלך");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 65);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 107);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 587);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 26);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("             אגוז            מלך\n\n\n\n\n          \n\r           \r\r\r");
            Assert.IsTrue(phrase.NormalizedText == "אגוז מלך");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 65);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 107);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 587);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 26);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("ארוחת צהריים");
            Assert.IsTrue(phrase.NormalizedText == "ארוחת צהריים");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 144);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 970);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 1530);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 43);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("תאריך        לידה");
            Assert.IsTrue(phrase.NormalizedText == "תאריך לידה");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 107);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 680);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 1160);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 23);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
        }

        [TestMethod]
        public void GreekLanguageTest()
        {
            Phrase phrase = new Phrase("ὑμῶν δίκαια καὶ ὑμῖν τε ῥᾴδια");
            Assert.IsTrue(phrase.NormalizedText == "ΥΜΩΝ ΔΙΚΑΙΑ ΚΑΙ ΥΜΙΝ ΤΕ ΡΑΔΙΑ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 254);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 2288);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 65);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("πείσας ὑπομεῖναι καὶ μὴ οἴχεσθαι φεύγοντα ἐγώ, πολλὰ ἱκετεύσας καὶ λαμβανόμενος τῶν γονάτων");
            Assert.IsTrue(phrase.NormalizedText == "ΠΕΙΣΑΣ ΥΠΟΜΕΙΝΑΙ ΚΑΙ ΜΗ ΟΙΧΕΣΘΑΙ ΦΕΥΓΟΝΤΑ ΕΓΩ ΠΟΛΛΑ ΙΚΕΤΕΥΣΑΣ ΚΑΙ ΛΑΜΒΑΝΟΜΕΝΟΣ ΤΩΝ ΓΟΝΑΤΩΝ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 936);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 8649);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 297);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("και     αναστεναξας τωι     πνευματι αυτου λεγει τι η γενεα αυτη σημειον επιζητει αμην λεγω υμιν \n ει δοθησεται τηι γενεαι ταυτηι σημειον");
            Assert.IsTrue(phrase.NormalizedText == "ΚΑΙ ΑΝΑΣΤΕΝΑΞΑΣ ΤΩΙ ΠΝΕΥΜΑΤΙ ΑΥΤΟΥ ΛΕΓΕΙ ΤΙ Η ΓΕΝΕΑ ΑΥΤΗ ΣΗΜΕΙΟΝ ΕΠΙΖΗΤΕΙ ΑΜΗΝ ΛΕΓΩ ΥΜΙΝ ΕΙ ΔΟΘΗΣΕΤΑΙ ΤΗΙ ΓΕΝΕΑΙ ΤΑΥΤΗΙ ΣΗΜΕΙΟΝ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 1196);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 9872);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 413);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("ἐγγυητάς,%%$%..,/'΄[][] οὓς ἔδει ἐν τοῖς $%$%αὐτοῖς ἐνέχεσθαι ἐν οἷσπερ οὓς ἠγγυήσαντο!@#$%^&&*&)");
            Assert.IsTrue(phrase.NormalizedText == "ΕΓΓΥΗΤΑΣ ΟΥΣ ΕΔΕΙ ΕΝ ΤΟΙΣ ΑΥΤΟΙΣ ΕΝΕΧΕΣΘΑΙ ΕΝ ΟΙΣΠΕΡ ΟΥΣ ΗΓΓΥΗΣΑΝΤΟ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 714);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 6348);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 228);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("σωκράΤΗς");
            Assert.IsTrue(phrase.NormalizedText == "ΣΩΚΡΑΤΗΣ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 126);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 1629);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 27);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("ουρανόΣ");
            Assert.IsTrue(phrase.NormalizedText == "ΟΥΡΑΝΟΣ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 108);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 891);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 27);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("θάλασσα");
            Assert.IsTrue(phrase.NormalizedText == "ΘΑΛΑΣΣΑ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 64);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 442);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 19);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);
        }

        [TestMethod]
        public void MixedLanguageTest()
        {
            Phrase phrase = new Phrase(" ΕΝ ΠΑΝΤΙ ΑΒΑΡΗ ΕΜΑΥΤΟΝ ΥΜΙΝ   345345   THE QUICK BROWN FOX    345345345     תאריך לידה       $%$^$^^$^%$%    ");
            Assert.IsTrue(phrase.NormalizedText == "ΕΝ ΠΑΝΤΙ ΑΒΑΡΗ ΕΜΑΥΤΟΝ ΥΜΙΝ THE QUICK BROWN FOX תאריך לידה");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 1266);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 582);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 4647);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 1160);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 192);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Mixed);

            phrase = new Phrase("אגוז מלךγδδφδφ         σδφasdasdadaΣΔΣΔΑΣΣΦdasdasddasddasdασδφσδφ   SDASFAS  φδσαφσαφ σ φσα         δσ φγδσφ γσδ φγ");
            Assert.IsTrue(phrase.NormalizedText == "אגוז מלךΓΔΔΦΔΦ ΣΔΦASDASDADAΣΔΣΔΑΣΣΦDASDASDDASDDASDΑΣΔΦΣΔΦ SDASFAS ΦΔΣΑΦΣΑΦ Σ ΦΣΑ ΔΣ ΦΓΔΣΦ ΓΣΔ ΦΓ");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 1386);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 940);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 10432);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 587);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 253);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Mixed);

            phrase = new Phrase("                                                                                                                                ");
            Assert.IsTrue(phrase.NormalizedText == "");
            Assert.IsTrue(phrase.Values[CalculationMethod.Sumerian] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Ordinal] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Gematria] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.MisparGadol] == 0);
            Assert.IsTrue(phrase.Values[CalculationMethod.Reduced] == 0);
            Assert.IsTrue(phrase.Alphabet == Alphabet.None);
        }
    }
}