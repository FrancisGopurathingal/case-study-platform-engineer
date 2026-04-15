using Microsoft.Data.Sqlite;
using Calculator.Core.Interfaces;
using Calculator.Core.Models;
using Calculator.Infrastructure.Data;

namespace Calculator.Infrastructure.Repositories;

public class SqliteOperationRepository : IOperationRepository
{
    private readonly string _connectionString;
    private readonly int _maxRecords;

    public SqliteOperationRepository(string dbPath, int maxRecords)
    {
        _connectionString = $"Data Source={dbPath}";
        _maxRecords = maxRecords;
        Initialize();
    }

    private void Initialize()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = SqlQueryProvider.Get("create_table.sql");

        cmd.ExecuteNonQuery();
    }

    public void Save(Operation op)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = SqlQueryProvider.Get("insert_operation.sql");

        cmd.Parameters.AddWithValue("$a", op.Operand1);
        cmd.Parameters.AddWithValue("$b", op.Operand2);
        cmd.Parameters.AddWithValue("$t", op.Type);
        cmd.Parameters.AddWithValue("$r", op.Result);
        cmd.Parameters.AddWithValue("$ts", op.Timestamp.ToString());

        cmd.ExecuteNonQuery();

        Cleanup(conn); // reuse same connection (better practice)
    }

    public List<Operation> GetRecent()
    {
        var list = new List<Operation>();

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        var cmd = conn.CreateCommand();
        cmd.CommandText = SqlQueryProvider.Get("get_recent.sql");
        cmd.Parameters.AddWithValue("$limit", _maxRecords);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            list.Add(new Operation
            {
                Operand1 = reader.GetDouble(0),
                Operand2 = reader.GetDouble(1),
                Type = reader.GetString(2),
                Result = reader.GetDouble(3),
                Timestamp = DateTime.Parse(reader.GetString(4))
            });
        }

        return list;
    }

    private void Cleanup(SqliteConnection conn)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = SqlQueryProvider.Get("cleanup.sql");

        cmd.Parameters.AddWithValue("$limit", _maxRecords);
        cmd.ExecuteNonQuery();
    }
}