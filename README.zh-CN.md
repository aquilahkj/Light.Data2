# Light.Data

[![dotnet](https://img.shields.io/badge/dotnet%20standard-2.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)](https://github.com/aquilahkj/Light.Data2/blob/master/LICENSE)
[![nuget](https://img.shields.io/nuget/v/Light.Data.svg)](https://www.nuget.org/packages/Light.Data/)
[![download](https://img.shields.io/nuget/dt/Light.Data.svg)](https://www.nuget.org/packages/Light.Data/)
[![gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/light-data/community)

简体中文 | [English](./README.md)

`Light.Data`是一个轻量级的基于`dotnet standard 2.0`的ORM框架, 通过对实体模型类的`Attribute`或者配置文件进行配置与数据表的对应关系. 使用核心类`DataContext`对数据表进行`CURD`的操作.

```
PM> Install-Package Light.Data
```

支持数据库

| 数据库 | 说明 |
|:------|:------|
| SqlServer | 安装`Light.Data.Mssql`类库, 支持SqlServer 2008或以上 |
| Mysql | 安装`Light.Data.Mysql`类库, 支持Mysql5.5或以上 |
| Postgre | 安装`Light.Data.Postgre`类库, 支持PostgreSQL9.3或以上 |

* 使用文档: [https://aquilahkj.github.io/Light.Data.Site/#/zh-cn/](https://aquilahkj.github.io/Light.Data.Site/#/zh-cn/)

#### 基本操作

* 基本CURD
* 批量CUD
* 支持事务处理
* 支持数据字段默认值和自动时间戳
* 支持数据字段读写控制
* 查询结果指定类或匿名类输出
* 查询直接插入数据表

```csharp
var context = new DataContext();
// 查询单个数据
var item = context.Query<TeUser>().Where(x => x.Id == 10).First();
// 查询集合数据
var list = context.Query<TeUser>().Where(x => x.Id > 10).ToList();
// 新增数据
var user = new TeUser() {
    Account = "foo",
    Password = "bar"
};
context.Insert(user);
// 修改数据
user.Password = "bar1";
context.Update(user);
// 删除数据
context.Delete(user);
```

#### 数据汇总

* 单列数据直接汇总
* 多列数据分组汇总
* 格式化分组字段
* 汇总数据直接插入数据表

```csharp
// 普通汇总
var list = context.Query<TeUser> ()
                  .Where (x => x.Id >= 5)
                  .Aggregate (x => new LevelIdAgg () {
                      LevelId = x.LevelId,
                      Data = Function.Count ()
                   })
                  .ToList ();

// 日期格式化统计
var list = context.Query<TeUser> ()
                  .Aggregate (x => new RegDateFormatAgg () {
                      RegDateFormat = x.RegTime.ToString("yyyy-MM-dd"),
                      Data = Function.Count ()
                   })
                  .ToList ();	
```

#### 连表查询

* 多表连接, 支持内连接, 左连接和右连接
* 支持查询结果和汇总数据连接
* 连接查询结果指定类或匿名类输出
* 连接查询结果直接插入数据表

```csharp
// 内连接
var join = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id);

// 统计结果连接实体表             
var join = context.Query<TeMainTable>()
                  .Aggregate(x => new {
                      MId = x.MId,
                      Count = Function.Count(),
                   })
                  .Join<TeSubTable>((x, y) => x.MId == y.Id);
```

#### 执行SQL语句

* 直接使用SQL语句和存储过程
* 支持对象参数
* 查询结果指定类或匿名类输出
* 存储过程支持output参数

```csharp
// 普通参数
var sql = "update Te_User set NickName=@P2 where Id=@P1";
var ps = new DataParameter[2];
ps[0] = new DataParameter("P1", 5);
ps[1] = new DataParameter("P2", "abc");
var executor = context.CreateSqlStringExecutor(sql, ps);
var ret = executor.ExecuteNonQuery();

// 对象参数
var sql = "update Te_User set NickName={nickname} where Id={id}";
var executor = context.CreateSqlStringExecutor(sql, new { nickname = "abc", id = 5 });
var ret = executor.ExecuteNonQuery();
```