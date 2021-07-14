/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
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
            Phrase phrase = new("World Wide Web");
            Assert.IsTrue(phrase.NormalizedText == "WORLD WIDE WEB");
            Assert.IsTrue(phrase.Sumerian == 858);
            Assert.IsTrue(phrase.Ordinal == 143);
            Assert.IsTrue(phrase.Gematria == 1709);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 62);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("The quick brown fox");
            Assert.IsTrue(phrase.NormalizedText == "THE QUICK BROWN FOX");
            Assert.IsTrue(phrase.Sumerian == 1266);
            Assert.IsTrue(phrase.Ordinal == 211);
            Assert.IsTrue(phrase.Gematria == 1993);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 85);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("The#$( *&^$&^*(     quick            brown fox     ;'[]/..,<>?:      ");
            Assert.IsTrue(phrase.NormalizedText == "THE QUICK BROWN FOX");
            Assert.IsTrue(phrase.Sumerian == 1266);
            Assert.IsTrue(phrase.Ordinal == 211);
            Assert.IsTrue(phrase.Gematria == 1993);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 85);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase(@"   HELLO, MY 
NAME IS INIGO 
MONTOYA                                                                                                                             ");
            Assert.IsTrue(phrase.NormalizedText == "HELLO MY NAME IS INIGO MONTOYA");
            Assert.IsTrue(phrase.Sumerian == 1848);
            Assert.IsTrue(phrase.Ordinal == 308);
            Assert.IsTrue(phrase.Gematria == 2324);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 128);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("serendipity");
            Assert.IsTrue(phrase.NormalizedText == "SERENDIPITY");
            Assert.IsTrue(phrase.Sumerian == 864);
            Assert.IsTrue(phrase.Ordinal == 144);
            Assert.IsTrue(phrase.Gematria == 1242);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 63);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.IsTrue(phrase.NormalizedText == "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.IsTrue(phrase.Sumerian == 2106);
            Assert.IsTrue(phrase.Ordinal == 351);
            Assert.IsTrue(phrase.Gematria == 4095);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 126);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);

            phrase = new Phrase("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.");
            Assert.IsTrue(phrase.NormalizedText == "LOREM IPSUM DOLOR SIT AMET CONSECTETUR ADIPISCING ELIT SED DO EIUSMOD TEMPOR");
            Assert.IsTrue(phrase.Sumerian == 4752);
            Assert.IsTrue(phrase.Ordinal == 792);
            Assert.IsTrue(phrase.Gematria == 4221);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 306);
            Assert.IsTrue(phrase.Alphabet == Alphabet.English);
        }

        [TestMethod]
        public void HebrewLanguageTest()
        {
            Phrase phrase = new("גימטריה");
            Assert.IsTrue(phrase.NormalizedText == "גימטריה");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 70);
            Assert.IsTrue(phrase.Gematria == 277);
            Assert.IsTrue(phrase.MisparGadol == 277);
            Assert.IsTrue(phrase.Reduced == 25);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("ראשית");
            Assert.IsTrue(phrase.NormalizedText == "ראשית");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 74);
            Assert.IsTrue(phrase.Gematria == 911);
            Assert.IsTrue(phrase.MisparGadol == 911);
            Assert.IsTrue(phrase.Reduced == 11);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("אבטיח");
            Assert.IsTrue(phrase.NormalizedText == "אבטיח");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 30);
            Assert.IsTrue(phrase.Gematria == 30);
            Assert.IsTrue(phrase.MisparGadol == 30);
            Assert.IsTrue(phrase.Reduced == 21);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("אוניברסיטה");
            Assert.IsTrue(phrase.NormalizedText == "אוניברסיטה");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 92);
            Assert.IsTrue(phrase.Gematria == 353);
            Assert.IsTrue(phrase.MisparGadol == 353);
            Assert.IsTrue(phrase.Reduced == 38);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("אגוז מלך");
            Assert.IsTrue(phrase.NormalizedText == "אגוז מלך");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 65);
            Assert.IsTrue(phrase.Gematria == 107);
            Assert.IsTrue(phrase.MisparGadol == 587);
            Assert.IsTrue(phrase.Reduced == 26);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("             אגוז            מלך\n\n\n\n\n          \n\r           \r\r\r");
            Assert.IsTrue(phrase.NormalizedText == "אגוז מלך");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 65);
            Assert.IsTrue(phrase.Gematria == 107);
            Assert.IsTrue(phrase.MisparGadol == 587);
            Assert.IsTrue(phrase.Reduced == 26);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("ארוחת צהריים");
            Assert.IsTrue(phrase.NormalizedText == "ארוחת צהריים");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 144);
            Assert.IsTrue(phrase.Gematria == 970);
            Assert.IsTrue(phrase.MisparGadol == 1530);
            Assert.IsTrue(phrase.Reduced == 43);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);

            phrase = new Phrase("תאריך        לידה");
            Assert.IsTrue(phrase.NormalizedText == "תאריך לידה");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 107);
            Assert.IsTrue(phrase.Gematria == 680);
            Assert.IsTrue(phrase.MisparGadol == 1160);
            Assert.IsTrue(phrase.Reduced == 23);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Hebrew);
        }

        [TestMethod]
        public void GreekLanguageTest()
        {
            Phrase phrase = new("ὑμῶν δίκαια καὶ ὑμῖν τε ῥᾴδια");
            Assert.IsTrue(phrase.NormalizedText == "ΥΜΩΝ ΔΙΚΑΙΑ ΚΑΙ ΥΜΙΝ ΤΕ ΡΑΔΙΑ");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 254);
            Assert.IsTrue(phrase.Gematria == 2288);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 65);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("πείσας ὑπομεῖναι καὶ μὴ οἴχεσθαι φεύγοντα ἐγώ, πολλὰ ἱκετεύσας καὶ λαμβανόμενος τῶν γονάτων");
            Assert.IsTrue(phrase.NormalizedText == "ΠΕΙΣΑΣ ΥΠΟΜΕΙΝΑΙ ΚΑΙ ΜΗ ΟΙΧΕΣΘΑΙ ΦΕΥΓΟΝΤΑ ΕΓΩ ΠΟΛΛΑ ΙΚΕΤΕΥΣΑΣ ΚΑΙ ΛΑΜΒΑΝΟΜΕΝΟΣ ΤΩΝ ΓΟΝΑΤΩΝ");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 936);
            Assert.IsTrue(phrase.Gematria == 8649);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 297);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("και     αναστεναξας τωι     πνευματι αυτου λεγει τι η γενεα αυτη σημειον επιζητει αμην λεγω υμιν \n ει δοθησεται τηι γενεαι ταυτηι σημειον");
            Assert.IsTrue(phrase.NormalizedText == "ΚΑΙ ΑΝΑΣΤΕΝΑΞΑΣ ΤΩΙ ΠΝΕΥΜΑΤΙ ΑΥΤΟΥ ΛΕΓΕΙ ΤΙ Η ΓΕΝΕΑ ΑΥΤΗ ΣΗΜΕΙΟΝ ΕΠΙΖΗΤΕΙ ΑΜΗΝ ΛΕΓΩ ΥΜΙΝ ΕΙ ΔΟΘΗΣΕΤΑΙ ΤΗΙ ΓΕΝΕΑΙ ΤΑΥΤΗΙ ΣΗΜΕΙΟΝ");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 1196);
            Assert.IsTrue(phrase.Gematria == 9872);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 413);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("ἐγγυητάς,%%$%..,/'΄[][] οὓς ἔδει ἐν τοῖς $%$%αὐτοῖς ἐνέχεσθαι ἐν οἷσπερ οὓς ἠγγυήσαντο!@#$%^&&*&)");
            Assert.IsTrue(phrase.NormalizedText == "ΕΓΓΥΗΤΑΣ ΟΥΣ ΕΔΕΙ ΕΝ ΤΟΙΣ ΑΥΤΟΙΣ ΕΝΕΧΕΣΘΑΙ ΕΝ ΟΙΣΠΕΡ ΟΥΣ ΗΓΓΥΗΣΑΝΤΟ");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 714);
            Assert.IsTrue(phrase.Gematria == 6348);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 228);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("σωκράΤΗς");
            Assert.IsTrue(phrase.NormalizedText == "ΣΩΚΡΑΤΗΣ");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 126);
            Assert.IsTrue(phrase.Gematria == 1629);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 27);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("ουρανόΣ");
            Assert.IsTrue(phrase.NormalizedText == "ΟΥΡΑΝΟΣ");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 108);
            Assert.IsTrue(phrase.Gematria == 891);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 27);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);

            phrase = new Phrase("θάλασσα");
            Assert.IsTrue(phrase.NormalizedText == "ΘΑΛΑΣΣΑ");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 64);
            Assert.IsTrue(phrase.Gematria == 442);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 19);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Greek);
        }

        [TestMethod]
        public void MixedLanguageTest()
        {
            Phrase phrase = new(" ΕΝ ΠΑΝΤΙ ΑΒΑΡΗ ΕΜΑΥΤΟΝ ΥΜΙΝ   345345   THE QUICK BROWN FOX    345345345     תאריך לידה       $%$^$^^$^%$%    ");
            Assert.IsTrue(phrase.NormalizedText == "ΕΝ ΠΑΝΤΙ ΑΒΑΡΗ ΕΜΑΥΤΟΝ ΥΜΙΝ THE QUICK BROWN FOX תאריך לידה");
            Assert.IsTrue(phrase.Sumerian == 1266);
            Assert.IsTrue(phrase.Ordinal == 582);
            Assert.IsTrue(phrase.Gematria == 4647);
            Assert.IsTrue(phrase.MisparGadol == 1160);
            Assert.IsTrue(phrase.Reduced == 192);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Mixed);

            phrase = new Phrase("אגוז מלךγδδφδφ         σδφasdasdadaΣΔΣΔΑΣΣΦdasdasddasddasdασδφσδφ   SDASFAS  φδσαφσαφ σ φσα         δσ φγδσφ γσδ φγ");
            Assert.IsTrue(phrase.NormalizedText == "אגוז מלךΓΔΔΦΔΦ ΣΔΦASDASDADAΣΔΣΔΑΣΣΦDASDASDDASDDASDΑΣΔΦΣΔΦ SDASFAS ΦΔΣΑΦΣΑΦ Σ ΦΣΑ ΔΣ ΦΓΔΣΦ ΓΣΔ ΦΓ");
            Assert.IsTrue(phrase.Sumerian == 1386);
            Assert.IsTrue(phrase.Ordinal == 940);
            Assert.IsTrue(phrase.Gematria == 10432);
            Assert.IsTrue(phrase.MisparGadol == 587);
            Assert.IsTrue(phrase.Reduced == 253);
            Assert.IsTrue(phrase.Alphabet == Alphabet.Mixed);

            phrase = new Phrase("                                                                                                                                ");
            Assert.IsTrue(phrase.NormalizedText == "");
            Assert.IsTrue(phrase.Sumerian == 0);
            Assert.IsTrue(phrase.Ordinal == 0);
            Assert.IsTrue(phrase.Gematria == 0);
            Assert.IsTrue(phrase.MisparGadol == 0);
            Assert.IsTrue(phrase.Reduced == 0);
            Assert.IsTrue(phrase.Alphabet == Alphabet.None);
        }
    }
}