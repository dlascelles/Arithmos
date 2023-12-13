/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System.Data;

namespace ArithmosDataAccess;

/// <summary>
/// Provides a base implementation for repositories by handling the database transaction and connection.
/// </summary>
public abstract class RepositoryBase(IDbTransaction transaction)
{
    /// <summary>
    /// Gets the database transaction associated with the repository.
    /// </summary>
    protected IDbTransaction Transaction { get; private set; } = transaction;

    /// <summary>
    /// Gets the database connection associated with the repository.
    /// </summary>
    protected IDbConnection Connection => Transaction.Connection;
}
