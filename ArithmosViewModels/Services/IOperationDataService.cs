/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArithmosViewModels.Services
{
    public interface IOperationDataService
    {       
        Task<List<Operation>> RetrieveAllAsync();
        Task<int> DeleteAsync(List<Operation> operations);
    }
}