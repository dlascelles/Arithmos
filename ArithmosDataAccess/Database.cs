/*
* Copyright (c) 2018 - 2024 Daniel Lascelles, https://github.com/dlascelles
* This code is licensed under The MIT License. See LICENSE file in the project root for full license information.
* License URL: https://github.com/dlascelles/Arithmos/blob/master/LICENSE
*/
using ArithmosModels;
using System.Data;
using System.Data.SQLite;

namespace ArithmosDataAccess;

/// <summary>
/// Provides functionality for database operations using SQLite.
/// </summary>
public static class Database
{
    /// <summary>
    /// Static constructor to initialize the <see cref="MainPath"/> and <see cref="ConnectionString"/> properties.
    /// </summary>
    static Database()
    {
        MainPath = Path.Combine(Environment.CurrentDirectory);
        ConnectionString = $@"Data Source={Path.Combine(MainPath, "ArithmosDatabase.sqlite")};Version=3;foreign keys = 1;locking mode = exclusive";
    }

    /// <summary>
    /// Gets a new instance of <see cref="IDbConnection"/> for the SQLite database.
    /// </summary>
    /// <returns>A new instance of <see cref="IDbConnection"/>.</returns>
    public static IDbConnection GetConnection()
    {
        return new SQLiteConnection(ConnectionString);
    }

    /// <summary>
    /// Creates the SQLite database with necessary tables, indexes, and initial data.
    /// </summary>
    public static void CreateDatabase()
    {
        if (File.Exists(Path.Combine(MainPath, "ArithmosDatabase.sqlite"))) return;

        using (SQLiteConnection con = new(ConnectionString))
        {
            string query = $@"
BEGIN TRANSACTION;
CREATE TABLE GematriaMethod (Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Name TEXT UNIQUE NOT NULL, Sort INTEGER NOT NULL, CipherBody TEXT NOT NULL, AddsTotalNumberOfCharacters BOOLEAN NOT NULL DEFAULT(0), AddsTotalNumberOfWords BOOLEAN NOT NULL DEFAULT(0));
CREATE TABLE Operation (Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Name UNIQUE NOT NULL, EntryDate DATETIME NOT NULL);
CREATE TABLE Phrase (Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Content TEXT UNIQUE NOT NULL, OperationId INTEGER REFERENCES Operation (Id) ON DELETE CASCADE ON UPDATE CASCADE, Alphabet INTEGER NOT NULL);
CREATE INDEX Index_Phrase_Content ON Phrase(Content);
CREATE INDEX Index_Phrase_Alphabet ON Phrase(Alphabet);
CREATE INDEX Index_Phrase_OperationId ON Phrase(OperationId);
CREATE TABLE PhraseGematriaMethod (Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, PhraseId INTEGER REFERENCES Phrase (Id) ON DELETE CASCADE ON UPDATE CASCADE, GematriaMethodId INTEGER REFERENCES GematriaMethod (Id) ON DELETE CASCADE ON UPDATE CASCADE, Value INTEGER NOT NULL);
CREATE INDEX Index_PhraseGematriaMethod_PhraseId ON PhraseGematriaMethod(PhraseId);
CREATE INDEX Index_PhraseGematriaMethod_GematriaMethodId ON PhraseGematriaMethod(GematriaMethodId);
CREATE INDEX Index_PhraseGematriaMethod_Value ON PhraseGematriaMethod(Value);
CREATE TABLE Setting (Id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE NOT NULL, Key TEXT UNIQUE NOT NULL, Value TEXT NOT NULL);
INSERT INTO GematriaMethod (Name, Sort, CipherBody) VALUES ('Standard', 1, '{Constants.Ciphers.EnglishStandard},{Constants.Ciphers.GreekStandard},{Constants.Ciphers.HebrewStandard}');
INSERT INTO GematriaMethod (Name, Sort, CipherBody) VALUES ('Ordinal', 2, '{Constants.Ciphers.EnglishOrdinal},{Constants.Ciphers.GreekOrdinal},{Constants.Ciphers.HebrewOrdinal}');
INSERT INTO GematriaMethod (Name, Sort, CipherBody) VALUES ('Reduced', 3, '{Constants.Ciphers.EnglishReduced},{Constants.Ciphers.GreekReduced},{Constants.Ciphers.HebrewReduced}');
INSERT INTO GematriaMethod (Name, Sort, CipherBody) VALUES ('Sumerian', 4, '{Constants.Ciphers.EnglishSumerian}');
INSERT INTO Setting (Key, Value) VALUES ('{Constants.Settings.Theme}', '{Constants.Settings.ThemeDark}');
INSERT INTO Setting (Key, Value) VALUES ('{Constants.Settings.ShowColumnAlphabet}', '{Constants.Settings.True}');
INSERT INTO Setting (Key, Value) VALUES ('{Constants.Settings.ShowColumnOperationId}', '{Constants.Settings.True}');
COMMIT TRANSACTION;";
            using (SQLiteCommand cmd = new(query, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Optimizes the SQLite database by performing a VACUUM operation.
    /// </summary>
    /// <returns>True if the optimization is successful; otherwise, false.</returns>
    public static bool OptimizeDatabase()
    {
        if (!File.Exists(Path.Combine(MainPath, "ArithmosDatabase.sqlite"))) return false;

        using (SQLiteConnection con = new(ConnectionString))
        {
            string optimize = "VACUUM;";
            using (SQLiteCommand cmd = new(optimize, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        return true;
    }

    /// <summary>
    /// Gets the connection string for the SQLite database.
    /// </summary>
    public static string ConnectionString { get; }

    /// <summary>
    /// Gets the main path where the SQLite database is stored.
    /// </summary>
    public static string MainPath { get; }
}
