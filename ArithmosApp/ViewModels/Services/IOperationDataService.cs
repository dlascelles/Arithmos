/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels.Services;

/// <inheritdoc cref="ArithmosDataAccess.OperationRepository"/>
public interface IOperationDataService
{
    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.Create"/>
    int Create(Operation operation);

    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.RetrieveAll"/>
    IEnumerable<Operation> RetrieveAll();

    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.DeleteAsync(int)"/>
    Task<int> DeleteAsync(int id);

    /// <inheritdoc cref="ArithmosDataAccess.OperationRepository.DeleteAsync(List{int})"/>
    Task<int> DeleteAsync(List<int> ids);
}