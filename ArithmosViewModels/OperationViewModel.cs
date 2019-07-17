/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;

namespace ArithmosViewModels
{
    public class OperationViewModel : ModelBase
    {        
        public OperationViewModel(Operation operation)
        {
            this.Operation = operation;
        }       
       
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.SetField(ref this.isSelected, value); }
        }

        private Operation operation;
        public Operation Operation
        {
            get { return this.operation; }
            set { this.SetField(ref this.operation, value); }
        }        
    }
}