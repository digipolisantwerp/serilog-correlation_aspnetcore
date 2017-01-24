# Serilog Correlation Library.

Serilog Correlation enricher for Digipolis.Correlation package.

## Table of Contents

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->


- [CorrelationEnricher](#correlationenricher)
- [Installation](#installation)
- [Usage](#usage)
- [Enricher](#enricher)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

## CorrelationEnricher

This library contains an enricher for Serilog that adds the ICorrelationContext properties to the LogEvent.
You can find more info about the ICorrelationContext here : [https://github.com/digipolisantwerp/correlation_aspnetcore](https://github.com/digipolisantwerp/correlation_aspnetcore).

## Installation

This package is hosted on Myget on the following feed : https://www.myget.org/F/digipolisantwerp/api/v3/index.json.
To add it to a project, you add the package to the project.json :

``` json 
"dependencies": {
    "Digipolis.Serilog.Correlation":  "1.2.1",
 }
``` 

In Visual Studio you can also use the NuGet Package Manager to do this.

## Usage

The CorrelationEnricher has a dependency on the ICorrelationContext of the **Digipolis.Correlation** package, so make sure the needed services are 
[registered](https://github.com/digipolisantwerp/correlation_aspnetcore#startup). Then register the CorrelationEnricher in the .NET core DI container. This can be done 
in combination with the Serilog Extensions library found here : [https://github.com/digipolisantwerp/serilog_aspnetcore](https://github.com/digipolisantwerp/serilog_aspnetcore) 
by calling the **AddCorrelationEnricher()** method in the Configure method of the Startup class :

```csharp
services.AddCorrelation();

services.AddSerilogExtensions(options => {
    options.MessageVersion = "1";
    options.AddCorrelationEnricher();
});
```  

## Enricher

The enricher adds the following fields to the Serilog LogEvent :

- CorrelationId : the unique correlation id (generated at the start of a request).
- CorrelationSourceId : unique id of the application that started the request (chain).
- CorrelationSourceName : the name of the application that started the request (chain).
- CorrelationInstanceId : the unique identification of the application's running instance that started the request (chain).
- CorrelationInstanceName : the name of the application's running instance that started the request (chain).
- CorrelationHostName : the name or IP address of the machine where the request (chain) was started.
- CorrelationUserName : the user's name that started the request (chain).