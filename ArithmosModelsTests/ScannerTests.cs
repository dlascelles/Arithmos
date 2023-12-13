/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using ArithmosModels.Enums;

namespace ArithmosModelsTests;

[TestClass]
public class ScannerTests
{

    [TestMethod]
    public async Task ScanAsync_DefaultOptionsNoValues()
    {
        // Arrange
        string text = "LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. AENEAN LOBORTIS NEQUE QUIS LECTUS TEMPOR EUISMOD. ETIAM SIT AMET MAGNA ANTE. VIVAMUS EGET PURUS MOLESTIE, SOLLICITUDIN AUGUE QUIS, ALIQUAM VELIT. DUIS IN VARIUS MAURIS, SIT AMET INTERDUM TURPIS. PRAESENT HENDRERIT INTERDUM JUSTO, A SOLLICITUDIN MI ULLAMCORPER EGET. CRAS CONSECTETUR NISI EFFICITUR LOREM PELLENTESQUE POSUERE.";
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken);

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 40);
        Assert.IsTrue(phrases[0].Values[0].Value == 63);
        Assert.IsTrue(phrases[1].Values[0].Value == 78);
        Assert.IsTrue(phrases[15].Values[0].Value == 48);
        Assert.IsTrue(phrases[16].Values[0].Value == 36);
        Assert.IsTrue(phrases[38].Values[0].Value == 151);
        Assert.IsTrue(phrases[39].Values[0].Value == 99);
    }

    [TestMethod]
    public async Task ScanAsync_DefaultOptionsWithValues()
    {
        // Arrange
        string text = "LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. AENEAN LOBORTIS NEQUE QUIS LECTUS TEMPOR EUISMOD. ETIAM SIT AMET MAGNA ANTE. VIVAMUS EGET PURUS MOLESTIE, SOLLICITUDIN AUGUE QUIS, ALIQUAM VELIT. DUIS IN VARIUS MAURIS, SIT AMET INTERDUM TURPIS. PRAESENT HENDRERIT INTERDUM JUSTO, A SOLLICITUDIN MI ULLAMCORPER EGET. CRAS CONSECTETUR NISI EFFICITUR LOREM PELLENTESQUE POSUERE.";
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 50, 51, 64, 66, 90, 92, 134, 110, 200, 465, 526, 15 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor);

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 6);
        Assert.IsTrue(phrases[0].Values[0].Value == 64);
        Assert.IsTrue(phrases[1].Values[0].Value == 110);
        Assert.IsTrue(phrases[2].Values[0].Value == 66);
        Assert.IsTrue(phrases[3].Values[0].Value == 90);
        Assert.IsTrue(phrases[4].Values[0].Value == 134);
        Assert.IsTrue(phrases[5].Values[0].Value == 51);
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsNoValues()
    {
        // Arrange
        string text = "LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. AENEAN LOBORTIS NEQUE QUIS LECTUS TEMPOR EUISMOD. ETIAM SIT AMET MAGNA ANTE. VIVAMUS EGET PURUS MOLESTIE, SOLLICITUDIN AUGUE QUIS, ALIQUAM VELIT. DUIS IN VARIUS MAURIS, SIT AMET INTERDUM TURPIS. PRAESENT HENDRERIT INTERDUM JUSTO, A SOLLICITUDIN MI ULLAMCORPER EGET. CRAS CONSECTETUR NISI EFFICITUR LOREM PELLENTESQUE POSUERE.";
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken)
        {
            TextSeparators = TextSeparator.GetAllSeparators(),
            MinimumCharactersPerPhrase = 4,
            MinimumWordsPerPhrase = 2,
            MaximumWordsPerPhrase = 5
        };

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 200);
        Assert.IsTrue(phrases[0].Values[0].Value == 141 && phrases[0].Content == "LOREM IPSUM");
        Assert.IsTrue(phrases[1].Values[0].Value == 205 && phrases[1].Content == "LOREM IPSUM DOLOR");
        Assert.IsTrue(phrases[15].Values[0].Value == 367 && phrases[15].Content == "SIT AMET CONSECTETUR ADIPISCING ELIT");
        Assert.IsTrue(phrases[16].Values[0].Value == 182 && phrases[16].Content == "AMET CONSECTETUR");
        Assert.IsTrue(phrases[198].Values[0].Value == 313 && phrases[198].Content == "LOREM PELLENTESQUE POSUERE");
        Assert.IsTrue(phrases[199].Values[0].Value == 250 && phrases[199].Content == "PELLENTESQUE POSUERE");
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsWithValues()
    {
        // Arrange
        string text = "LOREM IPSUM DOLOR SIT AMET, CONSECTETUR ADIPISCING ELIT. AENEAN LOBORTIS NEQUE QUIS LECTUS TEMPOR EUISMOD. ETIAM SIT AMET MAGNA ANTE. VIVAMUS EGET PURUS MOLESTIE, SOLLICITUDIN AUGUE QUIS, ALIQUAM VELIT. DUIS IN VARIUS MAURIS, SIT AMET INTERDUM TURPIS. PRAESENT HENDRERIT INTERDUM JUSTO, A SOLLICITUDIN MI ULLAMCORPER EGET. CRAS CONSECTETUR NISI EFFICITUR LOREM PELLENTESQUE POSUERE.";
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 141, 273, 173, 263, 588, 86, 461, 462 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor)
        {
            TextSeparators = TextSeparator.GetAllSeparators(),
            MinimumCharactersPerPhrase = 4,
            MinimumWordsPerPhrase = 2,
            MaximumWordsPerPhrase = 5
        };

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 8);
        Assert.IsTrue(phrases[0].Values[0].Value == 141 && phrases[0].Content == "LOREM IPSUM");
        Assert.IsTrue(phrases[1].Values[0].Value == 273 && phrases[1].Content == "AMET CONSECTETUR ADIPISCING");
        Assert.IsTrue(phrases[2].Values[0].Value == 86 && phrases[2].Content == "ELIT AENEAN");
        Assert.IsTrue(phrases[3].Values[0].Value == 173 && phrases[3].Content == "TEMPOR EUISMOD");
        Assert.IsTrue(phrases[4].Values[0].Value == 461 && phrases[4].Content == "PURUS MOLESTIE SOLLICITUDIN AUGUE QUIS");
        Assert.IsTrue(phrases[5].Values[0].Value == 263 && phrases[5].Content == "AUGUE QUIS ALIQUAM VELIT");
        Assert.IsTrue(phrases[6].Values[0].Value == 86 && phrases[6].Content == "JUSTO A");
        Assert.IsTrue(phrases[7].Values[0].Value == 461 && phrases[7].Content == "NISI EFFICITUR LOREM PELLENTESQUE POSUERE");
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsNoValues2()
    {
        // Arrange
        string text = "In the quiet stillness of the forest, the leaves rustled softly in the gentle breeze, creating a soothing symphony of nature's whispers. Sunlight filtered through the dense canopy, painting dappled patterns on the forest floor. A curious squirrel darted from tree to tree, while a family of deer cautiously grazed nearby. It was a moment of serenity, a snapshot of the timeless beauty that the natural world offers.";
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken)
        {
            TextSeparators = TextSeparator.GetAllSeparators(),
            MinimumCharactersPerPhrase = 2,
            MinimumWordsPerPhrase = 1,
            MaximumWordsPerPhrase = 3
        };

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 181);
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsWithValues2()
    {
        // Arrange
        string text = "In the quiet stillness of the forest, the leaves rustled softly in the gentle breeze, creating a soothing symphony of nature's whispers. Sunlight filtered through the dense canopy, painting dappled patterns on the forest floor. A curious squirrel darted from tree to tree, while a family of deer cautiously grazed nearby. It was a moment of serenity, a snapshot of the timeless beauty that the natural world offers.";
        Cipher cipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 111, 215, 222, 333, 444, 555, 666, 777, 888, 999 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor)
        {
            TextSeparators = TextSeparator.GetAllSeparators(),
            MinimumCharactersPerPhrase = 2,
            MinimumWordsPerPhrase = 1,
            MaximumWordsPerPhrase = 3
        };

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 3);
        Assert.IsTrue(phrases[0].Values[0].Value == 222 && phrases[0].Content == "quiet stillness of");
        Assert.IsTrue(phrases[1].Values[0].Value == 215 && phrases[1].Content == "nature's whispers");
        Assert.IsTrue(phrases[2].Values[0].Value == 222 && phrases[2].Content == "canopy painting dappled");
    }

    [TestMethod]
    public async Task ScanAsync_DefaultOptionsNoValuesGreek()
    {
        // Arrange
        string text = "Χθονὸς μὲν ἐς τηλουρὸν ἥκομεν πέδον,\r\nΣκύθην ἐς οἷμον, ἄβροτον εἰς ἐρημίαν.\r\nἭφαιστε, σοὶ δὲ χρὴ μέλειν ἐπιστολὰς\r\nἅς σοι πατὴρ ἐφεῖτο, τόνδε πρὸς πέτραις\r\nὑψηλοκρήμνοις τὸν λεωργὸν ὀχμάσαι\r\nἀδαμαντίνων δεσμῶν ἐν ἀρρήκτοις πέδαις.\r\nτὸ σὸν γὰρ ἄνθος, παντέχνου πυρὸς σέλας,\r\nθνητοῖσι κλέψας ὤπασεν. τοιᾶσδέ τοι\r\nἁμαρτίας σφε δεῖ θεοῖς δοῦναι δίκην,\r\nὡς ἂν διδαχθῇ τὴν Διὸς τυραννίδα\r\nστέργειν, φιλανθρώπου δὲ παύεσθαι τρόπου.";
        Cipher cipher = new(Constants.Ciphers.GreekStandard);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken);

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 53);
        Assert.IsTrue(phrases[0].Values[0].Value == 999 && phrases[0].Content == "Χθονος");
        Assert.IsTrue(phrases[1].Values[0].Value == 95 && phrases[1].Content == "μεν");
        Assert.IsTrue(phrases[15].Values[0].Value == 489 && phrases[15].Content == "πατηρ");
        Assert.IsTrue(phrases[16].Values[0].Value == 890 && phrases[16].Content == "εφειτο");
        Assert.IsTrue(phrases[51].Values[0].Value == 706 && phrases[51].Content == "παυεσθαι");
        Assert.IsTrue(phrases[52].Values[0].Value == 1020 && phrases[52].Content == "τροπου");
    }

    [TestMethod]
    public async Task ScanAsync_DefaultOptionsWithValuesGreek()
    {
        // Arrange
        string text = "Κράτος Βία τε, σφῷν μὲν ἐντολὴ Διὸς\r\nἔχει τέλος δὴ κοὐδὲν ἐμποδὼν ἔτι.\r\nἐγὼ δ᾿ ἄτολμός εἰμι συγγενῆ θεὸν\r\nδῆσαι βίᾳ φάραγγι πρὸς δυσχειμέρῳ.\r\nπάντως δ᾿ ἀνάγκη τῶνδέ μοι τόλμαν σχεθεῖν.\r\nεὐωριάζειν γὰρ πατρὸς λόγους βαρύ.\r\nτῆς ὀρθοβούλου Θέμιδος αἰπυμῆτα παῖ,\r\nἄκοντά σ᾿ ἄκων δυσλύτοις χαλκεύμασι\r\nπροσπασσαλεύσω τῷδ᾿ ἀπανθρώπῳ πάγῳ,\r\nἵν᾿ οὔτε φωνὴν οὔτε του μορφὴν βροτῶν\r\nὄψῃ, σταθευτὸς δ᾿ ἡλίου φοίβῃ φλογὶ\r\nχροιᾶς ἀμείψεις ἄνθος. ἀσμένῳ δέ σοι\r\nἡ ποικιλείμων νὺξ ἀποκρύψει φάος,\r\nπάχνην θ᾿ ἑῴαν ἥλιος σκεδᾷ πάλιν.\r\nαἰεὶ δὲ τοῦ παρόντος ἀχθηδὼν κακοῦ\r\nτρύσει σ᾿. ὁ λωφήσων γὰρ οὐ πέφυκέ πω.\r\nτοιαῦτ᾿ ἐπηύρου τοῦ φιλανθρώπου τρόπου.\r\nθεὸς θεῶν γὰρ οὐχ ὑποπτήσσων χόλον\r\nβροτοῖσι τιμὰς ὤπασας πέρα δίκης.\r\nἀνθ᾿ ὧν ἀτερπῆ τήνδε φρουρήσεις πέτραν\r\nὀρθοστάδην, ἄυπνος, οὐ κάμπτων γόνυ.\r\nπολλοὺς δ᾿ ὀδυρμοὺς καὶ γόους ἀνωφελεῖς\r\nφθέγξῃ. Διὸς γὰρ δυσπαραίτητοι φρένες.\r\nἅπας δὲ τραχὺς ὅστις ἂν νέον κρατῇ.";
        Cipher cipher = new(Constants.Ciphers.GreekStandard);
        GematriaMethod gematriaMethod = new(cipher) { Id = 1 };
        List<GematriaMethod> gematriaMethods = new() { gematriaMethod };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 691, 1550, 104, 60, 503, 333, 33, 712, 443, 284 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor);

        // Act
        List<Phrase> phrases = await scanner.ScanTextAsync(text);

        // Assert
        Assert.IsTrue(phrases.Count == 7);
        Assert.IsTrue(phrases[0].Values[0].Value == 691 && phrases[0].Content == "Κρατος");
        Assert.IsTrue(phrases[1].Values[0].Value == 1550 && phrases[1].Content == "σφων");
        Assert.IsTrue(phrases[2].Values[0].Value == 284 && phrases[2].Content == "Διος");
        Assert.IsTrue(phrases[3].Values[0].Value == 104 && phrases[3].Content == "γαρ");
        Assert.IsTrue(phrases[4].Values[0].Value == 503 && phrases[4].Content == "βαρυ");
        Assert.IsTrue(phrases[5].Values[0].Value == 284 && phrases[5].Content == "θεος");
        Assert.IsTrue(phrases[6].Values[0].Value == 60 && phrases[6].Content == "ανθ");
    }

    [TestMethod]
    public async Task ScanAsync_DefaultOptionsWithValuesFromFile()
    {
        // Arrange
        Cipher gematriaCipher = new(Constants.Ciphers.EnglishStandard);
        GematriaMethod gematria = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.EnglishReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { gematria, ordinal, reduced };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 50, 100, 200, 300, 400 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor);
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\bible.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 531);
    }

    [TestMethod]
    public async Task ScanAsync_DefaultOptionsWithValuesFromFileHebrew()
    {
        // Arrange
        Cipher gematriaCipher = new(Constants.Ciphers.HebrewStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.HebrewOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.HebrewReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { standard, ordinal, reduced };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 333 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor);
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\hunger.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 5);
    }

    [TestMethod]
    public async Task ScanAsync_DefaultOptionsWithValuesFromFileGreek()
    {
        // Arrange
        Cipher gematriaCipher = new(Constants.Ciphers.GreekStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.GreekOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.GreekReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { standard, ordinal, reduced };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 111, 222, 333 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor);
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\prometheus.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 21);
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsWithValuesFromFile()
    {
        // Arrange               
        Cipher gematriaCipher = new(Constants.Ciphers.EnglishStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.EnglishOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.EnglishReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { standard, ordinal, reduced };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 112, 444 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor)
        {
            MaximumWordsPerPhrase = 3
        };
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\bible.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 6341);
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsWithValuesFromFile2()
    {
        // Arrange               
        Cipher gematriaCipher = new(Constants.Ciphers.EnglishStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher sumerianCipher = new(Constants.Ciphers.EnglishSumerian);
        GematriaMethod sumerian = new(sumerianCipher) { Id = 4, Name = "Sumerian" };
        List<GematriaMethod> gematriaMethods = new() { standard, sumerian };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 418 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor)
        {
            MaximumWordsPerPhrase = 6
        };
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\law.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 19);
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsWithValuesFromFileHebrew()
    {
        // Arrange
        Cipher gematriaCipher = new(Constants.Ciphers.HebrewStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.HebrewOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.HebrewReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { standard, ordinal, reduced };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 333, 555 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor)
        {
            MaximumWordsPerPhrase = 3
        };
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\hunger.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 83);
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsWithValuesFromFileGreek()
    {
        // Arrange
        Cipher gematriaCipher = new(Constants.Ciphers.GreekStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.GreekOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.GreekReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { standard, ordinal, reduced };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 111, 222, 333 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor)
        {
            MaximumWordsPerPhrase = 3
        };
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\prometheus.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 140);
    }

    [TestMethod]
    public async Task ScanAsync_CustomOptionsWithValuesFromFileGreek2()
    {
        // Arrange
        Cipher gematriaCipher = new(Constants.Ciphers.GreekStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.GreekOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.GreekReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { standard, ordinal, reduced };
        CancellationToken cancellationToken = new();
        HashSet<int> valuesToLookFor = new() { 777, 888, 999 };
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken, valuesToLookFor)
        {
            MaximumWordsPerPhrase = 3
        };
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\apocalypse.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 20);
    }

    [TestMethod]
    public async Task ScanAsync_WithoutSpaceSeparator()
    {
        // Arrange
        Cipher gematriaCipher = new(Constants.Ciphers.GreekStandard);
        GematriaMethod standard = new(gematriaCipher) { Id = 1, Name = "Standard" };
        Cipher ordinalCipher = new(Constants.Ciphers.GreekOrdinal);
        GematriaMethod ordinal = new(ordinalCipher) { Id = 2, Name = "Ordinal" };
        Cipher reducedCipher = new(Constants.Ciphers.GreekReduced);
        GematriaMethod reduced = new(reducedCipher) { Id = 3, Name = "Reduced" };
        List<GematriaMethod> gematriaMethods = new() { standard, ordinal, reduced };
        CancellationToken cancellationToken = new();
        Separator separator = Separator.FullStop | Separator.Colon;
        TextSeparator textSeparator = new(separator);
        Scanner scanner = new(gematriaMethods, gematriaMethods, cancellationToken)
        {
            MinimumCharactersPerPhrase = 3,
            MinimumWordsPerPhrase = 1,
            MaximumWordsPerPhrase = 50,
            TextSeparators = textSeparator.GetSelectedSeparators()
        };
        string filePath = @$"{Environment.CurrentDirectory}\\Assets\\bible.txt";

        // Act
        List<Phrase> phrases = await scanner.ScanFileAsync(filePath);

        // Assert
        Assert.IsTrue(phrases.Count == 42459);
        Assert.IsTrue(phrases[2].Content == "In the beginning God created the heaven and the earth");
        Assert.IsTrue(phrases[42458].Content == "Even so come Lord Jesus");
    }
}