using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace SQLite
{

    public class Database
    {
        private const String ConnString = "data source=\"Settings\\moviemanager.sqlite\"";
        //private const String CONN_STRING = "data source=\"M:\\projects\\Open Source\\MovieManagerCSharp\\Settings\\moviemanager.sqlite\"";
        private static SQLiteConnection _conn;

        public static void ExecuteSQL(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand cmd = GetCommand(conn, transaction, sql, @params);
            cmd.ExecuteNonQuery();
        }

        public static void ExecuteSQL(string sql, params SQLiteParameter[] @params)
        {
            ExecuteSQL(GetConnection(), null, sql, @params);
        }

        public static SQLiteConnection GetConnection()
        {
            if (_conn == null)
            {
                _conn = new SQLiteConnection(ConnString);
                _conn.Open();
            }
            return _conn;
        }

        public static SQLiteCommand GetCommand(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql) {Transaction = transaction, Connection = conn};
            foreach (SQLiteParameter param in @params)
            {
                cmd.Parameters.Add(param);
            }
            return cmd;
        }

        public static SQLiteCommand GetCommand(string sql, params SQLiteParameter[] @params)
        {
            return GetCommand(GetConnection(), null, sql, @params);
        }

        public static SQLiteDataReader GetReader(string sql, params SQLiteParameter[] @params)
        {
            return GetReader(GetConnection(), null, sql, @params);
        }

        public static SQLiteDataReader GetReader(SQLiteConnection conn, SQLiteTransaction transaction, string sql, params SQLiteParameter[] @params)
        {
            SQLiteCommand cmd = GetCommand(conn, transaction, sql, @params);
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
                adapter = new SQLiteDataAdapter(cmd);
            }
            catch
            { }
            return adapter;
        }

        public static bool FillDataset( DsVideos dataSet, Dictionary<String,String> tableNames)
        {
            bool retVal = true;
            foreach (KeyValuePair<String,String> pair in tableNames)
            {
                retVal &= FillDataset(dataSet, pair.Key, pair.Value);
            }
            return retVal;
        }

        public static bool FillDataset(DsVideos dataSet, string tableName, string query)
        {
            bool retval = true;
            try
            {
                SQLiteDataAdapter adap = GetAdapter(query);
                adap.Fill(dataSet, tableName);
            }
            catch
            {
                retval = false;
            }

            return retval;
        }

        public static void UpdateDatabase(DsVideos dataset)
        {

        }
    }
}