/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDataAccess;
using ArithmosModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels.Services;

/// <summary>
/// Represents a service for managing Phrases data.
/// </summary>
public class PhraseDataService : IPhraseDataService
{
    /// <inheritdoc />
    public async Task<int> CreateAsync(List<Phrase> phrases, CancellationToken cts, Operation operation = null)
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = await unitOfWork.PhraseRepository.CreateAsync(phrases, cts, operation);
            unitOfWork.Commit();
            return result;
        }
    }

    /// <inheritdoc />
    public bool Create(Phrase phrase, int? operationId)
    {
        using (UnitOfWork unitOfWork = new())
        {
            bool result = unitOfWork.PhraseRepository.Create(phrase, operationId);
            unitOfWork.Commit();
            return result;
        }
    }

    /// <inheritdoc />
    public async Task<List<Phrase>> RetrieveAllAsync()
    {
        using (UnitOfWork unitOfWork = new())
        {
            return await unitOfWork.PhraseRepository.RetrieveAllAsync();
        }
    }

    /// <inheritdoc />
    public async Task<List<Phrase>> RetrieveAllOrphansAsync()
    {
        using (UnitOfWork unitOfWork = new())
        {
            return await unitOfWork.PhraseRepository.RetrieveAllOrphansAsync();
        }
    }

    /// <inheritdoc />
    public async Task<List<Phrase>> RetrieveByOperationAsync(int operationId)
    {
        using (UnitOfWork unitOfWork = new())
        {
            return await unitOfWork.PhraseRepository.RetrieveByOperationAsync(operationId);
        }
    }

    /// <inheritdoc />
    public async Task<List<Phrase>> SearchForSimilarPhrasesAsync(Phrase phrase, int[] gematriaIds = null)
    {
        using (UnitOfWork unitOfWork = new())
        {
            return await unitOfWork.PhraseRepository.SearchForSimilarPhrasesAsync(phrase, gematriaIds);
        }
    }

    /// <inheritdoc />
    public async Task<List<Phrase>> SearchByValuesAsync(List<GematriaMethod> methods, List<int> values)
    {
        using (UnitOfWork unitOfWork = new())
        {
            return await unitOfWork.PhraseRepository.SearchByValuesAsync(methods, values);
        }
    }

    /// <inheritdoc />
    public async Task<List<Phrase>> SearchByTextAsync(string text)
    {
        using (UnitOfWork unitOfWork = new())
        {
            return await unitOfWork.PhraseRepository.SearchByTextAsync(text);
        }
    }

    /// <inheritdoc />
    public async Task<int> RecreatePhrasesAsync()
    {
        using (UnitOfWork unitOfWork = new())
        {
            int result = await unitOfWork.PhraseRepository.RecalculatePhrasesAsync();
            unitOfWork.Commit();
            return result;
        }
    }

    /// <inheritdoc />
    public async Task<long> DeleteAsync(long id)
    {
        using (UnitOfWork unitOfWork = new())
        {
            long result = await unitOfWork.PhraseRepository.DeleteAsync(id);
            unitOfWork.Commit();
            return result;
        }
    }

    /// <inheritdoc />
    public async Task<long> DeleteAsync(List<long> ids)
    {
        using (UnitOfWork unitOfWork = new())
        {
            long result = await unitOfWork.PhraseRepository.DeleteAsync(ids);
            unitOfWork.Commit();
            return result;
        }
    }
}
