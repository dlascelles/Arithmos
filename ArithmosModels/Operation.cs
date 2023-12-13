/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
namespace ArithmosModels;

/// <summary>
/// Represents an operation under which several phrases can be grouped together
/// </summary>
public class Operation
{
    /// <summary>
    /// Gets or sets the unique identifier for the operation.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the operation.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the entry date of the operation.
    /// </summary>
    public DateTime EntryDate { get; set; }

    /// <summary>
    /// Returns a string that represents name of the operation.
    /// </summary>   
    public override string ToString()
    {
        return Name;
    }
}
