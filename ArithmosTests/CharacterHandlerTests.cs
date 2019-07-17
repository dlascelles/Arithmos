/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosModels.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ArithmosTests
{
    [TestClass]
    public class CharacterHandlerTests
    {
        [TestMethod]
        public void GetAlphabet()
        {
            Assert.IsTrue(CharacterHandler.GetAlphabet("C") == Alphabet.English);
            Assert.IsTrue(CharacterHandler.GetAlphabet("Γ") == Alphabet.Greek);
            Assert.IsTrue(CharacterHandler.GetAlphabet("א") == Alphabet.Hebrew);
            Assert.IsTrue(CharacterHandler.GetAlphabet("LOREM IPSUM DOLOR SIT AMET CONSECTETUR ADIPISCING ELIT SED DO EIUSMOD TEMPOR") == Alphabet.English);
            Assert.IsTrue(CharacterHandler.GetAlphabet("ΚΑΙ ΑΝΑΣΤΕΝΑΞΑΣ ΤΩΙ ΠΝΕΥΜΑΤΙ ΑΥΤΟΥ ΛΕΓΕΙ ΤΙ Η ΓΕΝΕΑ ΑΥΤΗ ΣΗΜΕΙΟΝ ΕΠΙΖΗΤΕΙ ΑΜΗΝ ΛΕΓΩ ΥΜΙΝ ΕΙ ΔΟΘΗΣΕΤΑΙ ΤΗΙ ΓΕΝΕΑΙ ΤΑΥΤΗΙ ΣΗΜΕΙΟΝ") == Alphabet.Greek);
            Assert.IsTrue(CharacterHandler.GetAlphabet("ארוחת צהריים") == Alphabet.Hebrew);
            Assert.IsTrue(CharacterHandler.GetAlphabet("ΑΝΑΣΤΕΝΑΞΑΣארוחת צהרייםIPSUM") == Alphabet.Mixed);

            string path = @"..\\..\\Assets\\moby-dick.txt";
            string alltext = "";
            using (StreamReader sr = new StreamReader(path))
            {
                alltext = sr.ReadToEnd();
            }
            Assert.IsTrue(CharacterHandler.GetAlphabet(alltext) == Alphabet.English);

            path = @"..\\..\\Assets\\prometheus.txt";
            alltext = "";
            using (StreamReader sr = new StreamReader(path))
            {
                alltext = sr.ReadToEnd();
            }
            Assert.IsTrue(CharacterHandler.GetAlphabet(alltext) == Alphabet.Greek);

            path = @"..\\..\\Assets\\hunger.txt";
            alltext = "";
            using (StreamReader sr = new StreamReader(path))
            {
                alltext = sr.ReadToEnd();
            }
            Assert.IsTrue(CharacterHandler.GetAlphabet(alltext) == Alphabet.Hebrew);
        }

        [TestMethod]
        public void NormalizeString()
        {
            Assert.IsTrue(CharacterHandler.NormalizeText("The#$( *&^$&^*(     quick            brown fox     ;'[]/..,<>?:      ") == "THE QUICK BROWN FOX");
            Assert.IsTrue(CharacterHandler.NormalizeText(@"   HELLO, MY 
NAME IS INIGO 
MONTOYA                                                                                                                             ") == "HELLO MY NAME IS INIGO MONTOYA");
            Assert.IsTrue(CharacterHandler.NormalizeText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor.") == "LOREM IPSUM DOLOR SIT AMET CONSECTETUR ADIPISCING ELIT SED DO EIUSMOD TEMPOR");

            Assert.IsTrue(CharacterHandler.NormalizeText("πείσας ὑπομεῖναι καὶ μὴ οἴχεσθαι φεύγοντα ἐγώ, πολλὰ ἱκετεύσας καὶ λαμβανόμενος τῶν γονάτων") == "ΠΕΙΣΑΣ ΥΠΟΜΕΙΝΑΙ ΚΑΙ ΜΗ ΟΙΧΕΣΘΑΙ ΦΕΥΓΟΝΤΑ ΕΓΩ ΠΟΛΛΑ ΙΚΕΤΕΥΣΑΣ ΚΑΙ ΛΑΜΒΑΝΟΜΕΝΟΣ ΤΩΝ ΓΟΝΑΤΩΝ");
            Assert.IsTrue(CharacterHandler.NormalizeText("ουρανόΣ") == "ΟΥΡΑΝΟΣ");
            Assert.IsTrue(CharacterHandler.NormalizeText("ὑμῶν δίκαια καὶ ὑμῖν τε ῥᾴδια") == "ΥΜΩΝ ΔΙΚΑΙΑ ΚΑΙ ΥΜΙΝ ΤΕ ΡΑΔΙΑ");

            Assert.IsTrue(CharacterHandler.NormalizeText("אבטיח") == "אבטיח");
            Assert.IsTrue(CharacterHandler.NormalizeText("             אגוז            מלך\n\n\n\n\n          \n\r           \r\r\r") == "אגוז מלך");
            Assert.IsTrue(CharacterHandler.NormalizeText("תאריך        לידה") == "תאריך לידה");

            Assert.IsTrue(CharacterHandler.NormalizeText(" ΕΝ ΠΑΝΤΙ ΑΒΑΡΗ ΕΜΑΥΤΟΝ ΥΜΙΝ   345345   THE QUICK BROWN FOX    345345345     תאריך לידה       $%$^$^^$^%$%    ") == "ΕΝ ΠΑΝΤΙ ΑΒΑΡΗ ΕΜΑΥΤΟΝ ΥΜΙΝ THE QUICK BROWN FOX תאריך לידה");
            Assert.IsTrue(CharacterHandler.NormalizeText("אגוז מלךγδδφδφ         σδφasdasdadaΣΔΣΔΑΣΣΦdasdasddasddasdασδφσδφ   SDASFAS  φδσαφσαφ σ φσα         δσ φγδσφ γσδ φγ") == "אגוז מלךΓΔΔΦΔΦ ΣΔΦASDASDADAΣΔΣΔΑΣΣΦDASDASDDASDDASDΑΣΔΦΣΔΦ SDASFAS ΦΔΣΑΦΣΑΦ Σ ΦΣΑ ΔΣ ΦΓΔΣΦ ΓΣΔ ΦΓ");
            Assert.IsTrue(CharacterHandler.NormalizeText("                                                                                                                                ") == "");
        }
    }
}