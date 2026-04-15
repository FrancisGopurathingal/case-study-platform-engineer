using Calculator.Core.Services;
using Calculator.Core.Interfaces;
using Calculator.Core.Models;

namespace Calculator.Tests;

public class FakeRepo : IOperationRepository
{
    private readonly List<Operation> _data = new();

    public void Save(Operation op) => _data.Add(op);

    public List<Operation> GetRecent() => _data;
}

public class CalculatorServiceTests
{
    private readonly CalculatorService _service;
    private readonly FakeRepo _repo;

    public CalculatorServiceTests()
    {
        _repo = new FakeRepo();
        _service = new CalculatorService(_repo);
    }

    [Fact]
    public void Add_ShouldReturnCorrectResult()
    {
        var result = _service.Add(10, 5);

        Assert.Equal(15, result);
    }

    [Fact]
    public void Sub_ShouldReturnCorrectResult()
    {
        var result = _service.Sub(10, 5);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Mul_ShouldReturnCorrectResult()
    {
        var result = _service.Mul(3, 4);

        Assert.Equal(12, result);
    }

    [Fact]
    public void Div_ShouldReturnCorrectResult()
    {
        var result = _service.Div(20, 4);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Div_ByZero_ShouldThrowException()
    {
        Assert.Throws<DivideByZeroException>(() => _service.Div(10, 0));
    }

    [Fact]
    public void Operation_ShouldBeSaved_ToRepository()
    {
        _service.Add(2, 3);

        var history = _repo.GetRecent();

        Assert.Single(history);
        Assert.Equal(5, history[0].Result);
        Assert.Equal("Add", history[0].Type);
    }

    [Fact]
    public void MultipleOperations_ShouldBeStored()
    {
        _service.Add(1, 1);
        _service.Sub(5, 2);
        _service.Mul(2, 3);

        var history = _repo.GetRecent();

        Assert.Equal(3, history.Count);
    }

    [Fact]
    public void History_ShouldReturnAllStoredOperations()
    {
        _service.Add(1, 1);
        _service.Add(2, 2);

        var history = _service.History();

        Assert.Equal(2, history.Count);
    }
}