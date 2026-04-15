using Calculator.Core.Interfaces;
using Calculator.Core.Models;

namespace Calculator.Core.Services;

public class CalculatorService
{
    private readonly IOperationRepository _repo;

    public CalculatorService(IOperationRepository repo)
    {
        _repo = repo;
    }

    public double Add(double a, double b) => Save(a, b, "Add", a + b);
    public double Sub(double a, double b) => Save(a, b, "Sub", a - b);
    public double Mul(double a, double b) => Save(a, b, "Mul", a * b);

    public double Div(double a, double b)
    {
        if (b == 0) throw new DivideByZeroException();
        return Save(a, b, "Div", a / b);
    }

    private double Save(double a, double b, string type, double result)
    {
        _repo.Save(new Operation
        {
            Operand1 = a,
            Operand2 = b,
            Type = type,
            Result = result,
            Timestamp = DateTime.UtcNow
        });

        return result;
    }

    public List<Operation> History() => _repo.GetRecent();
}