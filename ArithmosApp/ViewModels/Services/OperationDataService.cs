/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDataAccess;
using ArithmosModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels.Services;

/// <summary>
/// Represents a service for managing Operations data.
/// </summary>
public class OperationDataService : IOperationDataService
{
    /// <inheritdoc />
    public int Create(Operation operation)
    {
        using (UnitOfWork unitOfWork = new())
        {
            return unitOfWork.OperationRepository.Create(operation);
        }
    }

    /// <inheritdoc />
    public IEnumerable<Operation> RetrieveAll()
    {
        using (UnitOfWork unitOfWork = new())
        {
            return unitOfWork.OperationRepository.RetrieveAll();
        }
    }

    /// <inheritdoc />
    public async Task<int> DeleteAsync(int id)
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = await unitOfWork.OperationRepository.DeleteAsync(id);
            unitOfWork.Commit();
            return result;
        }
    }

    /// <inheritdoc />
    public async Task<int> DeleteAsync(List<int> ids)
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = await unitOfWork.OperationRepository.DeleteAsync(ids);
            unitOfWork.Commit();
            return result;
        }
    }
}
