/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosDAL
{
    public class PhraseDataAccess
    {
        /// <summary>
        /// Creates a new phrase
        /// </summary>
        /// <param name="phrase">The phrase to be saved</param>
        public async Task<int> CreateAsync(Phrase phrase)
        {
            int result = 0;

            if (IsValidPhrase(phrase))
            {
                using (SQLiteConnection con = new(Database.ConnectionString))
                {
                    string query = $"INSERT OR IGNORE INTO Phrase (Text, GematriaValue, OrdinalValue, ReducedValue, SumerianValue, PrimesValue, SquaredValue, MisparGadolValue, MisparShemiValue,  Alphabet, OperationId) VALUES (@Text, @GematriaValue, @OrdinalValue, @ReducedValue, @SumerianValue, @PrimesValue, @SquaredValue, @MisparGadolValue, @MisparShemiValue,  @Alphabet, @OperationId)";

                    using (SQLiteCommand cmd = new(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Text", phrase.NormalizedText);
                        cmd.Parameters.AddWithValue("@GematriaValue", phrase.Gematria);
                        cmd.Parameters.AddWithValue("@OrdinalValue", phrase.Ordinal);
                        cmd.Parameters.AddWithValue("@ReducedValue", phrase.Reduced);
                        cmd.Parameters.AddWithValue("@SumerianValue", phrase.Sumerian);
                        cmd.Parameters.AddWithValue("@PrimesValue", phrase.Primes);
                        cmd.Parameters.AddWithValue("@SquaredValue", phrase.Squared);
                        cmd.Parameters.AddWithValue("@MisparGadolValue", phrase.MisparGadol);
                        cmd.Parameters.AddWithValue("@MisparShemiValue", phrase.MisparShemi);
                        cmd.Parameters.AddWithValue("@Alphabet", (int)phrase.Alphabet);
                        cmd.Parameters.AddWithValue("@OperationId", phrase.OperationId == 0 ? (object)DBNull.Value : phrase.OperationId);
                        await con.OpenAsync();
                        result = await cmd.ExecuteNonQueryAsync();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates multiple phrases
        /// </summary>
        /// <param name="phrases">A list of phrases</param>
        public async Task<int> CreateAsync(List<Phrase> phrases, CancellationToken cts)
        {
            int result = 0;

            using (SQLiteConnection con = new(Database.ConnectionString))
            {
                await con.OpenAsync(cts);
                using (SQLiteTransaction trans = con.BeginTransaction())
                {
                    string query = $"INSERT OR IGNORE INTO Phrase (Text, GematriaValue, OrdinalValue, ReducedValue, SumerianValue, PrimesValue, SquaredValue, MisparGadolValue, MisparShemiValue,  Alphabet, OperationId) VALUES (@Text, @GematriaValue, @OrdinalValue, @ReducedValue, @SumerianValue, @PrimesValue, @SquaredValue, @MisparGadolValue, @MisparShemiValue,  @Alphabet, @OperationId)";
                    using (SQLiteCommand cmd = new(query, con))
                    {
                        foreach (Phrase phrase in phrases)
                        {
                            if (cts.IsCancellationRequested)
                            {
                                con.Cancel();
                                return -1;
                            }

                            if (IsValidPhrase(phrase))
                            {
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@Text", phrase.NormalizedText);
                                cmd.Parameters.AddWithValue("@GematriaValue", phrase.Gematria);
                                cmd.Parameters.AddWithValue("@OrdinalValue", phrase.Ordinal);
                                cmd.Parameters.AddWithValue("@ReducedValue", phrase.Reduced);
                                cmd.Parameters.AddWithValue("@SumerianValue", phrase.Sumerian);
                                cmd.Parameters.AddWithValue("@PrimesValue", phrase.Primes);
                                cmd.Parameters.AddWithValue("@SquaredValue", phrase.Squared);
                                cmd.Parameters.AddWithValue("@MisparGadolValue", phrase.MisparGadol);
                                cmd.Parameters.AddWithValue("@MisparShemiValue", phrase.MisparShemi);
                                cmd.Parameters.AddWithValue("@Alphabet", (int)phrase.Alphabet);
                                cmd.Parameters.AddWithValue("@OperationId", phrase.OperationId == 0 ? (object)DBNull.Value : phrase.OperationId);
                                result += await cmd.ExecuteNonQueryAsync(cts);
                            }
                        }
                    }
                    trans.Commit();
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a list of phrases and associates them with a specific operation
        /// </summary>
        /// <param name="phrases">A list of phrases</param>
        /// <param name="operation">An Operation</param>
        public async Task<int> CreateAsync(List<Phrase> phrases, Operation operation, CancellationToken cts)
        {
            int result = 0;

            using (SQLiteConnection con = new(Database.ConnectionString))
            {
                await con.OpenAsync(cts);
                using (SQLiteTransaction trans = con.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new(con))
                    {
                        if (await new OperationDataAccess().CreateAsync(operation, con) > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "SELECT last_insert_rowid()";
                            int latestId = Convert.ToInt32(await cmd.ExecuteScalarAsync(cts));
                            if (latestId > 0)
                            {
                                cmd.Parameters.Clear();
                                foreach (Phrase phrase in phrases)
                                {
                                    if (cts.IsCancellationRequested)
                                    {
                                        con.Cancel();
                                        return -1;
                                    }
                                    if (IsValidPhrase(phrase))
                                    {
                                        cmd.CommandText = $"INSERT OR IGNORE INTO Phrase (Text, GematriaValue, OrdinalValue, ReducedValue, SumerianValue, PrimesValue, SquaredValue, MisparGadolValue, MisparShemiValue,  Alphabet, OperationId) VALUES (@Text, @GematriaValue, @OrdinalValue, @ReducedValue, @SumerianValue, @PrimesValue, @SquaredValue, @MisparGadolValue, @MisparShemiValue,  @Alphabet, @OperationId)";
                                        cmd.Parameters.AddWithValue("@Text", phrase.NormalizedText);
                                        cmd.Parameters.AddWithValue("@GematriaValue", phrase.Gematria);
                                        cmd.Parameters.AddWithValue("@OrdinalValue", phrase.Ordinal);
                                        cmd.Parameters.AddWithValue("@ReducedValue", phrase.Reduced);
                                        cmd.Parameters.AddWithValue("@SumerianValue", phrase.Sumerian);
                                        cmd.Parameters.AddWithValue("@PrimesValue", phrase.Primes);
                                        cmd.Parameters.AddWithValue("@SquaredValue", phrase.Squared);
                                        cmd.Parameters.AddWithValue("@MisparGadolValue", phrase.MisparGadol);
                                        cmd.Parameters.AddWithValue("@MisparShemiValue", phrase.MisparShemi);
                                        cmd.Parameters.AddWithValue("@Alphabet", (int)phrase.Alphabet);
                                        cmd.Parameters.AddWithValue("@OperationId", latestId);
                                        result += await cmd.ExecuteNonQueryAsync(cts);
                                    }
                                }
                            }
                        }
                    }
                    trans.Commit();
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a list of phrases depending on their values, calculation methods and alphabet.
        /// </summary>
        /// <param name="values">The values to search for</param>
        /// <param name="calculationMethod">The calculatiopn methods to search for</param>
        /// <param name="alphabet">The alphabet to search for</param>
        /// <returns>A list of phrases</returns>
        public async Task<List<Phrase>> RetrieveAsync(List<int> values, CalculationMethod calculationMethod, Alphabet alphabet)
        {
            List<Phrase> phrases = new();

            if (values != null && values.Count > 0 && calculationMethod != CalculationMethod.None && alphabet != Alphabet.None)
            {
                string query = $"SELECT Text, OperationId FROM Phrase WHERE ({SQLValuesString(values, calculationMethod, alphabet)}) ";
                using (SQLiteConnection con = new(Database.ConnectionString))
                {
                    using (SQLiteCommand cmd = new(query, con))
                    {
                        await con.OpenAsync();
                        using (var dare = await cmd.ExecuteReaderAsync())
                        {
                            while (await dare.ReadAsync())
                            {
                                Phrase phrase = new(dare["Text"].ToString(), dare["OperationId"] == DBNull.Value ? 0 : Convert.ToInt32(dare["OperationId"]));
                                phrases.Add(phrase);
                            }
                        }
                    }
                }
            }

            return phrases;
        }

        /// <summary>
        /// Retrieves a list of phrases that are associated with specific operations
        /// </summary>
        /// <param name="operations">A list of operations</param>
        /// <returns>A list of phrases</returns>
        public async Task<List<Phrase>> RetrieveAsync(List<Operation> operations)
        {
            List<Phrase> phrases = new();

            if (operations != null && operations.Count > 0)
            {
                string query = $"SELECT Id, Text, OperationId FROM Phrase WHERE OperationId IN ({OperationDataAccess.SQLOperationsString(operations)})";

                using (SQLiteConnection con = new(Database.ConnectionString))
                {
                    using (SQLiteCommand cmd = new(query, con))
                    {
                        await con.OpenAsync();
                        using (var dare = await cmd.ExecuteReaderAsync())
                        {
                            while (await dare.ReadAsync())
                            {
                                Phrase phrase = new(dare["Text"].ToString(), Convert.ToInt32(dare["OperationId"]));
                                phrases.Add(phrase);
                            }
                        }
                    }
                }
            }

            return phrases;
        }

        /// <summary>
        /// Retrieves a list of phrases that are not associated with any Operation
        /// </summary>
        /// <param name="operations">A list of operations</param>
        /// <returns>A list of phrases</returns>
        public async Task<List<Phrase>> RetrieveOrphansAsync()
        {
            List<Phrase> phrases = new();

            string query = $"SELECT Id, Text FROM Phrase WHERE OperationId IS NULL";

            using (SQLiteConnection con = new(Database.ConnectionString))
            {
                using (SQLiteCommand cmd = new(query, con))
                {
                    await con.OpenAsync();
                    using (var dare = await cmd.ExecuteReaderAsync())
                    {
                        while (await dare.ReadAsync())
                        {
                            Phrase phrase = new(dare["Text"].ToString(), 0);
                            phrases.Add(phrase);
                        }
                    }
                }
            }

            return phrases;
        }

        /// <summary>
        /// Deletes multiple phrases from the database
        /// </summary>
        /// <param name="phrases">The list of phrases to delete</param>
        public async Task<int> DeleteAsync(List<Phrase> phrases)
        {
            int result = 0;

            if (phrases != null && phrases.Count > 0)
            {
                using (SQLiteConnection con = new(Database.ConnectionString))
                {
                    string query = $"DELETE FROM Phrase WHERE Id IN ({SQLPhrasesString(phrases)})";
                    await con.OpenAsync();
                    using (SQLiteCommand cmd = new(query, con))
                    {
                        result += await cmd.ExecuteNonQueryAsync();
                    }
                }
            }

            return result;
        }

        private string SQLPhrasesString(List<Phrase> phrases)
        {
            StringBuilder sb = new();

            if (phrases != null && phrases.Count > 0)
            {
                foreach (Phrase phrase in phrases)
                {
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        private string SQLValuesString(List<int> values, CalculationMethod calculationMethod, Alphabet alphabet)
        {
            StringBuilder sb = new();

            if (values != null && values.Count > 0)
            {
                foreach (CalculationMethod cm in Enum.GetValues(typeof(CalculationMethod)))
                {
                    if (cm != CalculationMethod.None && cm != CalculationMethod.All)
                    {
                        if (calculationMethod.HasFlag(cm))
                        {
                            sb.Append($"{cm}Value IN (");
                            sb.Append(SQLValuesINString(values));
                            sb.Append(" OR ");
                        }
                    }
                }
                sb.Replace(" OR ", "", sb.Length - 4, 4);

                sb.Append(" AND Alphabet IN (");

                foreach (Alphabet al in Enum.GetValues(typeof(Alphabet)))
                {
                    if (al != Alphabet.None && al != Alphabet.All)
                    {
                        if (alphabet.HasFlag(al))
                        {
                            sb.Append($"{(int)al},");
                        }
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(')');
            }

            return sb.ToString();
        }

        private string SQLValuesINString(List<int> values)
        {
            StringBuilder sb = new();

            if (values != null && values.Count > 0)
            {
                foreach (int value in values)
                {
                    sb.Append($"{value},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(')');
            }

            return sb.ToString();
        }

        private bool IsValidPhrase(Phrase phrase)
        {
            if (!string.IsNullOrEmpty(phrase.NormalizedText) && phrase.Gematria > 0)
            {
                return true;
            }
            return false;
        }
    }
}