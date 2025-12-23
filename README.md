# RcpIntegrator

A small .NET console utility that parses multiple RCP/CSV time reports from different companies, normalizes them into a common WorkDay model, and deduplicates entries by (Company, EmployeeCode, Date) before printing or exporting.
It’s ideal for merging night‑shift aware records and cleaning up overlapping sources.

## Features

- Parsers per company (e.g., Company1WorkDayParser, Company2WorkDayParser)
- Deduplication by (Company, EmployeeCode, Date) (“first occurrence wins”)
- Support for overnight shifts (exit earlier than entry → next day)
- Lightweight console output for quick validation
-  <ins> place rcp files inside RcpIntegrator.App\bin\<Configuration>\<TargetFramework> </ins>

## Requirements

.NET 8 SDK (or .NET 7+)

## Quick Start
```
1) Clone

git clone https://github.com/khartoum-cyber/RcpIntegrator
cd RcpIntegrator

2) Build

dotnet build

3) Run

dotnet run --project .\RcpIntegrator.App\
```
