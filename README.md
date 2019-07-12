# BHulk
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ppx80/bhulk/blob/master/LICENSE)

BHulk is a simple library to update a huge amount of records. Under the hood, use EF.Core ExecuteSqlCommandAsync, and dynamically build a query like this: 
```sql
UPDATE Orders SET OrderCode = {0} WHERE Id IN ({1}, {2}, ....., {N})
```
