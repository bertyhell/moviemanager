using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Globalization;
using System.Data.Common;
using System.Data.SQLite;

namespace SQLite
{

    public class Database
    {
        private static String _connString = "data source=\"M:\\projects\\Open Source\\MovieManagerCSharp\\Settings\\moviemanager.sqlite\"";
        private static SQLiteConnection _conn = null;

        private static Dictionary<string, SQLiteConnection> _connections = new Dictionary<string, SQLiteConnection>();

        public static void ExecuteSQL(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand cmd = GetCommand(conn, transaction, sql, @params);
        }

        public static void ExecuteSQL(string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand cmd = GetCommand(sql, @params);
            cmd.ExecuteNonQuery();
        }

        public static SQLiteConnection GetConnection()
        {
            if (_conn == null)
            {
                _conn = new SQLiteConnection(_connString);
                _conn.Open();
            }
            return _conn;
        }

        public static SQLiteCommand GetCommand(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql);
            cmd.Transaction = transaction;
            cmd.Connection = conn;
            foreach (SQLiteParameter param in @params)
            {
                cmd.Parameters.Add(param);
            }
            return cmd;
        }

        public static SQLiteCommand GetCommand(string sql, params SQLiteParameter[] @params)
        {
            SQLiteConnection conn = GetConnection();
            SQLiteCommand cmd = GetCommand(GetConnection(), null, sql, @params);
            return cmd;
        }

        public static SQLiteDataReader GetReader(string sql, params SQLiteParameter[] @params)
        {
            return GetReader(GetConnection(), null, sql, @params);
        }

        public static SQLiteDataReader GetReader(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand cmd = GetCommand(conn, transaction, sql, @params);
            string commandtext = cmd.CommandText;
            return cmd.ExecuteReader();
        }

        public static SQLiteDataAdapter GetAdapter(string sql, params SQLiteParameter[] @params)
        {
            return GetAdapter(GetConnection(), null, sql, @params);
        }

        public static SQLiteDataAdapter GetAdapter(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteDataAdapter adapter=null;
            try
            {
                SQLiteCommand cmd = GetCommand(conn, transaction, sql, @params);
                string commandtext = cmd.CommandText;
                adapter = new SQLiteDataAdapter(cmd);
            }
            catch { }
            return adapter;
        }

        public static bool FillDataset( DsVideos dataSet, Dictionary<String,String> tableNames)
        {
            bool RetVal = true;
            foreach (KeyValuePair<String,String> pair in tableNames)
            {
                RetVal &= FillDataset(dataSet, pair.Key, pair.Value);
            }
            return RetVal;
        }

        public static bool FillDataset(DsVideos dataSet, string tableName, string query)
        {
            bool Retval = true;
            try
            {
                SQLiteDataAdapter adap = Database.GetAdapter(query);
                adap.Fill(dataSet, tableName);
            }
            catch
            {
                Retval = false;
            }

            return Retval;
        }

        public static void UpdateDatabase(DsVideos dataset)
        {

        }
    }
}