using Microsoft.Data.Sqlite;

namespace EmailSender.Api.Internal.Extensions;

internal static class SqliteConnectionExtensions
{
    /// <summary>
    /// SQLite built-in function.
    /// </summary>
    public static long SelectLastInsertRowId(this SqliteConnection connection)
    {
        const string commandText = "select last_insert_rowid()";

        using var command = connection.CreateCommand();
        command.CommandText = commandText;

        var id = command.ExecuteScalar();
        return (long) id;
    }
}
