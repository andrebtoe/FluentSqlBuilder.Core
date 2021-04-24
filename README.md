## FluentSqlBuilder.Core

Package to generate SQL queries. Group by, COUNT, AVG, MAX, MIN, ORDER BY, and more.
This package is only responsible for generating the select command, it does not intermediate the execution of the query.
The idea is not to replace an ORM framework, just to support applications that cannot use

Package on nuget: [FluentSqlBuilder.Core](https://www.nuget.org/packages/FluentSqlBuilder.Core/).

For the examples consider the classes below

To define the correct name of the table in the database, use the DataAnnotations classes: "System.ComponentModel.DataAnnotations.Schema.TableAttribute" and "System.ComponentModel.DataAnnotations.Schema.ColumnAttribute"

```csharp
[Table("order", Schema = "checkout")]
public class OrderDataModel
{
    public int Id { get; set; }
    [Column("customer_id")]
    public int CustomerId { get; set; }
    public OrderStatus Status { get; set; }
}

[Table("customer", Schema = "customers")]
public class CustomerDataModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CustomerType Type { get; set; }
}
```

## Simple select
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
~~~~

## Select with INNER JOIN
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .InnerJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);					 
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
INNER JOIN [customer] ON ([order].[customer_id] = [customer].[Id])
~~~~

## Select with LEFT JOIN
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .LeftJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);					 
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
LEFT JOIN [customer] ON ([order].[customer_id] = [customer].[Id])
~~~~

## Select with RIGHT JOIN
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                    .RightJoin<CustomerDataModel>((order, customer) => order.CustomerId == customer.Id);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
RIGHT JOIN [customer] ON ([order].[customer_id] = [customer].[Id])
~~~~

## Select with WHERE simple
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order] WHERE ([order].[Status] = @Param1
AND [order].[customer_id] = @Param2)
~~~~

## Select with WHERE simple and INNER JOIN
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .Where(x => x.Status == OrderStatus.Paid && x.CustomerId == 1);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
INNER JOIN [customer] ON ([order].[customer_id] = [customer].[Id])
WHERE ([order].[Status] = @Param1
AND [order].[customer_id] = @Param2)
AND [customer].[Type] = @Param3
~~~~

## Select with ORDER BY ASC
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .OrderBy(x => x.CustomerId);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
ORDER BY [order].[customer_id] ASC
~~~~

## Select with ORDER BY DESC
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                 .OrderByDescending(x => x.CustomerId);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order] ORDER BY [order].[customer_id] DESC
~~~~

## Select with MIN AND GROUP BY
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .Min(x => x.CustomerId)
                     .GroupBy(x => x.CustomerId);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT MIN([order].[customer_id])
FROM [checkout].[order]
GROUP BY [order].[customer_id]
~~~~

## Select with GROUP BY and HAVING
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .Min(x => x.CustomerId)
                     .GroupBy(x => x.CustomerId);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT MIN([order].[customer_id])
FROM [checkout].[order]
GROUP BY [order].[customer_id]
HAVING MIN([order].[customer_id]) > @Param1
~~~~

## Select with GROUP BY and HAVING
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                    .Projection(x => new { x.Id, x.Status });

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[Status]
FROM [checkout].[order]
~~~~

## Select with Limit
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .Limit(10);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT TOP(10) [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
~~~~

## Select with Pagination
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented)
                     .Limit(10);

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order].[Id],
[order].[customer_id] AS CustomerId,
[order].[Status]
FROM [checkout].[order]
ORDER BY [order].[Id] ASC
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY
~~~~

## Select with alias
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, "order_alias");

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT [order_alias].[Id],
[order_alias].[customer_id] AS CustomerId,
[order_alias].[Status]
FROM [checkout].[order_alias] AS order_alias
~~~~

## Select with Projection, WHERE, INNER JOIN ORDER BY and Limit
```csharp
var sqlBuilder = new FluentSqlBuilder<OrderDataModel>(SqlAdapterType.SqlServer2019, SqlBuilderFormatting.Indented, "order_alias");

var parameters = sqlBuilder.GetParameters();
var sqlSelect = sqlBuilder.ToString();
var results = YourConnection.Query<OrderDataModel>(sqlSelect, parameters);
```
output
~~~~sql
SELECT TOP(10) [order_alias].[customer_id] AS CustomerId,
[customer_alias].[Id],
[customer_alias].[customer_id] AS CustomerId,
[customer_alias].[Status]
FROM [checkout].[order_alias] AS order_alias
INNER JOIN [customer] AS customer_alias ON ([order_alias].[customer_id] = [customer_alias].[Id])
WHERE ([order_alias].[Status] = @Param1
AND [order_alias].[customer_id] = @Param2)
ORDER BY [order_alias].[Id] ASC
~~~~
