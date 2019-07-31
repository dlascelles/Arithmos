/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosViewModels.Services
{
    public interface IPhraseDataService
    {
        Task<int> CreateAsync(Phrase phrase);
        Task<int> CreateAsync(List<Phrase> phrases, CancellationToken cts);
        Task<int> CreateAsync(List<Phrase> phrases, Operation operation, CancellationToken cts);
        Task<List<Phrase>> RetrieveAsync(List<int> values, CalculationMethod calculationMethod, Alphabet alphabet);
        Task<List<Phrase>> RetrieveAsync(List<Operation> operations);
        Task<List<Phrase>> RetrieveOrphansAsync();
        Task<int> DeleteAsync(List<Phrase> phrases);
    }
}