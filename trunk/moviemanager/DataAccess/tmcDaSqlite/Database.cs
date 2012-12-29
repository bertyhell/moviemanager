using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlServerCe;

namespace Tmc.DataAccess.Sqlite
{
    public class Database
    {
        //private const String ConnString = @"data source='C:\ProgramData\MovieManager\Database\moviemanager.sqlite'";
        //private static SqlCeConnection _conn;

        //public static SqlCeConnection Init(string pathToDatabase)
        //{
        //    _conn = GetConnection(string.Format("Data Source = {0};", pathToDatabase));
        //    return _conn;
        //}

        //public static void ExecuteSql(SqlCeConnection conn, SqlCeTransaction transaction, string sql, params SqlCeParameter[] @params)
        //{
        //    SqlCeCommand Cmd = GetCommand(conn, transaction, sql, @params);
        //    Cmd.ExecuteNonQuery();
        //}

        //public static void ExecuteSql(string connectionString, string sql, params SqlCeParameter[] @params)
        //{
        //    ExecuteSql(GetConnection(connectionString), null, sql, @params);
        //}

        //public static void ExecuteSql(string sql, params SqlCeParameter[] @params)
        //{
        //    SqlCeCommand Cmd = GetCommand(_conn, null, sql, @params);

        //    Cmd.ExecuteNonQuery();
        //}

        //public static void CreateDatabaseFile(string pathToDatabase)
        //{
        //    throw new NotImplementedException();
        //    //SqlCeConnection.CreateFile(pathToDatabase);
        //}

        //public static SqlCeConnection GetConnection(string connectionString)
        //{
        //    if (_conn == null)
        //    {
        //        _conn = new SqlCeConnection(connectionString);
        //        _conn.Open();
        //    }
        //    return _conn;
        //}

        //public static SqlCeConnection GetConnection()
        //{
        //    return _conn;
        //}

        //public static SqlCeCommand GetCommand(SqlCeConnection conn, SqlCeTransaction transaction, string sql, params SqlCeParameter[] @params)
        //{
        //    SqlCeCommand Cmd = new SqlCeCommand(sql) { Transaction = transaction, Connection = conn };
        //    foreach (SqlCeParameter Param in @params)
        //    {
        //        Cmd.Parameters.Add(Param);
        //    }
        //    return Cmd;
        //}


        //public static SqlCeCommand GetCommand(SqlCeConnection conn, string sql, params SqlCeParameter[] @params)
        //{
        //    if (@params == null) return new SqlCeCommand(sql) { Connection = conn };
        //    SqlCeCommand Cmd = new SqlCeCommand(sql) { Connection = conn };
        //    foreach (SqlCeParameter Param in @params)
        //    {
        //        Cmd.Parameters.Add(Param);
        //    }
        //    return Cmd;
        //}

        //public static SqlCeDataReader GetReader(string sql, params SqlCeParameter[] @params)
        //{
        //    return GetReader(_conn, null, sql, @params);
        //}

        //public static SqlCeDataReader GetReader(SqlCeConnection conn, SqlCeTransaction transaction, string sql, params SqlCeParameter[] @params)
        //{
        //    SqlCeCommand Cmd = GetCommand(conn, transaction, sql, @params);
        //    return Cmd.ExecuteReader();
        //}
        
        //public static SqlCeDataAdapter GetAdapter(string sql, params SqlCeParameter[] @params)
        //{
        //    return GetAdapter(null, sql, @params);
        //}

        //public static SqlCeDataAdapter GetAdapter(SqlCeTransaction transaction, string sql, params SqlCeParameter[] @params)
        //{
        //    SqlCeDataAdapter Adapter = null;
        //    try
        //    {
        //        SqlCeCommand Cmd = GetCommand(_conn, transaction, sql, @params);
        //        Adapter = new SqlCeDataAdapter(Cmd);
        //    }
        //    catch (Exception E)
        //    {
        //        Console.WriteLine("Exception in Database: " + E.Message);
        //    }
        //    return Adapter;
        //}

        //public static bool FillDataset(DsVideos dataSet, Dictionary<String, String> tableNames)
        //{
        //    return tableNames.Aggregate(true, (current, pair) => current & FillDataset(dataSet, pair.Key, pair.Value));
        //}

        //public static bool FillDataset(DsVideos dataSet, string tableName, string query)
        //{
        //    bool Retval = true;
        //    try
        //    {
        //        SqlCeDataAdapter Adap = GetAdapter(query);
        //        Adap.Fill(dataSet, tableName);
        //    }
        //    catch
        //    {
        //        Retval = false;
        //    }

        //    return Retval;
        //}

        //public static void UpdateDatabase(DsVideos dataset)
        //{
        //    throw new Exception("not yet implemented (database.cs: updateDatabase)");
        //}
    }
}