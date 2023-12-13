/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosDataAccess.Interfaces;

/// <inheritdoc cref="ArithmosDataAccess.GematriaMethodRepository"/>
public interface IGematriaMethodRepository
{
    /// <inheritdoc cref="ArithmosDataAccess.GematriaMethodRepository.Create"/>
    int Create(GematriaMethod gematriaMethod);

    /// <inheritdoc cref="ArithmosDataAccess.GematriaMethodRepository.RetrieveAll"/>
    IEnumerable<GematriaMethod> RetrieveAll();

    /// <inheritdoc cref="ArithmosDataAccess.GematriaMethodRepository.Retrieve"/>
    GematriaMethod Retrieve(int id);

    /// <inheritdoc cref="ArithmosDataAccess.GematriaMethodRepository.Update"/>
    int Update(GematriaMethod gematriaMethod);

    /// <inheritdoc cref="ArithmosDataAccess.GematriaMethodRepository.Delete"/>
    int Delete(int id);

    /// <inheritdoc cref="ArithmosDataAccess.GematriaMethodRepository.IsNameDuplicate"/>
    bool IsNameDuplicate(string methodName, int id = 0);
}