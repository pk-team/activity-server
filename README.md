# Timesheet


## appsettings.Development.json

```json
{
    "AppTitle": "Timesheet",
    "ConnectionStrings": {
        "Default": "server=localhost,9301;database=Activity;uid=sa;pwd=DevDevDude119#;MultipleActiveResultSets=True;Encrypt=false;TrustServerCertificate=False;Connection Timeout=30;"
    }
}
```
## Run

```bash
export ConnectionStrings__Default="server=localhost,9301;database=Activity;uid=sa;pwd=DevDevDude119#;MultipleActiveResultSets=True;Encrypt=false;TrustServerCertificate=False;Connection Timeout=30;"
export AppTitle="Timesheet App"
export ASPNETCORE_URLS="http://localhost:5200"
export ASPNETCORE_ENVIRONMENT="Development"

dotnet run --project app.server

```

## local dev
```
docker run -d \
    -p 9301:1433 \
    -e ACCEPT_EULA=1 \
    -e MSSQL_SA_PASSWORD="DevDevDude119#" \
    --cap-add=SYS_PTRACE \
    --name local-mssql \
    mcr.microsoft.com/azure-sql-edge:latest
```