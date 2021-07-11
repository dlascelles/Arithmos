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
    public class Operation
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime EntryDate { get; set; }

    }
}