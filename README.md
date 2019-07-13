# BHulk
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ppx80/bhulk/blob/master/LICENSE)

BHulk is a simple library to update a huge amount of records. Under the hood, use EF.Core ExecuteSqlCommandAsync, and dynamically build a query like this: 
```sql
UPDATE Orders SET OrderCode = {0} WHERE Id IN ({1}, {2}, ....., {N})
```
## Usage

Performs the update for the given list of primary key values in 1000-line steps at a time
```csharp
var update = BHulk<Order>
                .UseContext(() => ContextFactory())
                .Set(o => o.OrderCode, "test")
                .Set(o => o.Status, OrderStatus.Executed)
                .Set(o => o.ModifiedDate, DateTime.UtcNow)
                .For(Enumerable.Range(1,10000).ToArray())
                .InStepOf(1000);

var sql = await update.ExecuteAsync();
            
```