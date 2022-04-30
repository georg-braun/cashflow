using System.Reflection;
using DbUp;
using Oakton;

namespace utility_cli.Commands;

public class MigrateInput
{
    [Description("The connection string of the postgresql database")]
    public string ConnectionString;
}

public class MigrateCommand : OaktonCommand<MigrateInput>
{
    public override bool Execute(MigrateInput input)
    {
        var connectionString = input.ConnectionString;
        
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        // set the .sql file as embedded resource and copy
        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
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