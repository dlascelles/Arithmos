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
/// Represents a repository for operations, providing CRUD operations for the 'Operation' entity.
/// </summary>
internal class OperationRepository(IDbTransaction transaction) : RepositoryBase(transaction), IOperationRepository
{
    /// <summary>
    /// Creates a new operation in the database.
    /// </summary>
    /// <param name="operation">The operation to be created.</param>
    /// <returns>The Id of the newly created operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="operation"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the Id of <paramref name="operation"/> is not 0.</exception>
    /// <exception cref="DuplicateNameException">Thrown if the name of the operation already exists in the database.</exception>
    public int Create(Operation operation)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation), "Operation to be saved, must not be null");
        if (operation.Id != 0) throw new ArgumentOutOfRangeException(nameof(operation), "The Id of an operation to be created must be 0.");
        if (IsNameDuplicate(operation.Name)) throw new DuplicateNameException("The name of the operation already exists.");

        string insertOperation = @"INSERT INTO Operation (Name, EntryDate) VALUES (@Name, @EntryDate)";
        Transaction.Connection.Execute(insertOperation, operation);
        int operationId = Transaction.Connection.ExecuteScalar<int>("SELECT last_insert_rowid();");
        return operationId;
    }

    /// <summary>
    /// Retrieves all operations from the database.
    /// </summary>
    /// <returns>An IEnumerable of all operations.</returns>
    public IEnumerable<Operation> RetrieveAll()
    {
        string retrieveOperations = @"SELECT Id, Name, EntryDate FROM Operation;";
        IEnumerable<Operation> operations = Transaction.Connection.Query<Operation>(retrieveOperations);
        return operations;
    }

    /// <summary>
    /// Asynchronously deletes an operation with the specified Id from the database.
    /// </summary>
    /// <param name="id">The Id of the operation to be deleted.</param>
    /// <returns>The number of affected rows in the Operation table (total count of deleted operations).</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="id"/> is less than or equal to zero.</exception>
    public async Task<int> DeleteAsync(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Cannot delete operation with zero or negative Id");

        string deleteOperation = @"DELETE FROM Operation WHERE Id = @Id ";
        int result = await Transaction.Connection.ExecuteAsync(deleteOperation, new { Id = id });
        return result;
    }

    /// <summary>
    /// Asynchronously deletes multiple operations with the specified Ids from the database.
    /// </summary>
    /// <param name="ids">The list of Ids of operations to delete.</param>
    /// <returns>The number of affected rows in the Operation table (total count of deleted operations).</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="ids"/> is <c>null</c> or empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if any Id in <paramref name="ids"/> is less than or equal to zero.</exception>
    public async Task<int> DeleteAsync(List<int> ids)
    {
        if (ids == null || ids.Count == 0) throw new ArgumentNullException(nameof(ids), "The list of ids cannot be empty");
        if (ids.Any(id => id <= 0)) throw new ArgumentOutOfRangeException(nameof(ids), "Cannot delete operation with zero or negative Id");

        string deleteOperations = $"DELETE FROM Operation WHERE Id IN ({string.Join(",", ids)})";
        int result = await Transaction.Connection.ExecuteAsync(deleteOperations);
        return result;
    }

    // <summary>
    /// Checks if an operation name already exists in the database.
    /// </summary>
    /// <param name="operationName">The name of the operation to check for duplicates.</param>
    /// <param name="id">The Id of the operation to exclude from the duplicate check.</param>
    /// <returns><c>true</c> if a duplicate name exists; otherwise, <c>false</c>.</returns>
    public bool IsNameDuplicate(string operationName, int id = 0)
    {
        string query = "SELECT Id FROM Operation WHERE Name = @OperationName AND Id != @OperationId";
        int matchingId = Transaction.Connection.ExecuteScalar<int>(query, new { OperationName = operationName, OperationId = id });
        return matchingId > 0;
    }
}
