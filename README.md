# LightData 使用文档
 
`Light.Data`是一个轻量级的ORM框架, 通过对实体模型类的Attribute或者配置文件进行配置与数据表的对应关系. 使用核心类`DataContext`对数据表进行增删改查的操作.

支持数据库

| 数据库 | 说明 |
|:------|:------|
| SqlServer | 支持SqlServer2008或以上 |
| Mysql | 需nuget安装Light.Data.Mysql类库 |
| Postgre | 需nuget安装Light.Data.Postgre类库 |

## 目录

* [对象映射](#object_mapping)
* [数据库配置](#database_config)
* [DataContext类](#datacontext_class)
* [SQL语句输出](#sql_output)
* [数据表别名](#table_alias_name)
* [清空数据表](#truncate)
* [增加数据](#insert)
* [更新数据](#update)
* [删除数据](#delete)
* [事务处理](#transaction)
* [字段扩展](#field_extend)
* [主键/自增字段查询数据](#key_query)
* [查询数据](#query_data)
* [汇总统计数据](#aggregate_data)
* [表连接](#join_table)

<h2 id="object_mapping"> 对象映射(Object Mapping)</h2>

### 实体类特性

```csharp
[DataTable("Te_User", IsEntityTable = true)]
public class TeUser
{
    /// <summary>
    /// Id
    /// </summary>
    /// <value></value>
    [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
    public int Id
    {
        get;
        set;
    }

    /// <summary>
    /// Account
    /// </summary>
    /// <value></value>
    [DataField("Account")]
    public string Account
    {
        get;
        set;
    }

    /// <summary>
    /// Telephone
    /// </summary>
    /// <value></value>
    [DataField("Telephone", IsNullable = true)]
    public string Telephone
    {
        get;
        set;
    }
    ....
}
```

DataTable:指定对应数据表的表名

* IsEntityTable 指定数据表的是否实体表, 否则为视图, 不可以进行增删改操作, 可空, 默认为true

DataField:指定对应数据字段的字段名

* IsIdentity 字段是否自增, 默认为false
* IsPrimaryKey 字段是否主键, 默认为false
* IsNullable 字段是否可为空, 默认为false, 影响数据新增与更新
* DefaultValue 默认值,可空类型字段在新增数据时, 如果数据为空, 自动使用该默认值；如数据类型是datetime, 空值或最小值时可使用`DefaultTime.Now`表示默认值为当前时间; `DefaultTime.Today`表示默认值为当天

### 配置文件
`Light.Data`支持使用json格式的配置文件进行对数据实体的映射配置, 用于对数据层的解耦, 数据层不需引用`Light.Data`类库进行类Attribute和字段Attribute标记, 如实体类可为

```csharp
public class TeUser
{
    /// <summary>
    /// Id
    /// </summary>
    /// <value></value>
    public int Id
    {
        get;
        set;
    }

    /// <summary>
    /// Account
    /// </summary>
    /// <value></value>
    public string Account
    {
       get;
       set;
    }
    
    /// <summary>
    /// Telephone
    /// </summary>
    /// <value></value>
    public string Telephone
    {
       get;
       set;
    }
    ....
}
```

配置文件地址可通过在程序初始化时进行设置, 可设置多个

```csharp
DataMapperConfiguration.AddConfigFilePath("Config/lightdata.json");
```

默认配置文件为当前目录下的`lightdata.json`, 如文件不存在则自动忽略.

配置数据结构为

```json
{
  "lightDataMapper": {
    "dataTypes": [
      {
        "type": "MyTest.TeUser,MyTest",
        "tableName": "Te_User",
        "isEntityTable": true,
        "dataFields": [
          {
            "fieldName": "Id",
            "isPrimaryKey": true, 
            "isIdentity": true, 
            "name": "Id"
          },
          {
            "fieldName": "Account",
            "name": "Account"
          },
          {
            "fieldName": "Telephone",
            "name": "Telephone"
          },
        ]
      },
    ]
  }
}
```

`lightDataMapper`为映射配置的根节点, 可加入到任意json的配置文件中. 

dataTypes 指定实体类(dataType)的类型集合

dataType 字段定义

| 字段名 | 值类型 | 可空 | 说明 |
|:------|:------|:------|:------|
| type | string | false | 实体类的类名全名 | 
| tableName | string | false | 对应数据表的表名 | 
| isEntityTable | bool | true | 指定数据表的是否实体表, 否为视图, 不可以进行增删改操作, 默认为true | 
| dataFields | array(dataField) | true | 该实体类下的映射字段集合 |

dataField 字段定义

| 字段名 | 值类型 | 可空 | 说明 |
|:------|:------|:------|:------|
| fieldName | string | false | 实体类的字段名 | 
| isPrimaryKey | bool | true | 字段是否自增, 默认为false | 
| isIdentity | bool | true | 字段是否自增, 默认为false | 
| isNullable | bool  | true | 字段是否可为空, 默认为false, 影响数据新增与更新 |
| name | string | true | 字段映射的数据库字段名, 默认为实体类的字段名 | 
| defaultValue | string | true | 默认值, 可空类型字段在新增数据时, 如果数据为空, 自动使用该默认值; 如数据类型是datetime, 空值或最小值时可使用`DefaultTime.Now`表示默认值为当前时间; `DefaultTime.Today`表示默认值为当天 | 

### 字段类型(Field Type)

`Light.Data`对于映射字段支持以下类型

* 原生数据类型 `bool`, `byte`, `sbyte`, `short`, `ushort `, `int`, `uint`, `long`, `ulong`, `double`, `float`, `decimal`, `DateTime`及其可空类型, 数据库需对应类型, 如数据不支持该类型, 可使用位数更大的类型, 如在Mssql中`sbyte `=>`smallint`, `ushort`=>`int`, `uint`=>`bigint`, `ulong`=>`decimal(20,0)`
* `Enum`类型及其可空类型, 数据库需对应Enum本身所继承的数字类型
* `string`类型, 数据库需使用字符串类型, 如`char`, `varchar`, `text`等
* `byte[]`类型, 数据库需使用二进制类型, 如`binary`, `varbinary`等, 该类型只支持是否为空的条件查询
* 自定义对象类型, 数据库需使用字符串类型, 如`char`, `varchar`, `text`等, Light.Data会自动把对象序列化为json格式存取, 该类型只支持是否为空和整体相等的条件查询

### 主表与子表关联

如数据库有主表和子表需要做关联查询, 可以把子表的映射以单体(一对一)或集合(一对多)作为主表映射类的一个属性, 就如主表的自有字段. 如有主表映射

```csharp
[DataTable("Te_User", IsEntityTable = true)]
public class TeUser
{
    [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
    public int Id
    {
        get;
        set;
    }

    [DataField("Account")]
    public string Account
    {
        get;
        set;
    }
    
    [DataField("Telephone", IsNullable = true)]
    public string Telephone
    {
        get;
        set;
    }
}
```

子表映射

```csharp
[DataTable("Te_UserExtend", IsEntityTable = true)]
public class TeUserExtend
{
    [DataField("Id", IsIdentity = true, IsPrimaryKey = true)]
    public int Id
    {
        get;
        set;
    }

    [DataField("MainId")]
    public int MainId
    {
        get;
        set;
    }
    
    [DataField("Data", IsNullable = true)]
    public string Data
    {
        get;
        set;
    }
}
```

继承`TeUser`新增一个类

```csharp
public class TeUserAndExtend : TeUser
{
    [RelationField("Id", "MainId")]
    public TeUserExtend Extend
    {
        get;
        set;
    }
}
```

通过Attribute`RelationField`把`TeUserExtend`作为一个名为`Extend`属性, 在查询`TeUserAndExtend`时, 会把对应关联相连的`TeUserExtend`内容也一并查出. `RelationField`的参数1是主表的关联字段属性名, 参数2是子表的关联字段属性名, 如有一组主表和子表有多组关联字段, 可以设置多组Attribute, 如

```csharp
[RelationField("MKey1", "SKey1")]
[RelationField("MKey2", "SKey2")]
public TeUserExtend Extend
{
    get;
    set;
}
```

如主表与子表是一对多关系, 属性类型需要使用`ICollection<T>`或`LCollection<T>`, T为子表类型, 以集合形式输出结果, 如

```csharp
public class TeUserAndExtend : TeUser
{
    [RelationField("Id", "MainId")]
    public ICollection<TeUserExtend> Extends
    {
        get;
        set;
    }
}
```

如不希望新增继承类, 也可以把子表类的属性直接放入主表类中, 但如果这样做, 在无论需不需查询子表内容的场景, 都会对子表数据进行查询, 降低查询效率. 同样, 关联属性也可以在配置文件中的`dataType`节点中进行配置, 关联字段节点为`relationFields`. 继承类的配置会默认继承基类的`dataFields`和`relationFields`配置, 如

```csharp
      {
        "type": "MyTest.TeUserAndExtend,MyTest",
        "tableName": "Te_User",
        "isEntityTable": true,
        "relationFields": [
          {
            "fieldName": "Extend",
            "relationPairs": [
              {
                "masterKey": "Id",
                "relateKey": "MainId"
              }
            ]
          }
        ]
      }
```

relationFields 字段定义

| 字段名 | 值类型 | 可空 | 说明 |
|:------|:------|:------|:------|
| fieldName | string | false | 实体类的字段名 | 
| relationPairs | array(relationPair) | false | 关联字段组合 |

relationPair 字段定义

| 字段名 | 值类型 | 可空 | 说明 |
|:------|:------|:------|:------|
| masterKey | string | false | 主表的关联字段属性名 | 
| masterKey | string | false | 子表的关联字段属性名 | 

#### 一个主表类支持多个不同子表类做关联属性, 如

```csharp
public class TeRelateA_BC : TeRelateA
{
    [RelationField("Id", "RelateAId")]
    public TeRelateB RelateB {
        get;
        set;
    }

    [RelationField("Id", "RelateAId")]
    public TeRelateC RelateC {
        get;
        set;
    }
}

public class TeRelateA_B_Collection : TeRelateA
{
    [RelationField("Id", "RelateAId")]
    public TeRelateB RelateB {
        get;
        set;
    }

    [RelationField("Id", "RelateAId")]
    public ICollection<TeRelateCollection> RelateCollection {
        get;
        set;
    }
}
```

#### 子表类下可以作为主表连接另外的子表, 如

```csharp
public class TeRelateA_BC : TeRelateA
{
    [RelationField("Id", "RelateAId")]
    public TeRelateB_C RelateB {
        get;
        set;
    }


public class TeRelateB_C : TeRelateB
{
    [RelationField("Id", "RelateBId")]
    public TeRelateC RelateC {
        get;
        set;
    }
}
```

#### 当两个表类互为主从时, 需确保双方的关联字段主从相反, 定义保持一致, 否则会出现错误

```csharp
 public class TeRelateA_B_A : TeRelateA
 {
     [RelationField("Id", "RelateAId")]
     public TeRelateB_A_B RelateB {
         get;
         set;
     }
 }

 public class TeRelateB_A_B : TeRelateB
 {
     [RelationField("RelateAId", "Id")]
     public TeRelateA_B_A RelateA {
         get;
         set;
     }
 }
```

### 实体类与配置生成模板

[模版地址](https://github.com/aquilahkj/Light.Data2/tree/master/template)

<h2 id="database_config"> 数据库配置(Database Config)</h2>

### 配置文件方式

`Light.Data`支持使用json格式的配置文件对数据库类型和数据连接字符串进行配置， 配置文件地址可通过在程序初始化时进行设置, 只可以设置一个
`DataContextConfiguration.SetConfigFilePath("Config/lightdata.json");` 
默认配置文件为当前目录下的`lightdata.json`, 如文件不存在则自动忽略.

#### 配置方法

配置数据结构为

```json
{
  "lightData": {
    "connections": [
      {
        "name": "mssql",
        "connectionString": "Data Source=127.0.0.1;User ID=sa;Password=***;Initial Catalog=LightData_Test;",
        "providerName": "Light.Data.Mssql.MssqlProvider"
      },
      {
        "name": "mssql_2012",
        "connectionString": "Data Source=192.168.0.1;User ID=sa;Password=***;Initial Catalog=LightData_Test;",
        "providerName": "Light.Data.Mssql.MssqlProvider",
        "configParams": [
          {
            "name": "version",
            "value": "11"
          }
        ]
      }
    ]
  }
}

```

`lightData`为映射配置的根节点, 可加入到任意json的配置文件中. 
`connections`指定数据库连接配置(connection)的集合

connections 字段定义

| 字段名 | 值类型 | 可空 | 说明 |
|:------|:------|:------|:------|
| name | string | false | 连接配置唯一名称 | 
| connectionString | string | false | 连接字符串，格式因数据库种类而异 | 
| providerName | string | false | 对应不同数据库种类的处理类的类名 | 
| configParams | array(configParam) | true | 额外的配置参数，因数据库类型而异 |

configParam 字段定义

| 字段名 | 值类型 | 可空 | 说明 |
|:------|:------|:------|:------|
| name | string | false | 参数名 | 
| value | string | false | 参数值 | 

providerName 对应数据库种类的类名

| 数据库 | Provider | 说明 |
|:------|:------|:------|
| SqlServer | Light.Data.Mssql.MssqlProvider | |
| Mysql | Light.Data.Mysql.MysqlProvider | 需nuget安装Light.Data.Mysql类库 |
| Postgre | Light.Data.Postgre.PostgreProvider | 需nuget安装Light.Data.Postgre类库 |

基本 configParam 参数设置

| 参数名 | 数据格式 | 说明 |
|:------|:------|:------|
| timeout | int | 执行语句超时时间，单位毫秒，默认60000 |
| batchInsertCount | int | 批量新增数据每组语句块处理数，默认10 |
| batchUpdateCount | int | 批量更新数据每组语句块处理数，默认10 |
| batchDeleteCount | int | 批量删除数据每组语句块处理数，默认10 |
| version | string | 对应的数据库版本

SqlServer version 功能列表

| version | 数据库 | 说明 |
|:------|:------|:------|
| 10.0 | SqlServer 2008 | version>=10.0 优化批量新增数据的SQL语句, 默认 |
| 11.0 | SqlServer 2012 | version>=11.0 支持offset分页查询 |
| 12.0 | SqlServer 2014 |  |

#### 使用配置

使用配置名创建核心类`DataContext`, `DataContext`无参构造函数默认使用一个配置节.

```csharp
DataContext context = new DataContext("mssql");
```

可继承DataContext创建指定DataContext类

```csharp
 public class MyDataContext : DataContext
 {
     public MyDataContext() : base("mssql")
     {

     }
 }
```

### DependencyInjection方式

使用`Microsoft.Extensions.DependencyInjection`实现依赖注入, 自定义连接字符串的配置. 
继承`DataContext`创建参数为`DataContextOptions<T>`的指定`DataContext`类

```csharp
 public class MyDataContext : DataContext
 {
     public MyDataContext(DataContextOptions<MyDataContext> options) : base(options)
     {

     }
 }
```

在程序初始化时对`IServiceCollection`使用扩展静态函数`AddDataContext`加入对`DataContextOptionsBuilder<TContext>`的委托处理

```csharp
 IServiceCollection service = new ServiceCollection();
 service.AddDataContext<MyDataContext>(builder => {
     builder.UseMssql(connectionString);
     builder.SetTimeout(2000);
     builder.SetVersion("11.0");
 }, ServiceLifetime.Transient);
```

直接调用

```csharp
var provider = service.BuildServiceProvider();
var context = provider.GetRequiredService<MyDataContext>();
```

MVC依赖注入方式调用

```csharp
public class MyController : Controller
{
    MyDataContext _context;
    public MyController(MyDataContext context)
    {
        _context = context;
    }
}
```

DataContextOptionsBuilder<TContext>主要方法

| 方法 | 说明 |
|:------|:------|
| UseMssql(string connectionString) | 设置数据库为SqlServer时的连接字符串 |
| UseMysql(string connectionString) | 设置数据库为Mysql时的连接字符串, 需nuget安装Light.Data.Mysql类库 |
| UsePostgre(string connectionString) | 设置数据库为Postgre时的连接字符串, 需nuget安装Light.Data.Postgre类库 |
| SetTimeout(int timeout) | 设置执行语句超时时间，单位毫秒，默认60000 |
| SetBatchInsertCount(int timeout) | 设置批量新增数据每组语句块处理数，默认10 |
| SetBatchUpdateCount(int timeout) | 设置批量更新数据每组语句块处理数，默认10 |
| SetBatchDeleteCount(int timeout) | 设置批量删除数据每组语句块处理数，默认10 |
| SetVersion(string version) | 设置对应的数据库版本 |
| SetCommandOutput(ICommandOutput output) | 设置SQL语句输出 |

DataContextOptionsBuilder<TContext>也可通过使用ConfigName方法结合配置文件读取配置节的配置, 再做自定义设置. 

<h2 id="datacontext_class"> DataContext类(DataContext Class)</h2>

`DataContext`是`Light.Data`的核心类, 所有对数据库的操作均通过`DataContext`的方法完成

* 建议通过继承`DataContext`创建指定数据库的专用类. 
* `DataContext`非线程安全.
* `DataContext`中方法名含有`Async`的方法均为其对应方法的异步实现.

<h2 id="sql_output"> SQL语句输出(SQL Output)</h2>

`ICommandOutput`接口用于在对数据库ORM操作时, 同时把执行的SQL语句按指定方式输出, 主要用于debug. 使用SQL语句输出会造成一定的性能损耗. 可以直接使用自带的`CommandOutput`类或自行实现`ICommandOutput`接口.

创建`CommandOutput`

```csharp
 CommandOutput output = new CommandOutput();
 output.Enable = true;
 output.UseConsoleOutput = true;
 output.OutputFullCommand = true;
```

CommandOutput 主要属性与事件

| 属性 | 说明 |
|:------|:------|
| Enable | 启用命令输出 |
| UseConsoleOutput | 直接把SQL命令输出到控制台 |
| OutputFullCommand	| 把SQL参数填入语句中, 输出可以直接在数据库客户端直接执行的语句 |

| 事件 | 说明 |
|:------|:-----|
| OnCommandOutput | SQL语句输出事件, 用于自定义处理语句的方式, 如记入日志 |

直接在`DataContext`设置输出接口

```csharp
DataContext context = new DataContext();
context.SetCommanfOutput(output);
```

在`DataContextOptionsBuilder`设置输出接口, 用于依赖注入创建`DataCOntext`

```csharp
service.AddDataContext<TestContext>(builder => {
	builder.UseMssql(connectionString);
	builder.SetCommandOutput(output);
}, ServiceLifetime.Transient);
```

执行查询方法

```csharp
var list = context.Query<TeUser>().Where(x => x.Id == 10).ToList();
```

SQL语句输出

```
action  :QueryDataDefineList
type    :Text
level   :None
region  :0,2147483647
time    :2017-09-05 00:48:49.828
span    :20.7349
trans   :false
success :true
result  :1
params  :
  Input,Int32,@P1=10
command :
    select [Id],[Account],[Password],[NickName],[Gender],[Birthday],[Telephone],[Email],[LevelId],[RegTime],[LastLoginTime],[Status],[HotRate] from [Te_User] where [Id]=@P1
--------------------
    select [Id],[Account],[Password],[NickName],[Gender],[Birthday],[Telephone],[Email],[LevelId],[RegTime],[LastLoginTime],[Status],[HotRate] from [Te_User] where [Id]=10
```

<h2 id="table_alias_name"> 数据表别名(Table Alias Name)</h2>

`Light.Data`支持在`DataContext`中临时更改指定映射类型的对应映射表名, 用于特殊场景, 如日志表的水平分表记录数据. 更改的表名仅作用于该`DataContext`.

| 方法 | 说明 |
|:------|:------|
| GetTableName<T>() | 获取指定类型T的对应数据表名 |
| SetAliasTableName<T>(string name) | 对指定类型T设定别名 |
| ResetAliasTableName<T>() | 清除指定类型T的别名 |
| ClearAliasTableName() | 清除所有在此DataContext中的别名 |

设置别名并查询

```csharp
DataContext context = new DataContext();
//获取TeLog的对应表名Te_Log
var tableName = context.GetTableName<TeLog>();
//别名Te_Log_201709
var aliasName = tableName + "_201709";
//设置别名
context.SetAliasTableName<TeLog>(aliasName);
//查询表Te_Log_201709
var list = context.Query<TeLog>().ToList();
```

<h2 id="truncate"> 清空数据表(Truncate Table)</h2>

 ```csharp
 context.TruncateTable<TeUser> (); 
 ```
 
 注意：该操作直接使用`truncate table`命令清空数据表.

<h2 id="insert"> 增加数据(Insert Data)</h2>

自增ID会在Insert后自动赋值

```csharp
TeUser user = new TeUser ();
user.Account = "test";
user.Birthday = new DateTime (2001, 10, 20);
user.Email = "test@test.com";
user.Gender = GenderType.Female;
user.LevelId = 1;
user.NickName = "nicktest";
user.Password = "imtest";
user.RegTime = new DateTime (2015, 12, 30, 18, 0, 0);
user.Status = 1;
user.Telephone = "12345678";
user.HotRate = 1.0d;
context.Insert (user);
```

批量新增数据方法

```csharp
BatchInsert<T>(IEnumerable<T> datas)
```

<h2 id="update">更新数据(Update Data)</h2>

```csharp
int id = 1;
TeUser user = context.SelectById<TeUser> (id);
user.Status = 2;
context.Update (user);
```

批量更新数据方法

```csharp
BatchUpdate<T>(IEnumerable<T> datas)
```

<h2 id="delete">删除数据(Delete Data)</h2>

```csharp
int id = 1;
TeUser user = context.SelectById<TeUser> (id);
context.Delete (user);
```

批量删除数据方法

```csharp
BatchDelete<T>(IEnumerable<T> datas)
```

<h2 id="transaction">事务处理(Transaction)</h2>

对增删改多个操作需要在同一事务中做操作,通过`DataContext`的`CreateTransactionScope`方法生成事务`TransactionScope`, 并使用其事务方法进行事务操作

* BeginTrans 开始事务,每次事务开始前需执行
* CommitTrans 提交事务,提交后该事务才完成
* RollbackTrans 回滚事务,在using作用域逻辑上需要回滚才执行, 抛异常时会自动回滚

```csharp
//单次提交
using (TransactionScope trans = context.TransactionScope ()) {
           trans.BeginTrans ();
           TeUser user1 = context.SelectById<TeUser> (3);
           user1.Account = "testmodify";
           context.Update(user1);
           TeUser user2 = context.SelectById<TeUser> (4);
           context.Delete(user2);
           trans.CommitTrans ();
}
//多次提交
using (TransactionScope trans = context.TransactionScope ()) {
           trans.BeginTrans ();
           TeUser user1 = context.SelectById<TeUser> (1);
           user1.Account = "testmodify";
           context.Update(user1);
           trans.CommitTrans ();
           trans.BeginTrans ();
           TeUser user2 = context.SelectById<TeUser> (2);
           context.Delete(user2);
           trans.CommitTrans ();
}
//回滚
using (TransactionScope trans = context.TransactionScope ()) {
           trans.BeginTrans ();
           TeUser user1 = new <TeUser> ();
           user1.Account = "testmodify";
           context.Insert (user1);
           if(user1.Id > 5){
               trans.RollbackTrans ();
           }
           else{
               trans.CommitTrans ();
           }
}
```

<h2 id="field_extend">字段扩展(Field Extension)</h2>

查询或汇总数据时经常需要指定字段里的某部分数据进行提出查询或汇总, 例如从时间字段提出日期统计, 可以使用预设定的方法或属性进行提出汇总. 另外数字型字段支持使用`+`(加)`-`(减)`*`(乘)`/`(除)`%`(求余)与其他数学字段或常量数值做相应数学处理,字符串支持使用`+`(加)做字符串拼接.

#### 时间字段方法(DateTime)

| 方法 | 说明 |
|:------|:------|
| ToString(string format) | 格式化的日期字符串,年yyyy,月MM,日dd |

| 属性 | 说明 |
|:------|:------|
| Date | 日期时间格式 |
| Year | 时间中的年部分 |
| Month | 时间中的月部分 |
| Day | 时间中的日部分 |
| Hour | 时间中的时部分 |
| Minute | 时间中的分部分 |
| Second | 时间中的秒部分 |
| Week | 时间中的周部分(不同数据库定义可能不一致) |
| DayOfWeek | 时间为当周第几天 |
| DayOfYear | 时间为当年第几天 |

#### 字符串字段方法(String)

| 方法 | 说明 |
|:------|:------|
| Substring(int index, int length) | 截取字符串 |
| IndexOf(int value, int index) | 获取字符串位置 |
| Replace(string oldString, string newString) | 替换字符串 |
| ToLower() | 转换大写 |
| ToUpper() | 转换小写 |
| Trim() | 清空前后空格 |
| Concat(object obj1,object obj2,object obj3...) | 静态函数, 连接字符串 |	

| 属性 | 说明 |
|:------|:------|
| Length | 字符串长度 |

#### 数学方法(Math静态函数)

| 方法 | 说明 |
|:------|:------|
| Abs | 计算绝对值 |
| Sign | 返回表示数字符号的值 |
| Sin | 计算正弦值 |
| Cos | 计算余弦值 |
| Tan | 计算正切值 |
| Atan | 计算反正切值 |
| ASin | 计算反正弦值 |
| ACos | 计算反余弦值 |
| Atan2 | 计算从x 坐标轴到点的角度 |
| Ceiling | 将数字向上舍入为最接近的整数 |
| Floor | 将数字向下舍入为最接近的整数 |
| Round | 四舍五入为最接近的整数 |
| Truncate | 计算一个数字的整数部分 |
| Log | 返回指定数字的对数 |
| Log10 | 返回指定数字以 10 为底的对数 |
| Exp | 计算指数值 |
| Pow | 计算x 的y 次方 |
| Sqrt | 返回指定数字的平方根 |
| Max | 返回两个整数中较大的一个 |
| Min | 返回两个整数中较小的一个 |

<h2 id="key_query">主键/自增字段查询数据(Key Query Data)</h2>

通过自增ID查数据

```csharp
int id = 1;
TeUser user = context.SelectById<TeUser> (id);
```

通过主键查数据,若有多个主键,则全部主键值需全部输入

```csharp
int id = 1;
TeUser user = context.SelectByKey<TeUser> (id);
```

<h2 id="query_data">查询数据(Query Data)</h2>

使用context的Query方法生成IQuery对象,范型T为查询表的映射类
`IQuery query = context.Query<T>()`
`IQuery`通过Builder模式添加查询要素,最终可通过自身枚举方式或ToList()方式输出查询数据.

IQuery主要查询用方法:

| 方法 | 说明 |
|:------|:------|
| Where(Expression\<Func\<T, bool>> expression) | 把IQuery中查询条件置为当前查询条件,如Where(x=>x.Id>1 )|
| WhereWithAnd(Expression\<Func\<T, bool>> expression) | 把IQuery中查询条件以And方式连接当前查询条件 |
| WhereWithOr(Expression\<Func\<T, bool>> expression) | 把IQuery中查询条件以Or方式连接当前查询条件 |
| WhereReset() | 把IQuery中查询条件重置 |
| OrderBy\<TKey>(Expression\<Func\<T, TKey>> expression) | 把IQuery中排序替换为当前字段正序排序,如OrderBy(x=>x.Id) |
| OrderByDescending\<TKey>(Expression\<Func\<T, TKey>> expression) | 把IQuery中排序置为当前字段正序排序,如OrderByDescending(x=>x.Id) |
| OrderByCatch\<TKey>(Expression\<Func\<T, TKey>> expression) | 把IQuery中排序连接当前字段正序排序 |
| OrderByDescendingCatch\<TKey>(Expression\<Func\<T, TKey>> expression)	| 把IQuery中排序连接当前字段倒序排序 |
| OrderByReset() | 把IQuery中排序重置 |
| OrderByRandom() | 把IQuery中排序置为随机排序 |
| Take(int count) | 设定IQuery输出结果的数量 |
| Skip(int count) | 设定IQuery输出结果需要跳过的数量 |
| Range(int from, int to) | 设定IQuery输出结果的从from位到to位 |
| PageSize(int page, int size) | 设定IQuery输出结果的分页结果,page:从1开始页数,size:每页数量 |
| RangeReset() | 把IQuery中输出结果范围重置 |
| SetDistinct(bool distinct) | 设定是否使用Distinct方式输出结果 |
| ToList() | 结果以List<T>的方式输出 |
| ToArray() | 结果以T[]的方式输出 |
| First() | 输出的查询结果的首个数据结果对象,如无数据则为null	|
| ElementAt(int index) | 输出的查询结果的指定位数数据结果对象,如无数据则为null |
| Exists | 判断该IQuery是否有数据 |
| Count	| 返回该IQuery的数据长度,返回类型为int |
| LongCount | 返回该IQuery的数据长度,返回类型为long |


#### 全查询

```csharp
List<TeUser> list = context.Query<TeUser> ().ToList ();
```

#### 组合查询

```csharp
List<TeUser> list = context.Query<TeUser> ().Where (x => x.Id > 1).OrderBy (x => x.Id).Take(10).ToList ();
```
### 条件查询(Where)

***

使用`IQuery<T>.Where(lambda)`方法加入查询条件,查询参数为Lambda表达式, 有Where, WhereWithAnd, WhereWithOr, WhereReset四个方法

```csharp
context.Query<T> ().Where(x => x.Id > 1)
```

#### 普通条件查询

```csharp
 List<TeUser> list1 = context.Query<TeUser> ().Where (x => x.Id >= 5 && x.Id <= 10).ToList ();
 List<TeUser> list2 = context.Query<TeUser> ().Where (x => x.Id < 5 || x.Id > 10).ToList ();
```

#### In条件查询

使用`List<T>.Contains`方法,not查询在条件前面加"!"号

```csharp
int [] arrayx = new int [] { 3, 5, 7 };
List<int> listx = new List<int> (arrayx);
//in
List<TeUser> list1 = context.Query<TeUser> ().Where (x => listx.Contains (x.Id)).ToList ();
//not in
List<TeUser> list2 = context.Query<TeUser> ().Where (x => ！listx.Contains (x.Id)).ToList ();
```

#### Like条件查询

只支持string类型,使用`string.StartsWith`、`string.EndsWith`、`string.Contains`方法查询,可支持反向查,not查询在条件前面加"!"号

```csharp
//后模糊
List<TeUser> list1 = context.Query<TeUser> ().Where (x => x.Account.StartsWith ("test")).ToList ();
//前模糊
List<TeUser> list2 = context.Query<TeUser> ().Where (x => x.Account.EndsWith ("1")).ToList ();
//前后模糊
List<TeUser> list3 = context.Query<TeUser> ().Where (x => x.Account.Contains ("es")).ToList ();
//反向查
List<TeUser> list1 = context.Query<TeUser> ().Where (x => "mytest2".EndsWith (x.Account)).ToList ();
//not 查
List<TeUser> list1 = context.Query<TeUser> ().Where (x => !x.Account.StartsWith ("test")).ToList ();
```

#### null查询

查询字段需为可空类型(如int?)或string类型

```csharp
 //null查询
 List<TeUser> list1 = context.Query<TeUser> ().Where (x => x.RefereeId == null).ToList ();
 //非null查询
 List<TeUser> list2 = context.Query<TeUser> ().Where (x => x.RefereeId != null).ToList ();
```

如非可空类型可用扩展查询方式`ExtendQuery.IsNull()`

```csharp
//null查询
List<TeUser> list1 = context.Query<TeUser> ().Where (x => ExtendQuery.IsNull (x.Id)).ToList ();
//非null查询
List<TeUser> list2 = context.Query<TeUser> ().Where (x => !ExtendQuery.IsNull (x.Id)).ToList ();
```

#### 布尔值字段查询

查询字段需为布尔(boolean)类型

```csharp
//是查询
List<TeUser> list1 = context.Query<TeUser> ().Where (x => x.DeleteFlag).ToList ();
List<TeUser> list1 = context.Query<TeUser> ().Where (x => x.DeleteFlag == true).ToList ();
List<TeUser> list1 = context.Query<TeUser> ().Where (x => x.DeleteFlag != false).ToList ();
//非查询
List<TeUser> list2 = context.Query<TeUser> ().Where (x => !x.DeleteFlag).ToList ();
List<TeUser> list2 = context.Query<TeUser> ().Where (x => x.DeleteFlag != true).ToList ();
List<TeUser> list2 = context.Query<TeUser> ().Where (x => x.DeleteFlag == false).ToList ();
```
#### 跨表Exists查询

固定条件查询

```csharp
//Exist查询
List<TeUser> list1 = context.Query<TeUser> ().Where (x => ExtendQuery.Exists<TeUserLevel> (y => y.Status == 1)).ToList ();
//Not Exist查询
List<TeUser> list2 = context.Query<TeUser> ().Where (x => !ExtendQuery.Exists<TeUserLevel> (y => y.Status == 1)).ToList ();
```
关联条件查询

```csharp
//Exist查询
List<TeUser> list1 = context.Query<TeUser> ().Where (x => ExtendQuery.Exists<TeUserLevel> (y => y.Id == x.LevelId)).ToList ();
//Not Exist查询
List<TeUser> list2 = context.Query<TeUser> ().Where (x => !ExtendQuery.Exists<TeUserLevel> (y => y.Id == x.LevelId)).ToList ();
```

### 排序(OrderBy)

***

使用`IQuery.OrderBy(lambda)`方法加入查询条件,查询参数为Lambda表达式,有OrderBy, OrderByDescending, OrderByCatch, OrderByDescendingCatch,  OrderByReset, OrderByRandom六个方法

```csharp
context.Query<T> ().OrderBy(x => x.Id)
```

#### 正向排序

```csharp
List<TeUser> list = context.Query<TeUser> ().OrderBy (x => x.Id).ToList ();
```

#### 反向排序

```csharp
List<TeUser> list = context.Query<TeUser> ().OrderByDescending (x => x.Id).ToList ();
```

### 选择指定字段(Select)

***

使用`IQuery<T>.Select(lambda)`查询时指定字段输出新的结构类,支持匿名类输出,使用Lambda表达式中的new方式定义新结构类.

```csharp
 var list = context.Query<TeUser> ()
                   .Select (x => new TeUserSimple () {
                       Id = x.Id,
                       Account = x.Account,
                       LevelId = x.LevelId,
                       RegTime = x.RegTime
                    })
                   .ToList ();
//匿名类
 var list = context.Query<TeUser> ()
                   .Select (x => new {
                       x.Id,
                       x.Account,
                       x.LevelId,
                       x.RegTime
                    })
                   .ToList ();
```

查询单字段列表

```csharp
List<int> list = context.Query<TeUser> ().Select (x => x.Id).ToList ();
```

### 查询批量更新

***

使用`IQuery<T>.Update(lambda)`对查询数据进行批量更新操作, 以lambda表达式中的new方式定义数据的更新字段与更新内容, 左侧为更新字段名, 右侧为更新内容, 内容可为原字段, 返回结果为成功更新行数. 

```csharp
var result = context.Query<TeUser> ()
                    .Update (x => new TeUser {
                        LastLoginTime = DateTime.Now,
                        Status = 2
                     });
//更新内容为原字段
var result = context.Query<TeUser2> ()
                    .Update (x => new TeUser2 {
                        LastLoginTime = x.RecordTime,
                        Status = x.Status + 1
                     });
```

### 查询批量删除

***

使用`IQuery<T>.Delete()`对查询数据进行批量删除操作, 返回结果为成功删除行数.

```csharp
var result = context.Query<TeUser> ().Where(x => x.Id > 1)
                    .Delete();
```

### 查询批量插入

***

对查询数据进行全字段或指定字段插入指定的数据表,直接通过数据库的`insert into t1(x1,x2,x3...)select y1,y2,y3 from t2`方式直接插入数据

#### 全字段插入

使用`IQuery<T>.Insert<K>()`全数据插入,查询表T的字段必须与插入表K的字段一一对应,如果T与K有同位字段是自增字段,则插入时,K的自增字段数据为自增, 返回结果为成功插入行数.

```csharp
var result = context.Query<TeDataLog> ()
		    .Insert<TeDataLogHistory> ();
```

#### 指定字段插入

使用`IQuery<T>.SelectInsert<K>(lambda)`选择指定字段举行插入, lambda表达式中的new方式定义数据的插入表字段与查询表选择字段, 左侧为插入表字段, 查询表选择字段, 字段可以为常量, 返回结果为成功插入行数.

```csharp
var result = context.Query<TeDataLog> ()
                    .SelectInsert (x => new TeDataLogHistory () {
                        Id = x.Id,
                        UserId = x.UserId,
                        ArticleId = x.ArticleId,
                        RecordTime = x.RecordTime,
                        Status = x.Status,
                        Action = x.Action,
                        RequestUrl = x.RequestUrl,
                        CheckId = 3,
                     });
```

<h2 id="aggregate_data">汇总统计数据(Aggregate Data)</h2>

使用`IQuery<T>.GroupBy<K>(lambda)`进行汇总统计数据, lambda表达式中的new方式定义数据的统计字段与汇总函数, 输出类型K可以为匿名类. GroupBy函数返回`IAggregate<K>`接口, 用于后续处理.

`IAggregate<K>`接口方法

| 方法 | 说明 |
|:------|:------|
| Having(Expression\<Func\<K, bool>> expression) | 把IAggreate中过滤条件置为当前过滤条件,如Having(x=>x.Count>1) |
| HavingWithAnd(Expression\<Func\<K, bool>> expression) | 把IAggreate中过滤条件以And方式连接当前过滤条件 |
| HavingWithOr(Expression\<Func\<K, bool>> expression) | 把IAggreate中查询条件以Or方式连接当前过滤条件 |
| HavingReset() |把IAggreate中过滤条件重置|
| OrderBy\<TKey>(Expression\<Func\<K, TKey>> expression) |把IAggreate中排序替换为当前字段正序排序,如OrderBy(x=>x.Id) |
| OrderByDescending\<TKey> (Expression\<Func\<K, TKey>> expression) | 把IAggreate中排序置为当前字段正序排序,如OrderByDescending(x=>x.Id) |
| OrderByCatch\<TKey>(Expression\<Func\<K, TKey>> expression) | 把IAggreate中排序连接当前字段正序排序 |
| OrderByDescendingCatch\<TKey>(Expression\<Func\<K, TKey>> expression) | 把IAggreate中排序连接当前字段倒序排序 |
| OrderByReset() | 把IAggreate中排序重置 |
| OrderByRandom() | 把IAggreate中排序置为随机排序 |
| Take(int count) | 设定IAggreate输出结果的数量 |
| Skip(int count) | 设定IAggreate输出结果需要跳过的数量 |
| Range(int from, int to) | 设定IAggreate输出结果的从from位到to位 |
| PageSize(int page, int size) | 设定IAggreate输出结果的分页结果,page:从1开始页数,size:每页数量 |
| RangeReset() | 把IAggreate中输出结果范围重置 |
| ToList() | 结果以List<K>的方式输出 |
| ToArray() | 结果以K[]的方式输出 |
| First() | 输出的汇总结果的首个数据结果对象,如无数据则为null	|

### 汇总函数

汇总函数由`Function`类的静态函数实现

| 函数 | 说明 |
|:------|:------|
| Count() | 数据行计数汇总, 返回int类型结果 |
| LongCount() | 数据行计数汇总, 返回long类型结果 |
| CountCondition(condition) | 条件判断该数据行计数汇总, 返回int类型结果 |
| LongCountCondition(condition) | 条件判断该数据行计数汇总, 返回long类型结果 |
| Count(field) | 指定字段计数汇总, 返回int类型结果 |
| LongCount(field) | 指定字段计数汇总, 返回long类型结果 |
| DistinctCount(field) | 指定字段去重复后计数汇总, 返回int类型结果 |
| DistinctLongCount(field) | 指定字段去重复后计数汇总, 返回long类型结果 |
| Sum(field) | 指定字段数值累加汇总, 返回汇总字段类型结果 |
| LongSum(field) |指定字段数值累加汇总, 返回long类型结果 |
| DistinctSum(field) | 指定字段去重复后累加汇总, 汇总字段类型结果 |
| DistinctLongSum(field) | 指定字段去重复后累加汇总, long类型结果 |
| Avg(field) | 指定字段数值平均值汇总, 返回double类型结果 |
| DistinctAvg(field) | 指定字段去重复后数值平均值汇总,返回double类型结果 |
| Max(field) | 指定字段的最大值 |
| Min(field) | 指定字段的最小最 |


#### 数据行计数汇总

```csharp
//普通汇总
var list = context.Query<TeUser> ()
                  .Where (x => x.Id >= 5)
                  .GroupBy (x => new LevelIdAgg () {
                      LevelId = x.LevelId,
                      Data = Function.Count ()
                   })
                  .ToList ();
//使用匿名类汇总
var list = context.Query<TeUser> ()
                  .Where (x => x.Id >= 5)
                  .GroupBy (x => new {
                      LevelId = x.LevelId,
                      Data = Function.Count ()
                   })
		  .ToList ();
//条件判断汇总
var list = context.Query<TeUser> ()
                  .Where (x => x.Id >= 5)
                  .GroupBy (x => new {
                      LevelId = x.LevelId,
                      Valid = Function.CountCondition (x.Status = 1),
                      Invalid = Function.CountCondition (x.Status != 1)
                   })
                  .ToList ();
```

#### 指定字段计数汇总

```csharp
//统计指定字段
var list = context.Query<TeUser> ()
                  .GroupBy (x => new LevelIdAgg () {
                      LevelId = x.LevelId,
                      Data = Function.Count (x.Area)
                   })
		  .ToList ();
//条件判断统计指定字段
var list = context.Query<TeUser> ()
                  .Where (x => x.Id >= 5)
                  .GroupBy (x => new {
                      LevelId = x.LevelId,
                      Valid = Function.Count (x.Status = 1 ? x.Area : null),
                      Invalid = Function.Count (x.Status != 1 ? x.Area : null)
                   })
		  .ToList ();
```

### 汇总数据过滤(Having)

使用`IAggreate<K>.Having(lambda)`方法加入汇总条件,对汇总数据做二次过滤,查询参数为Lambda表达式,有Having,HavingWithAnd,HavingWithOr,HavingReset四个方法

```csharp
var list = context.Query<TeUser> ()
                  .GroupBy (x => new LevelIdAgg () {
                      LevelId = x.LevelId,
                      Data = Function.Sum (x.LoginTimes)
                   })
                  .Having (y => y.Data > 15)
                  .ToList ();
```

### 汇总数据排序(OrderBy)

使用`IAggreate<K>.OrderBy(lambda)`方法加入汇总条件, 查询参数为Lambda表达式, 有OrderBy, OrderByDescending, OrderByCatch, OrderByDescendingCatch, OrderByReset, OrderByRandom六个方法

```csharp
//汇总字段排序
var list = context.Query<TeUser> ()
                  .GroupBy (x => new LevelIdAgg () {
                      LevelId = x.LevelId,
                      Data = Function.Count ()
                   })
                  .OrderBy (x => x.LevelId)
                  .ToList ();
//汇总结果排序
var list = context.Query<TeUser> ()
                  .GroupBy (x => new LevelIdAgg () {
                      LevelId = x.LevelId,
                      Data = Function.Count ()
                   })
                  .OrderBy (x => x.Data)
                  .ToList ();
```

### 汇总字段扩展

汇总数据时经常需要指定字段里的某部分数据进行提出汇总, 例如从时间字段提出日期统计, 可以使用字段扩展方式进行提出汇总.详见
[字段扩展](#field_extend)

#### 日期类统计

```csharp
//Date统计
var list = context.Query<TeUser> ()
                  .GroupBy (x => new RegDateAgg () {
                      RegDate = x.RegTime.Date,
                      Data = Function.Count ()
		   })
		  .ToList ();
//日期格式化统计
var list = context.Query<TeUser> ()
                  .GroupBy (x => new RegDateFormatAgg () {
                      RegDateFormat = x.RegTime.ToString("yyyy-MM-dd"),
                      Data = Function.Count ()
                   })
                  .ToList ();	
//年统计
var list = context.Query<TeUser> ()
                  .GroupBy (x => new NumDataAgg () {
                      Name = x.RegTime.Year,
                      Data = Function.Count ()
                   })
                  .ToList ();	
```

#### 字符串类统计

```csharp
//截取字符串统计
var list = context.Query<TeUser> ()
                  .GroupBy (x => new StringDataAgg () {
                      Name = x.Account.Substring (0, 5),
                      Data = Function.Count ()
                   })
		  .ToList ();
//字符串长度统计
var list = context.Query<TeUser> ()
                  .GroupBy (x => new NumDataAgg () {
                      Name = x.Account.Length,
                      Data = Function.Count ()
		   })
		  .ToList ();
```

<h2 id="join_table">表连接(Join Table)</h2>

使用`IQuery<T>.Join<T1>(on_lambda,T1_where_lambda)`表连接查询数据, `on_lambda`表达式中的表T和表T1的on关联表达式, 如`T.Id==T1.Id`, `T1_where_lambda`表达式中表T1的where查询. 函数返回`IJoinTable<T,T1>`接口, 用于表连接结果查询后续处理.支持LeftJoin(左连接)和RightJoin(右连接), 最多支持10个表连接. 同时支持IQuery(查询结果)与ISelect(字段选择查询)与IAggregate(数据汇总)的连接.通过`Select`方法选择指定的字段输出数据.

IJoinTable主要查询方法

| 方法 | 说明 |
|:------|:------|
| Where(Expression\<Func\<T, T1, bool>> expression) | 把IJoinTable中查询条件置为当前查询条件,如Where((x,y)=>x.Id>1 && y.Status==1) |
| WhereWithAnd(Expression\<Func\<T, T1, bool>> expression) | 把IJoinTable中查询条件以And方式连接当前查询条件 |
| WhereWithOr(Expression\<Func\<T, T1, bool>> expression) | 把IJoinTable中查询条件以Or方式连接当前查询条件 |
| WhereReset() | 把IJoinTable中查询条件重置 |
| OrderBy\<TKey>(Expression\<Func\<T, T1, TKey>> expression) | 把IJoinTable中排序替换为当前字段正序排序,如OrderBy((x,y)=>x.Id) |
| OrderByDescending\<TKey> (Expression\<Func\<T, T1, TKey>> expression) | 把IJoinTable中排序置为当前字段正序排序,如OrderByDescending((x,y)=>x.Id) |
| OrderByCatch\<TKey> (Expression\<Func\<T, T1, TKey>> expression) | 把IJoinTable中排序连接当前字段正序排序 |
| OrderByDescendingCatch\<TKey> (Expression\<Func\<T, T1, TKey>> expression) | 把IJoinTable中排序连接当前字段倒序排序 |
| OrderByReset() | 把IJoinTable中排序重置 |
| OrderByRandom() | 把IJoinTable中排序置为随机排序 |
| Take(int count) | 设定IJoinTable输出结果的数量 |
| Skip(int count) | 设定IJoinTable输出结果需要跳过的数量 |
| Range(int from, int to) | 设定IJoinTable输出结果的从from位到to位 |
| PageSize(int page, int size) | 设定IJoinTable输出结果的分页结果,page:从1开始页数,size:每页数量 |
| RangeReset() | 把IJoinTable中输出结果范围重置 |
| SetDistinct(bool distinct) | 设定是否使用Distinct方式输出结果 |
| Select<K> (Expression\<Func\<T, T1, K>> expression)| 选择需要输出的字段 |
| SelectInsert<K> (Expression\<Func\<T, T1, K>> expression) | 选择需要输出的字段并插入指定的表 |
| Count	| 返回该IJoinTable的数据长度,返回类型为int |
| LongCount | 返回该IJoinTable的数据长度,返回类型为long|

### 连表方法(Join Table Method)

```csharp
//内连接
var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
	          .ToList();
	      
//左连接
var list = context.Query<TeUser> ()
                  .LeftJoin<TeUserExtend>((x,y) => x.Id == y.Id)
	          .ToList();
	      
//右连接
var list = context.Query<TeUser> ()
                  .RightJoin<TeUserExtend>((x,y) => x.Id == y.Id)
		  .ToList();
```

### 连表类型(Join Table Type)

#### 基本表连接

```csharp
//表连接
var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
	          .ToList();

//表连接+查询
var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>(y => y.ExtendData != null, (x, y) => x.Id == y.Id)
	          .ToList();

//查询连接
var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>(context.Query<TeUserExtend>()
		  .Where(y => y.ExtendData != null), (x,y) => x.Id == y.Id)
	          .ToList();
```

##### 统计表连接

```csharp
//实体表连接统计结果
var list = context.Query<TeUser>()
                  .Join(context.Query<TeUserSub>()
                               .GroupBy(x => new {
                                   MId = x.MId,
                                   Count = Function.Count(),
                                }), (x, y) => x.Id == y.MId
	           )
                  .Select((x, y) => new {
			   Id = x.Id,
			   Account = x.Account,
			   LevelId = x.LevelId,
			   MId = y.MId,
			   Count = y.Count,
                   })
	          .ToList();
	      
//统计结果连接实体表             
var list = context.Query<TeMainTable>()
                  .GroupBy(x => new {
                      MId = x.MId,
                      Count = Function.Count(),
                   })
                  .Join<TeSubTable>((x, y) => x.MId == y.Id)
                  .Select((x, y) => new {
  		      Id = y.Id,
  		      Account = y.Account,
  		      LevelId = y.LevelId,
  		      MId = x.MId,
  		      Count = x.Count,
  	           })
		  .ToList();
```

### 条件查询(Where)

***

使用`IJoinTable<T,T1>.Where(lambda)`方法加入查询条件,查询参数为Lambda表达式, 有Where, WhereWithAnd, WhereWithOr, WhereReset四个方法

```csharp
var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
		  .Where((x,y) => x.Id > 1 && y.Status == 1 )
		  .ToList();

var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
		  .Where((x,y) => x.Id > 1)
		  .WhereWithAnd((x,y) => y.Status == 1 )
		  .ToList();
```

### 排序(OrderBy)

***

使用`IJoinTable<T,T1>.OrderBy(lambda)`方法加入查询条件, 查询参数为Lambda表达式, 有OrderBy, OrderByDescending, OrderByCatch, OrderByDescendingCatch,  OrderByReset, OrderByRandom六个方法

```csharp
var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
		  .OrderBy((x,y) => x.Id)
		  .ToList();

var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
		  .OrderByDescending((x,y) => y.Date)
		  .ToList();

var list = context.Query<TeUser> ()
                  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
		  .OrderBy((x,y) => x.Id)
		  .OrderByDescendingCatch((x,y) => y.Date)
		  .ToList();
```

### 选择指定字段(Select)

***

使用`IJoinTable<T,T1>.Select(lambda)`查询时指定字段输出新的结构类, 支持匿名类输出, 使用Lambda表达式中的new方式定义新结构类.

```csharp
var list = context.Query<TeUser> ()
	          .Join<TeUserExtend>((x,y) => x.Id == y.Id)
	          .Select ((x,y) => new TeUserJoin () {
	              Id = x.Id,
	              Account = x.Account,
	              LevelId = x.LevelId,
	              RegTime = x.RegTime,
	              ExtendData = y.ExtendData
		   })
		  .ToList ();
//匿名类
var list = context.Query<TeUser> ()
		  .Join<TeUserExtend>((x,y) => x.Id == y.Id)
		  .Select ((x,y) => new {
		      x.Id,
		      x.Account,
		      x.LevelId,
		      x.RegTime,
		      y.ExtendData
	           })
		  .ToList ();
```

### 查询批量插入

***

对查询数据进行全字段或指定字段插入指定的数据表, 直接通过数据库的`insert into t1(x1,x2,x3...)select t2.y1,t2.y2,t2.y3,t3.z1 from t2 join t3`方式直接插入数据

#### 指定字段插入

使用`IQuery<T>.SelectInsert<K>(lambda)`选择指定字段举行插入, lambda表达式中的new方式定义数据的插入表字段与查询表选择字段, 左侧为插入表字段, 查询表选择字段, 字段可以为常量.

```csharp
var result = context.Query<TeUser> ()
                    .Join<TeUserExtend>((x,y) => x.Id == y.Id)
                    .SelectInsert ((x,y) => new TeUserJoin () {
                        Id = x.Id,
                        Account = x.Account,
                        LevelId = x.LevelId,
                        RegTime = x.RegTime,
                        ExtendData = y.ExtendData 
                    });
```
