# .NET CI/CD Demo with GitHub Actions

## Overview
This repository demonstrates the migration of a legacy Azure DevOps CI pipeline into GitHub Actions while preserving functional equivalence and modernizing implementation using native GitHub tooling.

The original pipeline focused on:

- .NET solution restore
- Build execution (with legacy x86 considerations)
- Unit test execution using Visual Studio Test tasks

The GitHub Actions workflow preserves this CI scope while replacing Azure DevOps-specific tasks with modern .NET CLI and marketplace actions.

## Legacy x86 Build Consideration
The original Azure DevOps pipeline included:

- ```dotnet-install.ps1``` targeting x86 architecture
- Visual Studio-based build and test tasks (```VSBuild@1```, ```VSTest@3```)
- ```vc_redist.x86.exe``` dependency installation

These configurations indicate potential support for **legacy 32-bit build environments.**

## Decision in GitHub Actions
The GitHub Actions workflow does **not enforce x86 explicitly** by default:

- The application is built using the standard .NET SDK (```dotnet build```)
- Platform targeting is left as **AnyCPU (default modern behavior)**

## Justification
Since the source repository was not provided and no strict runtime dependency on x86-specific binaries was observed, the workflow prioritizes:

- Modern .NET SDK compatibility
- Hosted runner portability (```windows-latest```)
- Simplified CI execution

## Extensibility
If legacy 32-bit compatibility is required, the workflow can be extended as follows:

```bash
dotnet build --configuration Release -p:PlatformTarget=x86 --no-restore
dotnet test --configuration Release -p:PlatformTarget=x86 --no-restore
```

or via matrix strategy for multi-platform validation.

## API vs CI Scope Separation
The application includes an optional API mode introduced via the ```--api``` flag.

## Purpose of API Mode
The API layer was added to:

- Demonstrate service-based architecture alongside CLI usage
- Provide lightweight HTTP endpoints for demonstration purposes
- Simulate real-world application exposure without introducing infrastructure complexity

## Example usage
```bash
dotnet run --project src/CalculatorApp --api
```
Example endpoint:
```bash
GET /add?a=10&b=5 → { "result": 15 }
GET /mul?a=10&b=5 → { "result": 50 }
GET /health → { "status": "Helathy" }
```
Example usge: Try the following in browser.
```
http://localhost:5000/add?a=10&b=5
````

## Running Locally

### Clone the repository
```
git clone https://github.com/FrancisGopurathingal/case-study-platform-engineer.git
cd case-study-platform-engineer
```
### Restore dependencies
```
dotnet restore DemoSolution.sln
```
### Build solution
```
dotnet build DemoSolution.sln --configuration Release
```
### Run CLI mode (default)
```
dotnet run --project src/CalculatorApp
```
Example:
```
=== Calculator CLI ===
Enter command: add 10 5 | sub 10 5 | mul 3 4 | div 20 2
Type 'history' to view last operations or 'exit' to quit

> add 6 7
Result: 13

> exit
```
### Run API mode
```
dotnet run --project src/CalculatorApp --api
```
### Test API endpoints
```
GET http://localhost:5000/add?a=10&b=5
```
Response:
```
{ "result": 15 }
```
### Running CI locally
```
dotnet test DemoSolution.sln
```

## How to Run CI in GitHub Actions
### 1. Fork the repository
Click the **Fork** button in GitHub to create your own copy of this repository.

### 2. Clone your fork

```bash
git clone https://github.com/<your-username>/case-study-platform-engineer.git
cd case-study-platform-engineer
```
### Create a feature branch
```
git checkout -b demo-ci-test
```
### Make a small change (required to trigger CI)
For example:

- update README
- or modify a comment in code

```
git add .
git commit -m "Trigger CI pipeline"
git push origin demo-ci-test
```
### Create a Pull Request
Open a PR to main branch.
This will automatically trigger the GitHub Actions CI pipeline.

# CI Scope Decision
The original Azure DevOps pipeline scope was limited to:

- Build verification
- Unit test execution

Accordingly, the GitHub Actions workflow maintains the same CI boundary:

- Build validation included
- Unit tests executed
- *API runtime validation excluded*

## Justification
API runtime testing was intentionally excluded because:

- It was not part of the original Azure DevOps pipeline
- CI is designed to validate build correctness and unit-level logic
- Introducing runtime/API tests would expand scope into integration testing, which was not requested

## Key Migration Principle
This migration follows the principle of:

  **Behavioral equivalence over literal task replication.**

Azure DevOps-specific tasks were replaced with:
- ```VSBuild@1``` → ```dotnet build```
- ```VSTest@3``` → ```dotnet test```
-  ```vc_redist.x86.exe``` → removed (not required in modern SDK-based builds)

# Summary
The final GitHub Actions workflow:

- Preserves original CI intent (build + test)
- Removes legacy Windows-specific dependencies
- Uses modern .NET SDK tooling and marketplace actions
- Remains extensible for both legacy x86 and API integration scenarios if required
