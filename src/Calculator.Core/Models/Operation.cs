namespace Calculator.Core.Models;

public class Operation
{
    public double Operand1 { get; set; }
    public double Operand2 { get; set; }
    public string Type { get; set; } = "";
    public double Result { get; set; }
    public DateTime Timestamp { get; set; }
}