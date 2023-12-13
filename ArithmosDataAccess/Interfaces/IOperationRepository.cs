/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosDataAccess.Interfaces;

/// <inheritdoc cref="ArithmosDataAccess.OperationRepository"/>
public interface IOperationRepository
{
    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.Create"/>
    int Create(Operation operation);

    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.RetrieveAll"/>
    IEnumerable<Operation> RetrieveAll();

    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.DeleteAsync"/>
    Task<int> DeleteAsync(int id);

    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.DeleteAsync"/>
    Task<int> DeleteAsync(List<int> ids);

    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.IsNameDuplicate"/>
    bool IsNameDuplicate(string operationName, int id = 0);
}