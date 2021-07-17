/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosModels
{
    public static class Exporter
    {
        public static string FormatPhrases(List<Phrase> phrases, char delimiter)
        {
            StringBuilder sb = new StringBuilder();

            if (phrases != null && phrases.Count > 0)
            {
                sb.AppendJoin(delimiter, "Phrase",
                        nameof(Phrase.Gematria), nameof(Phrase.Ordinal), nameof(Phrase.Reduced), nameof(Phrase.Sumerian),
                        nameof(Phrase.Primes), nameof(Phrase.Squared), nameof(Phrase.MisparGadol), nameof(Phrase.MisparShemi)).AppendLine();

                foreach (Phrase phrase in phrases)
                {
                    //If the phrase is in an rtl language we must switch back to ltr afterwards using the special character
                    string text = phrase.NormalizedText + ((Char)0x200E).ToString();
                    sb.AppendJoin(delimiter, text,
                        phrase.Gematria.ToString(), phrase.Ordinal.ToString(), phrase.Reduced.ToString(), phrase.Sumerian.ToString(),
                        phrase.Primes.ToString(), phrase.Squared.ToString(), phrase.MisparGadol.ToString(), phrase.MisparShemi.ToString()).AppendLine();
                }
            }

            return sb.ToString();
        }

        public static async Task ExportAsync(string formattedPhrases, string folderPath, CancellationToken ct, string fileNamePrefix = null, string fileExtension = null)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = Path.Combine(folderPath, FileHelper.GetFileNameFromCurrentDate(fileNamePrefix, fileExtension));

            using (FileStream stream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(formattedPhrases);
                try
                {
                    await stream.WriteAsync(bytes, ct);
                }
                catch (TaskCanceledException) { }
            }
        }
    }
}