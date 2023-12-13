/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System.IO.MemoryMappedFiles;
using System.Text;

namespace ArithmosModels;

/// <summary>
/// Represents a text scanner that extracts phrases according to specified criteria and gematria values.
/// </summary>
public class Scanner
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Scanner"/> class with the specified gematria methods, selected methods, and cancellation token.
    /// </summary>
    /// <param name="allGematriaMethods">The list of all available gematria methods.</param>
    /// <param name="selectedMethods">The list of selected gematria methods for scanning.</param>
    /// <param name="cancellationToken">The cancellation token to interrupt the scanning process.</param>
    /// <param name="valuesToLookFor">The set of gematria values to look for during scanning. If it's null, then we'll extract all eligible text, ignoring the values. Defaults to null.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the list of gematria methods is null or empty, or when the list of selected methods is null or empty.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the list of gematria methods is empty, or when the list of selected methods is empty.
    /// </exception>
    public Scanner(List<GematriaMethod> allGematriaMethods, List<GematriaMethod> selectedMethods, CancellationToken cancellationToken, HashSet<int> valuesToLookFor = null)
    {
        if (allGematriaMethods == null) throw new ArgumentNullException(nameof(allGematriaMethods), "List of methods was null");
        if (allGematriaMethods.Count == 0) throw new ArgumentException("You must use at least one Gematria method", nameof(allGematriaMethods));
        if (selectedMethods == null) throw new ArgumentNullException(nameof(selectedMethods), "List of selected methods was null");
        if (selectedMethods.Count == 0) throw new ArgumentException("You must select at least one Gematria method", nameof(selectedMethods));

        this.allGematriaMethods = allGematriaMethods;
        this.selectedMethods = selectedMethods;
        this.cancellationToken = cancellationToken;
        this.valuesToLookFor = valuesToLookFor;
    }

    /// <summary>
    /// Scans the content of a file asynchronously and returns a list of found phrases.
    /// </summary>
    /// <param name="filePath">The path to the file to be scanned.</param>
    /// <returns>A list of phrases extracted from the file content.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the file path is null or empty.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    public async Task<List<Phrase>> ScanFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath), "The file path cannot be empty");
        if (!File.Exists(filePath)) throw new FileNotFoundException("The file does not exist.", filePath);

        using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath))
        {
            using (Stream mappedStream = mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read))
            {
                using (StreamReader sr = new(mappedStream, Encoding.UTF8))
                {
                    string text = await sr.ReadToEndAsync();
                    return await ScanAsync(text);
                }
            }
        }
    }

    /// <summary>
    /// Scans the provided text asynchronously and returns a list of phrases.
    /// </summary>
    /// <param name="textToScan">The text to be scanned for phrases.</param>
    /// <returns>A list of phrases extracted from the provided text.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the text to scan is null or empty.</exception>
    public async Task<List<Phrase>> ScanTextAsync(string textToScan)
    {
        if (string.IsNullOrWhiteSpace(textToScan)) throw new ArgumentNullException(nameof(textToScan), "The text to scan cannot be empty");

        return await ScanAsync(textToScan);
    }

    /// <summary>
    /// Scans the given text for phrases based on specified criteria and constraints.
    /// </summary>
    /// <param name="textToScan">The text to be scanned for phrases.</param>
    /// <returns>A list of matched phrases.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="MinimumWordsPerPhrase"/> is larger than the <see cref="MaximumWordsPerPhrase"/>.</exception>
    private async Task<List<Phrase>> ScanAsync(string textToScan)
    {
        if (MinimumWordsPerPhrase > MaximumWordsPerPhrase) throw new InvalidOperationException("The Minimum words per phrase cannot be more than the Maximum words per phrase");

        HashSet<Phrase> matchedPhrases = [];
        hasSpaceSeparator = TextSeparators.Contains(" ");
        await Task.Run(() =>
        {
            string[] segments = textToScan.Split(TextSeparators, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < segments.Length; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string currentText = segments[i];
                int addedSegments = 0;
                while (true)
                {
                    Phrase currentPhrase = new(currentText, allGematriaMethods);
                    if (!IsPhraseWithinMaximumLengthConstraints(currentPhrase)) break;
                    if (valuesToLookFor != null && valuesToLookFor.Count != 0)
                    {
                        if (PhraseContainsAnyValue(currentPhrase))
                        {
                            if (IsPhraseWithinMinimumLengthConstraints(currentPhrase))
                            {
                                matchedPhrases.Add(currentPhrase);
                            }
                        }
                    }
                    else
                    {
                        if (IsPhraseWithinMinimumLengthConstraints(currentPhrase))
                        {
                            matchedPhrases.Add(currentPhrase);
                        }
                    }
                    if (!hasSpaceSeparator) break;
                    if (!IsPhraseWithinValueConstraints(currentPhrase)) break;
                    addedSegments++;
                    if (segments.Length <= i + addedSegments) break;
                    if (!string.IsNullOrWhiteSpace(segments[i + addedSegments]))
                    {
                        currentText += " " + segments[i + addedSegments];
                    }
                }
            }
        });

        return matchedPhrases.ToList();
    }

    /// <summary>
    /// Determines whether the phrase is within the specified gematria value constraints.
    /// </summary>
    /// <param name="phrase">The phrase to check.</param>
    /// <returns>True if the phrase is within the value constraints; otherwise, false.</returns>
    private bool IsPhraseWithinValueConstraints(Phrase phrase)
    {
        if (valuesToLookFor == null || valuesToLookFor.Count == 0) return true;

        int maximumValue = valuesToLookFor.Max();
        int minimumPhraseValue = phrase.Values.Min(m => m.Value);
        return minimumPhraseValue < maximumValue;
    }

    /// <summary>
    /// Determines whether the phrase content is within the minimum length constraints.
    /// </summary>
    /// <param name="phrase">The phrase to check.</param>
    /// <returns>True if the phrase content is within the minimum length constraints; otherwise, false.</returns>
    private bool IsPhraseWithinMinimumLengthConstraints(Phrase phrase)
    {
        return phrase.Content.Length >= MinimumCharactersPerPhrase && (phrase.Content.Split(" ").Length >= MinimumWordsPerPhrase);
    }

    /// <summary>
    /// Determines whether the phrase content is within the maximum length constraints.
    /// </summary>
    /// <param name="phrase">The phrase to check.</param>
    /// <returns>True if the phrase content is within the maximum length constraints; otherwise, false.</returns>
    private bool IsPhraseWithinMaximumLengthConstraints(Phrase phrase)
    {
        return phrase.Content.Split(" ").Length <= MaximumWordsPerPhrase;
    }

    /// <summary>
    /// Determines whether the phrase contains any value from <see cref="valuesToLookFor"/> list.
    /// </summary>
    /// <remarks>We use this to improve the scanner's performance, since if there is already a matching gematria method, then we don't need to go through all the other methods as well</remarks>
    /// <param name="phrase">The phrase to check.</param>
    /// <returns>True if the phrase contains any specified value; otherwise, false.</returns>
    private bool PhraseContainsAnyValue(Phrase phrase)
    {
        return phrase.Values.Any(tuple => valuesToLookFor.Contains(tuple.Value) && selectedMethods.Where(m => m.Id == tuple.GematriaMethod.Id).Any());
    }

    /// <summary>
    /// We use this during the scanning process to check if a space separator was selected by a user. 
    /// If they didn't, then we don't split by spaces, but instead we split by other separators which will probably return more meaningful sentences.
    /// </summary>
    private bool hasSpaceSeparator = true;

    /// <summary>
    /// These are all the gematria methods stored in the database. 
    /// </summary>
    private readonly List<GematriaMethod> allGematriaMethods;

    /// <summary>
    /// These are the methods that the user is interested in.
    /// </summary>
    private readonly List<GematriaMethod> selectedMethods;

    /// <summary>
    /// A token that allows the user to cancel the scanning.
    /// </summary>
    private readonly CancellationToken cancellationToken;

    /// <summary>
    /// The specific values that the user is interested in.
    /// </summary>
    private readonly HashSet<int> valuesToLookFor;

    /// <summary>
    /// Gets or sets the array of text separators used during scanning. Defaults to all separators.
    /// </summary>
    public string[] TextSeparators { get; init; } = TextSeparator.GetAllSeparators();

    /// <summary>
    /// Gets or sets the minimum number of characters per phrase.
    /// </summary>
    public int MinimumCharactersPerPhrase { get; init; } = 3;

    /// <summary>
    /// Gets or sets the minimum number of words per phrase.
    /// </summary>
    public int MinimumWordsPerPhrase { get; init; } = 1;

    /// <summary>
    /// Gets or sets the maximum number of words per phrase.
    /// </summary>
    public int MaximumWordsPerPhrase { get; init; } = 1;
}