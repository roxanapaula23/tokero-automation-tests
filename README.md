# Tokero Automation Tests

This repository contains automated tests for the [Tokero](https://tokero.dev/en/) platform. The tests are written in **C#** using the **NUnit** testing framework and **Microsoft Playwright** for browser automation.

## Prerequisites

Ensure you have the following installed:

- .NET 9.0 SDK
- Node.js
- Playwright CLI

---

## Clone the Repository

```bash
git clone https://github.com/roxanapaula23/tokero-automation-tests.git
cd tokero-automation-tests
```

---

## Install Dependencies

### .NET Dependencies
```bash
dotnet restore
```

### Node.js & Playwright
```bash
npm install
npx playwright install
```

### Add ExtentReports Package
```bash
dotnet add package ExtentReports
```

---

## Running Tests and Generating ExtentReport

### Run All Tests
```bash
dotnet test 
```

### After test execution, the report will be generated at:
```bash
./bin/Debug/net9.0/TestReports/ExtentReport.html
```

### Open the Report

#### On macOS:
```bash
open ./bin/Debug/net9.0/TestReports/ExtentReport.html
```

#### On Windows:
```bash
start .\bin\Debug\net9.0\TestReports\ExtentReport.html
```