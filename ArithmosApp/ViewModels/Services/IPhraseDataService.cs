/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosApp.ViewModels.Services;

/// <inheritdoc cref="ArithmosDataAccess.PhraseRepository"/>
public interface IPhraseDataService
{
    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.CreateAsync"/>
    Task<int> CreateAsync(List<Phrase> phrases, CancellationToken cts, Operation operation = null);

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.Create"/>
    bool Create(Phrase phrase, int? operationId);

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.RetrieveAllAsync"/>
    Task<List<Phrase>> RetrieveAllAsync();

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.RetrieveAllOrphansAsync"/>
    Task<List<Phrase>> RetrieveAllOrphansAsync();

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.RetrieveByOperationAsync"/>
    Task<List<Phrase>> RetrieveByOperationAsync(int operationId);

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.SearchForSimilarPhrasesAsync"/>
    Task<List<Phrase>> SearchForSimilarPhrasesAsync(Phrase phrase, int[] gematriaIds = null);

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.SearchByValuesAsync"/>
    Task<List<Phrase>> SearchByValuesAsync(List<GematriaMethod> methods, List<int> values);

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.SearchByTextAsync"/>
    Task<List<Phrase>> SearchByTextAsync(string text);

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.RecalculatePhrasesAsync"/>
    Task<int> RecreatePhrasesAsync();

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.DeleteAsync(long)"/>
    Task<long> DeleteAsync(long id);

    /// <inheritdoc cref="ArithmosDataAccess.PhraseRepository.DeleteAsync(List{long})"/>
    Task<long> DeleteAsync(List<long> ids);
}
