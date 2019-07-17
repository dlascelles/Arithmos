/*
* Copyright (c) 2018 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;

namespace ArithmosModels
{
    /// <summary>
    /// An Operation is used to group multiple Phrases together. It allows to retrieve all of the associated phrases, to remember where they came from or to delete them all at once.
    /// </summary>
    public class Operation : ModelBase
    {
        private int id;
        public int Id
        {
            get { return this.id; }
            set { this.SetField(ref this.id, value); }
        }

        private string description;
        public string Description
        {
            get { return this.description; }
            set { this.SetField(ref this.description, value); }
        }

        private DateTime entryDate = DateTime.Now;
        public DateTime EntryDate
        {
            get { return this.entryDate; }
            set { this.SetField(ref this.entryDate, value); }
        }
    }
}