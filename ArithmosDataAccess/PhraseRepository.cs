/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDataAccess.Interfaces;
using ArithmosModels;
using Dapper;
using System.Data;
using System.Text;

namespace ArithmosDataAccess;

/// <summary>
/// Represents a repository for managing phrases with database interactions.
/// </summary>
internal class PhraseRepository(IDbTransaction transaction) : RepositoryBase(transaction), IPhraseRepository
{
    /// <summary>
    /// Asynchronously creates a batch of phrases in the database.
    /// </summary>
    /// <param name="phrases">The list of phrases to create.</param>
    /// <param name="cts">The cancellation token to cancel the operation.</param>
    /// <param name="operation">The operation associated with the phrases.</param>
    /// <returns>The total number of phrases created.</returns>
    public async Task<int> CreateAsync(List<Phrase> phrases, CancellationToken cts, Operation operation = null)
    {
        if (phrases == null || phrases.Count == 0) throw new ArgumentException("There are no phrases to save");

        int? operationId = operation == null || operation.Id == 0 ? null : operation.Id;
        if (operation != null && operation.Id == 0)
        {
            string insertOperation = @"INSERT INTO Operation (Name, EntryDate) VALUES (@Name, @EntryDate)";
            await Transaction.Connection.ExecuteAsync(insertOperation, operation);
            operationId = await Transaction.Connection.ExecuteScalarAsync<int>("SELECT last_insert_rowid();");
        }
        int totals = 0;
        string phraseInsert = "INSERT OR IGNORE INTO Phrase (Content, OperationId, Alphabet) VALUES (@Content, @OperationId, @Alphabet);";
        Dictionary<long, Phrase> phrasesDictionary = [];
        foreach (Phrase phrase in phrases)
        {
            cts.ThrowIfCancellationRequested();
            int result = Transaction.Connection.Execute(phraseInsert, new { phrase.Content, OperationId = operationId, phrase.Alphabet });
            long lastId = result == 1 ? Transaction.Connection.ExecuteScalar<long>("SELECT last_insert_rowid();") : 0;
            if (lastId > 0)
            {
                phrasesDictionary.Add(lastId, phrase);
                totals++;
            }
        }
        StringBuilder stringBuilder = new();
        int batchSize = 500;
        int totalPhrases = phrasesDictionary.Count;
        int processedPhrases = 0;
        while (processedPhrases < totalPhrases)
        {
            Dictionary<long, Phrase> currentBatch = phrasesDictionary.Skip(processedPhrases).Take(batchSize).ToDictionary<long, Phrase>();
            stringBuilder.Append("INSERT INTO PhraseGematriaMethod (PhraseId, GematriaMethodId, Value) VALUES ");
            foreach (KeyValuePair<long, Phrase> kvp in currentBatch)
            {
                cts.ThrowIfCancellationRequested();
                foreach (((int, string), int) values in kvp.Value.Values)
                {
                    stringBuilder.Append($"({kvp.Key},{values.Item1.Item1},{values.Item2}),");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(';');
            Transaction.Connection.Execute(stringBuilder.ToString());
            stringBuilder.Clear();
            processedPhrases += currentBatch.Count;
        }
        return totals;
    }

    /// <summary>
    /// Creates a phrase in the database and associates it with an operation.
    /// </summary>
    /// <param name="phrase">The phrase to create.</param>
    /// <param name="operationId">The Id of the associated operation.</param>
    /// <returns>True if the creation was successful, false otherwise.</returns>
    public bool Create(Phrase phrase, int? operationId)
    {
        string insertPhrase = @"INSERT OR IGNORE INTO Phrase (Content, OperationId, Alphabet) VALUES (@Content, @OperationId, @Alphabet)";
        int result = Transaction.Connection.Execute(insertPhrase, new { phrase.Content, OperationId = operationId, phrase.Alphabet });
        if (result == 1)
        {
            long lastId = Transaction.Connection.ExecuteScalar<long>("SELECT last_insert_rowid();");
            string linkValues = @"INSERT INTO PhraseGematriaMethod (PhraseId, GematriaMethodId, Value) VALUES (@PhraseId, @GematriaMethodId, @Value)";
            foreach (((int, string), int) value in phrase.Values)
            {
                Transaction.Connection.Execute(linkValues, new { PhraseId = lastId, GematriaMethodId = value.Item1.Item1, Value = value.Item2 });
            }
        }
        return result == 1;
    }

    /// <summary>
    /// Asynchronously recalculates and updates all phrases in the database.
    /// </summary>
    /// <returns>The total number of recreated values in the database.</returns>
    public async Task<List<Phrase>> RetrieveAllAsync()
    {
        GematriaMethodRepository gematriaMethodRepository = new(Transaction);
        List<GematriaMethod> allGematriaMethods = gematriaMethodRepository.RetrieveAll().ToList();
        List<Phrase> result = [];
        string retrievePhrases = @"SELECT Id, Content, OperationId FROM Phrase";
        IEnumerable<(long, string, int)> rows = await Transaction.Connection.QueryAsync<(long, string, int)>(retrievePhrases);
        foreach ((long, string, int) row in rows)
        {
            result.Add(new Phrase(row.Item2, allGematriaMethods, row.Item1, row.Item3));
        }
        return result;
    }

    /// <summary>
    /// Asynchronously retrieves a list of orphan phrases from the database.
    /// </summary>
    /// <returns>
    /// A list of phrases that do not have an associated operation.
    /// </returns>
    /// <remarks>
    /// The method fetches phrases with a null OperationId.
    /// </remarks>
    public async Task<List<Phrase>> RetrieveAllOrphansAsync()
    {
        GematriaMethodRepository gematriaMethodRepository = new(Transaction);
        List<GematriaMethod> allGematriaMethods = gematriaMethodRepository.RetrieveAll().ToList();
        List<Phrase> result = [];
        string retrievePhrases = @"SELECT Id, Content, OperationId FROM Phrase WHERE OperationId IS NULL";
        IEnumerable<(long, string, int)> rows = await Transaction.Connection.QueryAsync<(long, string, int)>(retrievePhrases);
        foreach ((long, string, int) row in rows)
        {
            result.Add(new Phrase(row.Item2, allGematriaMethods, row.Item1, row.Item3));
        }
        return result;
    }

    /// <summary>
    /// Asynchronously retrieves a list of phrases from the database associated with a specific operation.
    /// </summary>
    /// <param name="operationId">The Id of the operation to retrieve phrases for.</param>
    /// <returns>
    /// A list of phrases associated with the specified operation.
    /// </returns>
    /// <remarks>
    /// The method fetches phrases based on the provided operation Id.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="operationId"/> is less than or equal to zero.
    /// </exception>   
    public async Task<List<Phrase>> RetrieveByOperationAsync(int operationId)
    {
        if (operationId <= 0) throw new ArgumentOutOfRangeException(nameof(operationId), "Operation Id must be larger than 0.");

        GematriaMethodRepository gematriaMethodRepository = new(Transaction);
        List<GematriaMethod> allGematriaMethods = gematriaMethodRepository.RetrieveAll().ToList();
        List<Phrase> result = [];
        string retrievePhrases = @"SELECT Id, Content, OperationId FROM Phrase WHERE OperationId = @OperationId";
        IEnumerable<(long, string, int)> rows = await Transaction.Connection.QueryAsync<(long, string, int)>(retrievePhrases, new { OperationId = operationId });
        foreach ((long, string, int) row in rows)
        {
            result.Add(new Phrase(row.Item2, allGematriaMethods, row.Item1, row.Item3));
        }
        return result;
    }

    /// <summary>
    /// Asynchronously searches for phrases in the database that are similar to the specified phrase based on gematria values.
    /// </summary>
    /// <param name="phrase">The reference phrase for similarity comparison.</param>
    /// <param name="gematriaIds">An optional array of gematria method Ids to filter the search. If <c>null</c> or empty, all gematria methods are considered.</param>
    /// <returns>
    /// A list of phrases that are similar to the reference phrase based on the specified gematria values.
    /// </returns>
    /// <remarks>
    /// The search is performed based on the combination of gematria method Ids and corresponding values.
    /// </remarks>
    public async Task<List<Phrase>> SearchForSimilarPhrasesAsync(Phrase phrase, int[] gematriaIds = null)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append(@"SELECT DISTINCT Phrase.Id, Phrase.Content, Phrase.OperationId FROM Phrase JOIN PhraseGematriaMethod ON Phrase.Id = PhraseGematriaMethod.PhraseId JOIN GematriaMethod ON PhraseGematriaMethod.GematriaMethodId = GematriaMethod.Id WHERE ");
        var allValues = gematriaIds == null || gematriaIds.Length == 0 ? phrase.Values : phrase.Values.Where(v => gematriaIds.Contains(v.GematriaMethod.Id)).ToArray();
        foreach (var method in allValues)
        {
            stringBuilder.Append($"PhraseGematriaMethod.GematriaMethodId = {method.GematriaMethod.Id} AND PhraseGematriaMethod.Value = {method.Value} AND PhraseGematriaMethod.Value > 0  OR ");
        }
        string searchQuery = stringBuilder.ToString();
        searchQuery = searchQuery.Remove(searchQuery.Length - 4, 4);
        searchQuery = searchQuery.Insert(searchQuery.Length, ";");
        GematriaMethodRepository gematriaMethodRepository = new(Transaction);
        List<GematriaMethod> allGematriaMethods = gematriaMethodRepository.RetrieveAll().ToList();
        List<Phrase> result = [];
        IEnumerable<(long, string, int)> rows = await Transaction.Connection.QueryAsync<(long, string, int)>(searchQuery);
        foreach (var row in rows)
        {
            result.Add(new Phrase(row.Item2, allGematriaMethods, row.Item1, row.Item3));
        }
        return result;
    }

    /// <summary>
    /// Asynchronously searches for phrases in the database based on selected gematria methods and values.
    /// </summary>
    /// <param name="selectedMethods">The list of gematria methods to include in the search.</param>
    /// <param name="values">The list of values associated with the gematria methods.</param>
    /// <returns>
    /// A list of phrases matching the specified gematria methods and values.
    /// </returns>
    /// <remarks>
    /// The search is based on the combination of gematria method Ids and corresponding values.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="selectedMethods"/> or <paramref name="values"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown if <paramref name="selectedMethods"/> or <paramref name="values"/> is empty.
    /// </exception>   
    public async Task<List<Phrase>> SearchByValuesAsync(List<GematriaMethod> selectedMethods, List<int> values)
    {
        if (selectedMethods == null) throw new ArgumentNullException(nameof(selectedMethods), "The list of gematria methods cannot be null.");
        if (values == null) throw new ArgumentNullException(nameof(values), "The list of values cannot be null.");
        if (selectedMethods.Count == 0) throw new ArgumentException("The list of gematria methods cannot be empty.", nameof(selectedMethods));
        if (values.Count == 0) throw new ArgumentException("The list of values cannot be empty.", nameof(values));

        StringBuilder stringBuilder = new();
        stringBuilder.Append(@"SELECT DISTINCT Phrase.Id, Phrase.Content, Phrase.OperationId FROM Phrase JOIN PhraseGematriaMethod ON Phrase.Id = PhraseGematriaMethod.PhraseId JOIN GematriaMethod ON PhraseGematriaMethod.GematriaMethodId = GematriaMethod.Id WHERE ");
        foreach (var method in selectedMethods)
        {
            foreach (var value in values)
            {
                stringBuilder.Append($"PhraseGematriaMethod.GematriaMethodId = {method.Id} AND PhraseGematriaMethod.Value = {value} AND PhraseGematriaMethod.Value > 0  OR ");
            }
        }
        string searchQuery = stringBuilder.ToString();
        searchQuery = searchQuery.Remove(searchQuery.Length - 4, 4);
        searchQuery = searchQuery.Insert(searchQuery.Length, ";");
        GematriaMethodRepository gematriaMethodRepository = new(Transaction);
        List<GematriaMethod> allGematriaMethods = gematriaMethodRepository.RetrieveAll().ToList();
        List<Phrase> result = [];
        IEnumerable<(long, string, int)> rows = await Transaction.Connection.QueryAsync<(long, string, int)>(searchQuery);
        foreach (var row in rows)
        {
            result.Add(new Phrase(row.Item2, allGematriaMethods, row.Item1, row.Item3));
        }
        return result;
    }

    /// <summary>
    /// Asynchronously searches for phrases in the database that contain the specified text.
    /// </summary>
    /// <param name="text">The text to search for within phrase content.</param>
    /// <returns>
    /// A list of phrases containing the specified text.
    /// </returns>
    /// <remarks>
    /// The search is not case-sensitive for English, but it is for other languages. It uses the LIKE SQL operator for partial matching.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="text"/> is <c>null</c>.</exception>    
    public async Task<List<Phrase>> SearchByTextAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text), "The text to search for cannot be empty.");

        string searchQuery = @"SELECT Id, Content, OperationId FROM Phrase WHERE Content LIKE @Content";
        IEnumerable<(long, string, int)> rows = await Transaction.Connection.QueryAsync<(long, string, int)>(searchQuery, new { Content = '%' + text + '%' });
        List<Phrase> result = [];
        GematriaMethodRepository gematriaMethodRepository = new(Transaction);
        List<GematriaMethod> allGematriaMethods = gematriaMethodRepository.RetrieveAll().ToList();
        foreach (var row in rows)
        {
            result.Add(new Phrase(row.Item2, allGematriaMethods, row.Item1, row.Item3));
        }
        return result;
    }

    /// <summary>
    /// Recreates all values in the PhraseGematriaMethod table based on the provided list of phrases.
    /// </summary>
    /// <param name="phrases">The list of phrases to use for recreating values.</param>
    /// <returns>The total number of recreated values in the PhraseGematriaMethod table.</returns>
    private int ReCreateAllValues(List<Phrase> phrases)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append("PRAGMA foreign_keys = off;");
        stringBuilder.Append("DROP TABLE PhraseGematriaMethod;");
        stringBuilder.Append("CREATE TABLE PhraseGematriaMethod (Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, PhraseId INTEGER REFERENCES Phrase (Id) ON DELETE CASCADE ON UPDATE CASCADE, GematriaMethodId INTEGER REFERENCES GematriaMethod (Id) ON DELETE CASCADE ON UPDATE CASCADE, Value INTEGER NOT NULL);");
        Transaction.Connection.Execute(stringBuilder.ToString());
        stringBuilder.Clear();
        int result = 0;
        int batchSize = 1000;
        int totalPhrases = phrases.Count;
        int processedPhrases = 0;
        while (processedPhrases < totalPhrases)
        {
            List<Phrase> currentBatch = phrases.Skip(processedPhrases).Take(batchSize).ToList();
            stringBuilder.Append("INSERT INTO PhraseGematriaMethod (PhraseId, GematriaMethodId, Value) VALUES ");
            foreach (Phrase phrase in currentBatch)
            {
                foreach (((int, string), int) values in phrase.Values)
                {
                    stringBuilder.Append($"({phrase.Id},{values.Item1.Item1},{values.Item2}),");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(';');
            result += Transaction.Connection.Execute(stringBuilder.ToString());
            stringBuilder.Clear();
            processedPhrases += currentBatch.Count;
        }
        stringBuilder.Clear();
        stringBuilder.Append("PRAGMA foreign_keys = on;");
        stringBuilder.Append("CREATE INDEX Index_PhraseGematriaMethod_PhraseId ON PhraseGematriaMethod(PhraseId);CREATE INDEX Index_PhraseGematriaMethod_GematriaMethodId ON PhraseGematriaMethod(GematriaMethodId);CREATE INDEX Index_PhraseGematriaMethod_Value ON PhraseGematriaMethod(Value);");
        Transaction.Connection.Execute(stringBuilder.ToString());
        return result;
    }

    /// <summary>
    /// Asynchronously recalculates and updates all phrases in the database.
    /// </summary>
    /// <returns>The total number of recreated values in the database.</returns>
    public async Task<int> RecalculatePhrasesAsync()
    {
        GematriaMethodRepository gematriaMethodRepository = new(Transaction);
        List<GematriaMethod> allGematriaMethods = gematriaMethodRepository.RetrieveAll().ToList();

        if (allGematriaMethods == null || allGematriaMethods.Count == 0) return 0;

        PhraseRepository phraseRepository = new(Transaction);
        List<Phrase> allPhrases = await phraseRepository.RetrieveAllAsync();

        if (allPhrases == null || allPhrases.Count == 0) return 0;

        int totalRecreated = ReCreateAllValues(allPhrases);

        if (allPhrases.Count != totalRecreated / allGematriaMethods.Count) throw new DataException("Recalculation of phrases failed. No changes were made to the database.");

        return totalRecreated;
    }

    /// <summary>
    /// Asynchronously deletes a phrase with the specified Id from the database.
    /// </summary>
    /// <param name="id">The Id of the phrase to delete.</param>
    /// <returns>
    /// The number of affected rows in the Phrase table (1 if the deletion was successful, 0 otherwise).
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if <paramref name="id"/> is less than or equal to zero.
    /// </exception>
    public async Task<long> DeleteAsync(long id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Cannot delete phrase with zero or negative Id");

        string deletePhrase = @"DELETE FROM Phrase WHERE Id = @Id ";
        int result = await Transaction.Connection.ExecuteAsync(deletePhrase, new { Id = id });
        return result;
    }

    /// <summary>
    /// Asynchronously deletes multiple phrases with the specified Ids from the database.
    /// </summary>
    /// <param name="ids">The list of IDs of phrases to delete.</param>
    /// <returns>
    /// The number of affected rows in the Phrase table (total count of deleted phrases).
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="ids"/> is <c>null</c> or empty.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if any Id in <paramref name="ids"/> is less than or equal to zero.
    /// </exception>
    public async Task<long> DeleteAsync(List<long> ids)
    {
        if (ids == null || ids.Count == 0) throw new ArgumentNullException(nameof(ids), "The list of ids to delete cannot be empty");
        if (ids.Any(id => id <= 0)) throw new ArgumentOutOfRangeException(nameof(ids), "Cannot delete a phrase with zero or negative Id");

        string deletePhrases = $"DELETE FROM Phrase WHERE Id IN ({string.Join(",", ids)});";
        int result = await Transaction.Connection.ExecuteAsync(deletePhrases);
        return result;
    }
}