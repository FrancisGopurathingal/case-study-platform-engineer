# .NET CI/CD Demo with GitHub Actions

## Overview
This project demonstrates a CI/CD pipeline using GitHub Actions with:
- Windows runner
- x86 build
- SQLite integration
- Unit testing

## Architecture
- CalculatorApp (Console)
- Calculator.Core (Core logic + DB)
- CalculatorTests (xUnit)

## Run Locally
```bash
dotnet restore
dotnet build
dotnet test