Imports System.Configuration
Imports System.IO
Imports System.Globalization
Imports System.Data.Common
Imports System.Data

Public Class Database

    Private Shared _connections As New Dictionary(Of String, DbConnection)

    Public Shared Function GetDatabaseFactory(ByVal providerName As String) As DbProviderFactory
        Return DbProviderFactories.GetFactory(providerName)
    End Function

    Public Shared Sub ExecuteSQL(ByVal providerName As String, ByVal connectionString As String, ByVal shareConnections As Boolean, ByRef conn As DbConnection, ByRef transaction As DbTransaction, ByVal sql As String, ByVal ParamArray params() As DbParameter)
        Dim cmd As DbCommand = GetCommand(providerName, conn, transaction, sql, params)
    End Sub

    Public Shared Sub ExecuteSQL(ByVal providerName As String, ByVal connectionString As String, ByVal shareConnections As Boolean, ByVal sql As String, ByVal ParamArray params() As DbParameter)
        Dim cmd As DbCommand = GetCommand(providerName, connectionString, shareConnections, sql, params)
        cmd.ExecuteNonQuery()
        If Not shareConnections Then
            'close connection
            cmd.Connection.Close()
        End If
    End Sub

    Public Shared Function GetConnection(ByVal providerName As String, ByVal connectionString As String, ByVal shareConnections As Boolean) As DbConnection
        Dim factory As DbProviderFactory = GetDatabaseFactory(providerName)
        Dim conn As DbConnection
        If shareConnections Then
            'leave connection open
            If _connections.ContainsKey(providerName) Then
                conn = _connections(providerName)
            Else
                conn = GetDatabaseFactory(providerName).CreateConnection
                conn.ConnectionString = connectionString
                conn.Open()
                _connections.Add(providerName, conn)
            End If
        Else
            'close connection after every use
            conn = factory.CreateConnection
            conn.ConnectionString = connectionString

            conn.Open()
        End If
        Return conn
    End Function

    Public Shared Function GetCommand(ByVal providerName As String, ByRef conn As DbConnection, ByRef transaction As DbTransaction, ByVal sql As String, ByVal ParamArray params() As DbParameter) As DbCommand
        Dim cmd As DbCommand = GetDatabaseFactory(providerName).CreateCommand

        'format sql to work with correct parameters
        Dim sqlParts() As String = sql.Split({"?"}, StringSplitOptions.None)
        Dim newSql As String = sqlParts(0)
        For i As Integer = 1 To sqlParts.Count - 1 'enkel vraagtekens opvullen
            Select Case providerName
                Case "System.Data.SqlClient"
                    '@name
                    newSql &= "@" & params(i - 1).ParameterName & sqlParts(i)
                Case "MySql.Data.MySqlClient"
                    '?name
                    newSql &= "?" & params(i - 1).ParameterName & sqlParts(i)
                Case "Oracle.DataAccess.Client"

                    ':name
                    newSql &= ":" & params(i - 1).ParameterName & sqlParts(i)
                Case Else
                    '?
                    newSql &= "?" & sqlParts(i)
            End Select
        Next
        cmd.CommandText = newSql
        cmd.Transaction = transaction
        cmd.Connection = conn
        For Each param As DbParameter In params
            cmd.Parameters.Add(param)
        Next
        Return cmd
    End Function

    Public Shared Function GetCommand(ByVal providerName As String, ByVal connectionString As String, ByVal shareConnections As Boolean, ByVal sql As String, ByVal ParamArray params() As DbParameter) As DbCommand
        Dim conn As DbConnection = GetConnection(providerName, connectionString, shareConnections)
        Dim cmd As DbCommand = GetCommand(providerName, GetConnection(providerName, connectionString, shareConnections), Nothing, sql, params)
        Return cmd
    End Function

    Public Shared Function GetReader(ByVal providerName As String, ByVal connectionString As String, ByVal shareConnections As Boolean, ByVal sql As String, ByVal ParamArray params() As DbParameter) As DbDataReader
        If Not shareConnections Then
            Return GetCommand(providerName, connectionString, shareConnections, sql, params).ExecuteReader(CommandBehavior.CloseConnection)
        Else
            Return GetReader(providerName, connectionString, GetConnection(providerName, connectionString, shareConnections), Nothing, sql, params)
        End If
    End Function

    Public Shared Function GetReader(ByVal providerName As String, ByVal connectionString As String, ByRef conn As DbConnection, ByRef transaction As DbTransaction, ByVal sql As String, ByVal ParamArray params() As DbParameter) As DbDataReader
        Dim cmd As DbCommand = GetCommand(providerName, conn, transaction, sql, params)
        Dim commandtext As String = cmd.CommandText
        Return cmd.ExecuteReader()
    End Function

    Public Shared Function CreateParameter(ByVal provider As String, ByVal key As String, ByVal value As Object)
        Dim param As DbParameter = GetDatabaseFactory(provider).CreateParameter
        param.ParameterName = key
        param.Value = value
        Return param
    End Function
End Class