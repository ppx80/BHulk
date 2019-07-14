# BHulk WIP
[![Build Status](https://fmichelucci.visualstudio.com/BHulk/_apis/build/status/ppx80.BHulk?branchName=master)](https://fmichelucci.visualstudio.com/BHulk/_build/latest?definitionId=1&branchName=master)
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

var affectedRows = await update.ExecuteAsync();      
```
Uses a predicate for searching the primary keys and then perform the update in 1000-line steps at a time
```csharp
var update = BHulk<Order>
                .UseContext(() => ContextFactory())
                .Set(o => o.Status, OrderStatus.Executed)
                .Set(o => o.ModifiedDate, DateTime.UtcNow)
                .For(o => o.Status == OrderStatus.Pending)
                .InStepOf(1000);

var affectedRows = await update.ExecuteAsync();      
```
