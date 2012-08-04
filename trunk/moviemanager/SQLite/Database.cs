using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;

namespace SQLite
{
    public class Database
    {
        //private const String ConnString = @"data source='C:\ProgramData\MovieManager\Database\moviemanager.sqlite'";
        private static SQLiteConnection _conn;

        public static void Init(string connectionString)
        {
            _conn = GetConnection(connectionString);
        }

        public static void ExecuteSQL(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand Cmd = GetCommand(conn, transaction, sql, @params);
            Cmd.ExecuteNonQuery();
        }

        public static void ExecuteSQL(string sql)
        {
            SQLiteCommand Cmd = GetCommand(_conn, sql);
            Cmd.ExecuteNonQuery();
        }

        public static void ExecuteSQL(string connectionString, string sql, params SQLiteParameter[] @params)
        {
            ExecuteSQL(GetConnection(connectionString), null, sql, @params);
        }

        public static void CreateDatabaseFile(string pathToDatabase)
        {
            SQLiteConnection.CreateFile(pathToDatabase);
        }

        public static SQLiteConnection GetConnection(string connectionString)
        {
            if (_conn == null)
            {
                _conn = new SQLiteConnection(connectionString);
                _conn.Open();
            }
            return _conn;
        }

        public static SQLiteConnection GetConnection()
        {
            return _conn;
        }

        public static SQLiteCommand GetCommand(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand Cmd = new SQLiteCommand(sql) { Transaction = transaction, Connection = conn };
            foreach (SQLiteParameter Param in @params)
            {
                Cmd.Parameters.Add(Param);
            }
            return Cmd;
        }

        public static SQLiteCommand GetCommand(SQLiteConnection conn, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand Cmd = new SQLiteCommand(sql) { Connection = conn };
            foreach (SQLiteParameter Param in @params)
            {
                Cmd.Parameters.Add(Param);
            }
            return Cmd;
        }

        public static SQLiteCommand GetCommand(SQLiteConnection conn, string sql)
        {
            return new SQLiteCommand(sql) { Connection = conn };
        }

        //public static SQLiteCommand GetCommand(string connectionString, string sql, params SQLiteParameter[] @params)
        //{
        //    return GetCommand(GetConnection(connectionString), null, sql, @params);
        //}

        public static SQLiteDataReader GetReader(string sql, params SQLiteParameter[] @params)
        {
            return GetReader(_conn, null, sql, @params);
        }

        public static SQLiteDataReader GetReader(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand Cmd = GetCommand(conn, transaction, sql, @params);
            return Cmd.ExecuteReader();
        }

        public static SQLiteDataAdapter GetAdapter(string sql, params SQLiteParameter[] @params)
        {
            return GetAdapter(null, sql, @params);
        }

        public static SQLiteDataAdapter GetAdapter(SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteDataAdapter Adapter = null;
            try
            {
                SQLiteCommand Cmd = GetCommand(_conn, transaction, sql, @params);
                Adapter = new SQLiteDataAdapter(Cmd);
            }
            catch (Exception E)
            {
                Console.WriteLine("Exception in Database: " + E.Message);
            }
            return Adapter;
        }

        public static bool FillDataset(DsVideos dataSet, Dictionary<String, String> tableNames)
        {
            return tableNames.Aggregate(true, (current, pair) => current & FillDataset(dataSet, pair.Key, pair.Value));
        }

        public static bool FillDataset(DsVideos dataSet, string tableName, string query)
        {
            bool Retval = true;
            try
            {
                SQLiteDataAdapter Adap = GetAdapter(query);
                Adap.Fill(dataSet, tableName);
            }
            catch
            {
                Retval = false;
            }

            return Retval;
        }

        public static void UpdateDatabase(DsVideos dataset)
        {
            throw new Exception("not yet implemented (database.cs: updateDatabase)");
        }
    }
}