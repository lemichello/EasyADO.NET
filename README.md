# Install
To install EasyADO.NET you need to use NuGet:
* Install via NuGet with Visual Studio or Rider: type 'EasyADO.NET' in the search field and click on install button
* Install via Package Manager: `Install-Package EasyADO.NET -Version 1.1.0`
* Install via .NET CLI: `dotnet add package EasyADO.NET --version 1.1.0`
* Install via PackageReference: `<PackageReference Include="EasyADO.NET" Version="1.1.0" />`
* Install via Paket CLI: `paket add EasyADO.NET --version 1.1.0`

# Documentation
## Creating instance:
You can create an EasyAdoNet instance passing only your connection string to the constructor:
```C#
var easyAdoNet = new EasyAdoNet("Your connection string");
```
## `FindAll` method:
`FindAll` method retrieves all the rows from a given table:
```C#
easyAdoNet.FindAll("Table name");
```

You can additionally pass equality conditions or string predicate.

For example, when you need to select only persons, whose names are John, you can do this in two ways.
1. Using equality conditions:
```C#
var result = easyAdoNet.FindAll("Table name", new Dictionary<string, object>{ {"Name", "John"} });
```
2. Using string predicate:
```C#
var result = easyAdoNet.FindAll("Table name", "WHERE Name = 'John'");
```
Both two ways give the same result. String predicate becomes more efficient when you want to pass more difficult query, e.g.
with inner joins.

## `Find` method:
`Find` method retrieves all the data from a given table, selecting only given columns:
```C#
var result = easyAdoNet.Find("Table name", new[] {"Name", "Surname"});
```

As well as in `FindAll` method, `Find` method can take equality conditions or string predicate to retrieve only that rows, that
you need.
1. Using equality conditions:
```C#
var result = easyAdoNet.Find("Table name", new[] {"Name", "Surname"}, new Dictionary<string, object>{ {"Name", "John"} });
```
2. Using string predicate:
```C#
var result = easyAdoNet.Find("Table name", "WHERE Name = 'John'", new[] {"Name", "Surname"});
```

## `Insert` method:
`Insert` method inserts given values to the given table:
```C#
var insertedId = easyAdoNet.Insert("Table name", new Dictionary<string, object>
{ 
  {"Name", "Maksym"},
  {"Surname", "Lemich"}
});
```

## `Update` method:
`Update` method updates given values in the given table. This method also can take equality conditions or string predicate:
1. Using equality conditions:
```C#
easyAdoNet.Update("Table name", new Dictionary<string, object>{ {"Name", "Maksym"} }, 
                  new Dictionary<string, object>{ {"Name", "John"} });
```
In this example second parameter is equality conditions, third parameter - new values, where the key is column name and the 
value is replacing value of that column. So, we are changing all people with name Maksym with John.
2. Using string predicate:
```C#
easyAdoNet.Update("Table name", "WHERE Name = 'Maksym'", new Dictionary<string, object>{ {"Name", "John"} });
```

## `ExecView` method:
`ExecView` method executes given view name and returns all the data from the result of that view:
```C#
var result = easyAdoNet.ExecView("View name");
```

## `ExecProcedure` method:
`ExecProcedure` executes given stored procedure name with optional parameters:
1. With optional parameters:
```C#
var result = easyAdoNet.ExecProcedure("Procedure name", new Dictionary<string, object>{ {"Name", "Maksym"} });
```
2. Without optional parameters:
```C#
var result = easyAdoNet.ExecProcedure("Procedure name");
```

## Generic methods:
There are generic versions for the methods, which return SqlDataReader. You can pass a type of your class to the 'Type parameter' section and the result of this method will be converted to the collection of that class instances.
Note: The properties' names of the class and columns names in the table must be equal. Otherwise, the value from the query result will not be set to a particular class property.

### Notes
If you have some questions, bug reports or feature requests, feel free to write it in the 'Issues' section.
Thank you.
