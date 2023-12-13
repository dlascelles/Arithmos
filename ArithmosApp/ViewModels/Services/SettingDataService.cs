/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDataAccess;

namespace ArithmosApp.ViewModels.Services;

/// <summary>
/// Represents a service for managing Setting data.
/// </summary>
public class SettingDataService : ISettingDataService
{
    /// <inheritdoc />
    public string Retrieve(string key)
    {
        using (UnitOfWork unitOfWork = new())
        {
            return unitOfWork.SettingRepository.Retrieve(key);
        }
    }

    /// <inheritdoc />
    public int Upsert(string key, string newValue)
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = unitOfWork.SettingRepository.Upsert(key, newValue);
            unitOfWork.Commit();
            return result;
        }
    }
}
