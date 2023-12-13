/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System.Data;
using ArithmosDataAccess.Interfaces;

namespace ArithmosDataAccess;

/// <summary>
/// Represents a unit of work for managing database transactions and repositories.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    public UnitOfWork()
    {
        connection = Database.GetConnection();
        connection.Open();
        transaction = connection.BeginTransaction();
    }

    /// <summary>
    /// Commits the current transaction and resets repositories. 
    /// If an error occurs during commit, the transaction is rolled back.
    /// </summary>
    public void Commit()
    {
        try
        {
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            transaction.Dispose();
            transaction = connection.BeginTransaction();
            ResetRepositories();
        }
    }

    /// <summary>
    /// Resets repositories to null.
    /// </summary>
    private void ResetRepositories()
    {
        gematriaMethodRepository = null;
        phraseRepository = null;
        operationRepository = null;
        settingRepository = null;
    }

    /// <summary>
    /// Releases the resources used by the <see cref="UnitOfWork"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="UnitOfWork"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    private void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                    transaction = null;
                }

                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }

            disposed = true;
        }
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    ~UnitOfWork()
    {
        Dispose(false);
    }

    private IGematriaMethodRepository gematriaMethodRepository;

    private IPhraseRepository phraseRepository;

    private IOperationRepository operationRepository;

    private ISettingRepository settingRepository;

    /// <summary>
    /// Gets the repository for Gematria methods.
    /// </summary>
    public IGematriaMethodRepository GematriaMethodRepository
    {
        get { return gematriaMethodRepository ??= new GematriaMethodRepository(transaction); }
    }

    /// <summary>
    /// Gets the repository for phrases.
    /// </summary>
    public IPhraseRepository PhraseRepository
    {
        get { return phraseRepository ??= new PhraseRepository(transaction); }
    }

    /// <summary>
    /// Gets the repository for operations.
    /// </summary>
    public IOperationRepository OperationRepository
    {
        get { return operationRepository ??= new OperationRepository(transaction); }
    }

    /// <summary>
    /// Gets the repository for settings.
    /// </summary>
    public ISettingRepository SettingRepository
    {
        get { return settingRepository ??= new SettingRepository(transaction); }
    }

    /// <summary>
    /// The database connection.
    /// </summary>
    private IDbConnection connection;

    /// <summary>
    /// The database transaction.
    /// </summary>
    private IDbTransaction transaction;

    /// <summary>
    /// Flag indicating whether the object has been disposed.
    /// </summary>
    private bool disposed;
}
