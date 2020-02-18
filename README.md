# DailyReportApi
Dotnet core server api for my daily report.

The api use 3 level of models: `Mantis`, `Project`, `DailyReport`.

Clients could save each model to a sqlite database on server.

Clients could query `ReadPeriodReport` model form server with given time interval.

## Usage
Simply run the dll, ex: `dotnet DailyReportApi.dll`
