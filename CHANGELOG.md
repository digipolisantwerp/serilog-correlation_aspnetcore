# Serilog Correlation Library

## 5.0.0

- upgrade to net5.0
- Require ICorrelationService via serviceProvider instead of HttpContext

## 4.0.1

- Do not enrich events originating from Digipolis.Correlation toolbox to avoid recursive enrichment causing a StackOverflowException.

## 4.0.0

- Upgrade to .NET Standard 2.0

## 3.0.0

- Conversion to csproj and MSBuild.

## 2.0.0

- Update Digipolis.Serilog package + alignment with new options system.
- More unit tests.

## 1.2.1

- Consolidation of namespaces.

## 1.2.0

- Alignment with version 1.2.0 of Digipolis.Serilog.

## 1.1.0

- Extension methods for registration.

## 1.0.0

- Correlation enricher.