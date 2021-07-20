/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosModels.Helpers
{
    public static class Scanner
    {
        /// <summary>
        /// Will scan a file and look for phrases that match the given criteria
        /// </summary>
        /// <param name="filePath">The full path to the file</param>
        /// <param name="values">The values you want to search for</param>
        /// <param name="calculationMethod">The Calculation Methods you want to use</param>
        /// <param name="minCharsPerPhrase">Set the minimum number of characters for each phrase</param>
        /// <param name="maximumWordsPerPhrase">Set the maximum amount of words for each phrase</param>
        /// <returns>A list of phrases that match our criteria</returns>
        public static async Task<List<Phrase>> ScanFileAsync(string filePath, HashSet<int> values, CalculationMethod calculationMethod, PhraseSeparator phraseSeparator = PhraseSeparator.All, int minCharsPerPhrase = 3, int minimumWordsPerPhrase = 1, int maximumWordsPerPhrase = 1, CancellationToken cts = default)
        {
            List<Phrase> phrasesFound = new();

            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath))
            {
                using (Stream mappedStream = mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read))
                {
                    using (StreamReader sr = new(mappedStream, UTF8Encoding.UTF8))
                    {
                        string text = await sr.ReadToEndAsync();
                        phrasesFound = await ScanTextAsync(text, values, calculationMethod, phraseSeparator, minCharsPerPhrase, minimumWordsPerPhrase, maximumWordsPerPhrase, cts);
                    }
                }
            }

            return phrasesFound;
        }

        /// <summary>
        /// Will scan a file and look for phrases that match the given criteria
        /// </summary>
        /// <param name="filePath">The full path to the file</param>
        /// <param name="minCharsPerPhrase">Set the minimum number of characters for each phrase</param>
        /// <param name="maximumWordsPerPhrase">Set the maximum amount of words for each phrase</param>
        /// <returns>A list of phrases that match our criteria</returns>
        public static async Task<List<Phrase>> ScanFileAsync(string filePath, PhraseSeparator phraseSeparator = PhraseSeparator.All, int minCharsPerPhrase = 3, int minimumWordsPerPhrase = 1, int maximumWordsPerPhrase = 1, CancellationToken cts = default)
        {
            List<Phrase> phrasesFound = new();

            using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath))
            {
                using (Stream mappedStream = mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.Read))
                {
                    using (StreamReader sr = new(mappedStream, UTF8Encoding.UTF8))
                    {
                        string text = await sr.ReadToEndAsync();
                        phrasesFound = await ScanTextAsync(text, phraseSeparator, minCharsPerPhrase, minimumWordsPerPhrase, maximumWordsPerPhrase, cts);
                    }
                }
            }

            return phrasesFound;
        }

        /// <summary>
        /// Will scan some text and look for phrases that match the given criteria
        /// </summary>
        /// <param name="text">The text you want to search</param>
        /// <param name="values">The values you want to search for</param>
        /// <param name="calculationMethod">The Calculation Methods you want to use</param>
        /// <param name="minCharsPerPhrase">Set the minimum number of characters for each phrase</param>
        /// <param name="maximumWordsPerPhrase">Set the maximum amount of words for each phrase</param>
        /// <returns>A list of phrases that match our criteria</returns>
        public static async Task<List<Phrase>> ScanTextAsync(string text, HashSet<int> values, CalculationMethod calculationMethod, PhraseSeparator phraseSeparator = PhraseSeparator.All, int minCharsPerPhrase = 3, int minimumWordsPerPhrase = 1, int maximumWordsPerPhrase = 1, CancellationToken cts = default)
        {
            List<Phrase> phrasesFound = new();

            if (!string.IsNullOrEmpty(text))
            {
                string[] words = await Task.Run(() => { return text.Split(GetSeparators(phraseSeparator), StringSplitOptions.RemoveEmptyEntries); }, cts);

                if (words != null && words.Length > 0)
                {
                    phrasesFound = await ScanAsync(words, phraseSeparator, values, calculationMethod, minCharsPerPhrase, minimumWordsPerPhrase, maximumWordsPerPhrase, cts);
                }
            }

            return phrasesFound;
        }

        /// <summary>
        /// Will scan some text and look for phrases that match the given criteria
        /// </summary>
        /// <param name="text">The text you want to search</param>
        /// <param name="minCharsPerPhrase">Set the minimum number of characters for each phrase</param>
        /// <param name="maximumWordsPerPhrase">Set the maximum amount of words for each phrase</param>
        /// <returns>A list of phrases that match our criteria</returns>
        public static async Task<List<Phrase>> ScanTextAsync(string text, PhraseSeparator phraseSeparator = PhraseSeparator.All, int minCharsPerPhrase = 3, int minimumWordsPerPhrase = 1, int maximumWordsPerPhrase = 1, CancellationToken cts = default)
        {
            List<Phrase> phrasesFound = new();

            if (!string.IsNullOrEmpty(text))
            {
                string[] words = await Task.Run(() => { return text.Split(GetSeparators(phraseSeparator), StringSplitOptions.RemoveEmptyEntries); }, cts);

                if (words != null && words.Length > 0)
                {
                    phrasesFound = await ScanAsync(words, phraseSeparator, minCharsPerPhrase, minimumWordsPerPhrase, maximumWordsPerPhrase, cts);
                }
            }

            return phrasesFound;
        }

        /// <summary>
        /// Will scan an array of strings and look for phrases that match the given criteria
        /// </summary>
        /// <param name="splittedPhrases">The text you want to search</param>
        /// <param name="values">The values you want to search for</param>
        /// <param name="calculationMethod">The Calculation Methods you want to use</param>
        /// <param name="minCharsPerPhrase">Set the minimum number of characters for each phrase</param>
        /// <param name="maximumWordsPerPhrase">Set the maximum amount of words for each phrase</param>
        /// <returns>A list of phrases that match our criteria</returns>
        public static async Task<List<Phrase>> ScanAsync(string[] splittedPhrases, PhraseSeparator phraseSeparator, HashSet<int> values, CalculationMethod calculationMethod, int minCharsPerPhrase = 3, int minimumWordsPerPhrase = 1, int maximumWordsPerPhrase = 1, CancellationToken cts = default)
        {
            HashSet<Phrase> matchedPhrases = new();

            if (splittedPhrases != null && values != null && splittedPhrases.Length > 0 && values.Count > 0)
            {
                await Task.Run(() =>
                {
                    int maxValue = values.Max();
                    int totalSplitted = splittedPhrases.Length;
                    if (phraseSeparator.HasFlag(PhraseSeparator.Space))
                    {
                        for (int i = 0; i < totalSplitted; i++)
                        {
                            if (cts.IsCancellationRequested)
                            {
                                matchedPhrases.Clear();
                                break;
                            }
                            int counter = 0;
                            bool isMaxPassed = false;
                            bool isLimitReached = false;
                            while (!isMaxPassed && !isLimitReached)
                            {
                                string currentPhrase = "";
                                for (int p = i; p <= i + counter && p < totalSplitted; p++)
                                {
                                    currentPhrase = string.Concat(currentPhrase, splittedPhrases[p], " ");
                                    if (p == totalSplitted - 1 || currentPhrase.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length > maximumWordsPerPhrase)
                                    {
                                        isLimitReached = true;
                                    }
                                }
                                Phrase phrase = new(currentPhrase);
                                int countPhraseWords = phrase.NormalizedText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
                                if (phrase.NormalizedText.Length >= minCharsPerPhrase && countPhraseWords >= minimumWordsPerPhrase && countPhraseWords <= maximumWordsPerPhrase)
                                {
                                    if (PhraseHelper.PhraseContainsAnyValue(phrase, values, calculationMethod, out int containedValue))
                                    {
                                        matchedPhrases.Add(phrase);
                                    }
                                    //Here we check to see whether the values of a phrase are larger than the maximum value we are looking for. 
                                    //If they are, then there is no need to continue adding more words to this phrase.
                                    if (PhraseHelper.PhraseExceedsMaxValue(phrase, calculationMethod, maxValue))
                                    {
                                        isMaxPassed = true;
                                    }
                                }
                                counter++;
                            }
                        }
                    }
                    else
                    {
                        if (phraseSeparator != PhraseSeparator.AllExceptSpace)
                        {
                            string[] separators = GetMissingSeparators(phraseSeparator);
                            for (int i = 0; i < splittedPhrases.Length; i++)
                            {
                                foreach (string s in separators)
                                {
                                    splittedPhrases[i] = splittedPhrases[i].Replace(s, " ");
                                }
                            }
                        }

                        for (int i = 0; i < totalSplitted; i++)
                        {
                            if (cts.IsCancellationRequested)
                            {
                                matchedPhrases.Clear();
                                break;
                            }
                            Phrase phrase = new(splittedPhrases[i]);
                            int countWords = phrase.NormalizedText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
                            if (phrase.NormalizedText.Length >= minCharsPerPhrase && countWords >= minimumWordsPerPhrase && countWords <= maximumWordsPerPhrase)
                            {
                                if (PhraseHelper.PhraseContainsAnyValue(phrase, values, calculationMethod, out int containedValue))
                                {
                                    matchedPhrases.Add(phrase);
                                }
                            }
                        }
                    }
                });
            }

            return matchedPhrases.ToList();
        }

        /// <summary>
        /// Will scan an array of strings and look for phrases that match the given criteria
        /// </summary>
        /// <param name="splittedPhrases">The text you want to search</param>
        /// <param name="minCharsPerPhrase">Set the minimum number of characters for each phrase</param>
        /// <param name="maximumWordsPerPhrase">Set the maximum amount of words for each phrase</param>
        /// <returns>A list of phrases that match our criteria</returns>
        public static async Task<List<Phrase>> ScanAsync(string[] splittedPhrases, PhraseSeparator phraseSeparator, int minCharsPerPhrase = 3, int minimumWordsPerPhrase = 1, int maximumWordsPerPhrase = 1, CancellationToken cts = default)
        {
            HashSet<Phrase> matchedPhrases = new();

            if (splittedPhrases != null && splittedPhrases.Length > 0)
            {
                await Task.Run(() =>
                {
                    int totalSplitted = splittedPhrases.Length;
                    if (phraseSeparator.HasFlag(PhraseSeparator.Space))
                    {
                        for (int i = 0; i < totalSplitted; i++)
                        {
                            if (cts.IsCancellationRequested)
                            {
                                matchedPhrases.Clear();
                                break;
                            }
                            int counter = 0;
                            bool isLimitReached = false;
                            while (!isLimitReached)
                            {
                                string currentPhrase = "";
                                for (int p = i; p <= i + counter && p < totalSplitted; p++)
                                {
                                    currentPhrase = string.Concat(currentPhrase, splittedPhrases[p], " ");
                                    if (p == totalSplitted - 1 || currentPhrase.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length > maximumWordsPerPhrase)
                                    {
                                        isLimitReached = true;
                                    }
                                }
                                counter++;
                                Phrase phrase = new(currentPhrase);
                                int countPhraseWords = phrase.NormalizedText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
                                if (phrase.NormalizedText.Length >= minCharsPerPhrase && countPhraseWords >= minimumWordsPerPhrase && countPhraseWords <= maximumWordsPerPhrase)
                                {
                                    matchedPhrases.Add(phrase);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (phraseSeparator != PhraseSeparator.AllExceptSpace)
                        {
                            string[] separators = GetMissingSeparators(phraseSeparator);
                            for (int i = 0; i < splittedPhrases.Length; i++)
                            {
                                foreach (string s in separators)
                                {
                                    splittedPhrases[i] = splittedPhrases[i].Replace(s, " ");
                                }
                            }
                        }

                        for (int i = 0; i < totalSplitted; i++)
                        {
                            if (cts.IsCancellationRequested)
                            {
                                matchedPhrases.Clear();
                                break;
                            }
                            Phrase phrase = new(splittedPhrases[i]);
                            int countWords = phrase.NormalizedText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
                            if (phrase.NormalizedText.Length >= minCharsPerPhrase && countWords >= minimumWordsPerPhrase && countWords <= maximumWordsPerPhrase)
                            {
                                matchedPhrases.Add(phrase);
                            }
                        }
                    }
                });
            }

            return matchedPhrases.ToList();
        }

        private static string[] GetSeparators(PhraseSeparator phraseSeparator)
        {
            if (phraseSeparator == PhraseSeparator.None || phraseSeparator == PhraseSeparator.All)
            {
                return allSeparators;
            }
            List<string> separators = new();
            if (phraseSeparator.HasFlag(PhraseSeparator.Space)) separators.Add(" ");
            if (phraseSeparator.HasFlag(PhraseSeparator.Comma)) separators.Add(",");
            if (phraseSeparator.HasFlag(PhraseSeparator.Semicolon)) separators.Add(";");
            if (phraseSeparator.HasFlag(PhraseSeparator.Semicolon)) separators.Add("·");
            if (phraseSeparator.HasFlag(PhraseSeparator.Colon)) separators.Add(":");
            if (phraseSeparator.HasFlag(PhraseSeparator.FullStop)) separators.Add(".");
            if (phraseSeparator.HasFlag(PhraseSeparator.Tab)) separators.Add("\t");
            if (phraseSeparator.HasFlag(PhraseSeparator.NewLine)) separators.Add(Environment.NewLine);
            return separators.ToArray();
        }

        private static string[] GetMissingSeparators(PhraseSeparator phraseSeparator)
        {
            if (phraseSeparator == PhraseSeparator.None || phraseSeparator == PhraseSeparator.All)
            {
                return allSeparators;
            }
            List<string> separators = new();
            if (!phraseSeparator.HasFlag(PhraseSeparator.Space)) separators.Add(" ");
            if (!phraseSeparator.HasFlag(PhraseSeparator.Comma)) separators.Add(",");
            if (!phraseSeparator.HasFlag(PhraseSeparator.Semicolon)) separators.Add(";");
            if (!phraseSeparator.HasFlag(PhraseSeparator.Semicolon)) separators.Add("·");
            if (!phraseSeparator.HasFlag(PhraseSeparator.Colon)) separators.Add(":");
            if (!phraseSeparator.HasFlag(PhraseSeparator.FullStop)) separators.Add(".");
            if (!phraseSeparator.HasFlag(PhraseSeparator.Tab)) separators.Add("\t");
            if (!phraseSeparator.HasFlag(PhraseSeparator.NewLine)) separators.Add(Environment.NewLine);
            return separators.ToArray();
        }

        private static readonly string[] allSeparators = new string[] { " ", ",", ";", ":", ".", "·", "\t", Environment.NewLine };
    }
}