/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDataAccess.Interfaces;
using ArithmosModels;
using Dapper;
using System.Data;

namespace ArithmosDataAccess;

/// <summary>
/// Represents a repository for Gematria methods, providing CRUD operations for the 'GematriaMethod' entity.
/// </summary>
internal class GematriaMethodRepository(IDbTransaction transaction) : RepositoryBase(transaction), IGematriaMethodRepository
{
    /// <summary>
    /// Creates a new Gematria method in the database.
    /// </summary>
    /// <param name="gematriaMethod">The Gematria method to be created.</param>
    /// <returns>The number of affected rows in the GematriaMethod table (should be 1 if the operation is successful).</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="gematriaMethod"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the Id of <paramref name="gematriaMethod"/> is not 0.</exception>
    /// <exception cref="ArgumentException">Thrown if the name of the Gematria method is null, empty, or whitespace.</exception>
    /// <exception cref="DuplicateNameException">Thrown if the name of the Gematria method already exists in the database.</exception>
    public int Create(GematriaMethod gematriaMethod)
    {
        if (gematriaMethod == null) throw new ArgumentNullException(nameof(gematriaMethod), "Cannot save a null gematria method");
        if (gematriaMethod.Id > 0) throw new ArgumentOutOfRangeException(nameof(gematriaMethod), "Cannot save gematria method with non-zero Id");
        if (string.IsNullOrWhiteSpace(gematriaMethod.Name)) throw new ArgumentException("The Gematria method name cannot be empty", nameof(gematriaMethod));
        if (IsNameDuplicate(gematriaMethod.Name)) throw new DuplicateNameException("The Gematria method name already exists");

        string insertGematriaMethod = @"INSERT INTO GematriaMethod (Name, Sort, CipherBody, AddsTotalNumberOfCharacters, AddsTotalNumberOfWords) VALUES (@Name, @Sort, @CipherBody, @AddsTotalNumberOfCharacters, @AddsTotalNumberOfWords);";
        int result = Transaction.Connection.Execute(insertGematriaMethod, new { gematriaMethod.Name, gematriaMethod.Sort, CipherBody = gematriaMethod.Cipher.Body, gematriaMethod.AddsTotalNumberOfCharacters, gematriaMethod.AddsTotalNumberOfWords });
        return result;
    }

    /// <summary>
    /// Retrieves a Gematria method with the specified Id from the database.
    /// </summary>
    /// <param name="id">The Id of the Gematria method to be retrieved.</param>
    /// <returns>The retrieved Gematria method.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="id"/> is less than or equal to zero.</exception>
    public GematriaMethod Retrieve(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Cannot retrieve a gematria method with zero or negative Id");

        string retrieveGematriaMethod = @"SELECT Id, Name, Sort, CipherBody, AddsTotalNumberOfCharacters, AddsTotalNumberOfWords FROM GematriaMethod WHERE Id = @Id;";
        (int, string, int, string, bool, bool) row = Transaction.Connection.Query<(int, string, int, string, bool, bool)>(retrieveGematriaMethod).SingleOrDefault();
        return new GematriaMethod(new Cipher(row.Item4)) { Id = row.Item1, Name = row.Item2, Sort = row.Item3, AddsTotalNumberOfCharacters = row.Item5, AddsTotalNumberOfWords = row.Item6 };
    }

    /// <summary>
    /// Retrieves all Gematria methods from the database.
    /// </summary>
    /// <returns>An IEnumerable of all Gematria methods, ordered by sort value.</returns>
    public IEnumerable<GematriaMethod> RetrieveAll()
    {
        string retrieveGematriaMethod = @"SELECT Id, Name, Sort, CipherBody, AddsTotalNumberOfCharacters, AddsTotalNumberOfWords FROM GematriaMethod ORDER BY Sort ASC;";
        List<GematriaMethod> gematriaMethods = [];
        IEnumerable<(int, string, int, string, bool, bool)> rows = Transaction.Connection.Query<(int, string, int, string, bool, bool)>(retrieveGematriaMethod);
        foreach ((int, string, int, string, bool, bool) row in rows)
        {
            gematriaMethods.Add(new GematriaMethod(new Cipher(row.Item4)) { Id = row.Item1, Name = row.Item2, Sort = row.Item3, AddsTotalNumberOfCharacters = row.Item5, AddsTotalNumberOfWords = row.Item6 });
        }
        return gematriaMethods;
    }

    /// <summary>
    /// Updates an existing Gematria method in the database.
    /// </summary>
    /// <param name="gematriaMethod">The Gematria method to be updated.</param>
    /// <returns>The number of affected rows in the GematriaMethod table (should be 1 if the operation is successful).</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="gematriaMethod"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the Id of <paramref name="gematriaMethod"/> is less than or equal to zero.</exception>
    /// <exception cref="ArgumentException">Thrown if the name of the Gematria method is null, empty, or whitespace.</exception>
    /// <exception cref="DuplicateNameException">Thrown if the name of the Gematria method already exists in the database.</exception>
    public int Update(GematriaMethod gematriaMethod)
    {
        if (gematriaMethod == null) throw new ArgumentNullException(nameof(gematriaMethod), "Cannot update null gematria method");
        if (gematriaMethod.Id <= 0) throw new ArgumentOutOfRangeException(nameof(gematriaMethod), "Cannot update gematria method with zero or negative Id");
        if (string.IsNullOrWhiteSpace(gematriaMethod.Name)) throw new ArgumentException("The Gematria method name cannot be empty", nameof(gematriaMethod));
        if (IsNameDuplicate(gematriaMethod.Name, gematriaMethod.Id)) throw new DuplicateNameException("The Gematria method name already exists");

        string updateGematriaMethod = @"UPDATE GematriaMethod SET Name = @Name, Sort = @Sort, CipherBody = @CipherBody, AddsTotalNumberOfCharacters = @AddsTotalNumberOfCharacters, AddsTotalNumberOfWords = @AddsTotalNumberOfWords WHERE Id = @Id;";
        int result = Transaction.Connection.Execute(updateGematriaMethod, new { gematriaMethod.Id, gematriaMethod.Name, gematriaMethod.Sort, CipherBody = gematriaMethod.Cipher.Body, gematriaMethod.AddsTotalNumberOfCharacters, gematriaMethod.AddsTotalNumberOfWords });
        return result;
    }

    /// <summary>
    /// Deletes a Gematria method with the specified Id from the database.
    /// </summary>
    /// <param name="id">The Id of the Gematria method to be deleted.</param>
    /// <returns>The number of affected rows in the GematriaMethod table (should be 1 if the operation is successful).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="id"/> is less than or equal to zero.</exception>
    public int Delete(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Cannot delete gematria method with zero or negative Id");

        string deleteGematriaMethod = @"DELETE FROM GematriaMethod WHERE Id = @Id;";
        int result = Transaction.Connection.Execute(deleteGematriaMethod, new { Id = id });
        return result;
    }

    /// <summary>
    /// Checks if a Gematria method name already exists in the database.
    /// </summary>
    /// <param name="methodName">The name of the Gematria method to check for duplicates.</param>
    /// <param name="id">The Id of the Gematria method to exclude from the duplicate check.</param>
    /// <returns><c>true</c> if a duplicate name exists; otherwise, <c>false</c>.</returns>
    public bool IsNameDuplicate(string methodName, int id = 0)
    {
        string query = "SELECT Id FROM GematriaMethod WHERE Name = @MethodName AND Id != @MethodId";
        int matchingId = Transaction.Connection.ExecuteScalar<int>(query, new { MethodName = methodName, MethodId = id });
        return matchingId > 0;
    }
}