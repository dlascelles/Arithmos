/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosModelsTests;

[TestClass]
public class ExporterTests
{
    [TestMethod]
    public void Constructor_WithValidPhrasesSameMethods_CreatesInstance()
    {
        // Arrange
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod1 = new(cipher) { Id = 1, Name = "Gematria1" };
        GematriaMethod gematriaMethod2 = new(cipher) { Id = 2, Name = "Gematria2" };
        GematriaMethod gematriaMethod3 = new(cipher) { Id = 3, Name = "Gematria3" };
        GematriaMethod gematriaMethod4 = new(cipher) { Id = 4, Name = "Gematria4" };
        GematriaMethod gematriaMethod5 = new(cipher) { Id = 5, Name = "Gematria5" };
        List<Phrase> phrases = new()
        {
            new Phrase("TestString1", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod3, gematriaMethod4, gematriaMethod5 }),
            new Phrase("TestString2", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod3, gematriaMethod4, gematriaMethod5 }),
            new Phrase("TestString3", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod3, gematriaMethod4, gematriaMethod5 }),
            new Phrase("TestString4", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod3, gematriaMethod4, gematriaMethod5 }),
            new Phrase("TestString5", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod3, gematriaMethod4, gematriaMethod5 })
        };

        // Act
        CancellationToken cancellationToken = new();
        Exporter exporter = new(phrases, cancellationToken);

        // Assert
        Assert.AreEqual(exporter.Data, @"Phrase,Gematria1,Gematria2,Gematria3,Gematria4,Gematria5
""TestString"",151,151,151,151,151
""TestString"",151,151,151,151,151
""TestString"",151,151,151,151,151
""TestString"",151,151,151,151,151
""TestString"",151,151,151,151,151
");
    }

    [TestMethod]
    public void Constructor_WithValidPhrasesDifferentMethods_CreatesInstance()
    {
        // Arrange
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod1 = new(cipher) { Id = 1, Name = "Gematria1" };
        GematriaMethod gematriaMethod2 = new(cipher) { Id = 2, Name = "Gematria2" };
        GematriaMethod gematriaMethod3 = new(cipher) { Id = 3, Name = "Gematria3" };
        GematriaMethod gematriaMethod4 = new(cipher) { Id = 4, Name = "Gematria4" };
        GematriaMethod gematriaMethod5 = new(cipher) { Id = 5, Name = "Gematria5" };
        List<Phrase> phrases = new()
        {
            new Phrase("TestString", new List<GematriaMethod>() { gematriaMethod2, gematriaMethod3, gematriaMethod4, gematriaMethod5 }),
            new Phrase("TestString", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod3, gematriaMethod4, gematriaMethod5 }),
            new Phrase("TestString", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod4, gematriaMethod5 }),
            new Phrase("TestString", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod3, gematriaMethod5 }),
            new Phrase("TestString", new List<GematriaMethod>() { gematriaMethod1, gematriaMethod2, gematriaMethod3, gematriaMethod4 })
        };

        // Act
        CancellationToken cancellationToken = new();
        Exporter exporter = new(phrases, cancellationToken);

        // Assert
        Assert.AreEqual(exporter.Data, @"Phrase,Gematria2,Gematria3,Gematria4,Gematria5,Gematria1
""TestString"",151,151,151,151,0
""TestString"",0,151,151,151,151
""TestString"",151,0,151,151,151
""TestString"",151,151,0,151,151
""TestString"",151,151,151,0,151
");
    }

    [TestMethod]
    public void Constructor_WithValidPhrasesSameMethodsRTL_CreatesInstance()
    {
        // Arrange
        Cipher cipherGematria = new(Constants.Ciphers.HebrewStandard);
        Cipher cipherOrdinal = new(Constants.Ciphers.HebrewOrdinal);
        Cipher cipherReduced = new(Constants.Ciphers.HebrewReduced);
        GematriaMethod gematria = new(cipherGematria) { Id = 1, Name = "Gematria" };
        GematriaMethod ordinal = new(cipherOrdinal) { Id = 2, Name = "Ordinal" };
        GematriaMethod reduced = new(cipherReduced) { Id = 3, Name = "Reduced" };

        List<Phrase> phrases = new()
        {
            new Phrase("וְאָהַבְתָּ לְרֵעֲךָ כָּמוֹךָ", new List<GematriaMethod>() { gematria, ordinal, reduced }),
            new Phrase("צְדָקָה תַּצִּיל מִמָּוֶת", new List<GematriaMethod>() { gematria, ordinal, reduced }),
            new Phrase("מַה שֶׁשָׂנוּא עָלֶיךָ אַל תַּעֲשֶׂה לַחֲבֵרְךָ", new List<GematriaMethod>() { gematria, ordinal, reduced }),
            new Phrase("אַל תִּסְתַּכֵּל בַּקַנְקַן, אֶלָּא בְּמַה שֶׁיֵשׁ בּוֹ", new List<GematriaMethod>() { gematria, ordinal, reduced }),
            new Phrase("עַל שְׁלוֹשָׁה דְבָרִים הָעוֹלָם עוֹמֵד:עַל הַתּוֹרָה, וְעַל הָעֲבוֹדָה, וְעַל גְּמִילוּת חֲסָדִים", new List<GematriaMethod>() { gematria, ordinal, reduced }),
            new Phrase("مرحبا صديقي كيف حالك اليوم؟", new List<GematriaMethod>() { gematria, ordinal, reduced })
        };

        // Act
        CancellationToken cancellationToken = new();
        Exporter exporter = new(phrases, cancellationToken);

        // Assert (The u+200E character will be placed at the end of the phrase if its last character is from an RTL language)
        Assert.AreEqual(exporter.Data, @"Phrase,Gematria,Ordinal,Reduced
""ואהבת לרעך כמוך‎"",820,160,46
""צדקה תציל ממות‎"",1215,162,54
""מה ששנוא עליך אל תעשה לחברך‎"",1898,284,80
""אל תסתכל בקנקן אלא במה שיש בו‎"",1940,268,68
""על שלושה דברים העולם עומד על התורה ועל העבודה ועל גמילות חסדים‎"",2899,574,226
""مرحبا صديقي كيف حالك اليوم‎"",0,0,0
");
    }
}