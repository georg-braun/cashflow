using System.Reflection;
using DbUp;
using Oakton;

namespace utility_cli.Commands;

public class MigrateInput
{
    [Description("The connection string of the postgresql database. Something like \"Host=hostUrl;Port=5432;Database=postgres;Username=postgres;Password=password\"")]
    public string ConnectionString;

    [Description("Path to the scripts directory.")]
    public string ScriptPath;
}

public class MigrateCommand : OaktonCommand<MigrateInput>
{
    public override bool Execute(MigrateInput input)
    {
        var connectionString = input.ConnectionString;
        var scriptPath = input.ScriptPath;

        if (!Directory.Exists(scriptPath))
        {
            Console.WriteLine($"The directory {scriptPath} doesn't exist.");
            return false;
        }
        
        EnsureDatabase.For.PostgresqlDatabase(connectionString);
        
        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsFromFileSystem(scriptPath)
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
            return false;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success!");
        Console.ResetColor();
        return true;
    }
}