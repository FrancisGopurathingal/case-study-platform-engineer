using System.Reflection;

namespace Calculator.Infrastructure.Data;

public static class SqlQueryProvider
{
    public static string Get(string fileName)
    {
        var basePath = Path.Combine(AppContext.BaseDirectory, "Sql");
        var fullPath = Path.Combine(basePath, fileName);

        return File.ReadAllText(fullPath);
    }
}