Json Particle
=============

C# Library for extending object capabilities with easy serialization and deserialization using ``JSON`` format.

Provides simple object mapper that allows to convert objects between different class types using ``JSON`` serialization.

Allows deserialization from [TOML](http://toml.io/) format.

Depends on brilliant ``Newtonsoft.Json`` package.

Example
=======

```Json<T>```

Derrive from ``BigBytes.JsonParticle.Json<T>`` to add static methods ``Serialize`` and ``Deserialize`` to the class.

```csharp
public class Record : BigBytes.JsonParticle.Json<Record>
{
    public class _Book
    {
        public string Title;
        public int Year;
    }

    public class _Author
    {
        public string Name;
    }

    public _Book Book;
    public _Author Author;
}
```

```
var record = new Record()
{
    Book = new Record._Book()
    {
        Title = "UML Distilled",
        Year = 2003,
    },
    Author = new Record._Author()
    {
        Name = "Martin Fowler",
    },
};
var json = Record.Serialize(record);
Console.WriteLine(json);
```

```json
{
  "book": {
    "title": "UML Distilled",
    "year": 2003
  },
  "author": {
    "name": "Martin Fowler"
  }
}
```

Reverse conversion is also possible.

```csharp
var json = @"
{
  ""book"": {
    ""title"": ""UML Distilled"",
    ""year"": 2003
  },
  ""author"": {
    ""name"": ""Martin Fowler""
  }
}
";

var record = Record.Deserialize(json);
            
Console.WriteLine(record.Book.Title);
Console.WriteLine(record.Book.Year);
Console.WriteLine(record.Author.Name);
```

```
UML Distilled
2003
Martin Fowler
```

More advanced example with [TOML](http://toml.io/) input.

```toml
[book]
title = "UML Distilled"
year = 2003

[author]
name = "Martin Fowler"
```

```csharp
var toml = @"
[book]
title = ""UML Distilled""
year = 2003

[author]
name = ""Martin Fowler""
";
var json = new Converter.TomlToJson().Convert(toml);
var record = Mock.Record.Deserialize(json);
Debug.WriteLine(record.Book.Title);  // expect "UML Distilled"
Debug.WriteLine(record.Book.Year);   // expect "2003"
Debug.WriteLine(record.Author.Name); // expect "Martin Fowler"
```

Last but not least example with *ungreedy*[<sup>1</sup>](#ungreedy_mapping) mapping.

```csharp
public class Person : Json<Person>
{
    public string Name;
    public string City;
    public int Age;
}
```

```csharp
public class Customer : Json<Customer>
{
    public string Name;
    public string City;
    public string Office;
}
```

```csharp
Person person;
Customer customer;

person = new Person()
{
    Name = "Andy",
    Age = 21,
    City = "Warsaw",
};

customer = new Mapper<Customer>()
    .Map(person);

Console.WriteLine(customer.Name);              // Outputs: "Andy"
Console.WriteLine(customer.City);              // Outputs: "Warsaw"
Console.WriteLine(customer.Office ?? "NULL");  // Outputs: "NULL"

person = new Mapper<Person>()
    .Map(customer);

Console.WriteLine(person.Name);  // Outputs: "Andy"
Console.WriteLine(person.City);  // Outputs: "Warsaw"
Console.WriteLine(person.Age);   // Outputs: "0"
```

```json
{
  "name": "Andy",
  "city": "Warsaw",
  "age": 21
}
```

If you get problems with mapping check if both classes contain ``Serialize`` and ``Deserialize`` methods. That's the minimum requirement for ``Mapper<>`` to work. You shouldn't get any problems when classes are using inheritance from ``Json<>``.

Functions
---------

```TomlSectionNameToCSharpName```

Converts TOML section name to its C# counterpart.

```csharp
var toml = "list.one.12";
var csharp = BigBytes.JsonParticle.Utility.TomlSectionNameToCSharpName(toml);
System.Diagnostics.Debug.WriteLine(csharp); // expect "ListOne_12"
```

```CSharpNameToJsonName```

Converts C# name to its JSON counterpart.

```csharp
var csharp = "MyObject";
var json = BigBytes.JsonParticle.Utility.CSharpNameToJsonName(csharp);
System.Diagnostics.Debug.WriteLine(json); // expect "myObject"
```

Project
-------

Main library project directory contains two project files.

``BigBytes.JsonParticle.Legacy.csproj`` 

Notes
-----

``1)`` Ungreedy convertion allows to rewrite values only for fields that are present in both objects without throwing an exception 

Author
------

Filip Golewski
