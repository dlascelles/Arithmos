/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDAL;
using ArithmosModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArithmosViewModels.Services
{
    public class PhraseDataService : IPhraseDataService
    {
        public async Task<List<Phrase>> RetrieveAsync(List<int> values, CalculationMethod calculationMethod, Alphabet alphabet)
        {
            return await Task.Run(async () => { return await new PhraseDataAccess().RetrieveAsync(values, calculationMethod, alphabet); });
        }

        public async Task<List<Phrase>> RetrieveAsync(List<Operation> operations)
        {
            return await Task.Run(async () => { return await new PhraseDataAccess().RetrieveAsync(operations); });
        }

        public async Task<List<Phrase>> RetrieveOrphansAsync()
        {
            return await Task.Run(async () => { return await new PhraseDataAccess().RetrieveOrphansAsync(); });
        }

        public async Task<int> CreateAsync(Phrase phrase)
        {
            return await Task.Run(async () => { return await new PhraseDataAccess().CreateAsync(phrase); });
        }

        public async Task<int> CreateAsync(List<Phrase> phrases, CancellationToken cts)
        {
            return await Task.Run(async () => { return await new PhraseDataAccess().CreateAsync(phrases, cts); });
        }

        public async Task<int> CreateAsync(List<Phrase> phrases, Operation operation, CancellationToken cts)
        {
            return await Task.Run(async () => { return await new PhraseDataAccess().CreateAsync(phrases, operation, cts); });
        }

        public async Task<int> DeleteAsync(List<Phrase> phrases)
        {
            return await Task.Run(async () => { return await new PhraseDataAccess().DeleteAsync(phrases); });
        }
    }
}