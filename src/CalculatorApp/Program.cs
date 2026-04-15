using Microsoft.Extensions.Configuration;
using Calculator.Core.Services;
using Calculator.Infrastructure.Repositories;

try
{
    // 🔧 Load configuration
    var config = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    var dbPath = config["Database:Path"]!;
    var maxHistory = int.Parse(config["Database:MaxHistory"]!);

    // 🧱 Setup dependencies (manual DI)
    var repo = new SqliteOperationRepository(dbPath, maxHistory);
    var service = new CalculatorService(repo);

    Console.WriteLine("=== Calculator CLI ===");
    Console.WriteLine("Enter command: add 10 5 | sub 10 5 | mul 3 4 | div 20 2");
    Console.WriteLine("Type 'history' to view last operations or 'exit' to quit");

    while (true)
    {
        Console.Write("\n> ");
        var input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            continue;

        if (input.ToLower() == "exit")
            break;

        if (input.ToLower() == "history")
        {
            var history = service.History();

            if (!history.Any())
            {
                Console.WriteLine("No history found.");
                continue;
            }

            foreach (var op in history)
            {
                Console.WriteLine($"{op.Type}: {op.Operand1}, {op.Operand2} = {op.Result}");
            }

            continue;
        }

        try
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 3)
            {
                Console.WriteLine("Invalid input. Example: add 10 5");
                continue;
            }

            var operation = parts[0].ToLower();
            var a = double.Parse(parts[1]);
            var b = double.Parse(parts[2]);

            double result = operation switch
            {
                "add" => service.Add(a, b),
                "sub" => service.Sub(a, b),
                "mul" => service.Mul(a, b),
                "div" => service.Div(a, b),
                _ => throw new Exception("Unsupported operation")
            };

            Console.WriteLine($"Result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Fatal error: {ex.Message}");
}