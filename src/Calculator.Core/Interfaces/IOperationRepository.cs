using Calculator.Core.Models;

namespace Calculator.Core.Interfaces;

public interface IOperationRepository
{
    void Save(Operation operation);
    List<Operation> GetRecent();
}