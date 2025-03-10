using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;

namespace LeaveManagement.Infrastructure.Configuration;

public static class DbConfiguration
{
    public static Func<IServiceProvider, IDbConnection> CreateDbConnection()
    {
        Batteries.Init(); //Permet l'utilisation de SQLite en mémoire

        return _ =>
        {
            SqliteConnection connection = new ("Data Source=InMemorySample;Mode=Memory;Cache=Shared;");
            connection.Open();
            InitializeDatabase(connection);
            return connection;
        };
    }

    private static void InitializeDatabase(SqliteConnection connection)
    {
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"
                 CREATE TABLE LeaveRequests (
                    Id TEXT PRIMARY KEY,
                    EmployeeId TEXT,
                    StartDate TEXT,
                    EndDate TEXT,
                    Type TEXT,
                    Status TEXT,
                    Comment TEXT,
                    ManagerComment TEXT
                )";
        command.ExecuteNonQuery();
    }
}
