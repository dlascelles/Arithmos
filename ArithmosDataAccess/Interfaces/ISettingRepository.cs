/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
namespace ArithmosDataAccess.Interfaces;

/// <inheritdoc cref="ArithmosDataAccess.SettingRepository"/>
public interface ISettingRepository
{
    /// <inheritdoc cref="ArithmosDataAccess.SettingRepository.Retrieve"/>
    string Retrieve(string key);

    /// <inheritdoc cref="ArithmosDataAccess.SettingRepository.Upsert"/>
    int Upsert(string key, string newValue);
}