/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDataAccess.Interfaces;
using Dapper;
using System.Data;

namespace ArithmosDataAccess;

/// <summary>
/// Represents a repository for managing settings with database interactions.
/// </summary>
internal class SettingRepository(IDbTransaction transaction) : RepositoryBase(transaction), ISettingRepository
{
    /// <summary>
    /// Retrieves the value associated with the specified key from the database.
    /// </summary>
    /// <param name="key">The key of the setting to retrieve.</param>
    /// <returns>The value associated with the specified key, or null if the key is not found.</returns>
    public string Retrieve(string key)
    {
        string retrieve = "SELECT Value FROM Setting WHERE Key = @Key";
        string value = Transaction.Connection.ExecuteScalar<string>(retrieve, new { Key = key });
        return value;
    }

    /// <summary>
    /// Inserts a new setting or updates an existing setting in the database.
    /// </summary>
    /// <param name="key">The key of the setting to upsert.</param>
    /// <param name="newValue">The new value to associate with the key.</param>
    /// <returns>The number of affected rows.</returns>
    public int Upsert(string key, string newValue)
    {
        string upsert = @"INSERT INTO Setting (Key, Value) VALUES (@Key, @Value)
                          ON CONFLICT (Key)
                          DO UPDATE SET Value = @Value WHERE Key = @Key";
        int result = Transaction.Connection.Execute(upsert, new { Key = key, Value = newValue });
        return result;
    }
}
