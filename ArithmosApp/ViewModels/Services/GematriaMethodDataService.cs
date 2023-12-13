/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDataAccess;
using ArithmosModels;
using System.Collections.Generic;

namespace ArithmosApp.ViewModels.Services;

/// <summary>
/// Represents a service for managing Gematria methods data.
/// </summary>
public class GematriaMethodDataService : IGematriaMethodDataService
{
    /// <inheritdoc />
    public int Create(GematriaMethod gematriaMethod)
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = unitOfWork.GematriaMethodRepository.Create(gematriaMethod);
            unitOfWork.Commit();
            return result;
        }
    }

    /// <inheritdoc />
    public GematriaMethod Retrieve(int id)
    {
        using (UnitOfWork unitOfWork = new())
        {
            return unitOfWork.GematriaMethodRepository.Retrieve(id);
        }
    }

    /// <inheritdoc />
    public IEnumerable<GematriaMethod> RetrieveAll()
    {
        using (UnitOfWork unitOfWork = new())
        {
            return unitOfWork.GematriaMethodRepository.RetrieveAll();
        }
    }

    /// <inheritdoc />
    public int Update(GematriaMethod gematriaMethod)
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = unitOfWork.GematriaMethodRepository.Update(gematriaMethod);
            unitOfWork.Commit();
            return result;
        }
    }

    /// <inheritdoc />
    public int Delete(int id)
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = unitOfWork.GematriaMethodRepository.Delete(id);
            unitOfWork.Commit();
            return result;
        }
    }
}