/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
namespace ArithmosDataAccess.Interfaces;

/// <inheritdoc cref="ArithmosDataAccess.UnitOfWork"/>
public interface IUnitOfWork : IDisposable
{
    /// <inheritdoc cref="ArithmosDataAccess.UnitOfWork.Commit"/>
    void Commit();
}
