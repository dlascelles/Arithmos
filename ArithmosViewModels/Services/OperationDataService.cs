/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosDAL;
using ArithmosModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArithmosViewModels.Services
{
    public class OperationDataService : IOperationDataService
    {
        public async Task<List<Operation>> RetrieveAllAsync()
        {
            return await Task.Run(async () => { return await new OperationDataAccess().RetrtieveAllAsync(); });
        }

        public async Task<int> DeleteAsync(List<Operation> operations)
        {
            return await Task.Run(async () => { return await new OperationDataAccess().DeleteAsync(operations); });
        }
    }
}