# Light.Data

[![dotnet](https://img.shields.io/badge/dotnet%20standard-2.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/aquilahkj/Light.Data2/blob/master/LICENSE)
[![nuget](https://img.shields.io/nuget/v/Light.Data.svg)](https://www.nuget.org/packages/Light.Data/)
[![download](https://img.shields.io/nuget/dt/Light.Data.svg)](https://www.nuget.org/packages/Light.Data/)
[![gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/light-data/community)

English | [简体中文](./README.zh-CN.md) |

`Light.Data` is a lightweight ORM framework which based on `dotnet standard 2.0`, through to the entity model class `Attribute` or configuration files relate data table. Use core class `DataContext` was carried out on the table to `CURD` operation.

Supported Database

| Database | Introduce |
|:------|:------|
| SqlServer | Need to install `nuget` install `Light.Data.Mssql` library, Support SqlServer 2008 or above |
| Mysql | Need to install `nuget` install `Light.Data.Mysql` library, Support Mysql5.5 or above |
| Postgre | Need to install `nuget` install `Light.Data.Postgre` library, Support PostgreSQL9.3 or above |

* Guide Document: [https://aquilahkj.github.io/Light.Data.Site/](https://aquilahkj.github.io/Light.Data.Site/)

#### Basic Operation

* Basic CURD
* Batch CUD
* Supports transaction processing
* Supports data fields default value and automatic timestamp
* Supports data fields read-write control
* THe Query results specify class or anonymous class output
* The query results insert into the data table  directly

```csharp
var context = new DataContext();
// query single data
var item = context.Query<TeUser>().Where(x => x.Id == 10).First();
// query collection datas
var list = context.Query<TeUser>().Where(x => x.Id > 10).ToList();
// create date
var user = new TeUser() {
    Account = "foo",
    Password = "bar"
};
context.Insert(user);
// update data
user.Password = "bar1";
context.Update(user);
// delete data
context.Delete(user);
```

#### Data Aggregation

* Single-Column data directly aggregate
* Multi-Column data grouped aggregate
* Format the grouped field
* Aggregate data insert into the data table  directly

```csharp
// basic
var list = context.Query<TeUser> ()
                  .Where (x => x.Id >= 5)
                  .GroupBy (x => new LevelIdAgg () {
                      LevelId = x.LevelId,
                      Data = Function.Count ()
                   })
                  .ToList ();

// date format
var list = context.Query<TeUser> ()
                  .GroupBy (x => new RegDateFormatAgg () {
                      RegDateFormat = x.RegTime.ToString("yyyy-MM-dd"),
                      Data = Function.Count ()
                   })
                  .ToList ();	
```

#### Join-Table Query

* Multi-Table join, Support inner join, left join and right join
* Support query results and aggregate data join together
* Join query results specify class or anonymous class output
* join query results insert into the data table directly

```csharp
// inner join
var join = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id);

// aggregate data join entity table          
var join = context.Query<TeMainTable>()
                  .GroupBy(x => new {
                      MId = x.MId,
                      Count = Function.Count(),
                   })
                  .Join<TeSubTable>((x, y) => x.MId == y.Id);
```

#### Execute SQL

* Use SQL and Stored Procedures directly
* Supports object parameters
* Query results specify class or anonymous class output
* Stored Procedures support use output parameters

```csharp
// basic parameter
var sql = "update Te_User set NickName=@P2 where Id=@P1";
var ps = new DataParameter[2];
ps[0] = new DataParameter("P1", 5);
ps[1] = new DataParameter("P2", "abc");
var executor = context.CreateSqlStringExecutor(sql, ps);
var ret = executor.ExecuteNonQuery();

// object parameter
var sql = "update Te_User set NickName={nickname} where Id={id}";
var executor = context.CreateSqlStringExecutor(sql, new { nickname = "abc", id = 5 });
var ret = executor.ExecuteNonQuery();
```