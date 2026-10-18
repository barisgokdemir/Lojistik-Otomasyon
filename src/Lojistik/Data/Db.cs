using System.Data;
using Microsoft.Data.SqlClient;

namespace Lojistik.Data;

/// <summary>
/// SQL Server erişimi için ince bir yardımcı. Her çağrı kendi bağlantısını açar ve kapatır
/// (bağlantı havuzu ADO.NET tarafından yönetilir); tüm sorgular parametrelidir.
/// </summary>
public sealed class Db
{
    private static Db? _default;
    private readonly string _connectionString;

    public Db(string connectionString) => _connectionString = connectionString;

    public static Db Default =>
        _default ?? throw new InvalidOperationException("Db.Initialize çağrılmadan veritabanı kullanılamaz.");

    public static void Initialize(string connectionString) => _default = new Db(connectionString);

    public DataTable Query(string sql, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = CreateCommand(connection, sql, parameters);
        using var adapter = new SqlDataAdapter(command);

        var table = new DataTable();
        adapter.Fill(table);
        return table;
    }

    public int Execute(string sql, params SqlParameter[] parameters)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = CreateCommand(connection, sql, parameters);

        connection.Open();
        return command.ExecuteNonQuery();
    }

    private static SqlCommand CreateCommand(SqlConnection connection, string sql, SqlParameter[] parameters)
    {
        var command = new SqlCommand(sql, connection) { CommandType = CommandType.Text };
        command.Parameters.AddRange(parameters);
        return command;
    }

    // ---- Parametre yardımcıları -------------------------------------------------------------

    /// <summary>Boş veya yalnızca boşluk içeren metni NULL olarak gönderir.</summary>
    public static SqlParameter Text(string name, string? value, int size) =>
        new(name, SqlDbType.NVarChar, size)
        {
            Value = string.IsNullOrWhiteSpace(value) ? DBNull.Value : value.Trim()
        };

    public static SqlParameter Int(string name, int value) =>
        new(name, SqlDbType.Int) { Value = value };

    public static SqlParameter Int(string name, int? value) =>
        new(name, SqlDbType.Int) { Value = value.HasValue ? value.Value : DBNull.Value };

    /// <summary>
    /// "içerir" araması için LIKE parametresi. Arama terimi boşsa NULL gönderir;
    /// sorgular bunu <c>(@ara IS NULL OR Kolon LIKE @ara)</c> kalıbıyla "tümü" olarak yorumlar.
    /// Kullanıcının yazdığı %, _ ve [ karakterleri joker olarak değil, metin olarak aranır.
    /// </summary>
    public static SqlParameter Like(string name, string? term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return new SqlParameter(name, SqlDbType.NVarChar, 400) { Value = DBNull.Value };
        }

        var escaped = term.Trim()
            .Replace("[", "[[]")
            .Replace("%", "[%]")
            .Replace("_", "[_]");

        return new SqlParameter(name, SqlDbType.NVarChar, 400) { Value = $"%{escaped}%" };
    }
}
