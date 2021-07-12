/*
* Copyright (c) 2018 - 2021 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using System;
using System.IO;

namespace ArithmosDAL
{
    public static class Database
    {
        static Database()
        {
            string mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.None), "ArithmosData");
            ConnectionString = $@"Data Source={mainPath}\arithmosdb.sqlite;Version=3;foreign keys = 1;locking mode = exclusive";
        }
        public static string ConnectionString { get; set; }
    }
}