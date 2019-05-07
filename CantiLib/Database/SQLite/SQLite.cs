//
// Copyright (c) 2018 Canti, The TurtleCoin Developers
// 
// Please see the included LICENSE file for more information.

using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Canti.Database
{
    // DEBUG / INCOMPLETE
    // TODO - Async and maybe a queued worker?
    public class SQLite : IDatabase
    {
        // Database connection
        private SqliteConnection Database;

        // Entry point, loads or creates a database
        public SQLite(string FileName)
        {
            // Load database
            Database = new SqliteConnection($"Data Source={FileName}");
            Database.Open();
        }

        // Frees up resources and closes a database if needed
        public void Dispose()
        {
            if (Database == null) return;
            else Database.Close();
        }

        // Non query command
        public void ExecuteNonQuery(string Command, Dictionary<string, object> Variables)
        {
            // Split variable list
            string[] VariableList = Command.Split('(', ')');

            // Check if variables are needed
            if (VariableList.Length > 1)
            {
                // Split variable list further to get variable names
                VariableList = VariableList[1].Split(',');
                for (int i = 0; i < VariableList.Length; i++)
                    VariableList[i] = VariableList[i].Trim();

                // Check if variables given match needed variables
                if (VariableList.Length != Variables.Count)
                {
                    // TODO - Throw error
                    return;
                }

                // Add values to command string
                Command += " VALUES (";
                foreach (string Variable in VariableList)
                    Command += "@" + Variable + ", ";
                Command = Command.Substring(0, Command.Length - 2);
                Command += ")";
            }
            else VariableList = new string[0];

            // Create a formatted SQL command
            SqliteCommand Formatted = new SqliteCommand(Command, Database);

            // Loop through variable list and add as a parameter
            for (int i = 0; i < VariableList.Length; i++)
                Formatted.Parameters.AddWithValue(VariableList[i], Variables[VariableList[i]]);

            // Execute command
            Formatted.ExecuteNonQuery();
        }

        // Query command, returns a list of rows, each of which being represented as a dictionary of column names and values
        public List<Dictionary<string, object>> ExecuteQuery(string Command, Dictionary<string, object> Variables)
        {
            // Split variable list
            string[] VariableList = Command.Split('(', ')');

            // Check if variables are needed
            if (VariableList.Length > 1)
            {
                // Split variable list further to get variable names
                VariableList = VariableList[1].Split(',');
                for (int i = 0; i < VariableList.Length; i++)
                    VariableList[i] = VariableList[i].Trim();

                // Check if variables given match needed variables
                if (VariableList.Length != Variables.Count)
                {
                    // TODO - Throw error
                    return null;
                }

                // Add values to command string
                Command += " VALUES (";
                foreach (string Variable in VariableList)
                    Command += "@" + Variable + ", ";
                Command = Command.Substring(0, Command.Length - 2);
                Command += ")";
            }
            else VariableList = new string[0];

            // Create a formatted SQL command
            SqliteCommand Formatted = new SqliteCommand(Command, Database);

            // Loop through variable list and add as a parameter
            for (int i = 0; i < VariableList.Length; i++)
                Formatted.Parameters.AddWithValue(VariableList[i], Variables[VariableList[i]]);

            // Execute command
            List<Dictionary<string, object>> Output = new List<Dictionary<string, object>>();
            using (SqliteDataReader Reader = Formatted.ExecuteReader())
                while (Reader.Read())
                {
                    // Create a dictionary holding row values
                    Dictionary<string, object> Row = new Dictionary<string, object>();

                    // Loop through column values
                    for (int i = 0; i < Reader.FieldCount; i++)
                        Row.Add(Reader.GetName(i), Reader.GetValue(i));

                    // Add row to output
                    Output.Add(Row);
                }

            // Return result
            return Output;
        }
    }
}
