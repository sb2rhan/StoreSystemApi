# StoreSystemApi

**Do not forget to create appsettings.json file in StoreSystemApi folder**
and paste here:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ConnStr": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=StoreSystemDB;Integrated Security=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
    "AzureDbConnection": "<paste Azure SQL Connection String>"
  },
  "JWT": {
    "ValidAudience": "http://localhost:4200",
    "ValidIssuer": "http://localhost:5000",
    "Secret": "<any secret>"
  }
}
```
