/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Enums;
using ArithmosModels.Extensions;
using System.Text;

namespace ArithmosModels;

/// <summary>
/// Represents a class for exporting phrases with their gematria values to a CSV file.
/// </summary>
public class Exporter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Exporter"/> class with the specified phrases,
    /// cancellation token, and delimiter.
    /// </summary>
    /// <param name="phrases">The list of phrases to export.</param>
    /// <param name="cts">The cancellation token for the export operation.</param>
    /// <param name="delimiter">The delimiter character for the CSV file. The default is ','.</param>
    public Exporter(List<Phrase> phrases, CancellationToken cts, char delimiter = ',')
    {
        Delimiter = delimiter;
        Data = GetPhrasesForExport(phrases);
        CancellationToken = cts;
    }

    /// <summary>
    /// Asynchronously exports data to a CSV file.
    /// </summary>
    /// <returns>A task representing the asynchronous operation of exporting data to a CSV file.</returns>
    public async Task ExportAsync()
    {
        if (!Directory.Exists(FolderPath))
        {
            Directory.CreateDirectory(FolderPath);
        }
        string fullPath = Path.Combine(FolderPath, (string.IsNullOrWhiteSpace(FileNamePrefix) ? "" : FileNamePrefix) + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss_fff") + (string.IsNullOrWhiteSpace(FileExtension) ? "" : '.' + FileExtension));
        FileName = Path.GetFileName(fullPath);
        using (FileStream stream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Data);
            await stream.WriteAsync(bytes, CancellationToken);
        }
    }

    /// <summary>
    /// Generates a CSV-formatted string containing phrases and associated values for export.
    /// </summary>
    /// <param name="phrases">The list of phrases to include in the CSV data.</param>
    /// <returns>A CSV-formatted string representing the provided phrases and their associated values.</returns>
    private string GetPhrasesForExport(List<Phrase> phrases)
    {
        StringBuilder csvData = new();

        csvData.Append("Phrase");
        HashSet<string> columnNames = [];

        foreach (Phrase phrase in phrases)
        {
            foreach (((int, string), int) value in phrase.Values)
            {
                columnNames.Add(value.Item1.Item2);
            }
        }

        foreach (string columnName in columnNames)
        {
            csvData.Append(Delimiter.ToString() + columnName);
        }
        csvData.AppendLine();

        foreach (Phrase phrase in phrases)
        {
            csvData.Append('"' + phrase.Content.Replace('"', '\''));
            Alphabet alphabet = phrase.Content[^1].GetAlphabet();
            //We need to add the special character 0x200E to handle RTL languages
            csvData.Append(alphabet == Alphabet.Hebrew || alphabet == Alphabet.Arabic || alphabet == Alphabet.Unknown ? ((char)0x200E).ToString() : "");
            csvData.Append('"');
            foreach (string columnName in columnNames)
            {
                ((int, string), int) value = phrase.Values.FirstOrDefault(v => v.GematriaMethod.Name == columnName);
                csvData.Append(Delimiter.ToString() + value.Item2);
            }
            csvData.AppendLine();
        }

        return csvData.ToString();
    }

    /// <summary>
    /// Gets the data to be exported.
    /// </summary>
    public readonly string Data;

    /// <summary>
    /// Gets or sets the prefix for the exported file name. Default is "ArithmosData-".
    /// </summary>
    public string FileNamePrefix { get; init; } = "ArithmosData-";

    /// <summary>
    /// Gets or sets the file extension for the exported file. Default is "csv".
    /// </summary>
    public string FileExtension { get; init; } = "csv";

    /// <summary>
    /// Gets or sets the folder path where the exported file will be saved. Default is the current application path.
    /// </summary>
    public string FolderPath { get; init; } = Path.Combine(Environment.CurrentDirectory);

    /// <summary>
    /// Gets the generated file name after exporting.
    /// </summary>
    public string FileName { get; private set; }

    /// <summary>
    /// Gets or sets the delimiter character for the CSV file.
    /// </summary>
    public char Delimiter { get; init; }

    /// <summary>
    /// Gets or sets the cancellation token for the export operation.
    /// </summary>
    public CancellationToken CancellationToken { get; init; }
}
