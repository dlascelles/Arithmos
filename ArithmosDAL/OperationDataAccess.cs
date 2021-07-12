/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace ArithmosDAL
{
    public class OperationDataAccess
    {
        /// <summary>
        /// Creates an operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="con"></param>
        /// <returns>Integer</returns>
        public async Task<int> CreateAsync(Operation operation, SQLiteConnection con)
        {
            int result = 0;

            if (operation != null)
            {
                using (SQLiteCommand cmd = new($"INSERT INTO Operation (Description, EntryDate) VALUES (@Description, @EntryDate)", con))
                {
                    cmd.Parameters.AddWithValue("@Description", operation.Description);
                    cmd.Parameters.AddWithValue("@EntryDate", DateTime.Now);
                    result = await cmd.ExecuteNonQueryAsync();
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves all saved operations
        /// </summary>
        /// <returns>A List of operations</returns>
        public async Task<List<Operation>> RetrtieveAllAsync()
        {
            List<Operation> operations = new();
            using (SQLiteConnection con = new(Database.ConnectionString))
            {
                using (SQLiteCommand cmd = new("SELECT Id, Description, EntryDate FROM Operation", con))
                {
                    await con.OpenAsync();
                    using (var dare = await cmd.ExecuteReaderAsync())
                    {
                        while (await dare.ReadAsync())
                        {
                            Operation operation = new()
                            {
                                Id = Convert.ToInt32(dare["Id"]),
                                Description = dare["Description"].ToString(),
                                EntryDate = Convert.ToDateTime(dare["EntryDate"]),
                            };
                            operations.Add(operation);
                        }
                    }
                }
            }
            return operations;
        }

        /// <summary>
        /// Deletes multiple operations
        /// </summary>
        /// <param name="operations">A list of operations</param>
        public async Task<int> DeleteAsync(List<Operation> operations)
        {
            int result = 0;

            if (operations != null && operations.Count > 0)
            {
                using (SQLiteConnection con = new(Database.ConnectionString))
                {
                    string query = $"DELETE FROM Operation WHERE Id IN ({SQLOperationsString(operations)})";

                    await con.OpenAsync();
                    using (SQLiteCommand cmd = new(query, con))
                    {
                        result = await cmd.ExecuteNonQueryAsync();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the ids from a list of operations and creates an appropriate SQL IN statement
        /// </summary>
        /// <param name="operations">A list of operations</param>
        /// <returns>A correctly formatted SQL IN statement</returns>
        internal static string SQLOperationsString(List<Operation> operations)
        {
            StringBuilder sb = new();

            if (operations != null && operations.Count > 0)
            {
                foreach (Operation operation in operations)
                {
                    sb.Append(operation.Id);
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}