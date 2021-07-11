/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;

namespace ArithmosViewModels
{
    public class OperationViewModel : ObservableObject
    {
        private readonly Operation operation;

        public OperationViewModel(Operation operation)
        {
            this.operation = operation;
        }

        public string Description
        {
            get => operation.Description;
            set => SetProperty(operation.Description, value, operation, (u, n) => u.Description = n);
        }

        public DateTime EntryDate
        {
            get => operation.EntryDate;
            set => SetProperty(operation.EntryDate, value, operation, (u, n) => u.EntryDate = n);
        }

        public int Id
        {
            get => operation.Id;
            set => SetProperty(operation.Id, value, operation, (u, n) => u.Id = n);
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get => this.isSelected;
            set => SetProperty(ref this.isSelected, value);
        }

        public Operation Operation
        {
            get => this.operation;
        }
    }
}